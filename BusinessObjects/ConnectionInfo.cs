namespace BusinessObjects
{
    public class ConnectionInfo
    {
        public ConnectionInfo()
        {
        }

        public string ConnectionName { get; set; }

        public string ConnectionString { get; set; }

        public string ProviderName { get; set; }

        public string Database { get; set; }
    }
}