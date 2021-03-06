﻿namespace NServiceBus
{
    // An internal type referenced by the API approvals test as it can't reference obsoleted types.
    class NinjectInternalType
    {
    }

    /// <summary>
    /// Ninject Container
    /// </summary>
    [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        TreatAsErrorFromVersion = "8",
        RemoveInVersion = "9")]
    public class NinjectBuilder
    {
    }

    /// <summary>
    /// Ninject extension to pass an existing Ninject container instance.
    /// </summary>
    [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        TreatAsErrorFromVersion = "8",
        RemoveInVersion = "9")]
    public static class NinjectExtensions
    {
    }

}

namespace NServiceBus.ObjectBuilder.Ninject
{
    using System;
    using global::Ninject.Syntax;

    /// <summary>
    /// Contains extension methods to configure UnitOfWork scoped bindings.
    /// </summary>
    [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        TreatAsErrorFromVersion = "8",
        RemoveInVersion = "9")]
    public static class NinjectObjectBuilderExtensions
    {
        /// <summary>
        /// Defines a conditional binding which is applied when the requested service is in an unit of work.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax</param>
        /// <returns>The binding</returns>
        [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingInNamedWithOrOnSyntax<T> WhenInUnitOfWork<T>(this IBindingWhenSyntax<T> syntax)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines a conditional binding which is applied when the requested service is NOT in an unit of work.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax</param>
        /// <returns>The binding</returns>
        [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingInNamedWithOrOnSyntax<T> WhenNotInUnitOfWork<T>(this IBindingWhenSyntax<T> syntax)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines the unit of work scope on the requested service.
        /// </summary>
        /// <typeparam name="T">The requested service type.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>The binding.</returns>
        [ObsoleteEx(Message = "Ninject is no longer supported via the NServiceBus.Ninject adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static IBindingNamedWithOrOnSyntax<T> InUnitOfWorkScope<T>(this IBindingInSyntax<T> syntax)
        {
            throw new NotImplementedException();
        }
    }
}