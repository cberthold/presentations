using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web3WinForm
{
    public partial class Form1 : Form
    {
        NotificationInput input;
        HubConnection connection;
        IHubProxy proxy;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            input = new NotificationInput();
            notificationInputBindingSource.DataSource = input;
            input.From = "WinForms";

            connection = new HubConnection(@"http://localhost:6712/");
            proxy = connection.CreateHubProxy("NotificationHub");
            proxy.On<NotificationOutput>("sendNotification", output =>
            {
                string message = string.Format("From: {0} Message: {1}", output.From, output.Data);
                if (OutputTextBox.InvokeRequired)
                {
                    InvokeDelegate del = ()=> AddMessageToOutput(OutputTextBox, message);
                    OutputTextBox.Invoke(del);
                }
                else { AddMessageToOutput(OutputTextBox, message); }
            });
            connection.Start().Wait();
        }

        delegate void InvokeDelegate();

        private void AddMessageToOutput(TextBox box, string message)
        {
            List<string> lines = box.Lines.ToList();
            lines.Insert(0, message);
            box.Lines = lines.ToArray();
        }

        private void SendToMeButton_Click(object sender, EventArgs e)
        {
            proxy.Invoke("SendToMe", input);
        }

        private void SendToEveryoneButton_Click(object sender, EventArgs e)
        {
            proxy.Invoke("SendToEveryone", input);
        }

        private void JoinExcitingGroupButton_Click(object sender, EventArgs e)
        {
            proxy.Invoke("JoinExcitingGroup", input);
        }

        private void SendToExcitingGroupButton_Click(object sender, EventArgs e)
        {
            proxy.Invoke("SendToExcitingGroup", input);
        }
    }
}
