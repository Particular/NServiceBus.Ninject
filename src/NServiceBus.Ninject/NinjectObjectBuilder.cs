namespace NServiceBus.ObjectBuilder.Ninject
{
    using Common;
    using global::Ninject;
    using global::Ninject.Extensions.NamedScope;
    using Internal;

    /// <summary>
    /// Implementation of IBuilderInternal using the Ninject Framework container
    /// </summary>
    class NinjectObjectBuilder : BaseContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectObjectBuilder"/> class.
        /// </summary>
        public NinjectObjectBuilder()
            : this(new StandardKernel())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectObjectBuilder"/> class.
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/> to base this instance of the ObjectBuilder on</param>
        public NinjectObjectBuilder(IKernel kernel)
            : base(kernel)
        {
            scopeName = RootScopeName;
            scope = kernel.CreateNamedScope(GetContext());
        }

        /// <summary>
        /// Returns a child instance of the container to facilitate deterministic disposal
        /// of all resources built by the child container.
        /// </summary>
        /// <returns>A new child container.</returns>
        public override IContainer BuildChildContainer()
        {
            var childContainer = kernel.Get<NinjectChildContainer>();

            scopeName = childContainer.GetContext();

            return childContainer;
        }
    }
}
