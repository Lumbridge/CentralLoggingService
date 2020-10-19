using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS.Core.StaticData
{
    public static class StaticData
    {
        public enum EnvironmentType
        {
            DEV,
            SIT,
            UAT,
            LIVE
        }

        public enum SystemType
        {
            REST,
            SOAP,
            Website,
            WindowsService,
            ConsoleApplication,
            Other
        }
    }
}
