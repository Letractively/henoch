using System;
using System.Collections.ObjectModel;
using System.Globalization;
using DataResource.Maintenance;
using ExceptionHandler;
using Maintenance;

namespace DataResource.DesignPatterns
{
    public class NumberParser:StringParser
    {
        /// <summary>
        /// Contains the elements read from the line.
        /// </summary>
        public new Collection<double> LineItems { get; set; }

        public NumberParser(string line, string separator)
        {
            base.LineItems = base.ParseLine(line, separator);
            LineItems = ParseLine(line, separator);
        }

        protected NumberParser()
        {

        }
        /// <summary>
        /// Parses the line for numbers.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public new Collection<double> ParseLine(string line, string separator)
        {
            Collection<string> collection = base.LineItems;
            Collection<double> doubles = new Collection<double>();
            string itemCurrent = "";
            try
            {
                foreach (string item in collection)
                {
                    itemCurrent = item;
                    doubles.Add(Convert.ToDouble(item,CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                string message = string.Format(CultureInfo.InvariantCulture,
                                               "Parse error voor item {0}:\n\r{1}", itemCurrent, ex.Message);
                Log.ConsoleWriteline(message);
                throw new CheckedException(ErrorType.ProcessFailure, message);
            }
            return doubles;
        }
    }

    public interface IStringParser
    {
        /// <summary>
        /// Contains the elements read from the line.
        /// </summary>
        Collection<string> LineItems { get; set; }
        /// <summary>
        /// Tokens read
        /// </summary>
        string[] Tokens { get; set; }
        /// <summary>
        /// Parses the line for chars.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        Collection<string> ParseLine(string line, string separator);
    }

    public class StringParser : IStringParser
    {


        /// <summary>
        /// Contains the elements read from the line.
        /// </summary>
        public Collection<string> LineItems { get; set; }

        public string[] Tokens { get; set; }

        public StringParser()
        {
            ///for inheritance
        }
        public StringParser(string line, string separator)
        {
            LineItems = ParseLine(line, separator);
        }
        public StringParser(string line, string separator, int itemCount)
        {
            Tokens = ParseLine(line, separator, itemCount);
        }
        /// <summary>
        /// Parses the line for chars (fast).
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        private string[] ParseLine(string line, string separator, int itemCount)
        {
            string lineToParse = line.Trim();
            ///Copy the line and parse it for items (numbers).
            string[] tokens = new string[itemCount];
            int index = 0;
            try
            {
                /// read first item (the head) from left to right...       
                string item = Utility.Prefix(lineToParse.Trim(), separator);

                while (!string.IsNullOrEmpty(item))
                {
                    tokens[index] = item;

                    /// Read the rest (the tail, the postfix) of the current line
                    lineToParse = Utility.Postfix(lineToParse, separator);
                    lineToParse = lineToParse.Trim();
                    item = Utility.Prefix(lineToParse.Trim(), separator);
                    index++;
                }
                ///Last item...
                item = lineToParse;
                tokens[index] = item;

            }
            catch (Exception ex)
            {
                Log.ConsoleWriteline(ex.Message);
                throw new CheckedException(ErrorType.ParseFailure,
                                           string.Format(CultureInfo.InvariantCulture,
                                                         "Busy with {0}\n\rHalted on {1}\n\rError:{2}\n\r", line, lineToParse, ex.Message));
            }
            return tokens;
        }

        /// <summary>
        /// Parses the line for chars.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public Collection<string> ParseLine(string line, string separator)
        {
            string lineToParse = line.Trim();
            ///Copy the line and parse it for items (numbers).
            Collection<string> collection = new Collection<string>();

            try
            {
                /// read first item (the head) from left to right...       
                string item = Utility.Prefix(lineToParse.Trim(), separator);                

                while (!string.IsNullOrEmpty(item))
                {
                    collection.Add(item);

                    /// Read the rest (the tail, the postfix) of the current line
                    lineToParse = Utility.Postfix(lineToParse, separator);
                    lineToParse = lineToParse.Trim();
                    item = Utility.Prefix(lineToParse.Trim(), separator);
                }
                ///Last item...
                item = lineToParse;
                collection.Add(item);
            }
            catch (Exception ex)
            {
                Log.ConsoleWriteline(ex.Message);
                throw new CheckedException(ErrorType.ParseFailure,
                                           string.Format(CultureInfo.InvariantCulture,
                                                         "Busy with {0}\n\rHalted on {1}\n\rError:{2}\n\r", line, lineToParse, ex.Message));
            }
            return collection;
        }
    }
}