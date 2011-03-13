using System;
using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace Scrap.Patterns
{
    /// <summary>
    /// Represents the custom functions by end-user to be adapted.
    /// </summary>
    /// 
    [Serializable]
    public class Adaptee : IDistributions ///Inheritance causes stack overflow: use interface inheritance.
    {
        #region IDistributions Members

        /// <summary>
        /// Normal distribution made by end-user.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public double NormCdf(double delta)
        {
            double cdf;
            //% Alternatief voor normcdf

            //% dt = 1.0E-04;
            //double dt = 1.0E-02; // RtN voor test 
            double dt = 1.0E-01; // RtN voor test 
            double og = -10.0;

            double sigma = 0;
            double max = delta - dt;
            double step = dt;
            double t;

            for (t = og; t <= max; t += step)
            {
                sigma = sigma + dt*Math.Exp(-(t*t)/2);
            }
            t = delta;
            sigma = sigma + 0.5*dt*Math.Exp(-(t*t)/2);

            cdf = (1/Math.Sqrt(2*Math.PI))*sigma;

            return cdf;
        }

        #endregion

        public double Precise(double a, double b)
        {
            return a/b;
        }
    }

    /// <summary>
    /// Represents the MathDotNet library for the adapter.
    /// </summary>
    [Serializable]
    public class Distributions : IDistributions
    {
        #region IDistributions Members

        /// <summary>
        /// Represents the target for the adapter.
        /// </summary>
        //public Distributions()
        //{
        //}
        public double NormCdf(double x)
        {
            double cdf;
            Debug.Assert(!double.IsNaN(x));
            var normCdf = new NormalDistribution(0, 1);

            cdf = normCdf.CumulativeDistribution(x);
            return cdf;
        }

        #endregion

        public string Estimate(int i)
        {
            return "Estimate is " + (int) Math.Round(i/3.0);
        }
    }

    /// <summary>
    /// Adapts math features of the adaptee.
    /// </summary>
    [Serializable]
    public class Adapter : Adaptee
    {
        private readonly IDistributions m_Distributions;
        private readonly Func<double, double> m_NormCdf;
        // Different constructors for the expected targets/adaptees
        // Adapter-Adaptee
        // ReSharper disable UnusedParameter.Local
        /// <summary>
        /// The Adeptee feature will be altered.
        /// </summary>
        /// <param name="adaptee"></param>
        public Adapter(Adaptee adaptee, double dt)
            // ReSharper restore UnusedParameter.Local
        {
            m_NormCdf = ((double arg) => new Distributions().NormCdf(arg));
            // Set the delegate to the new standard
            if (adaptee != null)
            {
                m_Distributions = adaptee;
            }
        }

        /// <summary>
        ///  Adapter-Target
        /// </summary>
        /// <param name="target"></param>
        public Adapter(Distributions target)
        {
            // Set the delegate to the existing standard
            //Request = (arg => target.Estimate(arg));
            m_NormCdf = ((double arg) => new Distributions().NormCdf(arg));
            m_Distributions = target;
        }

        /// <summary>
        /// Defaults to the adaptee (the math features made by the customer).
        /// </summary>
        public Adapter()
        {
            m_Distributions = new Adaptee();
            m_NormCdf = m_Distributions.NormCdf;
        }

        /// <summary>
        /// Represents the target function.
        /// </summary>
        public new Func<double, double> NormCdf
        {
            get { return m_NormCdf; }
        }

        /// <summary>
        /// Represents the instance created by the constructor.
        /// </summary>
        public IDistributions Distributions
        {
            get { return m_Distributions; }
        }
    }
}