import java.lang.reflect.Array;
import java.net.InetSocketAddress;
import java.util.ArrayList;

import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;


public class server extends WebSocketServer{
    private String message =null;
    public String account=null;
    public String password=null;

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
//        System.out.println(msg);
        setAccountAndPassword(message);
        userJoin(conn,message);//用户加入

        //初始化
        mail m=new mail();
        String host="pop3.163.com";
        //先读取
        m.mailStorage(host,account,password);
        //在cmd
        m.CMD();



        //TODO: 在这里调用返回判定的方法。

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

    //message的get，set方法
    public String getMessage(){
        return message;
    }
    public void setMessage(String m){
        this.message=m;
    }

    //将message的用户名和密码分离
    public void setAccountAndPassword(String m){
        String[] strArr = m.split("&");
        account=strArr[0];
        password=strArr[1];
        System.out.println(account);
        System.out.println(password);
    }
}
