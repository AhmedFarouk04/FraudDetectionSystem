using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FraudDetectionSystem.Entities;

namespace FraudDetectionSystem.Services
{
    public class DataStorageService
    {
        private readonly string _filePath;

        public DataStorageService(string filePath)
        {
            _filePath = filePath;
        }

        // 🟢 تحميل المستخدمين والمعاملات من الملف
        public List<User> LoadData()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("[INFO] No existing data file found. Starting fresh.");
                    return new List<User>();
                }

                string json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<User>();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var users = JsonSerializer.Deserialize<List<User>>(json, options) ?? new List<User>();

                // ✅ إصلاح العلاقة بين المستخدم والمعاملات بعد التحميل
                int totalTransactions = 0;
                foreach (var user in users)
                {
                    foreach (var tx in user.Transactions)
                    {
                        tx.User = user; // ← إعادة الربط بين المعاملة وصاحبها
                        totalTransactions++;
                    }
                }

                Console.WriteLine($"[INFO] Loaded {users.Count} user(s) and {totalTransactions} transaction(s) from data file.");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load data: {ex.Message}");
                return new List<User>();
            }
        }

        // 💾 حفظ المستخدمين والمعاملات في ملف JSON
        public void SaveData(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // يجعل JSON منسق وواضح
                };

                string json = JsonSerializer.Serialize(users, options);
                File.WriteAllText(_filePath, json);

                int totalTransactions = users.Sum(u => u.Transactions.Count);
                Console.WriteLine($"[INFO] Saved {users.Count} user(s) and {totalTransactions} transaction(s) to data file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to save data: {ex.Message}");
            }
        }
    }
}
