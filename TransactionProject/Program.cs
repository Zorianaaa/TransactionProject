/*Розробка програми для керування особистими фінансами:
Створіть програму, яка дозволяє користувачам вести облік своїх доходів та витрат.
Додайте функціонал для аналізу фінансової ситуації користувача (сумарний дохід, витрати за період тощо).
Забезпечте можливість зберігання та відновлення даних про фінанси користувача.*/

using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Channels;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace HomeWorkConsoleApp
{

    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsIncome { get; set; }
    }
    internal class Program
    {
        private static decimal earnings = 0; //  для зберігання отриманих доходів
        private static decimal spentMoney = 0; // для зберігання витрат
        private static decimal yourMoney = 0; // для зберігання отриманих і витрачених коштів
        private static decimal currentSum = 0; // для зберігання поточної суми на рахунку

        private static List<Transaction> transactions = new List<Transaction>();
        static void Main(string[] args)
        {
            ShowUI();
        }

        private static void ShowUI()
        {

            try
            {
                while (true)
                {
                    Console.WriteLine("Натисніть цифру, яка відповідає за обрану функцію: ");
                    Console.WriteLine("1 -> Ваш заробiток");
                    Console.WriteLine("2 -> Ваш розхiд");
                    Console.WriteLine("3 -> Аналiз стану рахунку");
                    Console.WriteLine("4 -> Збереження даних");
                    Console.WriteLine("5 -> Поради щодо управлiння фiнансами ");
                    Console.WriteLine("6 -> Очистити внесені дані");
                    Console.WriteLine("7 -> Вихiд");

                    int input = int.Parse(Console.ReadLine());

                    if (input < 1 || input > 7)
                    {
                        Console.WriteLine("Введений символ не вiдповiдає елементам меню. Спробуйте ввести iнший символ.");
                    }

                    switch (input)
                    {
                        case 1:
                            YourEarnings();
                            break;
                        case 2:
                            YourExpense();
                            break;
                        case 3:
                            AnalysisOfAccountStatus();
                            break;
                        case 4:
                            DataSaving("trasaction.txt");
                            break;
                        case 5:
                            Advice();
                            break;
                        case 6:
                            ClearAll();
                            break;
                        case 7:
                            Exit();
                            return;
                        default:
                            Console.WriteLine("Введений символ не відповідає елементам меню. Спробуйте ввести інший символ.");
                            break;
                    }
                    Console.WriteLine("Натиснiть будь-яку клавiшу, щоб продовжити...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private static void YourEarnings()
        {

            try
            {

                if (currentSum == 0)
                {
                    Console.WriteLine("Введiть суму, яка є на вашому рахунку на даний момент: ");

                    while (!decimal.TryParse(Console.ReadLine(), out currentSum))
                    {
                        Console.WriteLine("Невiрний формат. Будь ласка, введiть коректне число: ");
                    }
                }
                else
                {

                    Console.WriteLine("Введiть суму сьогоднiшнього заробiтку: ");
                    decimal newMoney;

                    while (!decimal.TryParse(Console.ReadLine(), out newMoney))
                    {
                        Console.WriteLine("Невiрний формат. Будь ласка, введiть коректне число: ");
                    }

                    earnings = currentSum + newMoney;

                    if (earnings == 0)
                    {
                        Console.WriteLine("Ваш рахунок залишився без змiн або рiвний нулю.");

                    }
                    else if (earnings < 0)
                    {
                        Console.WriteLine($"На вашому рахунку борг - {earnings}");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"Ваш рахунок збiльшився на {newMoney} ");
                    }
                    currentSum = earnings;
                    transactions.Add(new Transaction { Amount = newMoney, DateTime = DateTime.Now, IsIncome = true });

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void YourExpense()
        {
            try
            {
                Console.WriteLine("Введiть суму, яку ви витратили");

                while (!decimal.TryParse(Console.ReadLine(), out spentMoney))
                {
                    Console.WriteLine("Невiрний формат. Будь ласка, введiть коректне число: ");
                }

                if (spentMoney == 0)
                {
                    Console.WriteLine("У вас не було витрат.");
                }
                else if (spentMoney > 0)
                {
                    Console.WriteLine($"Ви витратили: {spentMoney}");
                }
                else
                {
                    Console.WriteLine("Введiть 0 якщо не витрачали сьогоднi, або додатню суму витрат.");
                }
                transactions.Add(new Transaction { Amount = spentMoney, DateTime = DateTime.Now, IsIncome = false });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void AnalysisOfAccountStatus()
        {
            try
            {
                decimal earning = 0;
                decimal expense = 0;

                foreach (var transaction in transactions)
                {
                    if (transaction.IsIncome)
                        earning += transaction.Amount;
                    else
                        expense += transaction.Amount;
                }

                decimal yourMoney = earning - expense;

                Console.WriteLine($"Ваш поточний стан рахунку: {currentSum + earning - spentMoney}");
                Console.WriteLine($"Ви заробили: {earning}, а витратили: {expense}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void DataSaving(string trasaction)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(trasaction))
                {
                    writer.WriteLine($"earnings:{earnings}");
                    writer.WriteLine($"spentMoney:{spentMoney}");
                    writer.WriteLine($"yourMoney:{yourMoney}");
                }

                Console.WriteLine("Данi успiшно збережено у файлi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка при збереженні даних: {ex.Message}");
            }

        }
        private static void Advice()
        {
            try
            {
                string[] advice = { "Щастя не в грошах", "Все треба вмiти i нiчого не робити, спрости собi життя",
                "Краще бiльше друзiв, нiж сто гривень", "Всiх грошей не заробиш, але спробувати треба", "За грошi кохання не купиш",
                "Життя – гра, а грошi – спосiб вести рахунок", "Грошi – як гнiй: якщо їх не розкидати, від них буде мало користi",
                "Працюйте так, немов грошi не мають для вас жодного значення", "Час i грошi здебiльшого взаємозамiннi",
                "Шилом моря не нагрiєш – вiд тяжкої роботи не розбагатiєш, насолоджуйся моментом",
                "Щоб заробити великi статки, потрiбна велика смiливiсть i велика обережнiсть"};

                Random random = new Random();
                int index = random.Next(advice.Length);
                string randomAdvice = advice[index];

                Console.WriteLine($"Порада для вас:\n {randomAdvice}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        private static void ClearAll()
        {
            earnings = 0;
            spentMoney = 0;
            yourMoney = 0;

            Console.WriteLine("Всi збереженi данi очищено.");
        }
        private static void Exit()
        {
            Console.WriteLine("До зустiчi!");
        }


    }
}
