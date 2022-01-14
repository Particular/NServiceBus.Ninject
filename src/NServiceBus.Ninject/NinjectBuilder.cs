namespace NServiceBus
{
    using Container;
    using Ninject;
    using ObjectBuilder.Common;
    using ObjectBuilder.Ninject;
    using Settings;

    /// <summary>
    /// Ninject Container
    /// </summary>
    [ObsoleteEx(Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
    TreatAsErrorFromVersion = "8",
    RemoveInVersion = "9")]
    public class NinjectBuilder : ContainerDefinition
    {
        /// <summary>
        /// Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            if (settings.TryGet(out KernelHolder kernelHolder))
            {
                settings.AddStartupDiagnosticsSection("NServiceBus.Ninject", new
                {
                    UsingExistingKernel = true
                });

                return new NinjectObjectBuilder(kernelHolder.ExistingKernel);
            }

            settings.AddStartupDiagnosticsSection("NServiceBus.Ninject", new
            {
                UsingExistingKernel = false
            });

            return new NinjectObjectBuilder();
        }

        internal class KernelHolder
        {
            public KernelHolder(IKernel kernel)
            {
                ExistingKernel = kernel;
            }

            public IKernel ExistingKernel { get; }
        }
    }
}