import  java.io.*;
import java.net.ServerSocket;
import java.net.Socket;


public class Server {
    //服务器

    public static void main(String[] args){
        try {
            //端口号
            ServerSocket serverSocket=new ServerSocket(8888);
            System.out.println("服务端已启动，等待客户端连接..");
            //监听并接受
            Socket socket=serverSocket.accept();

            //得到一个输入流，接受客户端传递的信息
            InputStream inputStream=socket.getInputStream();
            InputStreamReader inputStreamReader=new InputStreamReader(inputStream);

            BufferedReader bufferedReader=new BufferedReader(inputStreamReader);
            String Username=null;
            //String Password=null;
            while (true){
                Username=bufferedReader.readLine();
                if(!Username.equals("#")){
                    System.out.println("接受到客户机的信息");
                    System.out.println("当前客户机的ip"+socket.getInetAddress().getHostAddress());
                    System.out.println("用户的名字   "+Username);
                }else {
                    break;
                }
            }

//            //获取一个输出流，向服务端发送信息
//            OutputStream outputStream=socket.getOutputStream();
//            //将输出流包装成打印流
//            PrintWriter printwriter=new PrintWriter(outputStream);
//            printwriter.print("你好，服务端已接收到您的信息");
//            printwriter.flush();
//            socket.shutdownOutput();//关闭输出流


            //关闭相对应的资源
//            printwriter.close();
//            outputStream.close();
            bufferedReader.close();
            inputStream.close();
            socket.close();

        }catch (IOException e){
            e.printStackTrace();
        }
    }


}
