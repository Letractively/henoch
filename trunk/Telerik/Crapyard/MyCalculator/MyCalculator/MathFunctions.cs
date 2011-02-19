namespace MyCalculator
{
    public class MathFunctions<Calculator> : IMathFunctions
        where Calculator : ICalculator, new() 
    {
        private Calculator myCalculator;
        public MathFunctions( ) 
        {
            myCalculator = new Calculator();
            ///Code coverage tool cannot verify
        }
        public double Add (double x, double y)
        { return myCalculator.Add(x,y); }
    }
}