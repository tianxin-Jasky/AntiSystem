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
using System.IO;
using OpenPop.Mime;
using OpenPop.Common;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace MyMail
{
    public partial class detail : Form
    {
        int mCount = 0;
        int nowCount = 0;
        public static Pop3Client Client;
        ArrayList mails = new ArrayList();
        Dictionary<int ,string> filelist = new Dictionary<int,string>();
        public detail()
        {
            Client = Form1.Client;
            InitializeComponent();
            this.flowLayoutPanel1.AutoScroll = true;
        }
        public void updatedata()
        {
            //Client = new OpenPop.Pop3.Pop3Client();
            //Client.Connect("pop.qq.com", 995, true);
            //Client.Authenticate("313793439@qq.com", "oqmjdedmdmktbgeb");
            mCount = Client.GetMessageCount();
            nowCount = mCount - 10;
            if(nowCount < 0)
                for (int i = mCount; i > 0; i--)
                {
                    OpenPop.Mime.Message message = Client.GetMessage(i);
                    string zhuti = message.Headers.From.DisplayName;
                    string dizhi = message.Headers.From.Address;
                    string shijan = message.Headers.DateSent.ToString();
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
                    string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    //string path = i + ".txt";
                    //File.WriteAllText(path, l_strResult);
                    //string s = Cmd("python module1.py " + path);
                    mail nm = new mail(zhuti, dizhi, shijan, body, message);
                    mails.Add(nm);
                    this.flowLayoutPanel1.Controls.Add(nm);
                    Console.WriteLine(i);
                }
            else for (int i = mCount; i > mCount - 10; i--)
            {
                OpenPop.Mime.Message message = Client.GetMessage(i);
                string zhuti = message.Headers.From.DisplayName;
                string dizhi = message.Headers.From.Address;
                string shijan = message.Headers.DateSent.ToString();
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
                string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","");
                string mainbody = ReplaceHtmlTag(l_strResult);
                //string path = i + ".txt";
                //File.WriteAllText(path, l_strResult);
                //string s = Cmd("python module1.py " + path);
                mail nm = new mail(zhuti,dizhi,shijan,mainbody,message);
                nm.Name = (mCount - i).ToString();
                nm.clicked += new mail.thisclick(showdetail);
                mails.Add(nm);
                this.flowLayoutPanel1.Controls.Add(nm);
                Console.WriteLine(i);
            }
            initbackcolor();
        }
        public void initbackcolor()
        {
            int ts = 0;
            foreach (mail ma in mails){
                if((ts++)%2 == 1)
                ma.setBackcollor(Color.White);
            }
        }
        public void updatedata(int t)
        {
            nowCount -= 10;
            if (nowCount < 0) nowCount = 10;
            for (int i = nowCount; i > nowCount - 10; i--)
            {
                OpenPop.Mime.Message message = Client.GetMessage(i);
                string zhuti = message.Headers.From.DisplayName;
                string dizhi = message.Headers.From.Address;
                string shijan = message.Headers.DateSent.ToString();
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
                string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                //string path = i + ".txt";
                //File.WriteAllText(path, l_strResult);
                //string s = Cmd("python module1.py " + path);
                //mail nm = new mail(zhuti, dizhi, shijan, body);
                //this.flowLayoutPanel1.Controls.Add(nm);
                mail m = (mail)mails[nowCount - i];
                m.reset(zhuti, dizhi, shijan, body,message);
                Console.WriteLine(i);
            }
            mCount = Client.GetMessageCount();
        }
        public void updatedata2(int t)
        {
            mCount = Client.GetMessageCount();
            nowCount += 20;
            if (nowCount > mCount) nowCount = mCount;
            for (int i = nowCount; i > nowCount - 10; i--)
            {
                OpenPop.Mime.Message message = Client.GetMessage(i);
                string zhuti = message.Headers.From.DisplayName;
                string dizhi = message.Headers.From.Address;
                string shijan = message.Headers.DateSent.ToString();
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
                string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                //string path = i + ".txt";
                //File.WriteAllText(path, l_strResult);
                //string s = Cmd("python module1.py " + path);
                //mail nm = new mail(zhuti, dizhi, shijan, body);
                //this.flowLayoutPanel1.Controls.Add(nm);
                mail m = (mail)mails[nowCount - i];
                m.reset(zhuti, dizhi, shijan, body,message);
                Console.WriteLine(i);
            }
        }
        public void reflesh()
        {
            nowCount = mCount;
            for (int i = nowCount; i > nowCount - 10; i--)
            {
                OpenPop.Mime.Message message = Client.GetMessage(i);
                string zhuti = message.Headers.From.DisplayName;
                string dizhi = message.Headers.From.Address;
                string shijan = message.Headers.DateSent.ToString();
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
                string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                string mainbody = ReplaceHtmlTag(l_strResult);
                //string path = i + ".txt";
                //File.WriteAllText(path, l_strResult);
                //string s = Cmd("python module1.py " + path);
                //mail nm = new mail(zhuti, dizhi, shijan, body);
                //this.flowLayoutPanel1.Controls.Add(nm);
                mail m = (mail)mails[nowCount - i];
                m.reset(zhuti, dizhi, shijan, mainbody,message);
                Console.WriteLine(i);
            }
        }
        public string Cmd(string c)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.WriteLine(c);
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("exit");
            StreamReader reader = process.StandardOutput;//截取输出流

            string output = reader.ReadLine();//每次读取一行
            while (!reader.EndOfStream)
            {
                output = reader.ReadLine();
                if (output == "正常邮件") { return "正常邮件"; break; }
                else if (output == "垃圾邮件") { return "垃圾邮件"; break; }
            }
            return "未知";
        }
        public void isRubbish()
        {
            int i = 0;
            foreach (mail ma in mails)
            {
                string path = i + ".txt";
                File.WriteAllText(path, ma.getbody());
                i++;
                string s = Cmd("python module1.py " + path);
                Console.WriteLine(s);
                if (s=="垃圾邮件"||s== "未知")
                {
                    ma.rubbish();
                }
            }
        }
        private void detail_Shown(object sender, EventArgs e)
        {
            updatedata();
            Thread thread = new Thread(isRubbish);
            thread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            updatedata(nowCount);
            Thread thread = new Thread(isRubbish);
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            updatedata2(nowCount);
            Thread thread = new Thread(isRubbish);
            thread.Start();
        }
        private void clear()
        {
            foreach(mail ma in mails)
            {
                ma.disrubbish();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Client.Disconnect();
                Client.Connect(Form1.add, Form1.port, true);
                Client.Authenticate(Form1.username, Form1.password);
                int newCount = Client.GetMessageCount();
                if (newCount > mCount) { MessageBox.Show("收到新邮件"); reflesh(); }
                Console.WriteLine(mCount);
            }
            catch
            {
                Console.WriteLine("更新失败");
            }
        }
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "{[^}]+}", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        private void showdetail(object sender, MyEventArgs e)
        {
            OpenPop.Mime.Message message = e.message;
            maildetail mt = new maildetail(message);
            
            foreach(Control con in mails)
            {
                if(con.Name == e.name)
                {
                    int t =  this.flowLayoutPanel1.Controls.GetChildIndex(con);
                    this.flowLayoutPanel1.Controls.Add(mt);
                    this.flowLayoutPanel1.Controls.SetChildIndex(mt,t+1);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Visible = true;
            this.flowLayoutPanel2.Visible = false;
            this.flowLayoutPanel3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Visible = false;
            this.flowLayoutPanel2.Visible = true;
            this.flowLayoutPanel3.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Visible = false;
            this.flowLayoutPanel2.Visible = false;
            this.flowLayoutPanel3.Visible = true;
        }
    }
}
