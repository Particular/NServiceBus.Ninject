
namespace NServiceBus.ObjectBuilder.Ninject.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Ninject;

    /// <summary>
    /// Implements an more aggressive injection heuristic.
    /// </summary>
    class ObjectBuilderPropertyHeuristic : IObjectBuilderPropertyHeuristic
    {
        IList<Type> registeredTypes;
        IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilderPropertyHeuristic"/> class.
        /// </summary>
        public ObjectBuilderPropertyHeuristic(IKernel container)
        {
            registeredTypes = new List<Type>();
            kernel = container;
        }

        /// <summary>
        /// Gets the registered types.
        /// </summary>
        /// <value>The registered types.</value>
        public IList<Type> RegisteredTypes
        {
            get { return registeredTypes; }
            private set { registeredTypes = value; }
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public INinjectSettings Settings{get;set;}

        /// <summary>
        /// Determines whether a given type should be injected.
        /// </summary>
        /// <param name="member">The <paramref name="member"/> info to check.</param>
        /// <returns>
        ///   <see langword="true" /> if a given type needs to be injected; otherwise <see langword="false" />.
        /// </returns>
        public bool ShouldInject(MemberInfo member)
        {
            var propertyInfo = member as PropertyInfo;

            if (propertyInfo == null || propertyInfo.GetSetMethod(Settings.InjectNonPublic) == null)
            {
                return false;
            }

            var shouldInject = registeredTypes.Any(x => propertyInfo?.DeclaringType?.IsAssignableFrom(x) ?? false)
                   && RegisteredTypes.Any(x => propertyInfo.PropertyType.IsAssignableFrom(x)) 
                   && propertyInfo.CanWrite;

            if (shouldInject)
            {
                return true;
            }

            var instance = kernel.TryGet(propertyInfo.PropertyType);
            return instance != null;
        }

        public void Dispose()
        {
            //Injected at compile time
        }

        void DisposeManaged()
        {
            registeredTypes?.Clear();
        }
    }
}