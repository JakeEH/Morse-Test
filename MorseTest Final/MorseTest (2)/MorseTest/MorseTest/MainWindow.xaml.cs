using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Reflection;

namespace MorseTest
{

 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MorseTranslator MorseTranslator = new MorseTranslator();

        public MainWindow()
        {
            InitializeComponent();
            SetUpMorseDictionary();
        }



        /// <summary>
        /// Allows the desired file to be opened and it's contents displayed on screen for decoding 
        /// •	Read in a file containing the following single line of Morse code. 
        /// </summary>
        private void OpenFile()
        {

            OpenFileDialog openFile = new OpenFileDialog();

            //Only allow txt as that is the specified input
            openFile.DefaultExt = ".txt";
            openFile.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = openFile.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = openFile.FileName;
                //show the directory & contents of the file
                txtFileDir.Text = filename;
                txtFileContents.Text = File.ReadAllText(filename);

            }
        }

        /// <summary>
        /// •	Place the plain English version and the translated Morse code version of the user inputted reply into a new file to be sent back to the candidate.
        /// </summary>
        private void SaveFile()
        {
            //add the plain text version
            string fileText = txtUserPlainText.Text + Environment.NewLine + txtUserMorse.Text;
            //only allow plain text
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, fileText);
            }
        }

        /// <summary>
        /// set up the dictionary for the users file input to be matched by
        /// </summary>
        private void SetUpMorseDictionary()
        {
            string fileContents ="";
            string[] dictionary;
            //used embedded resource for portabilty
            var assembly = Assembly.GetExecutingAssembly();
            //TODO handle file file not found feedback 
            string resourceName = "MorseTest.dictionary_list.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                 fileContents = reader.ReadToEnd();
            }

            // Open dictionary document 
            //each line has a return and newline, replace with just a newline to use split()
            fileContents = fileContents.Replace("\r\n", "\n");       
            dictionary = fileContents.Split('\n');

            this.MorseTranslator.CreateMorseDictionary(dictionary);
            
        }

        /// <summary>
        /// read in the file contents from the txtbox and match each word
        /// </summary>
        /// <returns></returns>
        private string LookupFileContents()
        {
            string decodedFileContents = "";
            string fileContents = txtFileContents.Text;

            foreach (string splitInputMorse in fileContents.Split('/'))
            {
                decodedFileContents += this.MorseTranslator.MorseToEnglish(splitInputMorse);
              
            }
                return decodedFileContents;
        }


        private void btnEncode_Click(object sender, RoutedEventArgs e)
        {
            MorseTranslator morse = new MorseTranslator();
            txtUserMorse.Text = morse.EncodePlainText(txtUserPlainText.Text.ToLower());
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //check if empty
            SaveFile();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void btnDecode_Click(object sender, RoutedEventArgs e)
        {
            //TODO - display feedback for ambiguous words and multiple words mat
            txtFileDecoded.Text = LookupFileContents();
        }

        //private string DecodeFileContents(String fileContents, Boolean isReverseSort)
        //{
        //    MorseTranslator morse = new MorseTranslator();
        //    IEnumerable<string> morsePossibilities;
        //    morse.Count = 0;
        //    string decodedPossibilities = "";
        //    foreach (string splitInputMorse in fileContents.Split('/'))
        //    {
        //        morsePossibilities = morse.DecodeMorse(splitInputMorse, isReverseSort);
        //        var morseList = morsePossibilities.ToList();
        //        using (Hunspell hunspell = new Hunspell("en_gb.aff", "en_gb.dic"))
        //        {
        //            foreach (string s in morseList)
        //            {
        //                if (hunspell.Spell(s))
        //                {
        //                    if (s.Length > splitInputMorse.Length / 4 && s.Length <= splitInputMorse.Length)
        //                    {
        //                        decodedPossibilities += s;
        //                        decodedPossibilities += " ";
        //                    }
        //                }
        //            }
        //            //  decodedPossibilities = morseList.Where(t => hunspell.Spell(t)).ToList().FirstOrDefault();
        //            // Process all your words here, don't create the Hunspell object for every word.
        //        }
        //    }
        //    return decodedPossibilities.Trim();
        //}

    }
}
