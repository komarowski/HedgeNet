using System.Collections.Generic;
using System.IO;

namespace HedgeNet.Models
{
    class ParseTxt
    {
        public const string PasswordsFile = "data.txt";
        public const char Separator = '~';

        public static List<PasswordInfo> ReadPasswords()
        {
            // Read and decode lines from file "data.txt" into static List.
            var result = new List<PasswordInfo>();
            CheckFileExist();
            var lines = File.ReadAllLines(PasswordsFile);
            lines = Cryptographer.EncryptDecryptArray(lines);
            foreach (var line in lines)
            {
                var words = line.Split(Separator);
                if (words.Length == 8)
                {
                    PasswordInfo newItem = new PasswordInfo();
                    newItem.ID = int.Parse(words[0]);
                    newItem.Website = words[1];
                    newItem.Username = words[2];
                    newItem.Email = words[3];
                    newItem.Password = words[4];
                    newItem.UpdateDate = words[5];
                    newItem.Categoty = words[6];
                    newItem.Note = words[7];
                    result.Add(newItem);
                }
            }
            return result;
        }

        // Encode data from the dictionary and overwrite the file "data.txt".
        public static void WritePasswords(List<PasswordInfo> data)
        {
            var linesCount = data.Count;
            var rewriteLines = new string[linesCount];
            for (int i = 0; i < linesCount; i++)
            {
                PasswordInfo item = data[i];
                rewriteLines[i] = $"{item.ID}~{item.Website}~{item.Username}~{item.Email}~{item.Password}~{item.UpdateDate}~{item.Categoty}~{item.Note}";
            }
            rewriteLines = Cryptographer.EncryptDecryptArray(rewriteLines);
            File.WriteAllLines(PasswordsFile, rewriteLines);
        }

        public static void CheckFileExist()
        {
            if (!File.Exists(PasswordsFile))
            {
                using (File.Create(PasswordsFile)) { }
            }
        }
    }
}
