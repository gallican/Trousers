using System;
using System.Net;
using Trousers.Core.Infrastructure.Settings;

namespace Trousers.Web.Infrastructure.Settings
{
    public class SettingsBasedCredentials: ICredentials
    {
        private readonly ISettings _settings;

        public SettingsBasedCredentials(ISettings settings)
        {
            _settings = settings;
        }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            if (_settings.UseDefaultCredentials) return CredentialCache.DefaultNetworkCredentials;
            return new NetworkCredential(_settings.TfsUserName, _settings.TfsPassword);
        }
    }
}