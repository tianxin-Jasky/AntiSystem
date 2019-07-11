import org.java_websocket.WebSocketImpl;

import java.io.File;

public class Main {
    public static void main(String[] args) {
        //ReduceHTMLtext 作者顾宇凡
        //python文件     作者汪萌
        //server WsUtil  作者赵彪
        WebSocketImpl.DEBUG = false;
        server s = new server(1234);//实例化一个监听服务器
        s.start();//启动服务器
    }
}
