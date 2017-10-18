namespace NServiceBus.Features
{
    /// <summary>
    /// Adds Diagnostics information
    /// </summary>
    public class UnityDiagnostics : Feature
    {
        /// <summary>
        /// Constructor for diagnostics feature
        /// </summary>
        public UnityDiagnostics()
        {
            EnableByDefault();
        }

        /// <summary>
        /// Sets up diagnostics
        /// </summary>
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Settings.AddStartupDiagnosticsSection("NServiceBus.Unity", new
            {
                UsingExistingContainer = context.Settings.HasSetting<UnityBuilder.ContainerHolder>()
            });
        }
    }
}
