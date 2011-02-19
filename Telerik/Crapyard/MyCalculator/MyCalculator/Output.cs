namespace MyCalculator
{
    public class Output<Calculator> : IOutput
        where Calculator : ICalculator, new( ) 
    {
        private Calculator myCalculator;
        public Output( ) 
        {
            myCalculator = new Calculator( );
            ///Code coverage tool cannot verify
        }
        public double PersistResults ()
        {return myCalculator.PersistResults(); }



        public bool ReadInput()
        {
            return myCalculator.ReadInput();
        }


    }
}