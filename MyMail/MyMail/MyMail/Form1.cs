using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenPop.Pop3;
using OpenPop.Mime;
using OpenPop.Common;

namespace MyMail
{
    public partial class Form1 : Form
    {
        public static Pop3Client Client;
        public Form1()
        {
            InitializeComponent();
        }
        int whichmail = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.textBox1.Visible = true;
            this.textBox2.Visible = true;
            this.button3.Visible = true;
            this.label2.Visible = true;
            this.label3.Visible = true;
            this.label1.Text = "欢迎登录qq邮箱";
            whichmail = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.textBox1.Visible = true;
            this.textBox2.Visible = true;
            this.button3.Visible = true;
            this.label2.Visible = true;
            this.label3.Visible = true;
            this.label1.Text = "欢迎登录163邮箱";
            whichmail = 2;
        }
        public static string add = "";
        public static int port = 0;
        public static string username = "";
        public static string password = "";
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                switch (whichmail)
                {
                    case 1:
                        add = "pop.qq.com";
                        port = 995;
                        break;
                    case 2:
                        add = "pop.163.com";
                        port = 995;
                        break;
                }
                if(this.textBox1.Text != ""&& this.textBox2.Text != "")
                {
                    username = this.textBox1.Text;
                    password = this.textBox2.Text;
                }
                Client = new Pop3Client();
                Client.Connect(add, port, true);
                Client.Authenticate(username,password); 
                string cpath = Application.StartupPath;
                string path = cpath + "/" +username;
                if(false == System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                this.DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("登录失败");
            }
        }
    }
}
