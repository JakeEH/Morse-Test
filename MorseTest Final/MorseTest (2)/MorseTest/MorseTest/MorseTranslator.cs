using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MorseTest
{
    class MorseTranslator
    {
        public int Count { get; set; }
        private Dictionary<string, string> MorseDictionary = new Dictionary<string, string>();
        /**
         * By looping through all the KeyValuePair<TKey, TValue>'s in the dictionary (which will be a sizable performance hit if you have a number of entries in the dictionary)
            Use two dictionaries, one for value-to-key mapping and one for key-to-value mapping (which would take up twice as much space in memory).    
            Use Method 1 if performance is not a consideration, use Method 2 if memory is not a consideration.
          **/

        private Dictionary<char, string> CharToMorse = new Dictionary<char, string>()
        {
                                       {'a', ".-"},
                                       {'b', "-..."},
                                       {'c', "-.-."},
                                       {'d', "-.."},
                                       {'e', "."},
                                       {'f', "..-."},
                                       {'g', "--."},
                                       {'h', "...."},
                                       {'i', ".."},
                                       {'j', ".---"},
                                       {'k', "-.-"},
                                       {'l', ".-.."},
                                       {'m', "--"},
                                       {'n', "-."},
                                       {'o', "---"},
                                       {'p', ".--."},
                                       {'q', "--.-"},
                                       {'r', ".-."},
                                       {'s', "..."},
                                       {'t', "-"},
                                       {'u', "..-"},
                                       {'v', "...-"},
                                       {'w', ".--"},
                                       {'x', "-..-"},
                                       {'y', "-.--"},
                                       {'z', "--.."},
                                       {'0', "-----"},
                                       {'1', ".----"},
                                       {'2', "..---"},
                                       {'3', "...--"},
                                       {'4', "....-"},
                                       {'5', "....."},
                                       {'6', "-...."},
                                       {'7', "--..."},
                                       {'8', "---.."},
                                       {'9', "----."}
                                   };


        private Dictionary<string, string> MorseToChar = new Dictionary<string, string>()
            {
                        { ".-", "a" },
                        { "-...", "b" },
                        { "-.-.", "c" },
                        { "-..", "d" },
                        { ".", "e" },
                        { "..-.", "f" },
                        { "--.", "g" },
                        { "....", "h" },
                        { "..", "i" },
                        { ".---", "j" },
                        { "-.-", "k" },
                        { ".-..", "l" },
                        { "--", "m" },
                        { "-.", "n" },
                        { "---", "o" },
                        { ".--.", "p" },
                        { "--.-", "q" },
                        { ".-.", "r" },
                        { "...", "s" },
                        { "-", "t" },
                        { "..-", "u" },
                        { "...-", "v" },
                        { ".--", "w" },
                        { "-..-", "x" },
                        { "-.--", "y" },
                        { "--..", "z" },
            };

        /// <summary>
        /// create a morse dictionary of the plain text words from the lexicon dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        public void CreateMorseDictionary(String[] dictionary)
        {
            foreach (string word in dictionary)
            {
                //if the english word has not been added, encode and add to the dict
                if (!this.MorseDictionary.ContainsKey(word))
                {
                    this.MorseDictionary.Add(word, this.EncodePlainText(word));
                }                
            }
        }

        /// <summary>
        /// return the english for the passed in morse
        /// </summary>
        /// <param name="morseWord"></param>
        /// <returns></returns>
        public string MorseToEnglish(string morseWord)
        {
            string englishWord = "";
            Dictionary<string, string> morseWords = this.LookupMorse(morseWord);

            //for multiple matches return all the found values
            foreach (string lookedUpWord in morseWords.Keys)
            {
                englishWord += lookedUpWord;
                englishWord += " ";
            }

            return englishWord;
        }

        /// <summary>
        /// get the kvp where the morse word equals from the file
        /// </summary>
        /// <param name="morseWord"></param>
        /// <returns></returns>
        private Dictionary<string, string> LookupMorse(string morseWord)
        {
            return this.MorseDictionary.Where(morse => morse.Value == morseWord).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// matches the character to it's morse equivalent, builds up the encoded string 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string EncodePlainText(string plainText)
        {

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char character in plainText.ToCharArray())
            {
                if (CharToMorse.ContainsKey(character))
                {
                    stringBuilder.Append(CharToMorse[character]);
                }
                // if char not found, will be a space
                else
                {
                    stringBuilder.Append("/");
                }
            }
            //will always have a final / appended so trim last char
            if (stringBuilder.ToString().EndsWith("/"))
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
           
            return stringBuilder.ToString();
        }


        //public IEnumerable<string> DecodeMorse(string morse, Boolean isReverseSort)
        //{
        //    IEnumerable<string> query = new[] { "" };
        //    if (isReverseSort)
        //    {
        //        query = DecodeMorse(morse, MorseToChar.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        //    }
        //    else
        //    {
        //        query = DecodeMorse(morse, MorseToChar);
        //    }

        //    return query;
        //}

        //finds every possible combination, would error out with out of memory exception
        //private IEnumerable<string> DecodeMorse(string morse, Dictionary<String, String> wordDictionary)
        //{
        //    IEnumerable<string> query = new[] { "" };
        //    while (Count < 10000000)
        //    {
        //        var letters =
        //            wordDictionary
        //                .Where(kvp => morse.StartsWith(kvp.Key))
        //                .Select(kvp => new
        //                {
        //                    letter = kvp.Value,
        //                    remainder = morse.Substring(kvp.Key.Length)
        //                })
        //                .ToArray();
        //        if (letters.Any())
        //        {

        //            query =
        //               from l in letters
        //               from x in DecodeMorse(l.remainder, wordDictionary)

        //               select l.letter + x;

        //            Count++;
        //            return query.ToArray();
        //        }
        //        else
        //        {

        //            return new[] { "" };

        //        }
        //    }
        //    return query;
        //}



    }


}
