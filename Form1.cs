using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using Amazon.SQS;
using Amazon.SQS.Model;

namespace TEST_AWS_SNS
{
    public partial class Form1 : Form
    {
        private bool exit = false;
        private List<string> messages = new List<string>();
        //Chenge here to your SQS list name
        private string QueueName = "newseries2"; 

        public Form1()
        {
            InitializeComponent();
        }     

        private void readMessages()
        {            
            while (!exit)
            {
                var request = new ListQueuesRequest
                {
                    QueueNamePrefix = QueueName
                };
                var client = new AmazonSQSClient();
                var response = client.ListQueues(request);
                var url = response.ListQueuesResult.QueueUrl;

                var receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.MaxNumberOfMessages = 5;
                receiveMessageRequest.QueueUrl = url[0];

                var receiveMessageResponse = client.ReceiveMessage(receiveMessageRequest);

                foreach (var message in receiveMessageResponse.ReceiveMessageResult.Message)
                {
                    //If we want to prevent dispay the same message again.
                    //if(!messages.Contains(message.MessageId))
                    //{
                        MessageBox.Show(message.Body);
                    //Addin the message to the list
                    //    this.messages.Add(message.MessageId);
                    //}                   
                }
                //Sleeping time
                System.Threading.Thread.Sleep(5000);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
                exit = false;
                this.richTextBox1.Text = "Checking ...";
                var thread = new System.Threading.Thread(readMessages);
                thread.Start();                                        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "Stopped!\n";
            exit = true;
        }
    }
}
