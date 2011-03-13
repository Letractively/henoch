using System;
using System.Globalization;
using Scrap;

namespace Scrap
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string value = args[0];

                if (value != null)
                {

                    int i;
                    int fac = 0;
                    int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out i);
                    if (i > -1)
                    {
                        try
                        {
                            fac = MyMath.Fac(i);
                        }
                        catch(OverflowException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    return fac;
                }
            }

            return int.MinValue;
        }
    }
    
    public static class MyMath
    {
        public static int Fac(int i)
        {
            int res = 0;
            if (i == 0 || i == 1)
                return 1;
            if (i > 0 && i <=15)
            {
                checked
                {
                    res = Fac(i - 1) * i;
                }

                if (res > int.MaxValue)
                {
                    res = int.MaxValue;
                }
            }
            else
                throw new ArgumentException("Kies een positief getal < 16.");

            return res;         
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Sub(int a, int b)
        {
            return a - b;
        }

        public static double Pow(double x, double y)
        {
            return Math.Pow(x, y);
        }
    }
}


//public static class MyMath
//{
//    public static int Fac(int i)
//    {
//        return 0;
//    }
//}


        //public static int Main(string[] args)
        //{
        //    if (args != null && args.Length>0)
        //    {
        //        string value = args[0];

        //        if (value != null)
        //        {

        //            int i;
        //            int fac = 0;
        //            int.TryParse(value,NumberStyles.Any, CultureInfo.InvariantCulture, out i);
        //            if (i > -1)
        //            {
        //                fac = MyMath.Fac(i);
        //            }
        //            return fac;
        //        }
        //    }
        //    return int.MinValue;
        //}

//public static int Fac(int i)
//        {
//            if (i == 0) return 1;
//            int res = Fac(i - 1) * i ;
//            return res ;
//        }