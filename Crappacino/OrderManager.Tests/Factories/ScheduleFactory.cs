// <copyright file="ScheduleFactory.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>

using System;
using Microsoft.Pex.Framework;
using OrderManager.Rules;

namespace OrderManager.Rules
{
    /// <summary>A factory for OrderManager.Rules.Schedule instances</summary>
    public static partial class ScheduleFactory
    {
        /// <summary>A factory for OrderManager.Rules.Schedule instances</summary>
        [PexFactoryMethod(typeof(Schedule))]
        public static Schedule Create(
            TimeSpan startTime_ts,
            int numberOfDaysDelay_i,
            DateTime value_dt,
            bool value_b
        )
        {
            Schedule schedule = new Schedule(startTime_ts, numberOfDaysDelay_i);
            schedule.NextSendDate = value_dt;
            schedule.Enabled = value_b;
            return schedule;

            // TODO: Edit factory method of Schedule
            // This method should be able to configure the object in all possible ways.
            // Add as many parameters as needed,
            // and assign their values to each field by using the API.
        }
    }
}
