using System;
using System.IO;
using Trousers.Core.Infrastructure.Settings;

namespace Trousers.Web.Infrastructure.Settings
{
    /// <summary>
    ///   This class exists so that I can hack in credentials and other junk, check things in to GitHub and
    ///   not tell the world the credentials I'm using for my test TFS servers.   -andrewh 7/8/2012
    /// </summary>
    public class HackyFileSettings : ISettings
    {
        public Uri TfsUri
        {
            get { return new Uri(File.ReadAllText(@"C:\Temp\TfsUri.txt").Trim()); }
        }

        public bool UseDefaultCredentials
        {
            get { return false; }
        }

        public string TfsUserName
        {
            get { return File.ReadAllText(@"C:\Temp\TfsUserName.txt").Trim(); }
        }

        public string TfsPassword
        {
            get { return File.ReadAllText(@"C:\Temp\TfsPassword.txt").Trim(); }
        }
    }
}