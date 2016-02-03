namespace NServiceBus.ObjectBuilder.Ninject.Internal
{
    using System;
    using System.Collections.Generic;
    using global::Ninject.Selection.Heuristics;

    internal interface IObjectBuilderPropertyHeuristic : IInjectionHeuristic
    {
        IList<Type> RegisteredTypes
        {
            get;
        }
    }
}