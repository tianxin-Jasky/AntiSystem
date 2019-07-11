import java.io.*;
import java.text.SimpleDateFormat;
import java.util.*;
import javax.mail.BodyPart;
import javax.mail.Flags;
import javax.mail.Folder;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Multipart;
import javax.mail.Part;
import javax.mail.Session;
import javax.mail.Store;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeUtility;
import java.io.PrintStream;
import java.io.FileNotFoundException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Date;
import org.java_websocket.WebSocketImpl;








/**
 * @author yh
 *
 */
public class  mail {
    TimeZone timeZone = TimeZone.getTimeZone("GMT+8");
    private MimeMessage mimeMessage = null;
    private String saveAttachPath = ""; // 附件下载后的存放目录
    private StringBuffer bodyText = new StringBuffer(); // 存放邮件内容的StringBuffer对象
    private String dateFormat = "yy-MM-dd HH:mm"; // 默认的日前显示格式
    public ArrayList<String> mail_information=new ArrayList<>();
    public  int mail_number=-1;
    public  boolean Loginjudge=true;
    public  String signal = "!@#%&";
    public static ArrayList<String> CMD_judge=new ArrayList<>();


    //关于javamail的基础的函数 作者：顾宇凡
    /**
     * 构造函数,初始化一个MimeMessage对象
     */
    public  mail() {
    }

    public  mail(MimeMessage mimeMessage) {
        this.mimeMessage = mimeMessage;

    }

    public void setMimeMessage(MimeMessage mimeMessage) {
        this.mimeMessage = mimeMessage;

    }

    /**
     * 　*　获得发件人的地址和姓名 　
     */
    public String getFrom() throws Exception {
        InternetAddress address[] = (InternetAddress[]) mimeMessage.getFrom();
        String from = address[0].getAddress();
        if (from == null) {
            from = "";

        }
        String personal = address[0].getPersonal();

        if (personal == null) {
            personal = "";

        }

        String fromAddr = null;
        if (personal != null || from != null) {
            fromAddr = personal + "<" + from + ">";

        } else {

        }
        return fromAddr;
    }

    /**
     * 　*　获得邮件的收件人，抄送，和密送的地址和姓名，根据所传递的参数的不同
     * 　*　"to"----收件人　"cc"---抄送人地址　"bcc"---密送人地址 　
     */
    public String getMailAddress(String type) throws Exception {
        String mailAddr = "";
        String addType = type.toUpperCase();

        InternetAddress[] address = null;
        if (addType.equals("TO") || addType.equals("CC")
                || addType.equals("BCC")) {

            if (addType.equals("TO")) {
                address = (InternetAddress[]) mimeMessage
                        .getRecipients(Message.RecipientType.TO);
            } else if (addType.equals("CC")) {
                address = (InternetAddress[]) mimeMessage
                        .getRecipients(Message.RecipientType.CC);
            } else {
                address = (InternetAddress[]) mimeMessage
                        .getRecipients(Message.RecipientType.BCC);
            }

            if (address != null) {
                for (int i = 0; i < address.length; i++) {
                    String emailAddr = address[i].getAddress();
                    if (emailAddr == null) {
                        emailAddr = "";
                    } else {

                        emailAddr = MimeUtility.decodeText(emailAddr);

                    }
                    String personal = address[i].getPersonal();
                    if (personal == null) {
                        personal = "";
                    } else {

                        personal = MimeUtility.decodeText(personal);

                    }
                    String compositeto = personal + "<" + emailAddr + ">";

                    mailAddr += "," + compositeto;
                }
                mailAddr = mailAddr.substring(1);
            }
        } else {
            throw new Exception("错误的电子邮件类型!");
        }
        return mailAddr;
    }

    /**
     * 　*　获得邮件主题 　
     */
    public String getSubject() throws MessagingException {
        String subject = "";
        try {

            subject = MimeUtility.decodeText(mimeMessage.getSubject());

            if (subject == null) {
                subject = "";
            }
        } catch (Exception exce) {
            exce.printStackTrace();
        }
        return subject;
    }

    /**
     * 　*　获得邮件发送日期 　
     */
    public String getSentDate() throws Exception {
        Date sentDate = mimeMessage.getSentDate();

        SimpleDateFormat format = new SimpleDateFormat(dateFormat);
        String strSentDate = format.format(sentDate);

        return strSentDate;
    }

