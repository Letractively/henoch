using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCalculator
{
    /// <summary>
    /// Decimal space.
    /// </summary>
    public class Calculator:ICalculator
    {
        /// <summary>
        /// Calculates in decimal space using doubles.
        /// </summary>
        public Calculator()
        {

        }
        public double Antwoord { get; private set; }
        
        public double Add(double x, double y)
        {
            return x + y;
        }
        #region code

        public double TelOp(double[] doubles)
        {
            Antwoord = doubles.Aggregate((i, j) => i + j);
            return Antwoord;
        }

        public double PersistResults()
        {
            Console.Out.WriteLine(Antwoord);
            return Antwoord;
        }
        public bool ReadInput()
        {
            return false;
        }
        #endregion


    }
}
