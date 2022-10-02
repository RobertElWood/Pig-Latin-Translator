
using System;
using System.Linq;


namespace Pig_Latin
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //Main program writes lines to the user, explaining program and asking for input.
            //This array is then passed to a helper method, "PigLatinConvert", which actually modifies the words.

            bool keepGoing = true;
            
            Console.WriteLine("Welcome to our 'Pig Latin' Translator!\n");
            Console.WriteLine("This admittedly frivolous application will translate any English line you write into pig latin! Fun, right?");

            while (keepGoing)
            {
                Console.WriteLine("\nPlease enter a line of your choice below: \n");

                string input = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(input) == true)   //Prompts the user for additional input if their answer is blank or whitespace.
                {
                    Console.WriteLine("\nHmmm...It doesn't look like you've entered anything. Please try again!\n");
                    continue;
                }
         
                string[] strArr = input.Split();               
                var capitalIndexes = CapitalChecker(strArr);    //stores data on the capital letters present within the user's input.          

                string checkedArray = string.Join(" ", strArr).ToLower();       //Users input is converted to lower case prior to being modified.
                List <string> newList = PigLatinConvert(checkedArray.Split());  //Calls function to convert user input.

                List<string> modList = CapitalInsert(newList, capitalIndexes);  //re-inserts capital letters in the appropriate places before joining. 
                string result = string.Join(" ", modList); 

                Console.WriteLine("\nYour line in pig latin is: \n" + "\n" + result);

                keepGoing = askKeepGoing();
            }
        }

        //method with foreach loop to iterate over items in array and return list of modified items
        //If firstPosition == 1000000, that means there were no vowels in the word at all. This number should match the value in VowelChecker().
        public static List<string> PigLatinConvert(string[] wordsList)
        {
            List<string> listUpdated = new List<string>();
            
            foreach (string word in wordsList)
            {
                int firstPosition = VowelChecker(word);
                bool symbolPresent = SymbolChecker(word);

                if (symbolPresent == true)
                {
                    listUpdated.Add(word);
                } 
                else if (firstPosition == 1000000) 
                {
                    listUpdated.Add(word + "ay");
                }
                else if (firstPosition == 0)
                {
                    listUpdated.Add(word + "way");
                }
                else
                {            
                    string startOf = word.Substring(firstPosition);
                    string endOf = word.Substring(0, firstPosition);

                    string word2 = startOf + endOf + "ay";
                    listUpdated.Add(word2);
                }          
            }
            return listUpdated;
        }

        //method for iterating over possible vowels and saving the first occurence of a vowel's position in a given word.
        //returns an int, the index value of the first place in the word where a vowel occurs.
        public static int VowelChecker (string word)
        {
            string[] vowels = { "a", "e", "i", "o", "u" };

            //firstOccurs = counter to store the first position of a vowel in a word. 
            //The 1000000 is totally arbitrary. Just needs to be bigger than indices in 'word'.
            int firstOccurs = 1000000;  
         
            for (int i = 0; i < vowels.Length; i++)
            {
                string vowel = vowels[i];
                int wordPos = word.IndexOf(vowel);

                if (word.Contains(vowel) == true && wordPos < firstOccurs)
                {
                    firstOccurs = wordPos;  
                }
                else 
                {
                    continue;
                }
            }

            return firstOccurs;
        }
        
        //method for checking if symbols are present within each word of the user's input. Called by PigLatinConvert.
        public static bool SymbolChecker (string word)
        {
            string[] symbols = { "~", "`", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "!", "?", ".", ",",
            "=", "{", "}", "[", "]", "|", "/", ">", "<", "0","1", "2", "3", "4", "5", "6", "7", "8", "9" };
            
            bool isPresent = false;

            foreach (string symbol in symbols) 
            { 
                if (word.Contains(symbol) == true) 
                { 
                    isPresent = true;
                }                 
            }

            return isPresent;
        }

        //method to check if capital letters are present within user's input. Checks each letter in each word of user's input.
        //Assigns all indices either a '0'--no capital-- or '1'--a capital is present to be converted later.
        //This is reapplied to capitalize output before it is printed out to user.
        public static List <List <int>> CapitalChecker (string[] userInput)
        {

            string capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            List <List <int>> capitalBool = new List <List <int>> ();
            
            foreach (string word in userInput)
            {
                List <int> currentWord = new List <int> ();

                for (int i = 0; i < word.Length; i++) 
                { 
                    if (capitals.Contains(word[i]) == true) 
                    { 

                        currentWord.Add(1);                   
                        
                    }
                    else
                    {
                        currentWord.Add(0);
                                             
                    }              
                }

                capitalBool.Add (currentWord);
            }

            return capitalBool;
        }

        //method to re-insert saved capitalization into converted pig latin sentence. Uses the results of PigLatinConvert, as well as CapitalChecker.
        //creates a new, modified list of strings which is passed back to the call.
        public static List <string> CapitalInsert (List <string> plWords, List <List <int>> capitalIndexes) 
        {
                   
            List <string> result = new List<string> ();
            
            for (int j = 0; j < plWords.Count(); j++)
            {
                string word = plWords[j];

                for (int i = 0; i < capitalIndexes[j].Count(); i++)
                {
                    if (capitalIndexes[j][i] == 1)
                    {
                        word = word.Replace(word[i], Char.ToUpper(word[i]));
                    }
                }
                result.Add (word);
            }
            return result;
        }

        //method for determining if the user would like to run the program again.
        //Overwrites the boolean value at the start of the main program, causing the main program's while loop to exit if user chooses 'n'.
        public static bool askKeepGoing()
        {
            Console.WriteLine();
            Console.WriteLine("\nWould you like to translate another word into pig latin? Y/N?\n");
            string input = Console.ReadLine().ToLower();

            if (input == "y")
            {
                return true;
            }
            else if (input == "n")
            {
                Console.WriteLine("\nIn that case...Arewellfay!");
                return false;
            }
            else
            {
                Console.WriteLine("I didn't understand that. Please try again!");
                return askKeepGoing();
            }
        }
    }
}