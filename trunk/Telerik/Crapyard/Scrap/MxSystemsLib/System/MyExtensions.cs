using System;

namespace MxSystemsLib.System
{
    /// <summary>
    /// Only if the classes cannot be changed manually.
    /// </summary>
    public static class MyExtensions
    {
        /// <summary>
        /// Only for integers [0,..,9]!
        /// </summary>
        /// <param name="theStringToBePrefixed"></param>
        /// <param name="countSpaces"></param>
        public static string PrefixThisWithSpaces(this string theStringToBePrefixed, int countSpaces)
        {
            try
            {
                int number = Convert.ToInt32(theStringToBePrefixed);
                if (number < 10 && number >=0)
                {
                    for (int i = 0; i < countSpaces; i++)
                    {
                        theStringToBePrefixed = " " + theStringToBePrefixed;
                    }
                }

            }
            catch (Exception)
            {
                //////Log.Writeline(ex);
                throw new CheckedException(ErrorType.ProcessFailure, "Only for integers [0,..,9]!");
            }
            return theStringToBePrefixed;
        }
        public static string PostfixThisWithSpaces(this string theStringToBePostfixed, int countSpaces)
        {
            try
            {
                for (int i = 0; i < countSpaces; i++)
                {
                    theStringToBePostfixed = theStringToBePostfixed + " ";
                }

            }
            catch (Exception ex)
            {
                ///Log.Writeline(ex);
                throw;
            }
            return theStringToBePostfixed;
        }
        public static string PostfixThisWithSeperators(this string theStringToBePostfixed, int countSeperators, 
                                                       string seperator)
        {
            try
            {
                for (int i = 0; i < countSeperators; i++)
                {
                    theStringToBePostfixed = theStringToBePostfixed + seperator;
                }

            }
            catch (Exception ex)
            {
                ///Log.Writeline(ex);
                throw;
            }
            return theStringToBePostfixed;
        }
        public static void Dump(this string[] array)
        {
            int lenX = array.Length;
            string row;

            for (int i = 0; i < lenX; i++)
            {
                row = i + " " + array[i] + "\n\r";
                ///Log.ConsoleWriteline(row.Trim());
            }

        }
        public static void Dump(this double[] array)
        {
            int lenX = array.Length;
            string row;

            for (int i = 0; i < lenX; i++)
            {                                
                row = i + " " + array[i] + "\n\r";
                ///Log.ConsoleWriteline(row.Trim());
            }
            
        }
        public static void Dump(this double[][] matrix, int lenX, int lenY)
        {
            for (int i = 0; i < lenX; i++)
            {
                string row = "";
                if (matrix[i] != null)
                {
                    for (int j = 0; j < lenY; j++)
                    {
                        
                        row = row + "\t" + matrix[i][j] + "\t";
                       
                    }
                    ///Log.ConsoleWriteline("\t" + i + "\t" + row);///prefix 1 tab for empty column.
                }
            }
        }
        public static void Dump(this double[][] matrix)
        {
            int lenX = matrix.Length;
            int lenY = matrix[0].Length;
            

            for (int i = 0; i < lenX; i++)
            {
                string row = "";
                for (int j = 0; j < lenY; j++)
                {
                    row = row + @"\t" + matrix[i][j] + @"\t";
                }
                ///Log.ConsoleWriteline(@"\t" + i + @"\t" + row);///prefix 1 tab for empty column.
            }
        }
        public static void Dump(this double value)
        {
            ///Log.ConsoleWriteline(value.ToString());
        }
        public static void Dump(this double value, string message)
        {
            ///Log.ConsoleWriteline(message + value);
        }
        /// <summary>
        /// true if < 1E10 - 15
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool Equal(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 1E10 - 15;
        }

        public static bool Equal(this double value1, double value2, double precision)
        {
            return Math.Abs(value1 - value2) < precision;
        }
    }
}