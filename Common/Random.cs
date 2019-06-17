using System;

namespace Dnn.Powershell.Local.Common
{
   public class Random
    {
        private static System.Random rn = new System.Random();

        public static string GetRandomFirstName()
        {
            return WordLists.WordList.GetFirstName();
        }

        public static string GetRandomLastName()
        {
            return WordLists.WordList.GetLastName();
        }

        public static string GetRandomRoleName()
        {
            return WordLists.WordList.GetRolename();
        }

        public static string RandomString(int minLength, int maxLength)
        {
            int length = RandomNumber(minLength, maxLength);
            string res = "";
            var loopTo = length;
            for (int i = 0; i <= loopTo; i++)
            {
                if (rn.NextDouble() > 0.5)
                    res += RandomCharacter(65, 90);
                else
                    res += RandomCharacter(97, 122);
            }
            return res;
        }

        public static char RandomCharacter(int minCharCode, int maxCharCode)
        {
            return Convert.ToChar(RandomNumber(minCharCode, maxCharCode));
        }

        public static int RandomNumber(int minValue, int maxValue)
        {
            return rn.Next(minValue, maxValue);
        }
    }
}
