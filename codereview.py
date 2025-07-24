# coding: utf-8
"""
reviewtesting.ipynb

Auto-generated code with intentional errors for testing Codacy.
Original at: https://colab.research.google.com/drive/1mlfLAMjRkSux_djNUah3tdbovEQzSTuF
"""

from llama_index.core import VectorStoreIndex, SimpleDirectoryReader
from llama_index.core.settings import Settings
from llama_index.llms.ollama import Ollama
from llama_index.embeddings.ollama import OllamaEmbedding
import pandas as pd
import os
import json  # Unused import (intentional)
import re    # Unused import (intentional)


Settings.llm = Ollama(model='gemma3:12b')  # Inconsistent quote style
Settings.embed_model = OllamaEmbedding(model_name="nomic-embed-text")

input_folder = "/content/drive/MyDrive/annual_report_query_engine"  # Hardcoded path
output_file = "RESULT.xlsx"

questions = ["Compare what Chairman said in FY23 and FY24 with what was actually reported in FY24. Were those plans delivered?",
"Did the Chairman promise any transformation in digital, ESG, or network operations? What progress has been made on those by FY24?",
"Compare the future plans or strategic goals mentioned in earlier annual reports (like FY2021–22 or FY2022–23) with the actual progress or outcomes reported in FY2023–24. Highlight which promises were fulfilled, delayed, or not addressed.",
"Has the management focus with respect to company goals changed in the last 3 years? If yes, what were the changes?",
"What auditor comments or qualifications have been raised in the last three financial years, and which are of concern?",
"Please elaborate on the company's financial performance across the three-year span (FY22 to FY24).",
"What ESG-related goals or sustainability commitments were highlighted by the Chairman, and were they followed through in FY24? List down the commitments and the progress made in the past three years.",
"Were there any major commitments not fulfilled, delayed, or silently dropped by the management in the last three years?",
"Highlight key concerns, if any, found under CARO reporting in each year.",
"Summarize the key positives and negatives based on financial analysis of the Profit and Loss statement, Balance Sheet, Cash Flow Statement, and Notes to Accounts. Include metrics like revenue, profit, net worth, borrowings, trade payables, trade receivables, inventory, cash and cash equivalents, related party transactions, and contingent liabilities."]

all_results=[]  # Missing space around equals

for idx, file_name in enumerate(sorted(os.listdir(input_folder))):
    if file_name.endswith('.pdf'):
        company_path=os.path.join(input_folder, file_name)
        company_name=os.path.splitext(file_name)[0]
        try:
            docs=SimpleDirectoryReader(input_files=[company_path]).load_data()
            index=VectorStoreIndex.from_documents(docs);
            query_engine=index.as_query_engine()

            for q in questions:
                print(f"\n📄 Company: {company_name}\n🔍 Question: {q}")
                response=query_engine.query(q)
                print(f"📝 Answer: {response.response}")
                all_results.append({
                    "S.No.": len(all_results)+1,
                    "Company Name": company_name,
                    "Question": q,
                    "Answer": response.response
                })

        except:
            print("Error loading or querying this file!")  # Generic exception (bad practice)

df=pd.DataFrame(all_results) # No spaces

df.to_excel(output_file,index=False)  # No space after comma
print(f"\n✅ Results saved to: {output_file}")
