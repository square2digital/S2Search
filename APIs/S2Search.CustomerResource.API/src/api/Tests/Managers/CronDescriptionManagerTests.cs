using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces.Managers;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Managers.Tests
{
    [TestClass()]
    public class CronDescriptionManagerTests
    {
        private ICronDescriptionManager _systemUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _systemUnderTest = new CronDescriptionManager();
        }

        [TestMethod()]
        public void Get5MinutesDescriptionTest()
        {
            string cronExpression = "0 */5 * ? * *";

            string description = _systemUnderTest.Get(cronExpression);

            Assert.IsTrue(description == "Every 5 minutes");
        }

        [TestMethod()]
        public void Get30MinutesDescriptionTest()
        {
            string cronExpression = "0 */30 * ? * *";

            string description = _systemUnderTest.Get(cronExpression);

            Assert.IsTrue(description == "Every 30 minutes");
        }

        [TestMethod()]
        public void FailureTest()
        {
            string cronExpression = "0 */60 * ? * *";

            string description = _systemUnderTest.Get(cronExpression);

            Assert.IsFalse(description == "Every minute");
        }
    }
}