    /**
     * 　*　获得邮件正文内容 　
     */
    public String getBodyText() {
        return bodyText.toString();
    }

    /**
     * 　　*　解析邮件，把得到的邮件内容保存到一个StringBuffer对象中，解析邮件
     * 　　*　主要是根据MimeType类型的不同执行不同的操作，一步一步的解析 　　
     */

    public void getMailContent(Part part) throws Exception {

        String contentType = part.getContentType();
        // 获得邮件的MimeType类型


        int nameIndex = contentType.indexOf("name");

        boolean conName = false;

        if (nameIndex != -1) {
            conName = true;
        }



        if (part.isMimeType("text/plain") && conName == false) {
            // text/plain 类型
            bodyText.append((String) part.getContent());
        } else if (part.isMimeType("text/html") && conName == false) {
            // text/html 类型
            bodyText.append((String) part.getContent());
        } else if (part.isMimeType("multipart/*")) {
            // multipart/*
            Multipart multipart = (Multipart) part.getContent();
            int counts = multipart.getCount();
            for (int i = 0; i < counts; i++) {
                getMailContent(multipart.getBodyPart(i));
            }
        } else if (part.isMimeType("message/rfc822")) {
            // message/rfc822
            getMailContent((Part) part.getContent());
        } else {

        }
    }

    /**
     * 　　*　判断此邮件是否需要回执，如果需要回执返回"true",否则返回"false" 　
     */
    public boolean getReplySign() throws MessagingException {

        boolean replySign = false;

        String needReply[] = mimeMessage
                .getHeader("Disposition-Notification-To");

        if (needReply != null) {
            replySign = true;
        }
        if (replySign) {

        } else {

        }
        return replySign;
    }

    /**
     *　获得此邮件的Message-ID 　　
     */
    public String getMessageId() throws MessagingException {
        String messageID = mimeMessage.getMessageID();

        return messageID;
    }

    /**
     * 判断此邮件是否已读，如果未读返回false,反之返回true
     */
    public boolean isNew() throws MessagingException {
        boolean isNew = false;
        Flags flags = ((Message) mimeMessage).getFlags();
        Flags.Flag[] flag = flags.getSystemFlags();

        for (int i = 0; i < flag.length; i++) {
            if (flag[i] == Flags.Flag.SEEN) {
                isNew = true;

                // break;
            }
        }
        return isNew;
    }

    /**
     * 判断此邮件是否包含附件
     */
    public boolean isContainAttach(Part part) throws Exception {
        boolean attachFlag = false;
        // String contentType = part.getContentType();
        if (part.isMimeType("multipart/*")) {
            Multipart mp = (Multipart) part.getContent();
            for (int i = 0; i < mp.getCount(); i++) {
                BodyPart mPart = mp.getBodyPart(i);
                String disposition = mPart.getDisposition();
                if ((disposition != null)
                        && ((disposition.equals(Part.ATTACHMENT)) || (disposition
                        .equals(Part.INLINE))))
                    attachFlag = true;
                else if (mPart.isMimeType("multipart/*")) {
                    attachFlag = isContainAttach((Part) mPart);
                } else {
                    String conType = mPart.getContentType();

                    if (conType.toLowerCase().indexOf("application") != -1)
                        attachFlag = true;
                    if (conType.toLowerCase().indexOf("name") != -1)
                        attachFlag = true;
                }
            }
        } else if (part.isMimeType("message/rfc822")) {
            attachFlag = isContainAttach((Part) part.getContent());
        }
        return attachFlag;
    }

    /**
     * 　*　保存附件 　
     */

