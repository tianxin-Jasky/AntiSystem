from module2 import openemail
import joblib
import sys
m = sys.argv[1]
mo = joblib.load('test.pkl')
a = mo.predict(openemail(m))
if a[0] == 1:
    print("rubbish")
else:
    print("normal")
