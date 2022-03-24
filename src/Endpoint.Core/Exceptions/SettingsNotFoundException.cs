using System;

namespace Endpoint.Core.Exceptions
{
    public class SettingsNotFoundException : Exception
    {
        public SettingsNotFoundException()
            : base("Settings Not Found.")
        {

        }
    }
}
