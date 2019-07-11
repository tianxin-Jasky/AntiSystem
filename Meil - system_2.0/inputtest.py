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
m = sys.argv[1]
b = bytes(m,encoding='utf-8')
ms = string_to_file(b)
a = mo.predict(openemail2(ms))
if a[0] == 1:
    print("垃圾邮件")
else:
    print("正常邮件")
 

