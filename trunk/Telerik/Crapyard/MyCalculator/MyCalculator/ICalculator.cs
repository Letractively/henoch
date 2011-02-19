namespace MyCalculator
{
    public interface ICalculator 
    {
        double Add (double x , double y);
        double PersistResults();
        bool ReadInput();
    }
}