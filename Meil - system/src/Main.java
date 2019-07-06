import org.java_websocket.WebSocketImpl;

import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {
    public static void main(String[] args) {
        WebSocketImpl.DEBUG = false;
        server s = new server(1234);//实例化一个监听服务器
        s.start();//启动服务器


//        String acccount="17354422502@163.com";
//        String[] strarr=acccount.split("@");
//        String reg="163.com";
//        Pattern p =Pattern.compile(reg);
//        Matcher m = p.matcher(acccount);
//        boolean result=m.find();
//        System.out.println(result);





        //String temp2= Pattern.compile("E:\\test\\").matcher(temp).replaceAll("");
        //System.out.println(temp);
        //System.out.println(temp2);

    }
}
