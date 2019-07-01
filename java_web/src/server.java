import java.net.InetSocketAddress;

import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;


public class server extends WebSocketServer{
    //构造函数
    public server(int port) {
        super(new InetSocketAddress(port));
    }

    public server(InetSocketAddress address) {
        super(address);
    }

    @Override
    public void onOpen(WebSocket conn, ClientHandshake handshake) {
        // ws连接的时候触发的代码，onOpen中我们不做任何操作

    }

    @Override
    public void onClose(WebSocket conn, int code, String reason, boolean remote) {
        //断开连接时候触发代码
        userLeave(conn);
        System.out.println(reason);
    }

    //接受客户机的信息，并且向客户机发送相应的信息。
    @Override
    public void onMessage(WebSocket conn, String message) {
        String msg = "收到信息："+message;
        System.out.println(msg);
        userJoin(conn,message);//用户加入
        //
        //TODO: 在这里调用返回判定的方法。
        //
        WsUtil.sendMessageToUser(conn,msg);
    }

    @Override
    public void onError(WebSocket conn, Exception ex) {
        //错误时候触发的代码
        System.out.println("on error");
        ex.printStackTrace();
    }
    /**
     * 去除掉失效的websocket链接
     * @param conn
     */
    private void userLeave(WebSocket conn){
        WsUtil.removeUser(conn);
    }

    /**
     * 将websocket加入用户池
     */
    private void userJoin(WebSocket conn,String userName){
        WsUtil.addUser(userName, conn);
    }

}
