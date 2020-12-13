using CLS.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CLS.WindowsService.Tests
{
    [TestClass]
    public class ProcessorTests
    {
        protected List<Log> TestLogs { get; set; }

        [TestInitialize]
        public void StartUp()
        {
            //TestLogs = new List<Log>
            //{
            //    new Log
            //    {
            //        SeverityId = 4,
            //        Severity = new Severity {Id = 4, Code = "E", Name = "Error"},
            //        PublishingSystemId = 1,
            //        PublishingSystem = new PublishingSystem
            //        {
            //            Id = 1,
            //            EnvironmentType = new EnvironmentType {Id = 1, Code = "D", Name = "DEV"},
            //            EnvironmentTypeId = 1,
            //            PublishingSystemTypeId = 1002,
            //            PublishingSystemType = new PublishingSystemType {Code = "C", Id = 1002, Name = "ConsoleApplication"},
            //            Name = "CLS.SenderConsole"
            //        },
            //        Timestamp = new DateTime(2020, 6, 9, 10, 30, 00),
            //        Message = "This is a test.",
            //        Exception = null,
            //        StackTrace = null
            //    }
            //};
        }

        [TestMethod]
        public void Should_Find_Logs_With_Severity_Error_Using_Expression()
        {
            // Arrange
            var expression = $"Severity.Name==\"Error\"";

            // Act
            var foundMessage = TestLogs.AsQueryable().Where(expression).Any();
            
            // Assert
            Assert.IsTrue(foundMessage);
        }

        [TestMethod]
        public void Should_Compare_Timestamp_TimeOfDay_String()
        {
            // Arrange
            var testTimeStringLower = "09:00";
            var testTimeStringUpper = "11:00";
            var expression = $"Timestamp.TimeOfDay>=TimeSpan.Parse(\"{testTimeStringLower}\")&&Timestamp.TimeOfDay<=TimeSpan.Parse(\"{testTimeStringUpper}\")";

            // Act
            var foundMessage = TestLogs.AsQueryable().Where(expression).Any();

            // Assert
            Assert.IsTrue(foundMessage);
        }

        [TestMethod]
        public void Should_Compare_Timestamp_DayOfWeek_String()
        {
            // Arrange
            var dayOfWeekString = "Tuesday";
            var expression = $"Timestamp.DayOfWeek.ToString()==\"{dayOfWeekString}\"";

            // Act
            var foundMessage = TestLogs.AsQueryable().Where(expression).Any();

            // Assert
            Assert.IsTrue(foundMessage);
        }
    }
}
