using System;

namespace TestObservlet.SampleBusinessObjects
{
    public interface IAnimal
    {
        int Legs { get; set; }
        int Eyes { get; set; }
        string Name { get; set; }
        string Species { get; set; }	event EventHandler Hungry;
        string GetMood();
    }
}