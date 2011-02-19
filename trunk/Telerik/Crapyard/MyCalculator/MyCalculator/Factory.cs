namespace MyCalculator
{
    public class Factory<Calculator> : IFactory<Calculator>
        where Calculator : ICalculator, new( ) 
    {
        public IMathFunctions CreateNumeriekeModule( ) 
        {
            return new MathFunctions<Calculator>( );
        }
        public IOutput CreateUitvoer( ) {
            return new Output<Calculator>( );
        }
    }
}