import org.java_websocket.WebSocketImpl;

public class Main {
    public static void main(String[] args) {
        WebSocketImpl.DEBUG = false;
        server s = new server(1234);//实例化一个监听服务器
        s.start();//启动服务器


//        server(9090);
//        CMD();
    }
}
