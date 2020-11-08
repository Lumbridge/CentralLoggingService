using CLS.Core.Data;
using System.Collections.Generic;

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

        public enum SeverityType
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        public static List<Severity> Severities = new List<Severity>
        {
            new Severity { Id = 1, Name = "Debug", Code = "D" },
            new Severity { Id = 2, Name = "Info", Code = "I" },
            new Severity { Id = 3, Name = "Warn", Code = "W" },
            new Severity { Id = 4, Name = "Error", Code = "E" },
            new Severity { Id = 5, Name = "Fatal", Code = "F" },
        };
    }
}
