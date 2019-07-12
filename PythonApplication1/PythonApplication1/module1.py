from module2 import openemail
from module2 import openemail2
from module2 import openemail3
import joblib
import sys
import os
import tempfile
m1 = len(sys.argv)
#加载模型
mo = joblib.load('test.pkl')
def string_to_file(string):
    file_like_obj = tempfile.NamedTemporaryFile()
    file_like_obj.write(string)
    # 确保string立即写入文件
    file_like_obj.flush()
    # 将文件读取指针返回到文件开头位置
    file_like_obj.seek(0)
    return file_like_obj

def get_all_files(dir):
    files_ = []
    list = os.listdir(dir)
    for i in range(0, len(list)):
        path = os.path.join(dir, list[i])
        if os.path.isdir(path):
            files_.extend(get_all_files(path))
        if os.path.isfile(path):
            files_.append(path)
    return files_

if m1 == 2:
    m = sys.argv[1]
    '''if os.path.isfile(m):
    #预测
        a = mo.predict(openemail(m))
        print('ifflie')
        if a[0] == 1:
            print("垃圾邮件")
        else:
            print("正常邮件")
    elif os.path.isdir(m):
        print('ifdir')
        files = get_all_files(m)
        result = mo.predict(openemail3(files))
        for i in range(0,len(files)):
            if result[i] == 1:
                c = "垃圾邮件"
            else:
                c = "正常邮件"
            print("文件",files[i],"是",c)
    else:
        print('else')
        b = bytes(m,encoding='utf-8')
        ms = string_to_file(b)
        a = mo.predict(openemail2(ms))
        if a[0] == 1:
            print("垃圾邮件")
        else:
            print("正常邮件")
 '''

elif m1 == 1:
    print("no filename")
elif m1 > 2:
    a = [sys.argv[i] for i in range(0,m1) if os.path.exists(sys.argv[i])]
    b = mo.predict(openemail3(a))
    c = ""
    for i in range(1,len(a)):
        if b[i] == 1:
            c = "垃圾邮件"
        else:
            c = "正常邮件"
        print("文件",a[i],"是",c)
def predict1(filearray):
    c = ""
    result = mo.predict(openemail3(filearray))
    for i in range(0,len(filearray)):
        if result[i] == 1:
            c = "垃圾邮件"
        else:
            c = "正常邮件"
        print("文件",filearray[i],"是",c)
