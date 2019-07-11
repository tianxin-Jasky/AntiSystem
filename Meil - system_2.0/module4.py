#import PythonApplication2
from PythonApplication2 import predict2
from module1 import predict1
from module1 import get_all_files
import sys
import os
import re
zh_pattern = re.compile(u'[\u4e00-\u9fa5]+')
def contain_zh(word):
    global zh_pattern
    match = zh_pattern.search(word)
    return match
m1 = len(sys.argv)
if m1 == 2:
    m = sys.argv[1]
    if os.path.isfile(m):
        c = open(m,encoding = 'utf-8')
        s = c.read()
        if contain_zh(s):
            print('if')
            os.system(('python PythonApplication2.py ' + m))
        else:
            print('else')
            os.system('python module1.py '+ m)
    elif os.path.isdir(m):
        ch = []
        en = []
        files = get_all_files(m)
        for file in files:
            c = open(file,encoding = 'utf-8')
            s = c.read()
            if contain_zh(s):
                ch.append(file)
            else:
                en.append(file)
        predict1(en)
        predict2(ch)
