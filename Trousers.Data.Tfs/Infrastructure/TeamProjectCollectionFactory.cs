using System.Net;
using Microsoft.TeamFoundation.Client;
using Trousers.Core.Infrastructure.Settings;

namespace Trousers.Data.Tfs.Infrastructure
{
    public class TeamProjectCollectionFactory
    {
        private readonly ISettings _settings;
        private readonly ICredentials _credentials;

        public TeamProjectCollectionFactory(ISettings settings, ICredentials credentials)
        {
            _settings = settings;
            _credentials = credentials;
        }

        public TfsTeamProjectCollection Create()
        {
            return new TfsTeamProjectCollection(_settings.TfsUri, _credentials);
        }
    }
}