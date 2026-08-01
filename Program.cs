// Final Project - Budget Tracker
// Ileana Gonzalez, 07/31/2026
// This program allows the user to record input and expenses, view their balance and transaction history,
// search or delete transactions, generate monthly reports and save transaction information to a text file.   

using System;
using System.Diagnostics;
class Project
{
    static void Main()
    {
        // Runs automatic tests cases before starting the program
        RunTests();

        int choice;
        decimal balance = 0;

        Console.Clear();
        Console.Write("Welcome!\nTo acces your budget tracker, please enter your name: ");
        string userName = Console.ReadLine()!;

        string fileName = userName + "_transactions.txt";

        // Load saved transactions for selected user
        List<Transaction> transactions = new List<Transaction>();
        LoadTransactions(transactions, ref balance, fileName);

        // Keeps showing the menu until the user chooses exit
        do
        {
            Console.Clear();

            Console.WriteLine("====== Budget Tracker ======\n");
            Console.WriteLine("1. Add Income");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. View Balance");
            Console.WriteLine("4. View Transactions");
            Console.WriteLine("5. Search Transactions");
            Console.WriteLine("6. Delete Transactions");
            Console.WriteLine("7. Monthly Report");
            Console.WriteLine("8. Save Transactions");
            Console.WriteLine("9. Exit\n");

            choice = GetValidIntegrer("Choice: ");

            // Only allows a choice from the menu
            while (choice < 1 || choice > 9)
            {
                Console.WriteLine("Please choose an option from the menu.\n");
                choice = GetValidIntegrer("Choice: ");

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
                    SearchTransactions(transactions);
                    break;
                case 6:
                    DeleteTransaction(transactions, ref balance);
                    break;
                case 7:
                    MonthlyReport(transactions);
                    break;
                case 8:
                    SaveTransactions(transactions, fileName);
                    break;
                case 9:
                    SaveTransactions(transactions, fileName);
                    Console.WriteLine();
                    Console.WriteLine("Session Ended.");
                    break;

            }
            if (choice != 9)
            {
                Console.Write("\nPress enter to return to menu");
                Console.ReadLine();
            }

        } while (choice != 9);
    }

    // Adds a new income transaction and updates the current balance
    static void AddIncome(ref decimal balance, List<Transaction> transactions)
    {
        decimal income = GetValidAmount("Enter income amount: $");

        Console.Write("Enter a description: ");
        string description = Console.ReadLine()!;

        Transaction newTransaction = new Transaction();

        newTransaction.Type = "Income";
        newTransaction.Description = description;
        newTransaction.Amount = income;
        newTransaction.Date = DateTime.Now;

        // Creates a new transaction and stores it on the list
        transactions.Add(newTransaction);

        balance += income;
        Console.WriteLine($"\nIncome of ${income:F2} added succesfully.");
    }

