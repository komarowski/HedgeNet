using System;

namespace HedgeNet.Models
{
    class PasswordGenerator
    {
        const string SpecialSymbols = "!\"#$%&\'()*+,-./|\\:;<=>?@[]_{}";

        // Determine the groups of symbols from ASCII Table that will be used to generate the password.
        public static char GetSymbol(Random rand, int order)
        {
            int position;
            switch (order)
            {
                // Digits
                case 0:
                    position = rand.Next(10) + 48;
                    return (char)position;
                // Lowercase letters
                case 1:
                    position = rand.Next(26) + 97;
                    return (char)position;
                // Uppercase letters
                case 2:
                    position = rand.Next(26) + 65;
                    return (char)position;
                case 3:
                    var arraySymbols = SpecialSymbols.ToCharArray();
                    position = rand.Next(arraySymbols.Length);
                    return arraySymbols[position];
                default:
                    return '0';
            }
        }

        public static char[] Shuffle(char[] array)
        {
            var rand = new Random();
            int length = array.Length;
            while (length > 1)
            {
                int k = rand.Next(length--);
                var temp = array[length];
                array[length] = array[k];
                array[k] = temp;
            }
            return array;
        }

        // Write symbols from different groups into passwordArray.
        public static char[] GetRandomSymbols(char[] passwordArray, int[] symbolsCount)
        {
            int thresholdLowercase = symbolsCount[0];
            int thresholdUppercase = symbolsCount[1];
            int thresholdDigit = symbolsCount[2];
            var rand = new Random();
            var length = passwordArray.Length;

            for (int i = 0; i < length; i++)
            {
                if (i < thresholdLowercase)
                    passwordArray[i] = GetSymbol(rand, 1);
                else if (i < thresholdUppercase)
                    passwordArray[i] = GetSymbol(rand, 2);
                else if (i < thresholdDigit)
                    passwordArray[i] = GetSymbol(rand, 0);
                else
                    passwordArray[i] = GetSymbol(rand, 3);
            }
            return Shuffle(passwordArray);
        }

        // Count the number of characters of different symbol groups in the password.
        public static int[] GetSymbolsNumber(double[] fractions, int length)
        {
            var count = fractions.Length;
            var result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = (int)Math.Round(length * fractions[i]);
            }
            return result;
        }

        // Generate a password specified length with or without special characters.
        public static string GeneratePassword(int length, bool isSpecial)
        {
            var passwordArray = new char[length];
            if (isSpecial)
            {
                int[] symbolsCount = GetSymbolsNumber(new double[] { 0.3, 0.6, 0.8 }, length);
                passwordArray = GetRandomSymbols(passwordArray, symbolsCount);
            }
            else
            {
                int[] symbolsCount = GetSymbolsNumber(new double[] { 0.4, 0.8, 1.0 }, length);
                passwordArray = GetRandomSymbols(passwordArray, symbolsCount);
            }
            return new string(passwordArray);
        }
    }
}
