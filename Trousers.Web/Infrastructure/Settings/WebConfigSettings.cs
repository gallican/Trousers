using System;
using Trousers.Core;
using Trousers.Core.Settings;

namespace Trousers.Web.Infrastructure.Settings
{
    public class WebConfigSettings : ISettings
    {
        public Uri TfsUri
        {
            get { return new Uri("http://tfs.localtest.me:8080/tfs/DefaultCollection"); }
        }
    }
}