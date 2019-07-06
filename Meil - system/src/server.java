import java.lang.reflect.Array;
import java.net.InetSocketAddress;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

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

        setAccountAndPassword(message);
        userJoin(conn,message);//用户加入

        //初始化
        mail m=new mail();

        String reg="163.com";
        Pattern p =Pattern.compile(reg);
        Matcher matcher = p.matcher(account);
        boolean result=matcher.find();
        //true为163，false为qq
        String host="";
        if(result){
            host="pop3.163.com";
        }else {
            host = "pop.qq.com";
        }

        //先读取
        m.mailStorage(host,account,password);
        //对邮件的内容进行判断。
        m.CMD();

        String msg = null;
        msg = m.mail_number+m.signal;
        if(m.Loginjudge){
            //给msg赋值传给插件
            //msg = m.mail_number+m.signal;
            for(int i=0,j=0;i<m.mail_information.size();i++){
                msg=msg+m.mail_information.get(i)+m.signal;
                if(i%3 == 2){
                    msg=msg+m.CMD_judge.get(j)+m.signal;
                    //msg=msg+"normal"+m.signal;
                    j++;
                }
            }
        }


        //TODO: 在这里调用返回判定的方法。
        //String msg = "1!@#%&你好啊!@#%&1时间!@#%&收件人地址!@#%&1rubbishi";
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
        String[] strArr = m.split("!@#%&");
        account=strArr[0];
        password=strArr[1];
        System.out.println(account);
        //System.out.println(password);
    }


}
