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
    class mailsave
    {
        OpenPop.Mime.Message message { get; set; }
        string time { get; set; }
        string address { get; set; }
        string displayname { get; set; }
        bool isRubbish = false;
        bool isDelete = false;
        public string filename { set; get; } 
        mailsave(OpenPop.Mime.Message message,bool isRubbish,int filename)
        {
            this.message = message;
            this.address = message.Headers.From.Address;
            this.time = message.Headers.DateSent.ToString();
            this.displayname = message.Headers.From.DisplayName;
            this.isRubbish = isRubbish;
            this.filename = filename.ToString();
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
        public static ArrayList loadmail()
        {
            IFormatter formatter = new BinaryFormatter();
            var files = Directory.GetFiles("","*.bin");
            ArrayList arrayList = new ArrayList();
            foreach(var file in files)
            {
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
            mailsave myObj = (mailsave)formatter.Deserialize(stream);
            arrayList.Add(myObj);
            stream.Close();
            }         
            return arrayList;
        }
        public static void savedic(Dictionary<int,string> dics)
        {
            int max = 0;
            foreach(var dic in dics)
            {
                max = max > dic.Key ? max : dic.Key;
            }
            FileStream fs = new FileStream("dic.txt", FileMode.Create);
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
        public static int loaddic(ref Dictionary<int, string> dics)
        {
            StreamReader sr = new StreamReader("dic.txt", Encoding.Default);
            String line;
            int t = Convert.ToInt32(sr.ReadLine().ToString());
            while ((line = sr.ReadLine()) != null)
            {
                var dic = line.ToString().Split(',');
                dics.Add(Convert.ToInt32(dic[0]),dic[1]);
            }
            return t;
        }
    }
}
