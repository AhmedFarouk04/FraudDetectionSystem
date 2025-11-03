# 🕵️‍♂️ Fraud Detection System

### 🔍 Intelligent Transaction Monitoring & Risk Analysis (C# / .NET 8 Console Application)

---

## 📖 Overview

The **Fraud Detection System** is a console-based simulation built with **C# (.NET 8)** that analyzes financial transactions, detects suspicious activity, and provides real-time risk visualization using advanced fraud rules.

It’s designed to simulate how banking and fintech systems assess transaction risks based on:

- Transaction amount
- Geographical patterns
- Velocity of transactions
- Behavioral deviations
- Composite risk logic

---

## ⚙️ Key Features

| Feature                        | Description                                                                         |
| ------------------------------ | ----------------------------------------------------------------------------------- |
| 🧑‍💼 **Multi-User System**       | Manage multiple users with isolated transaction histories                           |
| 💳 **Transaction Analysis**    | Evaluate each transaction through active fraud rules                                |
| ⚠️ **Fraud Rules Engine**      | Includes High Amount, Country Mismatch, Velocity, GeoLocation, Night Activity, etc. |
| 📊 **Risk Scoring**            | Generates visual risk bars and colored indicators (Low/Medium/High)                 |
| 📁 **Persistent Data Storage** | Automatically saves users and transactions to `data.json`                           |
| 🧠 **Smart Reloading**         | Reloads all users and restores risk data from JSON file on startup                  |
| 💬 **Spectre.Console UI**      | Beautiful console UI with color, layout, and progress bars                          |
| 🧾 **Validation System**       | Ensures valid user and country inputs (no duplicates or numbers)                    |

---

## 🏗️ Project Structure

---

## 🧩 System Flow

1. **Startup:**
   - Loads users and transactions from `data.json`
   - If no users exist, prompts to create one

2. **Main Menu:**
   - Add User 🧑  
   - Switch User 🔄  
   - Add Transaction 💳  
   - View Dashboard 📊  
   - Exit ❌  

3. **On Transaction:**
   - Analyzes transaction using active fraud rules  
   - Displays visual “Risk Bar” and logs triggered rules  
   - Automatically saves data to file  

4. **Dashboard:**
   - Shows risk averages across users  
   - Displays alerts and trend indicators


---

## 🧮 Example Console Output

```plaintext
──────────────────────────────────────────────
Fraud Detection System
──────────────────────────────────────────────
Current User: Ahmed (Egypt)

Enter transaction for Ahmed (Egypt):
Amount: 45000
Country: Japan

Analyzing transaction...
Suspicious Transaction Detected!
Triggered Rules:
 - High Amount Rule
 - Country Mismatch Rule

██████████░░░░░░░░░░  50%  MEDIUM RISK
──────────────────────────────────────────────
End of transaction analysis




