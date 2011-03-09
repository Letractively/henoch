using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCalculator
{
    public class TertaireStelsel:ICalculator
    {
        #region ICalculator Members

        public double Add(double x, double y)
        {
            //changed to 2 in order to demonstrate how a developer may introduce bugs
            return 2;///Dummy
        }

        /// <summary>
        /// Bewaart in een database.
        /// </summary>
        /// <returns></returns>
        public double PersistResults()
        {
            return 2;
        }

        #endregion




        public bool ReadInput()
        {
            return false;
        }

    }
}
