﻿namespace NServiceBus.ObjectBuilder.Ninject
{
    using global::Ninject.Extensions.NamedScope;
    using global::Ninject.Syntax;

    /// <summary>
    /// Contains extension methods to configure UnitOfWork scoped bindings.
    /// </summary>
    [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        TreatAsErrorFromVersion = "8",
        RemoveInVersion = "9")]
    public static class NinjectObjectBuilderExtensions
    {
        const string ScopeName = "NinjectObjectBuilder";

        /// <summary>
        /// Defines a conditional binding which is applied when the requested service is in an unit of work.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax</param>
        /// <returns>The binding</returns>
        [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingInNamedWithOrOnSyntax<T> WhenInUnitOfWork<T>(this IBindingWhenSyntax<T> syntax)
        {
            return syntax.WhenAnyAncestorNamed(ScopeName);
        }

        /// <summary>
        /// Defines a conditional binding which is applied when the requested service is NOT in an unit of work.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax</param>
        /// <returns>The binding</returns>
        [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingInNamedWithOrOnSyntax<T> WhenNotInUnitOfWork<T>(this IBindingWhenSyntax<T> syntax)
        {
            return syntax.WhenNoAncestorNamed(ScopeName);
        }

        /// <summary>
        /// Defines the unit of work scope on the requested service.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>The binding.</returns>
        [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingNamedWithOrOnSyntax<T> InUnitOfWorkScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InNamedScope(ScopeName);
        }

        internal static void DefinesNinjectObjectBuilderScope<T>(this IBindingWhenInNamedWithOrOnSyntax<T> syntax)
        {
            syntax.Named(ScopeName).DefinesNamedScope(ScopeName);
        }
    }
}