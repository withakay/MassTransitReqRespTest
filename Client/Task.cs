using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magnum.Extensions;
using MassTransit;
using Messages;

namespace Client
{
    public class Task
    {
        public event EventHandler<EventArgs> ResponseHandled;
        public event EventHandler<EventArgs> Published;
        public bool Enabled { get; set; }
        private IServiceBus _serviceBus;

        public void Execute(IServiceBus bus)
        {
            _serviceBus = bus;
            
            Enabled = true;

            PublishNext();
        }

        public void Cancel()
        {
            this.Enabled = false;
        }

        public void PublishNext()
        {
            if (!Enabled)
                return;

            var message = new BasicRequest {CorrelationId = Guid.NewGuid()};

            if (Published != null) Published(this, null);

            _serviceBus.PublishRequest(message, rc =>
            {
                rc.SetTimeout(30.Seconds());
                rc.Handle<BasicResponse>(x =>
                {
                    if (ResponseHandled != null) ResponseHandled(this, null);

                    System.Threading.Thread.Sleep(1000);
                });
            });

            PublishNext();
        }
    }
}