    // Adds a new expense transaction and substracts it from the balance
    static void AddExpense(ref decimal balance, List<Transaction> transactions)
    {
        decimal expense = GetValidAmount("Enter expense amount: $");

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

    // Displays user's current balance
    static void ViewBalance(decimal balance)
    {
        Console.WriteLine($"Current balance: ${balance:F2}");
    }

    // Displays all transactions stored on the list
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
                Console.WriteLine("--------------------------");
                Console.WriteLine($"Type: {transaction.Type}");
                Console.WriteLine($"Description: {transaction.Description}");
                Console.WriteLine($"Amount: ${transaction.Amount:F2}");
                Console.WriteLine($"Date: {transaction.Date}");
            }
        }
    }

    // Saves all transactions from the list on the users text file
    static void SaveTransactions(List<Transaction> transactions, string fileName)
    {
        StreamWriter file = new StreamWriter(fileName);

        foreach (Transaction transaction in transactions)
        {
            file.WriteLine($"{transaction.Type},{transaction.Description},{transaction.Amount},{transaction.Date}");
        }

        file.Close();

        Console.WriteLine("Transactions saved succesfully.");
    }

    // Loads saved transactions from the file and recalculates the balance for specific user
    static void LoadTransactions(List<Transaction> transactions, ref decimal balance, string fileName)
    {
        if (!File.Exists(fileName))
        {
            return;
        }

        StreamReader file = new StreamReader(fileName);

        while (!file.EndOfStream)
        {
            string line = file.ReadLine()!;

            // Separates each saved line into the transaction properties
            string[] parts = line.Split(',');

            Transaction transaction = new Transaction();

            transaction.Type = parts[0].Trim();
            transaction.Description = parts[1].Trim();
            transaction.Amount = Convert.ToDecimal(parts[2].Trim());
            transaction.Date = Convert.ToDateTime(parts[3].Trim());

            transactions.Add(transaction);

            // Adds income or substract expenses when rebuilding the balance
            if (transaction.Type == "Income")
            {
                balance += transaction.Amount;
            }
            else if (transaction.Type == "Expense")
            {
                balance -= transaction.Amount;
            }
        }
        file.Close();
    }

    // Calculates income, expenses and balnce for a selected month and year
    static void MonthlyReport(List<Transaction> transactions)
    {
        int month = GetValidIntegrer("Enter wished month numebr for the report (1-12): ");
        while (!IsValidMonth(month))
        {
            Console.WriteLine("Please enter a valid month.\n");
            month = GetValidIntegrer("Enter wished month numebr for the report (1-12): ");
        }
        int year = GetValidIntegrer("Enter year: ");

        decimal totalIncome = 0;
        decimal totalExpense = 0;
        int transactionCount = 0;

        foreach (Transaction transaction in transactions)
        {
            // Only includes transactions from the selected month and year
            if (transaction.Date.Month == month && transaction.Date.Year == year)
            {
                transactionCount++;

                if (transaction.Type == "Income")
                {
                    totalIncome += transaction.Amount;
                }
                else if (transaction.Type == "Expense")
                {
                    totalExpense += transaction.Amount;
                }
            }
        }
        decimal monthBalance = CalculateBalance(totalIncome, totalExpense);

        Console.WriteLine($"\n=== Montlhy Report for {month}/{year} ===");
        Console.WriteLine($"{"Total Income:",-18} ${totalIncome:F2}");
        Console.WriteLine($"{"Total Expenses:",-18} ${totalExpense:F2}");
        Console.WriteLine($"{"Monthly Balance:",-18} ${monthBalance:F2}");
        Console.WriteLine($"{"Transactions:",-18} {transactionCount}");
        Console.WriteLine("==================================");
    }

    // Searches transaction descriptions asking for input from the user
    static void SearchTransactions(List<Transaction> transactions)
    {
        Console.Write("Enter a description to look for: ");
        string search = Console.ReadLine()!;

        bool found = false;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Description.ToLower().Contains(search.ToLower()))
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine($"Type: {transaction.Type}");
                Console.WriteLine($"Description: {transaction.Description}");
                Console.WriteLine($"Amount: ${transaction.Amount:F2}");
                Console.WriteLine($"Date: {transaction.Date}");

                found = true;
            }
        }
        if (!found)
        {
            Console.WriteLine("No matching transactions were found");
        }
    }

    // Deletes a selected transactions and updates the current balance
    static void DeleteTransaction(List<Transaction> transactions, ref decimal balance)
    {
        if (transactions.Count == 0)
        {
            Console.WriteLine("There are no transactions to delete.");
            return;
        }

        Console.WriteLine("Transactions List: ");

        for (int i = 0; i < transactions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {transactions[i].Type,-8} {transactions[i].Description,-20} {transactions[i].Amount:F2}");
        }

        Console.Write("\nEnter the transaction number to delete: ");
        int transactionNumber = Convert.ToInt32(Console.ReadLine());

        // Converts the displayed transaction number to its list index
        int index = transactionNumber - 1;

        if (index >= 0 && index < transactions.Count)
        {
            Transaction deletedTransaction = transactions[index];

            if (deletedTransaction.Type == "Income")
            {
                balance -= deletedTransaction.Amount;
            }
            else if (deletedTransaction.Type == "Expense")
            {
                balance += deletedTransaction.Amount;
            }

            transactions.RemoveAt(index);

            Console.WriteLine("Transaction deleted succesfully.");
        }
        else
        {
            Console.WriteLine("Invalid transaction number.");
        }
    }

    // Gets and validates an integrer entered by the user, prevents invalid input and crashing of the program
    static int GetValidIntegrer(string message)
    {
        int number;

        Console.Write(message);

        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid input. Please enter a valid integrer.\n");
            Console.Write(message);
        }
        return number;
    }

    // Gets a positive decimal amount and prevents invalid input
    static decimal GetValidAmount(string message)
    {
        decimal amount;

        Console.Write(message);

        while (true)
        {
            if (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid input. Please enter a valid amount.\n");
                Console.Write(message);
            }
            else if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than 0.\n");
                Console.Write(message);
            }
            else
            {
                break;
            }
        }
        return amount;
    }

    // Runs automatic test for methods used in the budget tracker
    static void RunTests()
    {
        Debug.Assert(CalculateBalance(1000m, 250m) == 750);
        Debug.Assert(CalculateBalance(500m, 0m) == 500m);

        Debug.Assert(IsValidMonth(1) == true);
        Debug.Assert(IsValidMonth(12) == true);
        Debug.Assert(IsValidMonth(0) == false);
        Debug.Assert(IsValidMonth(13) == false);

        Console.WriteLine("Automatic tests completed succesfully.");
    }

    // Calculates the remaining balance using income and expenses
    static decimal CalculateBalance(decimal income, decimal expenses)
    {
        return income - expenses;
    }

    // Checks wether a month number is between 1 and 12
    static bool IsValidMonth(int month)
    {
        return month >= 1 && month <= 12;
    }
}