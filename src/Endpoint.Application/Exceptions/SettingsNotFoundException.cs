using System;

namespace Endpoint.Application.Exceptions
{
    internal class SettingsNotFoundException: Exception
    {
        public SettingsNotFoundException()
            :base("Settings Not Found.")
        {

        }
    }
}
