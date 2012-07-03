using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;

namespace Messages
{
    public class BasicResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
