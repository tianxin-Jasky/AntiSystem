using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Common;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace MyMail
{
    [Serializable]
    class mailsave
    {
        //OpenPop.Mime.Message message { get; set; }
        public string time { get; set; }
        public string address { get; set; }
        public string displayname { get; set; }
        public bool isRubbish = false;
        public string body { set; get; }
        public Mymessage Mymessage { set; get; }
        public mailsave(OpenPop.Mime.Message message,bool isRubbish,string body)
        {
            //this.message = message;
            this.address = message.Headers.From.Address;
            this.time = message.Headers.DateSent.ToString();
            this.displayname = message.Headers.From.DisplayName;
            this.isRubbish = isRubbish;
            this.body = body;
            this.Mymessage = new Mymessage(message);
        }
        public mailsave(OpenPop.Mime.Message message, string filename)
        {
            //this.message = message;
            this.address = message.Headers.From.Address;
            this.time = message.Headers.DateSent.ToString();
            this.displayname = message.Headers.From.DisplayName;
            this.body = filename;
            this.Mymessage = new Mymessage(message);
        }
    }
    class mailsaved
    {
        public static void savemail(mailsave mails,string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename + ".bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, mails);
            stream.Close();
        }
        public static int getnewmail()
        {
            var files = Directory.GetFiles("", "*.bin");

            return 0;
        }
        public static mailsave loadmail(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename+".bin", FileMode.Open, FileAccess.Read, FileShare.None);
            mailsave myObj = (mailsave)formatter.Deserialize(stream);
            stream.Close();
            return myObj;
        }
        public static void savedic(Dictionary<int,string> dics)
        {
            int max = 0;
            foreach(var dic in dics)
            {
                max = max > dic.Key ? max : dic.Key;
            }
            FileStream fs = new FileStream(Form1.username+"\\dic.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(max.ToString());
            sw.Flush();
            foreach (var dic in dics)
            {
                string toWrite = dic.Key + ","+dic.Value;
                sw.WriteLine(toWrite);
                sw.Flush();
            }
            sw.Close();
            fs.Close();
        }
        public static void saverudic(Dictionary<int, string> dics)
        {
            int max = 0;
            foreach (var dic in dics)
            {
                max = max > dic.Key ? max : dic.Key;
            }
            FileStream fs = new FileStream(Form1.username+"\\rudic.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(max.ToString());
            sw.Flush();
            foreach (var dic in dics)
            {
                string toWrite = dic.Key + "," + dic.Value;
                sw.WriteLine(toWrite);
                sw.Flush();
            }
            sw.Close();
            fs.Close();
        }
        public static void savededic(Dictionary<int, string> dics)
        {
            int max = 0;
            foreach (var dic in dics)
            {
                max = max > dic.Key ? max : dic.Key;
            }
            FileStream fs = new FileStream(Form1.username+"\\dedic.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(max.ToString());
            sw.Flush();
            foreach (var dic in dics)
            {
                string toWrite = dic.Key + "," + dic.Value;
                sw.WriteLine(toWrite);
                sw.Flush();
            }
            sw.Close();
            fs.Close();
        }
        public static int loaddic(ref Dictionary<int, string> dics)
        {
            try
            {
                StreamReader sr = new StreamReader(Form1.username+"\\dic.txt", Encoding.Default);
                String line;
                int t = Convert.ToInt32(sr.ReadLine().ToString());
                while ((line = sr.ReadLine()) != null)
                {
                    var dic = line.ToString().Split(',');
                    dics.Add(Convert.ToInt32(dic[0]), dic[1]);
                }
                sr.Close();
                return t;
            }
            catch { return 0; }
        }
        public static int loadrudic(ref Dictionary<int, string> dics)
        {
            try
            {
                StreamReader sr = new StreamReader(Form1.username+"\\rudic.txt", Encoding.Default);
                String line;
                int t = Convert.ToInt32(sr.ReadLine().ToString());
                while ((line = sr.ReadLine()) != null)
                {
                    var dic = line.ToString().Split(',');
                    dics.Add(Convert.ToInt32(dic[0]), dic[1]);
                }
                sr.Close();
                return t;
            }
            catch {
                return 0;
            }
        }
        public static int loaddedic(ref Dictionary<int, string> dics)
        {
            try
            {
                StreamReader sr = new StreamReader(Form1.username+"\\dedic.txt", Encoding.Default);
                String line;
                int t = Convert.ToInt32(sr.ReadLine().ToString());
                while ((line = sr.ReadLine()) != null)
                {
                    var dic = line.ToString().Split(',');
                    dics.Add(Convert.ToInt32(dic[0]), dic[1]);
                }
                sr.Close();
                return t;
            }
            catch
            {
                return 0;
            }
        }
        public static void saveblacklist(ArrayList array)
        {
            FileStream fs = new FileStream(Form1.username+"\\blacklist.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            foreach (string s in array)
            {
                sw.WriteLine(s);
                sw.Flush();
            }
            sw.Close();
            fs.Close();
        }
        public static void loadblacklist(ref ArrayList array) {
            try
            {
                StreamReader sr = new StreamReader(Form1.username + "\\blacklist.txt", Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    array.Add(line);
                }
                sr.Close();
            }
            catch { }
        }
    }

    [Serializable]
        public class Mymessage
        {
            public string time { get; set; }
            public string address { get; set; }
            public string displayname { get; set; }
            public string body { set; get; }
            public Mymessage(OpenPop.Mime.Message message)
            {
                this.address = message.Headers.From.Address;
                this.time = message.Headers.DateSent.ToString();
                this.displayname = message.Headers.From.DisplayName;
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
                this.body = body;
            }
        }
}
