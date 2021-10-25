namespace NServiceBus.ObjectBuilder.Ninject.Internal
{
    using System;
    using System.Collections.Generic;
    using Common;
    using global::Ninject;
    using global::Ninject.Extensions.NamedScope;
    using global::Ninject.Syntax;

    class NinjectChildContainer : DisposeNotifyingObject, IContainer
    {
        IResolutionRoot resolutionRoot;

        public NinjectChildContainer(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public object Build(Type typeToBuild)
        {
            return resolutionRoot.Get(typeToBuild);
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            return resolutionRoot.GetAll(typeToBuild);
        }

        public IContainer BuildChildContainer()
        {
            throw new InvalidOperationException("Can't perform configurations on child containers");
        }

        public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
        {
            throw new InvalidOperationException("Can't perform configurations on child containers");
        }

        public void Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
            throw new InvalidOperationException("Can't perform configurations on child containers");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Not changing public API")]
        public void ConfigureProperty(Type component, string property, object value)
        {
            throw new InvalidOperationException("Can't perform configurations on child containers");
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            throw new InvalidOperationException("Can't perform configurations on child containers");
        }

        public bool HasComponent(Type componentType)
        {
            throw new InvalidOperationException();
        }

        public void Release(object instance)
        {
            throw new InvalidOperationException();
        }
    }
}