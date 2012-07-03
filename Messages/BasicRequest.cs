using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;

namespace Messages
{
    public class BasicRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
