import org.java_websocket.WebSocketImpl;

public class Main {
    public static void main(String[] args) {
        WebSocketImpl.DEBUG = false;
        int port = 9090; // 端口
        server s = new server(port);//实例化一个监听服务器
        s.start();//启动服务器
    }
}
