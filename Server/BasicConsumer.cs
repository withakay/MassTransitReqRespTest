﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using Messages;

namespace Server
{
    class BasicConsumer : Consumes<BasicRequest>.All
    {
        public event EventHandler<CompletedEventArgs> Completed;
        
        public void Consume(BasicRequest message)
        {
            if (Completed != null)
            {
                Completed(this, new CompletedEventArgs(message.CorrelationId));
            }
        }
    }
}