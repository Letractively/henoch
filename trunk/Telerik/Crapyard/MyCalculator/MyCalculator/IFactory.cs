namespace MyCalculator
{
    public interface IFactory<Calculator>
        where Calculator : ICalculator 
    {
        IMathFunctions CreateNumeriekeModule( );
        IOutput CreateUitvoer( );
    }
}