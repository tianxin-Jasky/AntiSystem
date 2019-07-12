using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMail
{
    public partial class maildetail : UserControl
    {
        public maildetail()
        {
            InitializeComponent();
        }
        public maildetail(OpenPop.Mime.Message message)
        {
            InitializeComponent();
            this.label1.Text = message.Headers.From.DisplayName;
            this.label3.Text = message.Headers.From.Address;
            this.label2.Text = message.Headers.DateSent.ToString();
            OpenPop.Mime.MessagePart messagePart = message.MessagePart;
            string body = " ";
            if (messagePart.IsText)
            {
                body = messagePart.GetBodyAsText();
            }
            else if (messagePart.IsMultiPart)
            {
                OpenPop.Mime.MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                if (plainTextPart != null)
                {
                    body = plainTextPart.GetBodyAsText();
                }
                else
                {
                    List<OpenPop.Mime.MessagePart> textVersions = message.FindAllTextVersions();
                    if (textVersions.Count >= 1)
                        body = textVersions[0].GetBodyAsText();
                    else
                        body = "<<OpenPop>> Cannot find a text version body in this message.";
                }
            }
            this.webBrowser1.DocumentText = body;
        }

        public maildetail(Mymessage message)
        {
            InitializeComponent();
            this.label1.Text = message.displayname;
            this.label3.Text = message.address;
            this.label2.Text = message.time;
            this.webBrowser1.DocumentText = message.body;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