    public void saveAttachMent(Part part) throws Exception {
        String fileName = "";
        if (part.isMimeType("multipart/*")) {
            Multipart mp = (Multipart) part.getContent();
            for (int i = 0; i < mp.getCount(); i++) {
                BodyPart mPart = mp.getBodyPart(i);
                String disposition = mPart.getDisposition();
                if ((disposition != null)
                        && ((disposition.equals(Part.ATTACHMENT)) || (disposition
                        .equals(Part.INLINE)))) {
                    fileName = mPart.getFileName();
                    if (fileName.toLowerCase().indexOf("gb2312") != -1) {
                        fileName = MimeUtility.decodeText(fileName);
                    }
                    saveFile(fileName, mPart.getInputStream());
                } else if (mPart.isMimeType("multipart/*")) {
                    saveAttachMent(mPart);
                } else {
                    fileName = mPart.getFileName();
                    if ((fileName != null)
                            && (fileName.toLowerCase().indexOf("GB2312") != -1)) {
                        fileName = MimeUtility.decodeText(fileName);
                        saveFile(fileName, mPart.getInputStream());
                    }
                }
            }
        } else if (part.isMimeType("message/rfc822")) {
            saveAttachMent((Part) part.getContent());
        }
    }

    /**
     *　设置附件存放路径
     */
    public void setAttachPath(String attachPath) {
        this.saveAttachPath = attachPath;
    }

    /**
     * 　*　设置日期显示格式 　
     */
    public void setDateFormat(String format) throws Exception {
        this.dateFormat = format;
    }

    /**
     * 　*　获得附件存放路径 　
     */
    public String getAttachPath() {
        return saveAttachPath;
    }

    /**
     * 　*　真正的保存附件到指定目录里 　
     */
    private void saveFile(String fileName, InputStream in) throws Exception {
        String osName = System.getProperty("os.name");
        String storeDir = getAttachPath();
        String separator = "";
        if (osName == null) {
            osName = "";
        }
        if (osName.toLowerCase().indexOf("win") != -1) {
            separator = "\\";
            if (storeDir == null || storeDir.equals(""))
                storeDir = "c:\\tmp";
        } else {
            separator = "/";
            storeDir = "/tmp";
        }
        File storeFile = new File(storeDir + separator + fileName);

        // for(int　i=0;storefile.exists();i++){
        // storefile　=　new　File(storedir+separator+fileName+i);
        // }
        BufferedOutputStream bos = null;
        BufferedInputStream bis = null;

        try {
            bos = new BufferedOutputStream(new FileOutputStream(storeFile));
            bis = new BufferedInputStream(in);
            int c;
            while ((c = bis.read()) != -1) {
                bos.write(c);
                bos.flush();
            }
        } catch (Exception exception) {
            exception.printStackTrace();
            throw new Exception("文件保存失败!");
        } finally {
            bos.close();
            bis.close();
        }
    }


