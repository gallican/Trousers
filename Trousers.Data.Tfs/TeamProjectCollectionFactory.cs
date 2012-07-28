using Microsoft.TeamFoundation.Client;
using Trousers.Core.Settings;

namespace Trousers.Data.Tfs
{
    public class TeamProjectCollectionFactory
    {
        private readonly ISettings _settings;

        public TeamProjectCollectionFactory(ISettings settings)
        {
            _settings = settings;
        }

        public TfsTeamProjectCollection Create()
        {
            return new TfsTeamProjectCollection(_settings.TfsUri);
        }
    }
}