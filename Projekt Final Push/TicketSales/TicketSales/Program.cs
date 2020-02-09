using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    #region initialization
    string path = @"..\..\..\Transactions.txt";
    string path2 = @"..\..\..\Bokföring.txt";

    List<Transaction> transactions = new List<Transaction>();

    public int CurrentID = 1000;
    #endregion
    static void Main()
    {
        Program p = new Program();
        p.Run();
    }
    void Run()
    {
        using (StreamWriter sw = File.AppendText(path)) { }
        Load(); 
        Intro();
        Commands();
    }

    private void Intro()
    {
        Console.WriteLine("TICKET REGISTER\n");
        Console.WriteLine("  <Type> ");
        Console.WriteLine("'buy' to create a transaction");
        Console.WriteLine("'refund' to refund a transaction");
        Console.WriteLine("'count' to view total number of tickets sold");
        Console.WriteLine("'sum' to view total income of tickets sold");
        Console.WriteLine("'clear' to clear up terminal (data is saved)");
        Console.WriteLine("'quit' to exit program and save data to file\n");
        Console.WriteLine("PLEASE DON'T FORGET TO QUIT THE PROGRAM AFTER USE TO SAVE DATA\n");
    }

    private void Commands()
    {
        while (true)
        {
            Console.WriteLine("Enter command: ");
            string input = Console.ReadLine().ToLower();

            if (input == "clear")
                Clear();
            else if (input == "buy")
                Buy();
            else if (input == "refund")
                Refund();
            else if (input == "count")
                DrawCount();
            else if (input == "sum")
                DrawIncomeSum();
            else if (input == "quit")
            {
                Save();
                SaveBokföring();
                Quit();
            }
            else
                Console.WriteLine("Unknown Command");
        }
    }

    private void Buy()
    {
        Console.WriteLine("Tonights Concerts:");
        Console.WriteLine("1. Johannes Brahms");
        Console.WriteLine("2. Wolfgang Amadeus Mozart");
        Console.WriteLine("3. Edvard Grieg");

        int concert = Concert();
        if (concert == 0)
        {
            Clear();
            Console.WriteLine("Cancelled transaction");
            return;
        }

        while (true)
        {
            try
            {
                #region ticket specification
                //Specifies adult tickets
                Console.WriteLine("Enter in the amount of adult tickets for this transaction, '0' if none");
                string input = Console.ReadLine();

                if (input == "cancel")
                { Clear(); Console.WriteLine("Cancelled transaction"); return; }

                int adults = int.Parse(input);
                if (adults > 2000)
                {
                    Console.WriteLine("Too many tickets requested");
                    continue;
                }

                //specifies child tickets
                Console.WriteLine("Enter in the amount of child tickets for this transaction, '0' if none");
                string input2 = Console.ReadLine();

                if (input == "cancel")
                { Clear(); Console.WriteLine("Cancelled transaction"); return; }

                int children = int.Parse(input2);
                if (children > 2000)
                {
                    Console.WriteLine("Too many tickets requested");
                    continue;
                }

                //specifies senior tickets
                Console.WriteLine("Enter in the amount of senior tickets for this transaction, '0' if none");
                string input3 = Console.ReadLine();

                if (input == "cancel")
                { Clear(); Console.WriteLine("Cancelled transaction"); return; }

                int seniors = int.Parse(input3);
                if (seniors > 2000)
                {
                    Console.WriteLine("Too many tickets requested");
                    continue;
                }

                if (adults + children + seniors == 0)
                {
                    Console.WriteLine("Minimun ticket amount: 1");
                    continue;
                }
                #endregion

                //creates transaction
                CurrentID++;
                Transaction transaction = new Transaction(CurrentID, adults, children, seniors, false);
                transactions.Add(transaction);
                transaction.SetPrice(adults * 100 + children * 25 + seniors * 75);
                transaction.SetShowNumber(concert);

                Console.WriteLine("Transaction added! " + adults + " adult tickets, " + children + " child tickets, and "
                    + seniors + " senior tickets to consert nr " + concert + ".\nPrice: " + transaction.GetPrice() + "kr," + " Order number: " + CurrentID);
                return;

            }
            catch
            {
                Console.WriteLine("Please specify tickets with whole numbers");
            }
        }
    }

    private int Concert()
    {
        while (true)
        {
            Console.WriteLine("Select consert (1-3), or 0 to cancel transaction");
            string consert = Console.ReadLine();

            try
            {
                int realConsert = int.Parse(consert);

                if (realConsert < 0 || realConsert > 3)
                {
                    Console.WriteLine("There is no consert tonight with that index");
                    continue;
                }
                return (realConsert);
            }
            catch
            {
                Console.WriteLine("Please specify the consert with a whole number");
            }
        }
    }

    private void DrawCount()
    {
        if (transactions.Count == 0)
        {
            Console.WriteLine("No tickets sold");
            return;
        }
        Console.WriteLine("\nTickets sold: \n" + "(" + Count().adults + ")" + " adult tickets\n"
            + "(" + Count().children + ")" + " child tickets\n" + "(" + Count().seniors + ")" + " senior tickets" +
            "\nTotal tickets sold: " + "(" + Count().tot + ")\n");
    }

    private (int adults, int children, int seniors, int tot) Count()
    {
        int adultCount = 0;
        int childCount = 0;
        int seniorCount = 0;

        if (transactions.Count == 0)
        {
            return (0, 0, 0, 0);
        }

        foreach (Transaction transaction in transactions)
        {
            bool refund = transaction.GetRefunded();
            if (!refund)
            {
                adultCount += transaction.GetAdult();
                childCount += transaction.GetChild();
                seniorCount += transaction.GetSenior();
            }
        }
        int totCount = adultCount + childCount + seniorCount;
        return (adultCount, childCount, seniorCount, totCount);
    }

    private void DrawIncomeSum()
    {
        if (transactions.Count == 0)
        {
            Console.WriteLine("No tickets sold");
            return;
        }
        Console.WriteLine("\nTotal income from tickets sold: " + IncomeSum() + "kr\n");
    }

    private int IncomeSum()
    {
        int sum = 0;

        if (transactions.Count == 0)
        {
            return 0;
        }
        foreach (Transaction transaction in transactions)
        {
            bool refund = transaction.GetRefunded();
            if (!refund)
            {
                sum += transaction.GetPrice();
            }
        }
        return sum;
    }

    void Save()
    {
        System.IO.File.WriteAllText(path, "");
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine("<Transactions in register>\n(order number, show nr, number of tickets type: adults, children, seniors, price (kr), if refunded or not: true/false)");
            foreach (Transaction transaction in transactions)
                sw.WriteLine(transaction.ToString());
        }
    }

    void SaveBokföring()
    {
        System.IO.File.WriteAllText(path2, "");
        using (StreamWriter sw = new StreamWriter(path2, true))
        {
            sw.WriteLine("Tickets sold: \n" + "(" + Count().adults + ")" + " adult tickets\n"
            + "(" + Count().children + ")" + " child tickets\n" + "(" + Count().seniors + ")" + " senior tickets" +
            "\nTotal tickets sold: " + "(" + Count().tot + ")\n");

            sw.WriteLine("Total income from tickets sold: " + IncomeSum() + "kr");
        }
    }

    void Load()
    {
        using (StreamReader sr = new StreamReader(path, true))
        {
            //skip appropriate amounts of lines
            sr.ReadLine();
            sr.ReadLine();

            string text;
            int ID = 1000;
            while ((text = sr.ReadLine()) != null)
            {
                string[] strings = text.Split(char.Parse(","));
                ID = int.Parse(strings[0]);
                int adults = int.Parse(strings[2]);
                int children = int.Parse(strings[3]);
                int seniors = int.Parse(strings[4]);
                string refunded = strings[6];
                bool realRefunded = bool.Parse(refunded);

                Transaction transaction = new Transaction(ID, adults, children, seniors, realRefunded);
                transaction.SetPrice(adults * 100 + children * 25 + seniors * 75);
                if (refunded == "True")
                    transaction.SetRefunded(true);

                transactions.Add(transaction);

            }
            CurrentID = ID;
        }
    }

    private void Refund()
    {
        while (true)
        {
            Console.WriteLine("Enter an order number:");
            string input = Console.ReadLine();

            try
            {
                int realID = int.Parse(input);
                for (int i = 0; i < transactions.Count; i++)
                {
                    if (realID == transactions[i].GetID() && transactions[i].GetRefunded() == true)
                    {
                        Console.WriteLine("This transaction has already been refunded");
                        return;
                    }
                }

                bool found = false;
                for (int i = 0; i < transactions.Count; i++)
                {
                    if (realID == transactions[i].GetID())
                    {
                        transactions[i].SetRefunded(true);
                        Console.WriteLine("You found: " + transactions[i].GetID() + " and refunded it!");
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("There was no transaction with that order number\n");
                }
                return;
            }
            catch
            {
                Console.WriteLine("Order numbers must be whole numbers");
            }
        }
    }

    private void Clear()
    {
        Console.Clear();
        Intro();
    }

    private void Quit()
    {
        Environment.Exit(0);
    }
}
