from re import sub
from os import listdir
from collections import Counter
from itertools import chain
from numpy import array
from jieba import cut
from sklearn.naive_bayes import MultinomialNB

import pickle
import traceback 
import chardet 
import re
zh_pattern = re.compile(u'[\u4e00-\u9fa5]+')
import logging
import jieba
jieba.setLogLevel(logging.INFO)
def contain_zh(word):
    global zh_pattern
    match = zh_pattern.search(word)
    return match
def mytoutf8(s): 
    return mytounicode(s).encode('utf-8') 
def mytounicode(s): 
    if type(s) == type(u''): 
        return s 
    try: 
        s = s.decode('utf-8') 
    except: 
        try: 
            s = s.decode('gb18030') 
        except: 
            s = repr(s) 
    return s
def load_obj(name ):
    with open(name + '.pkl', 'rb') as f2:
        return pickle.load(f2)

def save_obj(obj, name ):
    with open(name + '.pkl', 'wb') as f2:
        pickle.dump(obj, f2, pickle.HIGHEST_PROTOCOL)
allWords = []
def getWordsFromFile(txtFile):
    words = []
    with open(txtFile, encoding='utf8') as fp:
        for line in fp:
            line = line.strip()
            #过滤干扰字符或无效字符
            line = sub(r'[.【】0-9、—。，！~、*]','',line)
            line = cut(line)
            #过滤长度为1的词
            line = filter(lambda word: len(word)>1, line)
            try:
                words.extend(line)
            except:
                print("error")
    return words
def getTopNWords(topN):
    txtFiles = [str(i)+'.txt' for i in range(10000,20000)]
    for txtFile in txtFiles:
        allWords.append(getWordsFromFile(txtFile))
    freq = Counter(chain(*allWords))
    return [w[0] for w in freq.most_common(topN)]
topWords = load_obj("test2");
vector = []
for words in allWords:
    temp = list(map(lambda x: words.count(x), topWords))
    vector.append(temp)
vector = array(vector)
labels = array([1]*5000 + [0]*5000)
model = load_obj("dic2")
def predict(txtFile):
    #获取指定邮箱文件内容，返回分类结果
    words = getWordsFromFile(txtFile)
    currentVector = array(tuple(map(lambda x: words.count(x), topWords)))
    result = model.predict(currentVector.reshape(1,-1))
    return '垃圾邮件' if result==1 else '正常邮件'
def predict2(filearray):
    fa = []
    for file in filearray:
        words = getWordsFromFile(file)
        currentVector = tuple(map(lambda x: words.count(x), topWords))
        #pr = currentVector.reshape(1,-1)
        fa.append(currentVector)
    ca = model.predict(fa)
    for c in range(0,len(ca)):
        if ca[c] == 0:
            print(filearray[c],'normal mail')
        else:
            print(filearray[c],'spam mail')
