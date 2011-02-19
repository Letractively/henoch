using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCalculator
{
    /// <summary>
    /// Summary description for TestObjectContainer
    /// </summary>
    [TestClass]
    public class TestObjectContainer
    {
        protected static TestContext _Context;
        protected const int DataOmgevingId = 300;

// ReSharper disable EmptyConstructor
        public TestObjectContainer()
// ReSharper restore EmptyConstructor
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext TestContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return TestContextInstance;
            }
            set
            {
                TestContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }


        /// <summary>
        /// Runs once.
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize]
        public static void InitializeWaarden(TestContext testContext)
        {
            
            if (_Context == null)
            {
               
            }
        }
        #endregion

        [TestMethod]
        public void TestThreading()
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    Console.Write("Forcibly ");
                }
                Console.WriteLine("Woken!");
            });
            t.Start();
            t.Interrupt();
        }
        //[TestMethod]
        //public void TestOOErd()
        //{
        //    IComponent<PlaatsObject> plaatsObject = new Component<PlaatsObject>(new PlaatsObject { GeoObjectCode = "Aap" }, "1");
        //    IComponent<PlaatsObject> plaatsObject2 = new Component<PlaatsObject>(plaatsObject.Uid, "1");
        //    Assert.AreEqual(plaatsObject.Uid, plaatsObject2.Uid);
        //    Assert.AreNotEqual(plaatsObject, plaatsObject2);

        //    plaatsObject2 = new Component<PlaatsObject>(new PlaatsObject(), "1");
        //    Assert.AreNotEqual(plaatsObject.Uid, plaatsObject2.Uid);
        //    Assert.AreNotEqual(plaatsObject, plaatsObject2);


        //    var waterlichaam = new PlaatsObject{GeoObjectCode = "Aap"};
        //    var waterlichaam2 = new PlaatsObject { GeoObjectCode = "Aap" };
        //    Assert.AreNotEqual(waterlichaam, waterlichaam2);
        //    Assert.AreNotEqual(plaatsObject.Uid, waterlichaam);

        //    IWaarde waarde = new Waarde { GeoObjectCode = "AapWaarde", WaardeID = 1 };
        //    IWaarde waarde2 = new Waarde(waarde as Waarde);
        //    Assert.AreNotEqual(waarde, waarde2);

        //    Waarde waarde3Versie1 = new Waarde { GeoObjectCode = "AapWaarde", WaardeID=3 };
        //    IComponent<Waarde> waarde3Versie2 = new Component<Waarde>(waarde3Versie1,"waarde3");
        //    Waarde waarde4 = new Waarde(waarde3Versie1 as Waarde);
        //    Assert.AreNotEqual(waarde3Versie1, waarde4);

        //    Composite<Waarde> waarden = new Composite<Waarde>(new Waarde { GeoObjectCode = "AapWaarde", WaardeID = 1 }, "record1");
        //    waarden.Add(new Component<Waarde>(waarde2 as Waarde, "record2"));
        //    waarden.Add(new Component<Waarde>(waarde3Versie1, "record3"));
        //    waarden.Add(new Component<Waarde>(waarde4, "record4"));
        //    waarden.Add(new Component<Waarde>(new Waarde { WaardeID=5}, "record5"));
        //    Assert.AreEqual(5,waarden.Components.Count());

        //    var waarde5 =waarden.Components.Skip(1).First();
        //    Assert.AreEqual(waarde5, waarde5);
        //    Assert.AreNotEqual(waarden.Components.First(), waarden.Components.Skip(1).First());
            
        //    List<Waarde> listWaarde = new List<Waarde>();
        //    List<Waarde> exceptionaleListWaarde = new List<Waarde>();
        //    listWaarde.Add(waarde as Waarde);
        //    listWaarde.Add(waarde2 as Waarde);
        //    listWaarde.Add(waarde3Versie1);
        //    exceptionaleListWaarde.Add(waarde3Versie1 as Waarde);

        //    var deRest = listWaarde.Except(exceptionaleListWaarde, new WaardeComparer());
        //    Assert.AreEqual(1, deRest.Count());
        //    deRest = listWaarde.Except(exceptionaleListWaarde);
        //    Assert.AreEqual(2, deRest.Count());

        //    Dictionary<Waarde, IComponent<Waarde>> otherList = new Dictionary<Waarde,IComponent<Waarde>>();
        //    Dictionary<Waarde, IComponent<Waarde>> exceptionalOtherList = new Dictionary<Waarde, IComponent<Waarde>>();
        //    otherList.Add(waarde as Waarde, new Component<Waarde>(waarde as Waarde, "waarde"));
        //    otherList.Add(waarde2 as Waarde, new Component<Waarde>(waarde2 as Waarde, "waarde2"));
        //    otherList.Add(waarde3Versie1, new Component<Waarde>(waarde3Versie1, "waarde3"));
        //    exceptionalOtherList.Add(waarde3Versie1 as Waarde, new Component<Waarde>(waarde3Versie1 as Waarde, "waarde3Versie1"));

        //    List<IComponent<Waarde>> exceptionalListComponents = new List<IComponent<Waarde>>();
        //    exceptionalListComponents.Add(waarde3Versie2 as IComponent<Waarde>);
        //    //var deRest2 = waarden.Components.Except(exceptionalListComponents, new ComponentComparer<Waarde>());


            
        //}
        [TestMethod]
        public void TestCodeSamples()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 3) continue;
                Debug.WriteLine(i); 
            }
        }

    }


}
