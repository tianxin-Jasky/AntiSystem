from module2 import openemail
from module2 import openemail3
import joblib
import sys
import os
m1 = len(sys.argv)
#加载模型
mo = joblib.load('test.pkl')
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
    if os.path.isfile(m):
    #预测
        a = mo.predict(openemail(m))
        print(a)
        if a[0] == 1:
            print("ru")
        else:
            print("no")
    if os.path.isdir(m):
        files = get_all_files(m)
        result = mo.predict(openemail3(files))
        for i in range(0,len(files)):
            if result[i] == 1:
                c = "垃圾邮件"
            else:
                c = "正常邮件"
            print("文件",files[i],"是",c)

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
    
