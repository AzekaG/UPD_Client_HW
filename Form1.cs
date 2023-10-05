using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace UPD_Client_HW
{
    public partial class Form1 : Form
    {
        Controller controller;
        SynchronizationContext uiContect;
        public Form1()
        {
            InitializeComponent();
            controller = new Controller();
            uiContect = new SynchronizationContext();
            controller.AnswerToForm += AddNewMEssageToListBox;

        }

        void AddNewMEssageToListBox(string message)
        {
            listBox1.Items.Add(message);
        }


        async void  SendMessage()
        {
            string message = textBox1.Text;
            await Task.Run(() =>
            {
                uiContect.Send(x => listBox1.Items.Add(controller.SendMessage(message)), null);
            });
        }


        private void btn_Send_Click(object sender, EventArgs e)
        {
           if (textBox1.Text!="")
            {
                SendMessage();
            }

            textBox1.Text = "";
        }
    }
}
