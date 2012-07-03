using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class CompletedEventArgs : EventArgs
    {
        public Guid CorrelationId { get; set; }

        public CompletedEventArgs(Guid cid)
        {
            CorrelationId = cid;
        }
    }
}
