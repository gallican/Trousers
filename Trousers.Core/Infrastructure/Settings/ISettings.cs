using System;

namespace Trousers.Core.Infrastructure.Settings
{
    public interface ISettings
    {
        Uri TfsUri { get; }
        bool UseDefaultCredentials { get; }
        string TfsUserName { get; }
        string TfsPassword { get; }
    }
}