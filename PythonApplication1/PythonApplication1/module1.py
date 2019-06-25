from module2 import openemail
import joblib
import sys
m = sys.argv[1]
mo = joblib.load('C:\\Users\\lenovo\\Documents\\GitHub\\AntiSystem\\PythonApplication1\\PythonApplication1\\test.pkl')
a = mo.predict(openemail(m))
if a[0] == 1:
    print("垃圾邮件")
else:
    print("正常邮件")
