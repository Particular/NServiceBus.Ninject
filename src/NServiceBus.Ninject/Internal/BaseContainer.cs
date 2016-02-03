using Ninject;
using Ninject.Activation;
using Ninject.Extensions.NamedScope;
using Ninject.Infrastructure;
using Ninject.Injection;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Selection;
using Ninject.Extensions.ContextPreservation;
using NServiceBus.ObjectBuilder.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NServiceBus.ObjectBuilder.Ninject.Internal
{
    internal class BaseContainer : IContainer
    {
        protected const string RootScopeName = "Base";

        protected IKernel kernel;

        protected NamedScope scope;

        protected string scopeName;

        IDictionary<DependencyLifecycle, Func<IContext, object>> dependencyLifecycleToScopeMapping;

        IObjectBuilderPropertyHeuristic propertyHeuristic;

        protected BaseContainer(IKernel kernel)
        {
            dependencyLifecycleToScopeMapping =
            new Dictionary<DependencyLifecycle, Func<IContext, object>>
                {
                    { DependencyLifecycle.SingleInstance, StandardScopeCallbacks.Singleton },
                    { DependencyLifecycle.InstancePerCall, StandardScopeCallbacks.Transient },
                    { DependencyLifecycle.InstancePerUnitOfWork, ctx => GetContext() == RootScopeName ? StandardScopeCallbacks.Thread : ctx.GetNamedScope(GetContext()) },
                };

            this.kernel = kernel;

            RegisterNecessaryBindings();

            propertyHeuristic = this.kernel.Get<IObjectBuilderPropertyHeuristic>();

            AddCustomPropertyInjectionHeuristic();

            this.kernel
                .Bind<NinjectChildContainer>()
                .ToConstructor(ctx => new NinjectChildContainer(ctx.Context.Kernel, this));
        }

        public virtual object Build(Type typeToBuild)
        {
            if (!HasComponent(typeToBuild))
            {
                throw new ArgumentException(typeToBuild + " is not registered in the container");
            }

            var request = scope.CreateRequest(typeToBuild, null, new IParameter[0], false, true);
            return request.ParentContext.GetContextPreservingResolutionRoot().Resolve(request).FirstOrDefault();
        }

        public virtual IEnumerable<object> BuildAll(Type typeToBuild)
        {
            return scope.GetAll(typeToBuild);
        }

        public virtual IContainer BuildChildContainer()
        {
            throw new NotImplementedException();
        }

        public virtual void Configure(Type component, DependencyLifecycle dependencyLifecycle)
        {
            if (HasComponent(component))
            {
                return;
            }

            var instanceScope = GetInstanceScopeFrom(dependencyLifecycle);

            var isInstancePerUnitOfWork = dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork;
            var bindingConfigurations = BindComponentToItself(component, instanceScope, isInstancePerUnitOfWork);
            AddAliasesOfComponentToBindingConfigurations(component, bindingConfigurations);

            propertyHeuristic.RegisteredTypes.Add(component);
        }

        public virtual void Configure<T>(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            var componentType = typeof(T);

            if (HasComponent(componentType))
            {
                return;
            }

            var instanceScope = GetInstanceScopeFrom(dependencyLifecycle);

            var isInstancePerUnitOfWork = dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork;
            var bindingConfigurations = BindComponentToMethod(componentFactory, instanceScope, isInstancePerUnitOfWork);
            AddAliasesOfComponentToBindingConfigurations(componentType, bindingConfigurations);

            propertyHeuristic.RegisteredTypes.Add(componentType);
        }

        public virtual void ConfigureProperty(Type component, string property, object value)
        {
            var bindings = kernel.GetBindings(component);

            if (bindings == null || !bindings.Any())
            {
                throw new ArgumentException("Component not registered", component.FullName);
            }

            foreach (var binding in bindings)
            {
                binding.Parameters.Remove(new PropertyValue(property, c => null));
                binding.Parameters.Add(new PropertyValue(property, value));
            }
        }

        public virtual bool HasComponent(Type componentType)
        {
            var request = kernel.CreateRequest(componentType, null, new IParameter[0], false, true);

            return kernel.CanResolve(request);
        }

        public virtual void RegisterSingleton(Type lookupType, object instance)
        {
            if (propertyHeuristic.RegisteredTypes.Contains(lookupType))
            {
                kernel
                    .Rebind(lookupType)
                    .ToConstant(instance);
                return;
            }

            propertyHeuristic
                .RegisteredTypes
                .Add(lookupType);

            kernel
                .Bind(lookupType)
                .ToConstant(instance);
        }

        public virtual void Release(object instance)
        {
            kernel.Release(instance);
        }

        public void NotifyScopeChange(string newScopeName)
        {
            scopeName = newScopeName;
        }

        protected virtual void DisposeManaged()
        {
            if (kernel != null)
            {
                if (!kernel.IsDisposed)
                {
                    kernel.Dispose();
                }
            }
        }

        public void Dispose()
        {
            //must be empty
        }

        static IEnumerable<Type> GetAllServiceTypesFor(Type component)
        {
            if (component == null)
            {
                return new List<Type>();
            }

            var result = new List<Type>(component.GetInterfaces()) { component };

            foreach (var interfaceType in component.GetInterfaces())
            {
                result.AddRange(GetAllServiceTypesFor(interfaceType));
            }

            return result.Distinct();
        }

        void AddAliasesOfComponentToBindingConfigurations(Type component, IEnumerable<IBindingConfiguration> bindingConfigurations)
        {
            var services = GetAllServiceTypesFor(component).Where(t => t != component);

            foreach (var service in services)
            {
                foreach (var bindingConfiguration in bindingConfigurations)
                {
                    kernel.AddBinding(new Binding(service, bindingConfiguration));
                }
            }
        }

        Func<IContext, object> GetInstanceScopeFrom(DependencyLifecycle dependencyLifecycle)
        {
            Func<IContext, object> lifecycleScope;

            if (!dependencyLifecycleToScopeMapping.TryGetValue(dependencyLifecycle, out lifecycleScope))
            {
                throw new ArgumentException("The dependency lifecycle is not supported", dependencyLifecycle.ToString());
            }

            return lifecycleScope;
        }

        IEnumerable<IBindingConfiguration> BindComponentToItself(Type component, Func<IContext, object> instanceScope, bool addChildContainerScope)
        {
            var bindingConfigurations = new List<IBindingConfiguration>();
            if (addChildContainerScope)
            {
                var instanceScopeConfiguration = kernel
                    .Bind(component)
                    .ToSelf()
                    .WhenNotInUnitOfWork()
                    .InScope(instanceScope)
                    .BindingConfiguration;
                bindingConfigurations.Add(instanceScopeConfiguration);

                var unitOfWorkConfiguration = kernel
                    .Bind(component)
                    .ToSelf()
                    .WhenInUnitOfWork()
                    .InUnitOfWorkScope()
                    .BindingConfiguration;
                bindingConfigurations.Add(unitOfWorkConfiguration);
            }
            else
            {
                var instanceScopeConfiguration = kernel
                    .Bind(component)
                    .ToSelf()
                    .InScope(instanceScope)
                    .BindingConfiguration;
                bindingConfigurations.Add(instanceScopeConfiguration);
            }

            return bindingConfigurations;
        }

        IEnumerable<IBindingConfiguration> BindComponentToMethod<T>(Func<T> component, Func<IContext, object> instanceScope, bool addChildContainerScope)
        {
            var bindingConfigurations = new List<IBindingConfiguration>();
            if (addChildContainerScope)
            {
                var instanceScopeConfiguration = kernel
                    .Bind<T>()
                    .ToMethod(context => component.Invoke())
                    .WhenNotInUnitOfWork()
                    .InScope(instanceScope)
                    .BindingConfiguration;
                bindingConfigurations.Add(instanceScopeConfiguration);

                var unitOfWorkConfiguration = kernel
                    .Bind<T>()
                    .ToMethod(context => component.Invoke())
                    .WhenInUnitOfWork()
                    .InUnitOfWorkScope()
                    .BindingConfiguration;
                bindingConfigurations.Add(unitOfWorkConfiguration);
            }
            else
            {
                var instanceScopeConfiguration = kernel
                    .Bind<T>()
                    .ToMethod(context => component.Invoke())
                    .InScope(instanceScope)
                    .BindingConfiguration;
                bindingConfigurations.Add(instanceScopeConfiguration);
            }

            return bindingConfigurations;
        }

        void AddCustomPropertyInjectionHeuristic()
        {
            var selector = kernel.Components.Get<ISelector>();

            selector.InjectionHeuristics.Add(
                kernel.Get<IObjectBuilderPropertyHeuristic>());
        }

         void RegisterNecessaryBindings()
        {
            if (kernel.TryGet(typeof(IContainer)) == null)
            {
                kernel
                    .Bind<IContainer>()
                    .ToConstant(this)
                    .InSingletonScope();
            }

            if (kernel.TryGet(typeof(IObjectBuilderPropertyHeuristic)) == null)
            {
                kernel
                    .Bind<IObjectBuilderPropertyHeuristic>()
                    .To<ObjectBuilderPropertyHeuristic>()
                    .InSingletonScope()
                    .WithPropertyValue("Settings", context => context.Kernel.Settings);
            }

            if (kernel.TryGet(typeof(IInjectorFactory)) == null)
            {
                kernel
                    .Bind<IInjectorFactory>()
                    .ToMethod(context => context.Kernel.Components.Get<IInjectorFactory>());
            }
        }

        public string GetContext()
        {
            return scopeName;
        }
    }
}