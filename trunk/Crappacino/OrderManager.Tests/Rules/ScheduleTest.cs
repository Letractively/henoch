// <copyright file="ScheduleTest.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManager.Rules;

namespace OrderManager.Rules
{
    /// <summary>This class contains parameterized unit tests for Schedule</summary>
    [PexClass(typeof(Schedule))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ScheduleTest
    {
        /// <summary>Test stub for .ctor(TimeSpan, Int32)</summary>
        [PexMethod]
        public Schedule Constructor(TimeSpan startTime, int numberOfDaysDelay)
        {
            Schedule target = new Schedule(startTime, numberOfDaysDelay);
            return target;
            // TODO: add assertions to method ScheduleTest.Constructor(TimeSpan, Int32)
        }

        /// <summary>Test stub for IsGreater(TimeSpan, DateTime)</summary>
        [PexMethod]
        public bool IsGreater(TimeSpan mDate, DateTime date)
        {
            bool result = Schedule.IsGreater(mDate, date);
            return result;
            // TODO: add assertions to method ScheduleTest.IsGreater(TimeSpan, DateTime)
        }

        /// <summary>Test stub for IsTimeToWork(DateTime)</summary>
        [PexMethod, PexAllowedException(typeof(OverflowException))]
        public bool IsTimeToWork([PexAssumeUnderTest]Schedule target, DateTime pLastExecutedDate)
        {
            bool result = target.IsTimeToWork(pLastExecutedDate);
            return result;
            // TODO: add assertions to method ScheduleTest.IsTimeToWork(Schedule, DateTime)
        }

        /// <summary>Test stub for IsTimeToWork2(DateTime)</summary>
        [PexMethod]
        public bool IsTimeToWork2(
            [PexAssumeUnderTest]Schedule target,
            DateTime pSummaryLastDateExecuted
        )
        {
            bool result = target.IsTimeToWork2(pSummaryLastDateExecuted);
            return result;
            // TODO: add assertions to method ScheduleTest.IsTimeToWork2(Schedule, DateTime)
        }
        [PexMethod]
        public bool IsTimeToWorkOld([PexAssumeUnderTest]Schedule target, DateTime pSummaryLastDateExecuted)
        {
            bool result = target.IsTimeToWorkOld(pSummaryLastDateExecuted);
            return result;
            // TODO: add assertions to method ScheduleTest.IsTimeToWorkOld(Schedule, DateTime)
        }
    }
}
