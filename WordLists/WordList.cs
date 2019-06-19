using Dnn.Powershell.Local.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dnn.Powershell.Local.WordLists
{
    public static class WordList
    {
        private static List<string> glbDictionary;
        private static List<string> glbFirstnames;
        private static List<string> glbLastnames;
        private static List<string> glbRolenames;
        private static System.Random rn = new System.Random();

        public static string GetFirstName()
        {
            EnsureWordlistsLoaded();
            return glbFirstnames[rn.Next(glbFirstnames.Count)].Trim(new[] { '\r', '\n' }).Trim();
        }

        public static string GetLastName()
        {
            EnsureWordlistsLoaded();
            return glbLastnames[rn.Next(glbLastnames.Count)].Trim(new[] { '\r', '\n' }).Trim();
        }

        public static string GetRolename()
        {
            EnsureWordlistsLoaded();
            return MakePhrase(rn.Next(1, 3), true);
        }

        public static string MakePhrase(int nrWords, bool capitalize)
        {
            var res = new List<string>();
            for (var i = 0; i < nrWords; i++)
            {
                var newWord = glbDictionary[rn.Next(glbDictionary.Count)].Trim(new[] { '\r', '\n' }).Trim();
                if (capitalize || i == 0)
                {
                    newWord = newWord.Substring(0, 1).ToUpper() + newWord.Substring(1);
                }
                res.Add(newWord);
            }
            return string.Join(" ", res);
        }

        private static void EnsureWordlistsLoaded()
        {
            if (glbDictionary == null)
                glbDictionary = Globals.GetResource("WordLists.Lists.Dictionary.lst").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (glbFirstnames == null)
                glbFirstnames = Globals.GetResource("WordLists.Lists.GivenNames.lst").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (glbLastnames == null)
                glbLastnames = Globals.GetResource("WordLists.Lists.FamilyNames.lst").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (glbRolenames == null)
                glbRolenames = Globals.GetResource("WordLists.Lists.Roles.lst").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
