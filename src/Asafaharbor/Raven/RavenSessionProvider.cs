using Raven.Client.Document;

namespace Asafaharbor.Web.Raven
{
    public class RavenSessionProvider
    {
        private static DocumentStore _documentStore;

        public bool SessionInitialized { get; set; }

        public static DocumentStore DocumentStore
        {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
        }

        private static DocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "RAVENHQ_CONNECTION_STRING"
            };

            store.Initialize();

            return store;
        }
    }
}