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

namespace MyMail
{
    public partial class Main : Form
    {
        public static Pop3Client Client;
        public Main()
        {
            Client = Form1.Client;
            InitializeComponent();
        }
        public void updatedata()
        {
            Client = new OpenPop.Pop3.Pop3Client();
            Client.Connect("pop.qq.com", 995, true);
            Client.Authenticate("313793439@qq.com", "oqmjdedmdmktbgeb");
            int mCount = Client.GetMessageCount();
            DataTable mails = new DataTable();
            mails.Columns.Add(new DataColumn("主题", typeof(String)));
            //mails.Columns.Add(new DataColumn("发件人", typeof(String)));
            mails.Columns.Add(new DataColumn("发件邮箱", typeof(String)));
            mails.Columns.Add(new DataColumn("时间", typeof(String)));
            mails.Columns.Add(new DataColumn("内容", typeof(String)));
            mails.Columns.Add(new DataColumn("是否垃圾", typeof(String)));
            //mails.Rows.Add("A","B","C","D","E");
            BindingSource bs = new BindingSource();
            for (int i = mCount; i > mCount - 10; i--)
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
                string path = i + ".txt";
                File.WriteAllText(path, l_strResult);
                string s = Cmd("python module1.py " + path);
                Console.WriteLine(i);
                mails.Rows.Add(zhuti, dizhi, shijan, body, s);
            }
            /* bs.DataSource = mails;
             DataRow dr = mails.NewRow();
             dr["主题"] = "张三";
             dr["发件人"] = "28";
             dr["发件邮箱"] = "85.5";
             dr["时间"] = "s";
             mails.Rows.Add(dr);
             */
            this.dataGridView1.DataSource = mails;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        private void Main_Shown(object sender, EventArgs e)
        {
updatedata();
        }
    }
}
