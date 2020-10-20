namespace CLS.Core.Data
{
    public partial class Subscription
    {
        public string PublishingSystemName => PublishingSystem?.Name ?? "All";
        public string PublishingSystemEnvironmentTypeName => PublishingSystem?.EnvironmentType?.Name ?? "All";
    }
}
