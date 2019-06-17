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
            return glbRolenames[rn.Next(glbRolenames.Count)].Trim(new[] { '\r', '\n' }).Trim().Substring(0, 50);
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
