using System.Collections.ObjectModel;

namespace DataResource
{
    public interface IDataPoint : DataResource.IDataPointBase
    {
        double X { get; set; }
        double Y { get; set; }
        double[] Scalar { get; set; }
    }
    public partial interface IDataPointBase
    {
        double X { get; }
        double Y { get; }

        /// <summary>
        /// The elements of a Vector (linear algebra).
        /// </summary>
        double[] Scalar { get; }

        double this[int i] { get; set; }
    }

    public class DataPoint : IDataPointBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        /// <summary>
        /// The elements of a Vector (linear algebra).
        /// </summary>
        public double[] Scalar { get; private set; }
        public DataPoint(double x, double y)
        {
            Scalar = new[] {x, y};
            X = x;
            Y = y;
        }
        public double this[int i]
        {
            get { return Scalar[i]; }
            set { Scalar[i] = value; }
        }
    }
    /// <summary>
    /// This graph is surjective (no double x-values allowed: item[0]).
    /// See DataGraphTest for simple usage.
    /// </summary>
    public class DataGraph : KeyedCollection<double, DataPoint>
    {

        #region Overrides of KeyedCollection<double,DataPoint>

        protected override double GetKeyForItem(DataPoint item)
        {
            return item[0];
        }

        #endregion

    }
}
