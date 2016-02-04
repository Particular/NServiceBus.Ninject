namespace NServiceBus.ObjectBuilder.Ninject.Internal
{
    using System;
    using System.Collections.Generic;
    using global::Ninject;
    using global::Ninject.Extensions.NamedScope;
    using global::Ninject.Extensions.ChildKernel;
    using global::Ninject.Extensions.ContextPreservation;
    using global::Ninject.Parameters;
    using System.Linq;

    class NinjectChildContainer : BaseContainer
    {
        string parentScope;
        BaseContainer parentContainer;

        public NinjectChildContainer(IKernel resolutionRoot, BaseContainer parentContainer)
            : base(new ChildKernel(resolutionRoot))
        {
            parentScope = parentContainer.GetContext();
            scopeName = "Base.child";
            this.parentContainer = parentContainer;

            scope = kernel.CreateNamedScope(scopeName);
        }

        public override object Build(Type typeToBuild)
        {
            var request = scope.CreateRequest(typeToBuild, null, new IParameter[0], false, true);
            return request.ParentContext.GetContextPreservingResolutionRoot().Resolve(request).FirstOrDefault();
        }

        public override IEnumerable<object> BuildAll(Type typeToBuild)
        {
            return scope.GetAll(typeToBuild);
        }

        protected override void DisposeManaged()
        {
            if (!scope?.IsDisposed ?? false)
            {
                scope?.Dispose();

                parentContainer.NotifyScopeChange(parentScope);
            }

            base.DisposeManaged();
        }
    }
}