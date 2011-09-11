using System;

namespace OrderManager.Rules
{
    public class Schedule
    {
        public int NumberOfDaysDelay;
        public TimeSpan StartTime;

        private Schedule()
        {
            //by design
        }

        public Schedule(TimeSpan startTime, int numberOfDaysDelay)
        {
            StartTime = startTime;
            NumberOfDaysDelay = numberOfDaysDelay;
        }
        /// <summary>
        /// Everyday timer
        /// </summary>
        /// <param name="pLastExecutedDate"></param>
        /// <returns></returns>
        public bool IsTimeToWork(DateTime pLastExecutedDate)
        {
            long delay = new TimeSpan(NumberOfDaysDelay * 24, 0, 0).Ticks;
            return IsTimeToWork(pLastExecutedDate, delay, StartTime);
        }

        private static bool IsTimeToWork(DateTime pLastExecutedDate, long delay, TimeSpan startTime)
        {
            var now = DateTime.Now;
            var elapsed = now.Ticks - pLastExecutedDate.Ticks;
            var offset = new TimeSpan(now.Date.Ticks - pLastExecutedDate.Ticks).Ticks;

            return elapsed >= delay || 
                   (elapsed < delay && 
                    startTime.Ticks + offset >= (pLastExecutedDate.TimeOfDay.Ticks + offset) % delay && 
                    startTime.Ticks + offset <= (now.TimeOfDay.Ticks + offset) % delay);
        }
        /// <summary>
        /// Timer for periode longer than 1 day
        /// </summary>
        /// <param name="pSummaryLastDateExecuted"></param>
        /// <returns></returns>
        public bool IsTimeToWork2(DateTime pSummaryLastDateExecuted)
        {

            DateTime now = DateTime.Now;
            var timeElasped = DateTime.Now.Ticks - pSummaryLastDateExecuted.Ticks;
            long delay = new TimeSpan(NumberOfDaysDelay * 24, 0, 0).Ticks;

            return timeElasped >= delay || IsGreater(StartTime, now) &&
                    !pSummaryLastDateExecuted.AddDays(NumberOfDaysDelay).Date.Equals(now.Date);
        }


        public static bool IsGreater(TimeSpan mDate, DateTime date)
        {
            var res = TimeSpan.Compare(date.TimeOfDay, mDate);
            return res >= 0;
        }
    }
}
