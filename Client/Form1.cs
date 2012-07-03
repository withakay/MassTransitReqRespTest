namespace Client
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using MassTransit;

    public partial class Form1 : Form
    {
        Task _task = new Task();
        SynchronizationContext _context;

        public Form1()
        {
            InitializeComponent();

            _context = SynchronizationContext.Current;

            Bus.Initialize(sbc =>
                {
                    sbc.UseRabbitMqRouting(); //Calling this now implies that RabbitMQ will be used
                    sbc.ReceiveFrom("rabbitmq://localhost/mtreqresptest_client");
                });
            _task.Published += TaskOnPublished;
            _task.ResponseHandled += TaskOnResponseHandled;
        }

        void TaskOnPublished(object sender, EventArgs eventArgs)
        {
            _context.Post(_ => { richTextBox1.Text += "Message published...\r\n"; }, null);
        }

        void TaskOnResponseHandled(object sender, EventArgs e)
        {
            _context.Post(_ => { richTextBox1.Text += "Response handled...\r\n"; }, null);
        }

        void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Starting...\r\n";

            _task.Execute(Bus.Instance);
        }
    }
}