using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS.Core.Data
{
    public partial class PublishingSystem
    {
        public bool UserOwnsSystem(string userId)
        {
            return PublishingSystemOwners.Any(x => x.UserId == userId);
        }
    }
}
