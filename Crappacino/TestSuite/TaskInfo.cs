namespace UTS.AMS.TestSuite
{
    /// <summary>
    /// Taskduration uses DateTime.
    /// Taskduration2 uses Stopwatch.
    /// </summary>
    public struct TaskInfo
    {
        public long Taskduration
        {
            get
            {
                return TaskEnded - TaskStarted;
            }
        }
        public long Taskduration2
        {
            get
            {
                return TaskEnded2 - TaskStarted2;
            }
        }
        public string TaskDescription;

        public long TaskEnded;
        public long TaskStarted;

        public long TaskStarted2;
        public long TaskEnded2;
    }
}