using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MassTransit;
using Messages;

namespace Server
{
    public partial class Form1 : Form
    {

        private IServiceBus bus;

        public Form1()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text += "Starting...\r\n";

            Type t = typeof(BasicConsumer);

            bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseRabbitMqRouting();
                sbc.ReceiveFrom("rabbitmq://localhost/mtreqresptest_server");
                sbc.SetConcurrentConsumerLimit(1);
            });

            var unsubscribeAction = bus.SubscribeConsumer(t, 
                delegate
                {
                    richTextBox1.Text += "Subscribing consumer\r\n";
                    // in my real app Ninject would resolve this
                    BasicConsumer consumer = new BasicConsumer();

                    // When the handler is done is should raise the Completed event
                    // Handle that here and send a response back to the task that initiated the request.
                    consumer.Completed += (s, args) =>
                    {
                        richTextBox1.Text +=
                            "Consumer completed, CorrelationId is " +
                            args.CorrelationId + "\r\n";

                        var resp = new BasicResponse() { CorrelationId = args.CorrelationId };
                        bus.Context().Respond(resp);
                    };

                    return consumer;
                });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
