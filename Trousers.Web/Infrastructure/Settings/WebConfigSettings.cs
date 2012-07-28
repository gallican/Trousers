using System;
using Trousers.Core;
using Trousers.Core.Settings;

namespace Trousers.Web.Infrastructure.Settings
{
    public class WebConfigSettings : ISettings
    {
        public Uri TfsUri
        {
            get { return new Uri("http://win-gs9gmujits8:8080/tfs/DefaultCollection"); }
        }
    }
}