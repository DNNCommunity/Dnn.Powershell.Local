using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace Dnn.Powershell.Local.Common
{
    public static class Globals
    {

        public static string EncodeBase64(string input)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytesToEncode);
        }

        public static T Get<T>(this IDictionary dictionary, string name) where T : IConvertible
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            else
                return dictionary.Contains(name) ? (T)Convert.ChangeType(dictionary[name], typeof(T)) : default(T);
        }

        public static T Get<T>(this System.Xml.XmlNode node) where T : IConvertible
        {
            if (node == null)
                throw new ArgumentNullException("node");
            else
                return (T)Convert.ChangeType(node.InnerText, typeof(T));
        }

        public static T Get<T>(this System.Xml.XmlNode node, T defaultValue) where T : IConvertible
        {
            if (node == null)
                return defaultValue;
            else
                return (T)Convert.ChangeType(node.InnerText, typeof(T));
        }

        public static string ForceEnd(this string input, string endString)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (!input.EndsWith(endString))
                    input += endString;
            }
            return input;
        }

        public static string ForceStart(this string input, string startString)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (!input.StartsWith(startString))
                    input = startString + input;
            }
            return input;
        }

        public static System.Data.SqlClient.SqlCommand AddParameter(this System.Data.SqlClient.SqlCommand command, string parameterName, object parameterValue)
        {
            System.Data.SqlClient.SqlParameter p = new System.Data.SqlClient.SqlParameter(parameterName.ForceStart("@"), parameterValue);
            command.Parameters.Add(p);
            return command;
        }

        public static System.Data.SqlClient.SqlCommand AddOutputParameter(this System.Data.SqlClient.SqlCommand command, string parameterName, System.Data.SqlDbType parameterType)
        {
            System.Data.SqlClient.SqlParameter p = new System.Data.SqlClient.SqlParameter(parameterName.ForceStart("@"), parameterType);
            p.Direction = ParameterDirection.Output;
            command.Parameters.Add(p);
            return command;
        }

        public static string GetResource(string resourceName)
        {
            resourceName = "Dnn.Powershell.Local." + resourceName;
            string res = "";
            using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (System.IO.StreamReader rdr = new System.IO.StreamReader(stream))
                {
                    res = rdr.ReadToEnd();
                }
            }
            return res;
        }

        public static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath)) return "";
            var res = "";
            using (var sr = new StreamReader(filePath))
            {
                res = sr.ReadToEnd();
            }
            return res;
        }

    }
}
