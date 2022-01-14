namespace NServiceBus
{
    using Container;
    using Ninject;

    /// <summary>
    /// Ninject extension to pass an existing Ninject container instance.
    /// </summary>
    [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        TreatAsErrorFromVersion = "8",
        RemoveInVersion = "9")]
    public static class NinjectExtensions
    {
        /// <summary>
        /// Use a pre-configured Ninject kernel
        /// </summary>
        /// <param name="customizations"></param>
        /// <param name="kernel">The existing container instance.</param>
        [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
            TreatAsErrorFromVersion = "8",
            RemoveInVersion = "9")]
        public static void ExistingKernel(this ContainerCustomizations customizations, IKernel kernel)
        {
            customizations.Settings.Set<NinjectBuilder.KernelHolder>(new NinjectBuilder.KernelHolder(kernel));
        }
    }
}