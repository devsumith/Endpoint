using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.SharedKernal.Models
{
    public enum PersistenceStrategyType
    {
        EntityFrameworkCore,
        Dapper
    }
}
