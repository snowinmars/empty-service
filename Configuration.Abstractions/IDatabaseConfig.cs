using System;
using System.Security;

namespace EmptyService.Configuration.Abstractions
{
    public interface IDatabaseConfig
    {
        Uri Host { get; }

        int Port { get; }

        SecureString Username { get; }

        SecureString Password { get; }

        SecureString DatabaseName { get; }
    }
}