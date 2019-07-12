using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMail
{
    public partial class send : Form
    {
        public send()
        {
            InitializeComponent();
           
        }
        public void SendMailUse()
        {
            string host = "";
            if (Form1.add == "pop.qq.com")
            {
                host = "smtp.qq.com";
            }
            else host = "smtp.163.com";// 邮件服务器smtp.163.com表示网易邮箱服务器    
            string userName = Form1.username;// 发送端账号   
            string password = Form1.password;// 发送端密码(这个客户端重置后的密码)
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = host;//邮件服务器
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, password);//用户名、密码
            client.EnableSsl = true;
            //////////////////////////////////////
            string strfrom = userName;
            string strto = textBox1.Text;
            string subject = textBox2.Text;//邮件的主题             
            string body = richTextBox1.Text; ;//发送的邮件正文  

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(strfrom, "xyf");
            msg.To.Add(strto);
            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   


            try
            {
                client.Send(msg);
                Console.WriteLine("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message, "发送邮件出错");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMailUse();
        }
    }
}
