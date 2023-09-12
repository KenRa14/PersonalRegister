using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace PersonalRegister
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            bool hideInstructions = false;
            bool showRecords = false;
            bool _continue = true;

            List<Staff> records = new();
            Console.WriteLine($"Staff Register\n{getInstructions(hideInstructions, showRecords)}\nrecord count: {records.Count}\n");
            do
            {
                Console.Clear();
                Console.WriteLine($"Staff Register\n{getInstructions(hideInstructions, showRecords)}\nrecord count: {records.Count}\n");

                if (!showRecords)
                {
                    Console.Write("Enter data: ");
                }
                else
                {
                    foreach (var item in records)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("\n*To enter records press CTR+S");
                }

                ConsoleKeyInfo cki;
                // Prevent example from ending if CTL+C is pressed.
                Console.TreatControlCAsInput = true;

                do
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        _continue = false;
                        break;
                    }

                    if (!showRecords && ((cki.Modifiers & ConsoleModifiers.Control) == 0))
                    {
                        string input = "";
                        if (cki.Key != ConsoleKey.Enter)
                        {
                            input += cki.KeyChar;
                            Console.Write(input);
                        }
                        input += Console.ReadLine();
                        input = input.Trim();

                        if (String.IsNullOrEmpty(input))
                            break;
                        
                        records.Add(getStaff(input));
                        break;
                    }
                    else
                    {
                        if (((cki.Modifiers & ConsoleModifiers.Control) != 0) && cki.Key == ConsoleKey.I)
                        {
                            hideInstructions = !hideInstructions;
                            break;
                        }
                        else if (((cki.Modifiers & ConsoleModifiers.Control) != 0) && cki.Key == ConsoleKey.S)
                        {
                            showRecords = !showRecords;
                            break;
                        }
                    }
                    
                } while (true);

            } while (_continue);
        }

        public static Staff getStaff(string line)
        {
            string[] words = line.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            string name = "";

            int i = 0;
            if (!words[0].StartsWith('"'))
            {
                name = words[0];
                i = 1;
            }
            else
            {
                for (; i < words.Length; i++)
                {
                    string word = words[i];

                    if (word.StartsWith('"'))
                    {
                        word = word[1..];
                    }
                    else if (word.EndsWith("\""))
                    {
                        name += word.Substring(0, word.Length-1);
                        break;
                    }
                    name += word;

                }
            }

            decimal salary = 0;

            if (i < words.Length)
            {
                decimal.TryParse(words[i], out salary);
            }
            
            return new Staff(name, salary);
            
        }

        public static string getInstructions(bool hideInstructions, bool showRecords)
        {
            string keyInstructions = $"*{(hideInstructions ? "To show instructions press CTR+I" : "To hide instructions press CTR+I")}" +
                $"{(showRecords ? "": ", to show records press CTR+S")}, to exit press ESC\n";
            if (hideInstructions)
            {
                return keyInstructions;
            }
            return "*Instructions" +
                "\n*Follow the next syntax inside []: [name salary] and then press enter." +
                "\n*if name is separated by white spaces, surround it with \"." +
                "\n*Examples:\n  Vita 1.200\n  \"Ash Ketchum\" 65.536,50\n" + keyInstructions;
        }
    }
    public class Staff
    {
        public Staff(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Salary: {Salary}";
        }

        public string Name { get; set; }
        public decimal Salary { get; set; }
    }
}