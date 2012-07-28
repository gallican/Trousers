using System;

namespace Trousers.Core.Infrastructure.Settings
{
    public interface ISettings
    {
        Uri TfsUri { get; }
    }
}