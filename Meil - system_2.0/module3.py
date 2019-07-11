import hashlib
import json
import random
import requests


url="http://api.fanyi.baidu.com/api/trans/vip/translate"
appid = '20190628000311095'  # 你的appid
secretKey = 'SLgQMWgZb1F6genbtRuC'  # 你的密钥
salt = random.randint(32768, 65536)

def get_tra_res(q,fromLang='cht',toLang='en'):
#生成签名
    sign = appid + q + str(salt) + secretKey
    sign = hashlib.md5(sign.encode()).hexdigest()
#post请求参数
    data = {
        "appid": appid,
        "q": q,
        "from": fromLang,
        "to" : toLang,
        "salt" : str(salt),
        "sign" : sign,
    }
#post请求
    res = requests.post(url, data=data)
#返回时一个json
    trans_result = json.loads(res.content).get('trans_result')[0].get("dst")
    return trans_result

get_tra_res('你好')