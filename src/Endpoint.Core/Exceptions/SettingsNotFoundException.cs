using System;

namespace Endpoint.Core.Exceptions
{
    internal class SettingsNotFoundException : Exception
    {
        public SettingsNotFoundException()
            : base("Settings Not Found.")
        {

        }
    }
}
