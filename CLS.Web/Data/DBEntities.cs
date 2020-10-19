using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;

namespace CLS.Web.Data
{
    public class DBEntities : CLSDbEntities, IEntities
    {
    }
}