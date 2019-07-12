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
        int firstCount = 0;
        public static Pop3Client Client;
        ArrayList mails = new ArrayList();
        ArrayList rumails = new ArrayList();
        ArrayList demails = new ArrayList();
        ArrayList blacklist = new ArrayList();
        Dictionary<int ,string> filelist = new Dictionary<int,string>();
        Dictionary<int, string> rufilelist = new Dictionary<int, string>();
        Dictionary<int, string> defilelist = new Dictionary<int, string>();
        int tmax = 0;
        int rmax = 0;
        int dmax = 0;
        public detail()
        {
            tmax = mailsaved.loaddic(ref filelist);
            rmax = mailsaved.loadrudic(ref rufilelist);
            dmax = mailsaved.loaddedic(ref defilelist);
            mailsaved.loadblacklist(ref blacklist);
            Client = Form1.Client;
            InitializeComponent();
            this.flowLayoutPanel1.AutoScroll = true;
        }
        public void updatedata()
        {
            //Client = new OpenPop.Pop3.Pop3Client();
            //Client.Connect("pop.qq.com", 995, true);
            //Client.Authenticate("313793439@qq.com", "oqmjdedmdmktbgeb");
            int s = 0;
            mCount = Client.GetMessageCount();
            firstCount = mCount;
            nowCount = mCount - 15;
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
                    //string t = i + getchar(4);
                    //mailsave ma = new mailsave(message, t);
                    //filelist.Add(i, t);
                    //mail nm = new mail(zhuti, dizhi, shijan, body, message);
                    //mails.Add(nm);
                    //this.flowLayoutPanel1.Controls.Add(nm);
                    mail m = (mail)mails[i];
                    m.reset(zhuti, dizhi, shijan, body,new Mymessage(message));
                    Console.WriteLine(zhuti);
                }
            else for (int i = mCount,j = 0; (j < 15)||(i > mCount - 15); i--,j++)
            {
                Op:
                    if (!filelist.ContainsKey(i))
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
                        string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                        string mainbody = ReplaceHtmlTag(l_strResult);
                        string path = i + ".txt";
                        File.WriteAllText(path, l_strResult);
                        string sc = Cmd("python module1.py " + path);
                        mail m = (mail)mails[j];
                        string t = getname(zhuti, dizhi, shijan, body);
                        filelist.Add(i, t);
                        if(sc == "垃圾邮件"||blacklist.Contains(dizhi))
                        {
                            rufilelist.Add(++rmax, t);
                            mailsaved.saverudic(rufilelist);
                            mailsaved.savemail(new mailsave(message, body), t);
                            i--;
                            goto Op;
                        }
                       
                        m.reset(zhuti, dizhi, shijan, body,new Mymessage(message));
                        
                        //string t = i + zhuti.Substring(0,1)+dizhi.Substring(1,2)+shijan.Substring(8,2);
                        mailsave ma = new mailsave(message, t);
                        
                        mailsaved.savemail(new mailsave(message, body), t);
                        //mail nm = new mail(zhuti,dizhi,shijan,mainbody,message);
                        //nm.Name = (mCount - i).ToString();
                        //nm.clicked += new mail.thisclick(showdetail);
                        //mails.Add(nm);
                        //this.flowLayoutPanel1.Controls.Add(nm);
                        Console.WriteLine(i);
                    }
                    else
                    {
                        mailsave ms;
                        Eop:
                        if (!filelist.ContainsKey(tmax - s)) {
                            OpenPop.Mime.Message message = Client.GetMessage(tmax - s);
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
                            string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                            string mainbody = ReplaceHtmlTag(l_strResult);
                            string t = getname(zhuti, dizhi, shijan, body); mailsave ma = new mailsave(message, t);
                            filelist.Add(tmax - s, t);
                            mailsaved.savemail(new mailsave(message, body), t);
                        };
                        ms = mailsaved.loadmail(filelist[tmax-s]);
                        s++;
                        mail m = (mail)mails[j];
                        if(defilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body)) || rufilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body))|| blacklist.Contains(ms.address))
                        {
                            
                            nowCount--;
                            Console.WriteLine("as");

                            goto Eop;
                        }
                        m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                        Console.WriteLine(ms.displayname);
                        Console.WriteLine(i.ToString());
                        Console.WriteLine(s.ToString());
                    }
            }
            mailsaved.savedic(filelist);
        }
        public string getname(string zhuti,string dizhi,string shijan,string body)
        {
            string t = zhuti.Count() + dizhi.Count() + shijan.Replace("/","").Replace(":","").Replace(" ","") + body.Count();
            return t;
        }
        public void initbackcolor()
        {
            int ts = 0;
            foreach (mail ma in mails){
                if((ts++)%2 == 1)
                ma.setBackcollor(Color.White);
            }
        }
        public void updatedata(int sss)
        {
            Console.WriteLine("asda"+nowCount);
            int s = 0;
            int f = 0;
            firstCount = nowCount;
            if (nowCount -15 < 0) nowCount = 15;
            for (int i = nowCount,k = 0;k < 15 ||i > nowCount - 15; i-- , k++)
            {   Op:
                if (!filelist.ContainsKey(i))
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
                    string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                    string mainbody = ReplaceHtmlTag(l_strResult);
                    string path = i + ".txt";
                    File.WriteAllText(path, l_strResult);
                    string sc = Cmd("python module1.py " + path);
                    mail m = (mail)mails[k];
                    string t = getname(zhuti, dizhi, shijan, body);
                    filelist.Add(i, t);
                    if (sc == "垃圾邮件" || blacklist.Contains(dizhi))
                    {
                        rufilelist.Add(++rmax, t);
                        mailsaved.saverudic(rufilelist);
                        mailsaved.savemail(new mailsave(message, body), t);
                        i--;
                        goto Op;
                    }

                    m.reset(zhuti, dizhi, shijan, body, new Mymessage(message));

                    //string t = i + zhuti.Substring(0,1)+dizhi.Substring(1,2)+shijan.Substring(8,2);
                    mailsave ma = new mailsave(message, t);

                    mailsaved.savemail(new mailsave(message, body), t);
                }
                else
                {
                    mailsave ms;
                    Eop:
                    if (!filelist.ContainsKey(i))
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
                        string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                        string mainbody = ReplaceHtmlTag(l_strResult);
                        string path = i + ".txt";
                        File.WriteAllText(path, l_strResult);
                        string sc = Cmd("python module1.py " + path);
                        mail m2 = (mail)mails[k];
                        string t = getname(zhuti, dizhi, shijan, body);
                        filelist.Add(i, t);
                        if (sc == "垃圾邮件" || blacklist.Contains(dizhi))
                        {
                            rufilelist.Add(++rmax, t);
                            mailsaved.saverudic(rufilelist);
                            mailsaved.savemail(new mailsave(message, body), t);
                            i--;
                            goto Eop;
                        }

                        m2.reset(zhuti, dizhi, shijan, body, new Mymessage(message));

                        //string t = i + zhuti.Substring(0,1)+dizhi.Substring(1,2)+shijan.Substring(8,2);
                        mailsave ma = new mailsave(message, t);
                        mailsaved.savemail(new mailsave(message, body), t);
                    };
                    ms = mailsaved.loadmail(filelist[i]);
                    s++;
                    mail m = (mail)mails[k];
                    if (defilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body)) || rufilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body)) || blacklist.Contains(ms.address))
                    {

                        nowCount--;
                        i--;
                        Console.WriteLine("as");
                        goto Eop;
                    }
                    m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                    Console.WriteLine(ms.displayname);
                    Console.WriteLine(i.ToString());
                    Console.WriteLine(s.ToString());
                }
                f = i;
            }
            mailsaved.savedic(filelist);
            nowCount = f - 1;
            mCount = Client.GetMessageCount();
        }
        public void updatedata2(int ts)
        {
            updatedata();
            /*
            Console.WriteLine(nowCount);
            mCount = Client.GetMessageCount();
            nowCount = firstCount;
            int s = 0;
            int f = 0;
            if (nowCount > mCount) nowCount = mCount;
            for (int i = nowCount,j = 0;j < 15 || i < nowCount + 15; i++,j++)
            {
            Op:
                if (i > mCount){ f = mCount; break; }
                    if (!filelist.ContainsKey(i))
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
                    string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                    string mainbody = ReplaceHtmlTag(l_strResult);
                    string path = i + ".txt";
                    File.WriteAllText(path, l_strResult);
                    string sc = Cmd("python module1.py " + path);
                    mail m = (mail)mails[j];
                    string t = getname(zhuti, dizhi, shijan, body);
                    filelist.Add(i, t);
                    if (sc == "垃圾邮件" || blacklist.Contains(dizhi))
                    {
                        rufilelist.Add(++rmax, t);
                        mailsaved.saverudic(rufilelist);
                        mailsaved.savemail(new mailsave(message, body), t);
                        i--;
                        goto Op;
                    }

                    m.reset(zhuti, dizhi, shijan, body, new Mymessage(message));

                    //string t = i + zhuti.Substring(0,1)+dizhi.Substring(1,2)+shijan.Substring(8,2);
                    mailsave ma = new mailsave(message, t);

                    mailsaved.savemail(new mailsave(message, body), t);
                }
                else
                {
                    mailsave ms;
                Eop:
                    if (i > mCount) { f = mCount; break; }
                    if (!filelist.ContainsKey(i))
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
                        string l_strResult = body.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                        string mainbody = ReplaceHtmlTag(l_strResult);
                        string path = i + ".txt";
                        File.WriteAllText(path, l_strResult);
                        string sc = Cmd("python module1.py " + path);
                        mail m2 = (mail)mails[j];
                        string t = getname(zhuti, dizhi, shijan, body);
                        filelist.Add(i, t);
                        if (sc == "垃圾邮件" || blacklist.Contains(dizhi))
                        {
                            rufilelist.Add(++rmax, t);
                            mailsaved.saverudic(rufilelist);
                            mailsaved.savemail(new mailsave(message, body), t);
                            i++;
                            goto Eop;
                        }

                        m2.reset(zhuti, dizhi, shijan, body, new Mymessage(message));

                        //string t = i + zhuti.Substring(0,1)+dizhi.Substring(1,2)+shijan.Substring(8,2);
                        mailsave ma = new mailsave(message, t);
                        mailsaved.savemail(new mailsave(message, body), t);
                    };
                    ms = mailsaved.loadmail(filelist[i]);
                    s++;
                    mail m = (mail)mails[j];
                    if (defilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body)) || rufilelist.ContainsValue(getname(ms.displayname, ms.address, ms.time, ms.body)) || blacklist.Contains(ms.address))
                    {

                        nowCount++;
                        i++;
                        Console.WriteLine("as");
                        goto Eop;
                    }
                    m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                    Console.WriteLine(ms.displayname);
                    Console.WriteLine(i.ToString());
                    Console.WriteLine(s.ToString());
                }
                mailsaved.savedic(filelist);
                f = i;
            }
            firstCount = f;*/
        }
        //刷新
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
        //调用判断程序
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
        //判断是否垃圾
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
        //保存垃圾箱邮件
        public void saveRubbish()
        {
            foreach (int key in filelist.Keys)
            {
                if (key > tmax)
                {
                    mailsave ms = mailsaved.loadmail(filelist[key]);
                    string path = "1.txt";
                    File.WriteAllText(path, ms.body);
                    string s = Cmd("python module1.py " + path);
                    if (s == "垃圾邮件")
                    {
                        rufilelist.Add(++rmax, filelist[key]);
                    }
                    Console.WriteLine(key);
                }
            }
            mailsaved.saverudic(rufilelist);
        }
        //初始化
        private void detail_Shown(object sender, EventArgs e)
        {
            for(int i = 0; i < 15; i++)
            {
                mail nm = new mail("", "", "", "", new OpenPop.Mime.Message(new byte[1]));
                nm.Name = (i).ToString();
                nm.clicked += new mail.thisclick(showdetail);
                nm.deleted += new mail.thisclick(movetode);
                nm.toblacklist += new mail.thisclick(movetoblacklist);
                nm.torubox += new mail.thisclick(movetoru);
                mails.Add(nm);
                this.flowLayoutPanel1.Controls.Add(nm);
            }
            initbackcolor();
            updatedata();
            setru();
            setde();
            Thread thread = new Thread(saveRubbish);
            thread.Start();
        }
        private void movetode(object sender, MyEventArgs e)
        {if (getnowflow() == 1)
            {
                defilelist.Add(++dmax, getname(e.message.displayname, e.message.address, e.message.time, e.message.body));
                mailsaved.savededic(defilelist);
                updatedata();
            }
        }
        private void movetoblacklist(object sender, MyEventArgs e)
        {
            if (getnowflow() == 1){
                blacklist.Add(e.message.address);
                mailsaved.saveblacklist(blacklist);
                updatedata(); }
        }
        private void movetoru(object sender, MyEventArgs e)
        {
            if (getnowflow() == 1)
            {
                rufilelist.Add(++rmax, getname(e.message.displayname, e.message.address, e.message.time, e.message.body));
                mailsaved.saverudic(rufilelist);
                updatedata();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (getnowflow() == 1)
            {
                cleardetail();
                clear();
                updatedata(nowCount);

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (getnowflow() == 1)
            {
                cleardetail();
                clear();
                updatedata2(nowCount);

            }
        }
        //清除内容
        private void clear()
        {
            foreach(mail ma in mails)
            {
                ma.disrubbish();
            }
        }
        //计时器
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
        //替换html元素
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "{[^}]+}", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        //显示详细内容
        private void showdetail(object sender, MyEventArgs e)
        {
            Mymessage message = e.message;
            maildetail mt = new maildetail(message);
            mt.Name = "isdetail";
            
            foreach(Control con in mails)
            {
                if(con.Name == e.name)
                {
                    mail c = (mail)con;
                    if (!c.isdetail)
                    {
                        int t = this.flowLayoutPanel1.Controls.GetChildIndex(con);
                        this.flowLayoutPanel1.Controls.Add(mt);
                        this.flowLayoutPanel1.Controls.SetChildIndex(mt, t + 1);
                        c.isdetail = true;
                    }
                }
            }
        }
        //清除详细内容
        private void cleardetail()
        {
            foreach(Control c in this.flowLayoutPanel1.Controls)
            {
                if (c.Name == "isdetail")
                {
                    c.Dispose();
                }
            }
            foreach(mail m in mails)
            {
                m.isdetail = false;
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
            freshde();
        }
        //获得随机字母
        public static string getchar(int Length)
        {
            char[] constant = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);
            }
            return newRandom.ToString().ToLower();
        }
        //初始化垃圾邮箱
        public void setru()
        {
            for (int i = 0; i < 15; i++)
            {
                mail nm = new mail("", "", "", "", new OpenPop.Mime.Message(new byte[1]));
                nm.Name = (i).ToString();
                nm.clicked += new mail.thisclick(showdetail);
                nm.deleted += new mail.thisclick(nullfun);
                nm.toblacklist += new mail.thisclick(nullfun);
                nm.torubox += new mail.thisclick(nullfun);
                rumails.Add(nm);
                this.flowLayoutPanel2.Controls.Add(nm);
            }
            int ts = 0;
            foreach (mail ma in rumails)
            {
                if ((ts++) % 2 == 1)
                    ma.setBackcollor(Color.White);
            }
            int ft = 0;
            foreach (int k in rufilelist.Keys)
            {
                if (ft < 15)
                {
                    mailsave ms = mailsaved.loadmail(rufilelist[k]);
                    mail m = (mail)rumails[ft];
                    ft++;
                    m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                }
            }
        }
        //初始化垃圾箱
        public void setde()
        {
            for (int i = 0; i < 15; i++)
            {
                mail nm = new mail("", "", "", "", new OpenPop.Mime.Message(new byte[1]));
                nm.Name = (i).ToString();
                nm.clicked += new mail.thisclick(showdetail);
                nm.deleted += new mail.thisclick(nullfun);
                nm.toblacklist += new mail.thisclick(nullfun);
                nm.torubox += new mail.thisclick(nullfun);
                demails.Add(nm);
                this.flowLayoutPanel3.Controls.Add(nm);
            }
            int ts = 0;
            foreach (mail ma in demails)
            {
                if ((ts++) % 2 == 1)
                    ma.setBackcollor(Color.White);
            }
            int ft = 0;
            foreach (int k in defilelist.Keys)
            {
                if (ft < 15)
                {
                    mailsave ms = mailsaved.loadmail(defilelist[k]);
                    mail m = (mail)demails[ft];
                    ft++;
                    m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                }
            }
        }
        public void freshde()
        {
            int ft = 0;
            foreach (int k in defilelist.Keys)
            {
                if (ft < 15)
                {
                    mailsave ms = mailsaved.loadmail(defilelist[k]);
                    mail m = (mail)demails[ft];
                    ft++;
                    m.reset(ms.displayname, ms.address, ms.time, ms.body, ms.Mymessage);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach(string f in blacklist)
            {
                s = s + f + '\n';
            }
            MessageBox.Show(s);
        }
        private int getnowflow()
        {
            if (this.flowLayoutPanel1.Visible == true) return 1;
            if (this.flowLayoutPanel2.Visible == true) return 2;
            if (this.flowLayoutPanel3.Visible == true) return 3;
            return 0;
        }
        private void nullfun(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            send s = new send();
            s.Show();
        }
    }
}
