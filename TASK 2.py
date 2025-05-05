import re
import numpy as np
import nltk
import tkinter as tk
from tkinter import scrolledtext

from sklearn.metrics.pairwise import cosine_similarity
from sklearn.feature_extraction.text import TfidfVectorizer

from nltk.corpus import stopwords
from nltk.tokenize import RegexpTokenizer
from nltk.stem.snowball import SnowballStemmer

# Load and preprocess data
data = open('DATA.txt', 'r')
Queries = data.readlines()

response = open('Responses.txt', 'r')
Responses = response.readlines()

tokenizer = RegexpTokenizer(r'\w+')
en_stopwords = set(stopwords.words('english'))
sno = SnowballStemmer('english')

def getcleanedtext(text):
    text = text.lower()
    tokens = tokenizer.tokenize(text)
    new_tokens = [token for token in tokens if token not in en_stopwords]
    stemmed_tokens = [sno.stem(token) for token in new_tokens]
    clean_text = " ".join(stemmed_tokens)
    return clean_text

queries = [getcleanedtext(i) for i in Queries]

# TF-IDF Vectorization
tfidf = TfidfVectorizer()
tfidf_matrix = tfidf.fit_transform(queries)

def get_response(user_input):
    cleaned_input = getcleanedtext(user_input)
    query_tfidf = tfidf.transform([cleaned_input])
    similarity = cosine_similarity(query_tfidf, tfidf_matrix)
    response_index = np.argmax(similarity)
    return Responses[response_index].strip()

# UI using Tkinter

def send_message():
    user_input = entry.get()
    if user_input.strip() == "":
        return
    chat_window.insert(tk.END, "You: " + user_input + "\n", 'user')

    if user_input.lower() == "bye":
        chat_window.insert(tk.END, "Chatbot: Bye 👋\n", 'bot')
        entry.delete(0, tk.END)
        return

    if user_input.lower() == "okay":
        chat_window.insert(tk.END, "Chatbot: Thank you! 😊\n", 'bot')
    elif "how are you" in user_input.lower():
        chat_window.insert(tk.END, "Chatbot: I'm good, thank you! How are you? How can I help you today? 😊\n", 'bot')
    else:
        bot_response = get_response(user_input)
        chat_window.insert(tk.END, "Chatbot: " + bot_response + "\n", 'bot')

    entry.delete(0, tk.END)

# Tkinter window setup
root = tk.Tk()
root.title("Modern FAQ Chatbot")
root.geometry("600x500")
root.configure(bg='#1e1e2f')

chat_window = scrolledtext.ScrolledText(root, wrap=tk.WORD, width=70, height=20, font=("Segoe UI", 11), bg="#2c2f33", fg="white", insertbackground="white")
chat_window.pack(padx=10, pady=10)
chat_window.tag_config('user', foreground='lightblue')
chat_window.tag_config('bot', foreground='lightgreen')
chat_window.insert(tk.END, "Chatbot: Hello! I am your chatbot 🤖. How can I help you today?\n\n", 'bot')

entry = tk.Entry(root, width=60, font=("Segoe UI", 12), bg="#3a3f4b", fg="white", insertbackground="white")
entry.pack(padx=10, pady=5)
entry.bind("<Return>", lambda event: send_message())

send_button = tk.Button(root, text="Send", command=send_message, font=("Segoe UI", 12), bg="#4caf50", fg="white", activebackground="#45a049")
send_button.pack(pady=5)

root.mainloop()
