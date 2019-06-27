from module2 import openemail
import joblib
import sys
m = sys.argv[1]
#加载模型
mo = joblib.load('test.pkl')
#预测
a = mo.predict(openemail(m))
if a[0] == 1:
    print("ru")
else:
    print("no")
