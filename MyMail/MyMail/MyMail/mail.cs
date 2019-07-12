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
    public partial class mail : UserControl
    {
        public delegate void thisclick(object sender, MyEventArgs e);
        public event thisclick clicked;
        public event thisclick deleted;
        public event thisclick toblacklist;
        public event thisclick torubox;
        public OpenPop.Mime.Message message;
        public Mymessage mymessage;
        public bool isdetail = false;
        public mail()
        {
            InitializeComponent();
            this.textBox1.HideSelection = false;
        }
        public mail(string name, string add, string time, string body, OpenPop.Mime.Message message)
        {
            InitializeComponent();
            this.textBox1.Text = name;
            this.textBox2.Text = add;
            this.textBox3.Text = time;
            this.textBox4.Text = body;
            this.message = message;
        }
        public void reset(string name, string add, string time, string body, OpenPop.Mime.Message message)
        {
            this.textBox1.Text = name;
            this.textBox2.Text = add;
            this.textBox3.Text = time;
            this.textBox4.Text = body;
            this.message = message;
        }
        public void reset(string name, string add, string time, string body, Mymessage message)
        {
            this.Visible = true;
            this.textBox1.Text = name;
            this.textBox2.Text = add;
            this.textBox3.Text = time;
            this.textBox4.Text = body;
            this.mymessage = message;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void mail_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            MyEventArgs mye = new MyEventArgs(mymessage,this.Name);
            clicked(sender, mye);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            ActiveControl = this.button1;
        }
        public void setBackcollor(Color color)
        {
            this.BackColor = color;
            this.textBox1.BackColor = color;
            this.textBox2.BackColor = color;
            this.textBox3.BackColor = color;
            this.textBox4.BackColor = color;
        }
        delegate void rubbishcallback();
        public void rubbish()
        {
            if (this.label1.InvokeRequired)
            {
                rubbishcallback ru = new rubbishcallback(rubbish);
                this.Invoke(ru);

            }
            else
                this.label1.Visible = true;
        }
        public void disrubbish()
        {
            if (this.label1.InvokeRequired)
            {
                rubbishcallback ru = new rubbishcallback(disrubbish);
                this.Invoke(ru);

            }
            else
                this.label1.Visible = false;
        }
        public string getbody()
        {
            return this.textBox4.Text;
        }

        private void 删除邮件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MyEventArgs mye = new MyEventArgs(mymessage, this.Name);
            deleted(sender, mye);
            Console.WriteLine("de");
        }

        private void 将用户加入黑名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyEventArgs mye = new MyEventArgs(mymessage, this.Name);
            toblacklist(sender, mye);
        }

        private void 放入垃圾箱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MyEventArgs mye = new MyEventArgs(mymessage, this.Name);
            torubox(sender, mye);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    public class MyEventArgs : EventArgs
    {
        public Mymessage message;
        public string name;
        public MyEventArgs(Mymessage message,string name)
        {
            this.name = name;
            this.message = message;
        }
    }
}
