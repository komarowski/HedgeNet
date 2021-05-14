using System.Linq;

namespace HedgeNet.Models
{
    class Cryptographer
    {
        // Key to encrypt passwords.
        const uint Key1 = 0x5F2B14B1;

        // Key to encrypt the rest of the information.
        const uint Key2 = 0x9766EE02;


        public static string[] EncryptDecryptArray(string[] lineArray)
        {
            for (int i = 0; i < lineArray.Length; i++)
            {
                var words = lineArray[i].Split(ParseTxt.Separator);
                for (int j = 0; j < words.Length; j++)
                {
                    // A different encryption key is used for the password.
                    if (j == 4) words[j] = EncryptDecryptString(words[j], Key1);
                    else words[j] = EncryptDecryptString(words[j], Key2);
                }
                lineArray[i] = string.Join(ParseTxt.Separator.ToString(), words);
            }
            return lineArray;
        }

        // Encrypt or decrypt a string with a simple xor encryption
        public static string EncryptDecryptString(string line, uint key)
        {
            char[] lineArray = line.ToArray();
            for (int i = 0; i < lineArray.Length; i++)
            {
                lineArray[i] = (char)(lineArray[i] ^ key);
            }
            return new string(lineArray);
        }
    }
}
