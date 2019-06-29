import joblib
from sklearn.base import BaseEstimator, TransformerMixin
from scipy.sparse import csr_matrix
import email
import email.policy
import os
from sklearn.pipeline import Pipeline
import numpy as np
import nltk
import urlextract
import re
try:
    url_extractor = urlextract.URLExtract()
except ImportError:
    print("Error: replacing URLs requires the urlextract module.")
    url_extractor = None
    import re
from html import unescape
from collections import Counter
def structures_counter(emails):
    structures = Counter()
    for email in emails:
        structure = get_email_structure(email)
        structures[structure] += 1
    return structures
def html_to_plain_text(html):
    text = re.sub('<head.*?>.*?</head>', '', html, flags=re.M | re.S | re.I)
    text = re.sub('<a\s.*?>', ' HYPERLINK ', text, flags=re.M | re.S | re.I)
    text = re.sub('<.*?>', '', text, flags=re.M | re.S)
    text = re.sub(r'(\s*\n)+', '\n', text, flags=re.M | re.S)
    return unescape(text)
def email_to_text(email):
    html = None
    for part in email.walk():
        ctype = part.get_content_type()
        if not ctype in ("text/plain", "text/html"):
            continue
        try:
            content = part.get_content()
        except: # in case of encoding issues
            content = str(part.get_payload())
        if ctype == "text/plain":
            return content
        else:
            html = content
    if html:
        return html_to_plain_text(html)
    print("")
try:
    stemmer = nltk.PorterStemmer()

except ImportError:
    print("Error: stemming requires the NLTK module.")
    stemmer = None
import pickle
def load_obj(name ):
    with open(name + '.pkl', 'rb') as f2:
        return pickle.load(f2)

vac = load_obj("dic")
class EmailToWordCounterTransformer(BaseEstimator, TransformerMixin):
    def __init__(self, strip_headers=True, lower_case=True, remove_punctuation=True,
                 replace_urls=True, replace_numbers=True, stemming=True):
        self.strip_headers = strip_headers
        self.lower_case = lower_case
        self.remove_punctuation = remove_punctuation
        self.replace_urls = replace_urls
        self.replace_numbers = replace_numbers
        self.stemming = stemming
    def fit(self, X, y=None):
        return self
    def transform(self, X, y=None):
        X_transformed = []
        for email in X:
            text = email_to_text(email) or ""
            if self.lower_case:
                text = text.lower()
            if self.replace_urls and url_extractor is not None:
                urls = list(set(url_extractor.find_urls(text)))
                urls.sort(key=lambda url: len(url), reverse=True)
                for url in urls:
                    text = text.replace(url, " URL ")
            if self.replace_numbers:
                text = re.sub(r'\d+(?:\.\d*(?:[eE]\d+))?', 'NUMBER', text)
            if self.remove_punctuation:
                text = re.sub(r'\W+', ' ', text, flags=re.M)
            word_counts = Counter(text.split())
            if self.stemming and stemmer is not None:
                stemmed_word_counts = Counter()
                for word, count in word_counts.items():
                    stemmed_word = stemmer.stem(word)
                    stemmed_word_counts[stemmed_word] += count
                word_counts = stemmed_word_counts
            X_transformed.append(word_counts)
        return np.array(X_transformed)
class WordCounterToVectorTransformer(BaseEstimator, TransformerMixin):
    def __init__(self, vocabulary_size=1000):
        self.vocabulary_size = vocabulary_size
    def fit(self, X, y=None):
        total_count = Counter()
        for word_count in X:
            for word, count in word_count.items():
                total_count[word] += min(count, 10)
        most_common = total_count.most_common()[:self.vocabulary_size]
        self.most_common_ = most_common
        #self.vocabulary_ = {word: index + 1 for index, (word, count) in enumerate(most_common)}
        global vac
        self.vocabulary_ = vac
        return self
    def transform(self, X, y=None):
        rows = []
        cols = []
        data = []
        for row, word_count in enumerate(X):
            for word, count in word_count.items():
                rows.append(row)
                cols.append(self.vocabulary_.get(word, 0))
                data.append(count)
        return csr_matrix((data, (rows, cols)), shape=(len(X), self.vocabulary_size + 1))
#打开邮件进行预处理
def openemail(emails):
    f = open(emails,"rb")
    a = [email.parser.BytesParser(policy=email.policy.default).parse(f)]
    preprocess_pipeline = Pipeline([
        ("email_to_wordcount", EmailToWordCounterTransformer()),
        ("wordcount_to_vector", WordCounterToVectorTransformer()),
    ])
    
    X_train_transformed = preprocess_pipeline.fit_transform(a)
    return X_train_transformed

def openemail2(url):
    a = [email.parser.BytesParser(policy=email.policy.default).parse(url)]
    preprocess_pipeline = Pipeline([
        ("email_to_wordcount", EmailToWordCounterTransformer()),
        ("wordcount_to_vector", WordCounterToVectorTransformer()),
    ])
    X_train_transformed = preprocess_pipeline.fit_transform(a)
    return X_train_transformed

def of(file):
        f = open(file,"rb")
        a = email.parser.BytesParser(policy=email.policy.default).parse(f)
        return a
def openemail3(filearray):
    b = [of(file) for file in filearray]
    preprocess_pipeline = Pipeline([
        ("email_to_wordcount", EmailToWordCounterTransformer()),
        ("wordcount_to_vector", WordCounterToVectorTransformer()),
    ])
    X_train_transformed = preprocess_pipeline.fit_transform(b)
    return X_train_transformed

