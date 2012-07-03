namespace Server
{
    using System;
    using MassTransit;
    using Messages;

    class CompletedEventArgs : EventArgs
    {
        public CompletedEventArgs(Guid cid)
        {
            CorrelationId = cid;
        }

        public Guid CorrelationId { get; set; }
    }

    class BasicConsumer : 
        Consumes<BasicRequest>.Context
    {
        public void Consume(IConsumeContext<BasicRequest> context)
        {
            if (Completed != null)
            {
                Completed(this, new CompletedEventArgs(context.Message.CorrelationId));
            }

            var response = new BasicResponse {CorrelationId = context.Message.CorrelationId};

            context.Respond(response);
        }

        public event EventHandler<CompletedEventArgs> Completed;
    }
}