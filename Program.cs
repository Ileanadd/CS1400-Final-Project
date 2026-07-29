// Final Project
// Budget tracker

using System;
using System.Collections.Generic;
using System.IO;

class Project
{
    static void Main()
    {
        int choice;
        decimal balance = 0;

        List<Transaction> transactions = new List<Transaction>();
        LoadTransactions(transactions, ref balance);

        do
        {
            Console.Clear();

            Console.WriteLine("Budget Tracker\n");
            Console.WriteLine("1. Add Income");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. View Balance");
            Console.WriteLine("4. View Transactions");
            Console.WriteLine("5. Save Transactions");
            Console.WriteLine("6. Exit\n");

            Console.Write("Choice: ");
            choice = Convert.ToInt32(Console.ReadLine());

            if (choice < 1 || choice > 6)
            {
                Console.WriteLine("Choose an option from the menu.");
            }

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddIncome(ref balance, transactions);
                    break;
                case 2:
                    AddExpense(ref balance, transactions);
                    break;
                case 3:
                    ViewBalance(balance);
                    break;
                case 4:
                    ViewTransactions(transactions);
                    break;
                case 5:
                    SaveTransactions(transactions);
                    break;
                case 6:
                    Console.WriteLine("\nSession Ended");
                    break;

            }
            if (choice != 6)
            {
                Console.Write("\nPress enter to return to menu");
                Console.ReadLine();
            }

        } while (choice != 6);
    }
    static void AddIncome(ref decimal balance, List<Transaction> transactions)
    {
        Console.Write("Enter income amount: $");
        decimal income = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Enter a description: ");
        string description = Console.ReadLine()!;

        Transaction newTransaction = new Transaction();

        newTransaction.Type = "Income";
        newTransaction.Description = description;
        newTransaction.Amount = income;
        newTransaction.Date = DateTime.Now;

        transactions.Add(newTransaction);

        balance += income;
        Console.WriteLine($"\nIncome of ${income:F2} added succesfully.");
    }

    static void AddExpense(ref decimal balance, List<Transaction> transactions)
    {
        Console.Write("Enter expense amount: $");
        decimal expense = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Description: ");
        string description = Console.ReadLine()!;

        Transaction newTransaction = new Transaction();

        newTransaction.Type = "Expense";
        newTransaction.Description = description;
        newTransaction.Amount = expense;
        newTransaction.Date = DateTime.Now;

        transactions.Add(newTransaction);

        balance -= expense;
        Console.WriteLine($"\nExpense of ${expense:F2} recorded succesfully.");
    }

    static void ViewBalance(decimal balance)
    {
        Console.WriteLine($"Current balance: ${balance}");
    }

    static void ViewTransactions(List<Transaction> transactions)
    {
        Console.WriteLine("Transaction history: \n");

        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions recorded.");
        }
        else
        {
            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine("-------------------");
                Console.WriteLine($"Type: {transaction.Type}");
                Console.WriteLine($"Description: {transaction.Description}");
                Console.WriteLine($"Amount: ${transaction.Amount}");
                Console.WriteLine($"Date: {transaction.Date}");
            }
        }
    }

    static void SaveTransactions(List<Transaction> transactions)
    {
        StreamWriter file = new StreamWriter("transactions.txt");

        foreach (Transaction transaction in transactions)
        {
            file.WriteLine($"{transaction.Type}, {transaction.Description}, {transaction.Amount}, {transaction.Date}");
        }

        file.Close();

            Console.WriteLine("TRansactions saved succesfully.");
    }

    static void LoadTransactions(List<Transaction> transactions, ref decimal balance)
    {

    }
}
