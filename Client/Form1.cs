using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MassTransit;

namespace Client
{
    public partial class Form1 : Form
    {

        private Task _task = new Task();

        public Form1()
        {
            InitializeComponent();

            Bus.Initialize(sbc =>
            {
                sbc.UseRabbitMqRouting(); //Calling this now implies that RabbitMQ will be used
                sbc.ReceiveFrom("rabbitmq://localhost/mtreqresptest_client");
                sbc.SetConcurrentConsumerLimit(1);

            });

            _task.Published += TaskOnPublished;
            _task.ResponseHandled += TaskOnResponseHandled;
        }

        private void TaskOnPublished(object sender, EventArgs eventArgs)
        {
            MessageBox.Show("Publish");
            //richTextBox1.Text += "Published\r\n";
        }

        void TaskOnResponseHandled(object sender, EventArgs e)
        {
            MessageBox.Show("Response");
            //richTextBox1.Text += "Response handled\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Starting...\r\n";
            _task.Execute(Bus.Instance);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Stopping...\r\n";
            _task.Cancel();
        }
    }
}
