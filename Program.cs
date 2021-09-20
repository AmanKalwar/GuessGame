using GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GamePlay
{
    class Program
    {
        Regex regex;
        private readonly GamePlayContext gameContext;
        Score Score;
        Users User;
        List<string> GuessingWords;
        WordsAssigned wordsAssigned;
        string User_ID;
        public Program()
        {
            gameContext = new GamePlayContext();
        }
        void StartMenu()
        {

            Console.WriteLine("--------------------------\n1.To Register\n2.To Login\n--------------------------");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 1)
                    RegisterUser();
                else if (choice == 2)
                    Login();
                else
                {
                    Console.WriteLine("Enter Valid Choice");
                    StartMenu();
                }

            }
            else
            {
                Console.WriteLine("Enter Number only");
                StartMenu();
            }
        }
        public bool ValidateEmailAddress(string email)
        {
            bool match = false;
            try
            {
                regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                return match = regex.IsMatch(email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return match;
            }
        }
        public bool ValidatePassword(string plainText)
        {
            bool match = false;
            try
            {
                regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
                match = regex.IsMatch(plainText);
                return match;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return match;
            }

        }
        public static bool ValidatePhone(string phone)
        {
            bool match = false;
            try
            {
                return match = Regex.IsMatch(phone, "\\A[0-9]{10}\\z");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return match;
            }
        }
        void RegisterUser()
        {
            Console.WriteLine("Enter UserName or Email ID");
            string emailID = Console.ReadLine();

            if (ValidateEmailAddress(emailID) && emailID.Trim() != "")
            {
                var Info = gameContext.Users.Where(e => e.User_ID == emailID);
                if (!Info.Any())
                {
                    Console.WriteLine("Enter the Password");
                    string Password = Console.ReadLine();
                    if (Password.Trim() !="" && ValidatePassword(Password))
                    {
                        Console.WriteLine("Enter The Name of the User");
                        string name = Console.ReadLine();
                        if (name.Trim() != "")
                        {
                            Console.WriteLine("Enter Phone Number of User");
                            string Phone = Console.ReadLine();
                            if (Phone.Trim() != "" && ValidatePhone(Phone))
                            {
                                User = new Users() { User_ID = emailID, Password = Password, Phone = Phone, Name = name };
                                Score = new Score() { UserName = emailID, Scores = 0 };
                                gameContext.Scores.Add(Score);
                                gameContext.Users.Add(User);
                                gameContext.SaveChanges();
                                Console.WriteLine("Registration Successfull");
                                User_ID = emailID;
                                StartGameMenu();

                            }
                            else
                            {
                                PrintStatement("Phone number");
                                StartMenu();
                            }

                        }
                    }
                    else
                    {
                        PrintStatement("Password");
                        StartMenu();
                    }
                }
                else
                {
                    PrintStatement("Email");
                    StartMenu();
                }

            }
            else
            {
                PrintStatement("Email");
                StartMenu();
            }

        }
        public void StartGameMenu()
        {
            Console.WriteLine("------------------------\n1.To Play Game\n2.To Give user a word\n3.To Print Score\n0.To Exit\n--------------------------------------");
            Console.WriteLine("Enter Your Choice");
            if (int.TryParse(Console.ReadLine(), out int Choice))
            {
                if (Choice == 1)
                {
                    FetchWords();
                }
                else if (Choice == 2)
                {
                    GiveWord();
                }
                else if (Choice == 3)
                {
                    PrintScore();
                }
                else if (Choice == 0)
                {
                    Console.WriteLine("Bye Bye");
                }
                else
                {
                    PrintStatement("Input");
                    StartGameMenu();
                }
            }
            else
            {
                PrintStatement("Value");
                StartGameMenu();
            }

        }
        void FetchWords()
        {
            int i = 0;
            var words = gameContext.WordsAssigned.Where(e => e.UserName == User_ID);
            GuessingWords = new List<string>();
            if (words.Any())
            {
                Console.WriteLine("Words Assigned to you are:\n---------------------------------------------");
                foreach (var item in words)
                {
                    GuessingWords.Add(item.Word);
                    string x;
                    x = new String('X', item.Word.Length);
                    Console.WriteLine((i + 1) + "\t" + x);
                    i += 1;
                }
                Console.WriteLine("\n 0.Go to previous Menu\n---------------------------------------------");
                Console.WriteLine("Enter your Choice");
                if (int.TryParse(Console.ReadLine(), out int Choice))
                {
                    if (Choice <= GuessingWords.Count && Choice > 0)
                    {
                        Guess(GuessingWords[Choice - 1]);
                    }
                    else if (Choice == 0)
                    {
                        StartGameMenu();

                    }
                    else
                    {
                        PrintStatement("Choice");
                        FetchWords();
                    }
                }
                else
                {
                    PrintStatement("Choice");
                    FetchWords();
                }


            }
            else
            {
                Console.WriteLine("no Words Added Yet");
                StartGameMenu();
            }
        }
        void PrintScore()
        {
            foreach (var item in gameContext.Scores)
            {
                Console.WriteLine(item);
            }
            StartGameMenu();
        }
        void GiveWord()
        {
            int i = 0;
            Console.WriteLine("Available users are");
            Console.WriteLine("-------------------------------------------");
            List<Users> Use = gameContext.Users.Where(e => e.User_ID != User_ID).ToList();
            if (Use.Count > 0)
            {
                foreach (var item in Use)
                {
                    Console.WriteLine((i + 1) + " " + item.User_ID);
                    i += 1;
                }
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Select The Users");
                if (int.TryParse(Console.ReadLine(), out int Choice) && Choice <= Use.Count && Choice > 0)
                {
                    Console.WriteLine("Enter the Word to be given");
                    string pushword = Console.ReadLine();
                    if (pushword.Trim() != "")
                    {
                        var Infoo = gameContext.WordsAssigned.Where(e => e.UserName == Use[Choice - 1].User_ID && e.Word.ToLower() == pushword.ToLower());
                        if (!Infoo.Any())
                        {
                            wordsAssigned = new WordsAssigned() { Word_ID = gameContext.WordsAssigned.Count() + 1, UserName = Use[Choice - 1].User_ID, Word = pushword };
                            gameContext.WordsAssigned.Add(wordsAssigned);
                            try
                            {
                                gameContext.SaveChanges();
                                Console.WriteLine("Word Added Successfully");
                                StartGameMenu();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                StartGameMenu();
                            }

                        }
                        else
                        {
                            Console.WriteLine("Word Already Given Please Enter Another Word");
                            GiveWord();
                        }

                    }
                    else
                    {
                        PrintStatement("Word Enter Valid Word");
                        GiveWord();
                    }
                }
                else
                {
                    PrintStatement("Choice");
                    GiveWord();
                }
            }
            else
                Console.WriteLine("No Users Yet");
        }
        public void Login()
        {
            User = new Users();
            Console.WriteLine("Enter Your Email");
            string Email1 = Console.ReadLine();
            Console.WriteLine("Enter Your Password");
            string Pass1 = Console.ReadLine();

            List<Users> details = gameContext.Users.Where(e => e.User_ID == Email1 && e.Password == Pass1).ToList();
            bool isEmpty = !details.Any();
            if (isEmpty)
            {
                Console.WriteLine("Invalid input please enter valid Email and Password");
                StartMenu();
            }
            else
            {

                foreach (var item in details)
                {
                    User_ID = item.User_ID;
                    Console.WriteLine("Login Successfull");
                    Console.WriteLine("Welcome: " + item.Name);
                }
                StartGameMenu();
            }
        }
        static void PrintStatement(string msg)
        {
            Console.WriteLine("Invalid " + msg);
        }
        void Guess(string word)
        {
            int cows = 0, bulls = 0;
            Console.WriteLine("Enter Your Guess");
            string guessword = Console.ReadLine();
            guessword = guessword.ToLower();
            word = word.ToLower();
            if (word.Length != guessword.Length)
            {
                Console.WriteLine("Enter proper word to guess");
            }
            else if (String.Compare(word, guessword) != 0)
            {

                for (int i = 0; i < word.Length; i++)
                {

                    if (guessword[i] == word[i])
                    {
                        cows += 1;
                    }
                    else if (word.Contains(guessword[i]))
                    {
                        bulls++;
                    }


                }
                Console.WriteLine("Cows: " + cows + " Bulls: " + bulls);
                cows = bulls = 0;
                Guess(word);
            }
            else if (String.Compare(word, guessword) == 0)
            {
                var Info = gameContext.WordsAssigned.Where(e => e.UserName == User_ID && e.Word == word);
                foreach (var item in Info)
                {
                    gameContext.WordsAssigned.Remove(gameContext.WordsAssigned.Find(item.Word_ID, item.UserName));

                }
                try
                {
                    gameContext.SaveChanges();
                    int scc = gameContext.Scores.Find(User_ID).Scores;
                    Score sccc = gameContext.Scores.Find(User_ID);
                    sccc.Scores += 10;
                    gameContext.SaveChanges();
                    Console.WriteLine("Congrats That is the Word !!");
                    FetchWords();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    FetchWords();
                }
            }
        }
        static void Main(string[] args)
        {
            new Program().StartMenu();

        }
    }
}