    //服务器的连接以及相关的数据处理  作者：赵彪
    public Message[] mailconnection(String host,String username,String password){
            try {
                Properties props = new Properties();
                props.setProperty("mail.pop3.host", "pop.qq.com"); // 按需要更改
                props.setProperty("mail.pop3.port", "110");
                // SSL安全连接参数
                props.setProperty("mail.pop3.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
                props.setProperty("mail.pop3.socketFactory.fallback", "true");
                props.setProperty("mail.pop3.socketFactory.port", "110");

                Session session = Session.getDefaultInstance(props, null);

                Store store = session.getStore("pop3");
                store.connect(host, username, password);

                Folder folder = store.getFolder("INBOX");
                folder.open(Folder.READ_WRITE);
                Message message[] = folder.getMessages();

                return message;
            }catch (Exception e) {
                Loginjudge=false;
                System.out.println("邮箱登陆失败");
                return null;
            }
    }

    public void mailStorage(String host,String username,String password){
            try {

                Message message[] = mailconnection(host,username,password);

                //第一个是邮件的数量
                mail_number=message.length;
                mail re = null;

                for (int i = 0; i < message.length ; i++) {
                    re = new  mail((MimeMessage) message[i]);

                    re.setDateFormat("yy年MM月dd日　HH:mm");
                    re.getMailContent((Part) message[i]);
                    //creat a new folder
                    File file = new File("C:/test/"+username);
                    file.mkdirs();
                    PrintStream ps = new PrintStream("C:/test/"+username+"/"+i+".txt");  // 创建一个打印输出流，输出的目标是：E盘的txt文件
                    //PrintStream ps = new PrintStream("/Users/zhouqc/Desktop/txt/"+re.getSubject()+".txt");
                    ps.println(ReduceHtml2Text.removeHtmlTag(re.getBodyText()));

                    //对传输给插件的进行信息的处理
                    //主题
                    mail_information.add(re.getSubject());
                    System.out.println(re.getSubject());
                    //时间re.getSentDate()
                    mail_information.add(re.getSentDate());
                    System.out.println(re.getSentDate());
                    //发件人地址re.getMailAddress("to")
                    mail_information.add(re.getFrom());
                    System.out.println(re.getMailAddress("to"));
                }
            }catch (Exception e) {
                Loginjudge=false;
                System.out.println("邮箱登陆失败");
            }
        }

    public void maildelete(String host,String username,String password){
        try{
            Properties props = new Properties();
            props.setProperty("mail.pop3.host", "pop.qq.com"); // 按需要更改
            props.setProperty("mail.pop3.port", "110");
            // SSL安全连接参数
            props.setProperty("mail.pop3.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
            props.setProperty("mail.pop3.socketFactory.fallback", "true");
            props.setProperty("mail.pop3.socketFactory.port", "110");

            Session session = Session.getDefaultInstance(props, null);

            Store store = session.getStore("pop3");
            store.connect(host, username, password);

            Folder folder = store.getFolder("INBOX");
            folder.open(Folder.READ_WRITE);
            Message messages[] = folder.getMessages();

            boolean delresult = false;
            for(int i=0;i<messages.length;i++){
                Message msg = messages[i];
                //System.out.println("----------第"+i+"份---------");

                String subject = msg.getSubject();

               // System.out.println("主题"+subject);
                //发件人与这个相同，主题与这个相同则进行删除。
                if(CMD_judge.get(i).matches("spam mail")){
                    msg.setFlag(Flags.Flag.DELETED, true); // set the DELETED flag
                    //delete the txt
                    delete("C:/test/"+username+"/"+i+".txt");
                    System.out.println("C:/test/"+username+"/"+i+".txt");
                    delresult = true;
                    System.out.println("成功删除mail:"+msg.getSubject());
                }
            }
            //System.out.print("未读邮件："+folder.getUnreadMessageCount());
            folder.close(true); //退出收件箱时,删除做了删除标识的邮件
            if(delresult)
                System.out.println("成功删除该邮件！");
            else
                System.out.println("删除该邮件失败,或该邮件不存在！");
            store.close();
        }catch (Exception e){
            System.out.println("邮件操作出错了！");
        }
    }

    public  boolean delete(String fileName){
        File file = new File(fileName);
        if (!file.exists()) {
            System.out.println("删除文件失败:" + fileName + "不存在！");
            return false;
        } else {
            System.out.println("chenggong");
            return file.delete();
        }
    }

    //ptython文件 作者：汪萌
    //调用python文件的cmd 作者：顾宇凡
    //对cmd后续的数据处理。 作者：赵彪
    public  void   CMD(String username) {
        String cmd="python module4.py C:\\test\\"+username;

        String line = null;
        StringBuilder sb = new StringBuilder();
        Runtime runtime = Runtime.getRuntime(); //得到本程序
        try {
            Process process = runtime.exec(cmd);  //该实例可用来控制进程并获得相关信息
            //获取进程输出流
            BufferedReader  bufferedReader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            //用int数组存储排名

            ArrayList<Integer> rank=new ArrayList<>();
            while ((line = bufferedReader.readLine()) != null) {
                sb.append(line + "\n");
                //先用\\对其进行分割，取其中的第三个
                String[] linearr=line.split("\\\\");
                String[] linerarr2=linearr[3].split(".txt ");
                //首先添加数字
                rank.add(Integer.valueOf(linerarr2[0]));
                //然后添加判断
                CMD_judge.add(linerarr2[1]);
                //CMD_judge.add(line);
                System.out.println(line);
                //CMD_judge.add(line);
            }
            //冒泡排序
            for(int i=0;i<rank.size();i++){
                for(int j=0;j<rank.size()-1;j++){
                    if(rank.get(j)>rank.get(j+1)){
                        //两个位置交换值
                        int temp=rank.get(j);
                        rank.remove(j);
                        rank.add(j+1,temp);
                        //对应的data数组里的值也要进行改变。
                        String tmep1=CMD_judge.get(j);
                        CMD_judge.remove(j);
                        CMD_judge.add(j+1,tmep1);
                    }
                }
            }

        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("python程序调用失败");
        }
    }
}







