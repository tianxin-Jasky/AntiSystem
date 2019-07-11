import socket
from module2 import openemail2
import joblib
import sys
import tempfile
#远程连接
def string_to_file(string):
    file_like_obj = tempfile.NamedTemporaryFile()
    file_like_obj.write(string)
    # 确保string立即写入文件
    file_like_obj.flush()
    # 将文件读取指针返回到文件开头位置
    file_like_obj.seek(0)
    return file_like_obj
mo = joblib.load('test.pkl')
server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
server.bind(('127.0.0.1',9999))
server.listen(5)
while True:
    tcpCliSock, addr = server.accept()

    data = tcpCliSock.recv(64444)
    m = string_to_file(data)
    a = mo.predict(openemail2(m))
    if a[0] == 1:
        print("垃圾")
        msg = "垃圾邮件"
        tcpCliSock.send(msg.encode('utf8'))
    else:
        print("正常")
        msg = "正常邮件"
        tcpCliSock.send(msg.encode('utf8'))



'''
ip_port = ('192.168.153.63',9999)
server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server.bind(ip_port)
while True:
    data,client_addr = server.recvfrom(64444)
    m = string_to_file(data)
    a = mo.predict(openemail2(m))
    if a[0] == 1:
        print("垃圾")
        server.sendto(bytes("垃圾邮件", encoding = "utf8"),client_addr)
    else:
        print("正常")
        server.sendto(bytes("正常邮件", encoding = "utf8"),client_addr)
'''
