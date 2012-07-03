using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Magnum.Extensions;
using MassTransit;
using Messages;

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
            });
            _task.Published += TaskOnPublished;
            _task.ResponseHandled += TaskOnResponseHandled;
        }

        private void TaskOnPublished(object sender, EventArgs eventArgs)
        {
            MessageBox.Show("Message Published");
        }

        void TaskOnResponseHandled(object sender, EventArgs e)
        {
            MessageBox.Show("Response handled");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "Starting...\r\n";
            _task.Execute(Bus.Instance);
        }
    }
}
