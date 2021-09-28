using System;
using Newtonsoft.Json;
using SB.SharedModels.Helpers;

namespace SB.ClosePhoneCall
{
    class Program
    {
        private static PhoneCallService _service;
        static void Main()
        {
            Write("This is an application that selects all open calls and, if necessary, closes them. \n", ConsoleColor.Yellow);

            Login();

            Start();
        }

        private static void Login()
        {
            try
            {
                Write("\nPlease authorization: \n");

                var name = EnterTheData("UserName");
                var password = EnterTheData("UserPassword");

                Write("\nAuthorization continues. Please, wait...\n");

                _service = new PhoneCallService(name, password);

                Write("\nSuccessfully connection \n", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Write($"\n{e.Message}\n", ConsoleColor.Red);

                char action;

                do
                {
                    Write("\nChose action:\n");
                    Write("R", ConsoleColor.Magenta);
                    Write(" - repeat login\n");
                    Write("E", ConsoleColor.Magenta);
                    Write(" - exit\n");
                    Write("\nAction: ", ConsoleColor.DarkCyan);

                    Console.ForegroundColor = ConsoleColor.White;

                    action = Console.ReadKey().KeyChar;
                    action = char.ToUpper(action);

                    if (action == 'R' || action == 'E') continue;

                    Write("\nIncorrect action\n", ConsoleColor.Red);

                } while (action != 'E' && action != 'R');

                Console.WriteLine();

                switch (action)
                {
                    case 'R':
                        Login();
                        break;
                    case 'E':
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static void Start()
        {
            try
            {
                char action;

                do
                {
                    action = ChoseAction();

                    switch (action)
                    {
                        case 'G':
                        {
                            Write("\nGetting Phone Numbers... \n");

                            var response = _service.GetOpenPhoneNumbers();

                            if (response == null)
                            {
                                Write("\nOpen Phone Call not found", ConsoleColor.Red);
                            }
                            else
                            {
                                var phoneNumbers = JsonConvert.DeserializeObject<string[]>(response);

                                if (phoneNumbers == null)
                                {
                                    Write("\nNo Deserialize", ConsoleColor.Red);
                                }
                                else
                                {
                                    if (phoneNumbers.Length == 0)
                                    {
                                        Write("\nOpen Phone Call not found", ConsoleColor.Green);
                                    }
                                    else
                                    {
                                        Write("\nPhone Numbers:\n", ConsoleColor.DarkYellow);

                                        foreach (var phoneNumber in phoneNumbers)
                                        {
                                            Write($"\n{phoneNumber}", ConsoleColor.Green);
                                        }
                                    }

                                    Console.WriteLine();
                                }
                            }

                            break;
                        }
                        case 'C':
                        {
                            var phoneNumber = EnterPhoneNumber();

                            Write("\nProcessing request. Please, wait... \n");

                            var response = _service.ClosePhoneCall(phoneNumber);
                            
                            if (response == null)
                            {
                                Write("\nResponse is null\n", ConsoleColor.Red);
                            }
                            else
                            {
                                Write($"\nisSucceeded: {response.IsSucceeded}", ConsoleColor.Green);
                                Write(response.Result == null
                                    ? "\nresult: null"
                                    : $"\nresult:{{\n   number: {response.Result.Number}\n    callCenterLine: {response.Result.CallCenterLine}\n  }}", ConsoleColor.Green);
                                Write(response.ValidationErrors == null
                                    ? "\nvalidationErrors: null\n"
                                    : $"\nvalidationErrors: {response.ValidationErrors}\n", ConsoleColor.Green);
                            }

                            break;
                        }
                        case 'O':
                        {
                            Write("\nProcessing request. Please, wait...\n");

                            var response = _service.CloseOldPhoneCalls();

                            if (response == null)
                            {
                                Write("\nResponse is null\n", ConsoleColor.Red);
                            }
                            else
                            {
                                Write($"\nisSucceeded: {response.IsSucceeded}", ConsoleColor.Green);
                                Write(response.ValidationErrors == null
                                    ? "\nvalidationErrors: null\n"
                                    : $"\nvalidationErrors: {response.ValidationErrors}\n", ConsoleColor.Green);
                            }

                            break;
                        }
                    }

                } while (action != 'E');
            }
            catch (Exception e)
            {
                Write($"\n{e.Message}", ConsoleColor.Red);
                Start();
            }
        }

        private static char ChoseAction()
        {
            char action;

            do
            {
                Write("\nChose action:\n");
                Write("G", ConsoleColor.Magenta);
                Write(" - get open phone close\n");
                Write("C", ConsoleColor.Magenta);
                Write(" - close phone call\n");
                Write("O", ConsoleColor.Magenta);
                Write(" - close old phone calls\n");
                Write("E", ConsoleColor.Magenta);
                Write(" - exit\n");
                Write("\nAction: ", ConsoleColor.DarkCyan);

                Console.ForegroundColor = ConsoleColor.White;

                action = Console.ReadKey().KeyChar;
                action = char.ToUpper(action);

                if (action == 'G' || action == 'C' || action == 'O' || action == 'E') continue;

                Write("\nIncorrect action\n", ConsoleColor.Red);

            } while (action != 'G' && action != 'C' && action != 'O' && action != 'E');

            Console.WriteLine();

            return action;
        }

        private static string EnterTheData(string parameter)
        {
            string output;

            var isCorrect = false;

            do
            {
                Write($"\n{parameter}: ", ConsoleColor.DarkCyan);

                Console.ForegroundColor = ConsoleColor.White;

                output = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(output))
                {
                    Write("Enter correct value!\n", ConsoleColor.Red);
                }
                else
                {
                    isCorrect = true;
                }

            } while (!isCorrect);

            return output;
        }

        private static string EnterPhoneNumber()
        {
            string phoneNumber;

            try
            {
                Write("\nEnter Phone Number: ");

                Console.ForegroundColor = ConsoleColor.White;

                phoneNumber = Console.ReadLine();
                phoneNumber = VariableCheck.ValidatePhoneNumber(phoneNumber);

            }
            catch (Exception e)
            {
                Write($"{e.Message}\n", ConsoleColor.Red);

                phoneNumber = EnterPhoneNumber();
            }

            return phoneNumber;
        }

        private static void Write(string message, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }
    }
}