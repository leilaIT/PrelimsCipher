using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace PrelimsCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            accessCipher();
        }
        static void accessCipher()
        {
            List<char> alphabet = new List<char>();
            List<char> cipher = new List<char>();
            List<char> keyList = new List<char>();
            List<char> eMessage = new List<char>();
            List<char> dMessage = new List<char>();
            string key = "";
            string message = "";
            string mode = "";
            string ans = "";
            bool repeat = true;

            while (repeat)
            {
                mode = selectMode("");
                key = inputKey("");
                alphabet = iniAlphabet(new List<char>());
                keyList = keyCheck(key, keyList);
                cipher = AddKeyToCipher(cipher, keyList);
                cipher = CompleteCipherList(cipher, alphabet);


                if (mode == "E")
                {
                    message = inputMessage("");
                    eMessage = encryptMode(eMessage, cipher, alphabet, message);
                    writeToFile(eMessage);
                    Console.Clear();
                }
                else
                {
                    eMessage = readFile(eMessage);
                    if (eMessage.Count != 0)
                    {
                        dMessage = decryptMode(dMessage, cipher, alphabet, eMessage);
                        displayWord(dMessage, eMessage);
                        Console.Clear();
                    }
                    else
                        Thread.Sleep(2500);
                }

                while (true)
                {
                    Console.WriteLine("Would you like to return to main menu?");
                    ans = Console.ReadLine().ToUpper();

                    if (ans == "Y")
                    {
                        Console.Clear();
                        break;
                    }
                    else if (ans == "N")
                    {
                        Console.WriteLine("Press any key to close the program");
                        repeat = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input please try again. Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            Console.ReadKey();
        }
        static string selectMode(string mode)
        {
            while (true)
            {
                Console.WriteLine("Would you like to encrypt or decrypt a message? [E / D] : ");
                Console.SetCursorPosition(58, 0);
                mode = Console.ReadLine().ToUpper();
                if (mode == "E" || mode == "D")
                {
                    Console.WriteLine("Machine Mode has been set.");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Setting please try again. Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            return mode;
        }
        static string inputKey(string key)
        {
            string[] toSplit = new string[] { };

            Console.Clear();
            Console.WriteLine("What is the key you want to set? : ");
            Console.SetCursorPosition(35, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            key = Console.ReadLine().ToUpper();
            Console.ResetColor();
            if (key.Contains(' '))
            {
                toSplit = key.Split(' ');
                key = "";
                for (int x = 0; x < toSplit.Length; x++)
                {
                    key += toSplit[x];
                }
            }

            Console.WriteLine("Cipher has been set.");
            Console.ReadKey();
            Console.Clear();
            return key;
        }
        static List<char> iniAlphabet(List<char> alphabet)
        {
            for (int x = 65; x < 91; x++)
                alphabet.Add((char)(x));
            return alphabet;
        }
        static List<char> keyCheck(string key, List<char> keyList)
        {
            keyList.Clear();
            for (int x = 0; x < key.Length; x++)
            {
                if (!keyList.Contains(key[x]))
                {
                    if ((int)key[x] > 64 && (int)key[x] < 91)
                        keyList.Add(key[x]);
                }
            }
            return keyList;
        }
        static List<char> AddKeyToCipher(List<char> cipher, List<char> keyList)
        {
            cipher.Clear();
            for (int x = 0; x < keyList.Count; x++)
                cipher.Add(keyList[x]);
            return cipher;
        }
        static List<char> CompleteCipherList(List<char> cipher, List<char> alphabet)
        {
            for (int x = 0; x < alphabet.Count; x++)
            {
                if (!cipher.Contains(alphabet[x]))
                    cipher.Add(alphabet[x]);
                else
                    continue;
            }
            return cipher;
        }
        static string inputMessage(string message)
        {
            Console.WriteLine("Please input the message you want to encrypt: ");
            Console.SetCursorPosition(46, 0);
            Console.ForegroundColor = ConsoleColor.Blue;
            message = Console.ReadLine().ToUpper();
            Console.ResetColor();
            return message;
        }
        static List<char> encryptMode(List<char> eMessage, List<char> cipher, List<char> alphabet, string message)
        {
            eMessage.Clear();
            for (int x = 0; x < message.Length; x++)
            {
                for (int i = 0; i < alphabet.Count; i++)
                {
                    if (message[x] == alphabet[i])
                    {
                        eMessage.Add(cipher[i]);
                        break;
                    }
                    else if (message[x] == ' ')
                    {
                        eMessage.Add(' ');
                        break;
                    }
                    else if ((int)message[x] > 32 && (int)message[x] < 65)
                    {
                        eMessage.Add(message[x]);
                        break;
                    }
                }
            }
            Console.Write("Message has been successfully encrypted ");
            return eMessage;
        }
        static void writeToFile(List<char> eMessage)
        {
            string outFile = "eMessage.txt";
            Console.Write("and written to eMessage.txt\n");
            using (StreamWriter sw = new StreamWriter(outFile))
            {
                for (int x = 0; x < eMessage.Count; x++)
                    sw.Write(eMessage[x]);
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        static List<char> readFile(List<char> eMessage)
        {
            eMessage.Clear();
            Console.Write("Reading eMessage.txt and decrypting using the provided key." +
                          "\nThe computer is decrypting. . .");
            Thread.Sleep(1500);
            Console.Write("\nThe decrypted message is: ");
            try
            {
                string line = "";
                using (StreamReader sr = new StreamReader("eMessage.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        for (int x = 0; x < line.Length; x++)
                            eMessage.Add(line[x]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            return eMessage;
        }
        static List<char> decryptMode(List<char> dMessage, List<char> cipher, List<char> alphabet, List<char> eMessage)
        {
            dMessage.Clear();
            for (int x = 0; x < eMessage.Count; x++)
            {
                for (int i = 0; i < cipher.Count; i++)
                {
                    if (eMessage[x] == cipher[i])
                    {
                        dMessage.Add(alphabet[i]);
                        break;
                    }
                    else if (eMessage[x] == ' ')
                    {
                        dMessage.Add(' ');
                        break;
                    }
                    else if ((int)eMessage[x] > 32 && (int)eMessage[x] < 65)
                    {
                        dMessage.Add(eMessage[x]);
                        break;
                    }
                }
            }
            return dMessage;
        }
        static void displayWord(List<char> displayList, List<char> eMessage)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int x = 0; x < eMessage.Count; x++)
            {
                Console.Write(eMessage[x]);
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(0, 3);
            for (int x = 0; x < displayList.Count; x++)
            {
                Console.Write(displayList[x]);
                Thread.Sleep(100);
            }
            Console.ResetColor();

            Console.WriteLine("\nMessage has been successfully decrypted.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
