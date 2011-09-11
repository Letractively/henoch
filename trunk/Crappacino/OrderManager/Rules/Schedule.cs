using System;

namespace OrderManager.Rules
{
    public class Schedule
    {
        public int NumberOfDaysDelay { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public DateTime NextSendDate { get; set; }
        public bool Enabled { get; set; }

        private Schedule()
        {
            //by design
        }

        public Schedule(TimeSpan startTime, int numberOfDaysDelay)
        {
            // <pex>
            if (922337203685L < 3600L * 24 * numberOfDaysDelay)
                throw new ArgumentException("complex reason", "numberOfDaysDelay");
            // </pex>
            checked
            {
                if (startTime >= TimeSpan.MinValue && startTime <= TimeSpan.MaxValue)
                StartTime = startTime;
                if (numberOfDaysDelay < 1) numberOfDaysDelay = 1;
                NumberOfDaysDelay = numberOfDaysDelay;
            }
        }
        /// <summary>
        /// Everyday timer
        /// </summary>
        /// <param name="pLastExecutedDate"></param>
        /// <returns></returns>
        public bool IsTimeToWork(DateTime pLastExecutedDate)
        {
            bool isTimeToWork;
            checked
            {
                long delay = new TimeSpan(NumberOfDaysDelay * 24, 0, 0).Ticks;
                isTimeToWork = IsTimeToWork(pLastExecutedDate, delay, StartTime);               
            }
            return isTimeToWork;
        }

        private static bool IsTimeToWork(DateTime pLastExecutedDate, long delay, TimeSpan startTime)
        {
            bool isTimeToWork;
            checked
            {
                var now = DateTime.Now;
                var elapsed = now.Ticks - pLastExecutedDate.Ticks;
                var offset = new TimeSpan(now.Date.Ticks - pLastExecutedDate.Ticks).Ticks;

                isTimeToWork = elapsed >= delay ||
                       (elapsed < delay &&
                        startTime.Ticks + offset >= (pLastExecutedDate.TimeOfDay.Ticks + offset) % delay &&
                        startTime.Ticks + offset <= (now.TimeOfDay.Ticks + offset) % delay);                
            }
            return isTimeToWork;
        }
        /// <summary>
        /// Timer for periode longer than 1 day
        /// </summary>
        /// <param name="pSummaryLastDateExecuted"></param>
        /// <returns></returns>
        public bool IsTimeToWork2(DateTime pSummaryLastDateExecuted)
        {
            bool isTimeToWork2 = false;
            checked
            {
                if (pSummaryLastDateExecuted >= DateTime.MinValue &&
                    pSummaryLastDateExecuted <= DateTime.MaxValue)
                {
                    DateTime now = DateTime.Now;
                    var timeElasped = DateTime.Now.Ticks - pSummaryLastDateExecuted.Ticks;
                    var delay = new TimeSpan(NumberOfDaysDelay * 24, 0, 0).Ticks;

                    isTimeToWork2 = timeElasped >= delay || IsGreater(StartTime, now) &&
                                         !pSummaryLastDateExecuted.AddDays(NumberOfDaysDelay).Date.Equals(now.Date);                
                }
}

            return isTimeToWork2;
        }

        public bool IsTimeToWorkOld(DateTime pSummaryLastDateExecuted)
        {
            bool doWork = false;

            // Step 1: Check if Summary scan is enabled or not 
            if (Enabled)
            {
                // Step 2:Check if scan has not been conducted already
                if (pSummaryLastDateExecuted.Date < DateTime.Now.Date)
                {
                    DateTime _CurrentDate = DateTime.Now;

                    // Step 3: Check if Summary should be sent today
                    //If next date is the current date
                    //OR: If a summary has never ever been sent, the next date is today
                    if ((NextSendDate == _CurrentDate) || (pSummaryLastDateExecuted == DateTime.MinValue))
                    {
                        //Step 4: Compare the time
                        Int32 _Result = TimeSpan.Compare(_CurrentDate.TimeOfDay, StartTime);

                        if (_Result >= 0)
                        {
                            doWork = true;
                            NextSendDate = pSummaryLastDateExecuted;
                        }
                    }
                }
            }
            return doWork;
        }

        public static bool IsGreater(TimeSpan mDate, DateTime date)
        {
            var res = TimeSpan.Compare(date.TimeOfDay, mDate);
            return res >= 0;
        }
    }
}
