using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyCalculator;

namespace MyCalculator
{
    // Concrete Factories (both in the same one)

    // Interface IProductA
    // Interface IProductB
    // All concrete ProductA's
    // All concrete ProductB's
    // An interface for all Calculators


    public class Computer<Calculator>
    where Calculator : ICalculator, new( ) 
    {
        public Computer()
        {
            SetModus();
        }
        public void SetModus( ) //IFactory<Calculator> factory) 
        {
            IFactory<Calculator> factory = new Factory<Calculator>( );
            MathFunctions = factory.CreateNumeriekeModule();
            Output = factory.CreateUitvoer();
            //Console.WriteLine("I bought a MathFunctions which is made from " + bag.Add);
            //Console.WriteLine("I bought some shoes which cost " + shoes.PersistResults);
        }

        public IMathFunctions MathFunctions { get; private set; }
        public IOutput Output { get; private set; }
    }
}
