using System;

namespace Endpoint.SharedKernal.Exceptions
{
    internal class SettingsNotFoundException : Exception
    {
        public SettingsNotFoundException()
            : base("Settings Not Found.")
        {

        }
    }
}
