namespace Server
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using MassTransit;

    public partial class Form1 : Form
    {
        IServiceBus bus;
        SynchronizationContext _context;

        public Form1()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
        }

        void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text += "Starting...\r\n";

            _context = SynchronizationContext.Current;

            bus = ServiceBusFactory.New(sbc =>
                {
                    sbc.UseRabbitMqRouting();
                    sbc.ReceiveFrom("rabbitmq://localhost/mtreqresptest_server");
                    sbc.SetConcurrentConsumerLimit(1);

                    sbc.Subscribe(sc =>
                        {
                            sc.Consumer(() =>
                                {
                                    var consumer = new BasicConsumer();
                                    consumer.Completed += ConsumerCompletedCallback;


                                    return consumer;
                                });
                        });
                });

            richTextBox1.Text += "Started.\r\n";
        }

        void ConsumerCompletedCallback(object source, CompletedEventArgs args)
        {
            Guid correlationId = args.CorrelationId;

            // you are on a MT thread, so you have to post back to your UI thread here or get an InvalidOperationException
            // for cross-thread operations
            _context.Post(
                _ => { richTextBox1.Text += string.Format("Consumer completed, CorrelationId is {0}\r\n", correlationId); }, null);
        }

        void label1_Click(object sender, EventArgs e)
        {
        }
    }
}