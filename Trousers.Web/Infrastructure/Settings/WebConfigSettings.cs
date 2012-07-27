using System;
using Trousers.Core;

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