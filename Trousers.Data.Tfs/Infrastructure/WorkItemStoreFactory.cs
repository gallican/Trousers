using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Trousers.Data.Tfs.Infrastructure
{
    public class WorkItemStoreFactory
    {
        private readonly TfsTeamProjectCollection _teamProjectCollection;

        public WorkItemStoreFactory(TfsTeamProjectCollection teamProjectCollection)
        {
            _teamProjectCollection = teamProjectCollection;
        }

        public WorkItemStore Create()
        {
            return new WorkItemStore(_teamProjectCollection);
        }
    }
}