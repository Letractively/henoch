using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ApplicationTypes.DesignPatterns
{
    public class MySession
    {
        public bool TaskQIsEmtpty { get; set; }
        public bool HasWaitingConsumer { get; set; }
        public bool HasWaitingProducer { get; set; }
        public string SessionId { get; set; }
        public Thread ProducerThread { get; set; }
        public Thread ConsumerThread { get; set; }
    }
}
