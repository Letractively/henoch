using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyMath
{
    public static class FileSystem
    {
        public static string ReadAllText(string fileName)
        {
            string read = String.Empty;

            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    read = File.ReadAllText(fileName);
                }
                catch (Exception ex)
                {                
                    Console.WriteLine(ex);
                }
            }

            return read;
        }

        public static void Check()
        {
            if (DateTime.Now == new DateTime(2000, 1, 1)) 
              throw new ApplicationException("y2kbug!");
        }
    }
}