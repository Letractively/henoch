using System;
using System.Collections;
using System.Globalization;


namespace RetPok.Net
{


    /// <summary>
    /// Dit is de vertaling van InitData.m . Ook alle globals worden hierin gedeclareerd.
    /// De singleton pattern is toegepast ipv shared classes (statics).    
    /// </summary>
    public sealed class InitData 
    {
        /// De singleton pattern is toegepast ipv shared classes (statics):
        /// 1)Singletons can be used differently than static classes.
        /// 2)This design pattern can be used with interfaces (extract interface is niet mogelijk bij shared classes).       
        /// 3)Singletons can be used as parameters to methods.

        #region Consts

        /// <summary>
        /// 1000
        /// </summary>
        private const int MaxPeil = 1000;
        /// <summary>
        /// 1000
        /// </summary>
        private const int MaxOverschrijding = 1000;
        /// <summary>
        /// 18
        /// </summary>
        private const int Xsize = 18;
        /// <summary>
        /// 100
        /// </summary>
        private const int ConstLscenWsrpbu = 100;
        /// <summary>
        /// 5
        /// </summary>
        private const int ConstLscenDuur = 5;
        /// <summary>
        /// 3. Represents a 18 x 3 Matrix.
        /// </summary>
        private const int ConstLscenFase = 3;
        /// <summary>
        /// 100
        /// </summary>
        private const int ConstLscenWsos11 = 100;
        /// <summary>
        /// 5
        /// </summary>
        private const int ConstLscenWsn = 5;
        /// <summary>
        /// 2
        /// </summary>
        private const int ConstLscenSluit = 2;
        /// <summary>
        /// 12
        /// </summary>
        private const int ConstLscenWr = 12;
        /// <summary>
        /// 5
        /// </summary>
        private const int ConstLwrWsn = 5;
        #endregion

        /// <summary>
        ///  Private Constructor voorkomt instantiatie vanuit andere classes. Maak hier GEEN public constructors!
        /// </summary>
        private InitData()
        {
            //'Roompot Buiten   '
            //'Roompot Binnen   '
            //'Burghsluis       '
            //'Wemeldinge       '
            //'Rattekaai        '
            //'Marollegat       '
            //'Stavenisse       '
            //'Philipsdam West  '
            //'Colijnsplaat     '
            //'Zeelandbrug Noord'
            //'Roompot OSK      '
            //'Verval Roompot   '
            //'Verval Schaar    '
            //'Verval Hammen    '

            if (LocatieNamen==null) LocatieNamen = new ArrayList();
            LocatieNamen.Add("Roompot Buiten   ");
            LocatieNamen.Add("Roompot Binnen   ");
            LocatieNamen.Add("Burghsluis       ");
            LocatieNamen.Add("Wemeldinge       ");
            LocatieNamen.Add("Rattekaai        ");
            LocatieNamen.Add("Marollegat       ");
            LocatieNamen.Add("Stavenisse       ");
            LocatieNamen.Add("Philipsdam West  ");
            LocatieNamen.Add("Colijnsplaat     ");
            LocatieNamen.Add("Zeelandbrug Noord");
            LocatieNamen.Add("Roompot OSK      ");
            //LocatieNamen.Add("Verval Roompot   ");
            //LocatieNamen.Add("Verval Schaar    ");
            //LocatieNamen.Add("Verval Hammen    ");
            PrestatiePeilen = new[]{501, 316 ,317 ,355 ,382 ,382 ,345 ,365 ,323 ,325 ,501 ,420 ,418 , 422};
            Tpeilen = new[] {520d ,350d ,350d ,370d ,390d ,400d ,360d ,370d ,350d ,350d ,520d ,530d ,530d ,530d};
            Nimplic = 9;

            //['SSF00'
            // 'SSF01'
            // 'SSF02'
            // 'SSF05'
            // 'SSF10'
            // 'SSF16'
            // 'SSF31'
            // 'SSF47'
            // 'SSF62'
            // 'SNF00'
            // 'SNF01'
            // 'SNF02'
            // 'SNF05'
            // 'SNF10'
            // 'SNF16'
            // 'SNF31'
            // 'SNF47'
            // 'SNF62'
            if (ImplicNamen == null) ImplicNamen = new ArrayList();
            ImplicNamen.Add("SSF00");
            ImplicNamen.Add("SSF01");
            ImplicNamen.Add("SSF02");
            ImplicNamen.Add("SSF05");
            ImplicNamen.Add("SSF10");
            ImplicNamen.Add("SSF16");
            ImplicNamen.Add("SSF31");
            ImplicNamen.Add("SSF47");
            ImplicNamen.Add("SSF62");
            ImplicNamen.Add("SNF00");
            ImplicNamen.Add("SNF01");
            ImplicNamen.Add("SNF02");
            ImplicNamen.Add("SNF05");
            ImplicNamen.Add("SNF10");
            ImplicNamen.Add("SNF16");
            ImplicNamen.Add("SNF31");
            ImplicNamen.Add("SNF47");
            ImplicNamen.Add("SNF62");

            Nscen = 18;
            //'BEM00'
            //'BEM01'
            //'BEM02'
            //'BEM05'
            //'BEM10'
            //'BEM16'
            //'BEM31'
            //'BEM47'
            //'BEM62'
            //'ONB00'
            //'ONB01'
            //'ONB02'
            //'ONB05'
            //'ONB10'
            //'ONB16'
            //'ONB31'
            //'ONB47'
            //'ONB62'
            if (ScenarioNamen == null) ScenarioNamen = new ArrayList();
            ScenarioNamen.Add("BEM00");
            ScenarioNamen.Add("BEM01");
            ScenarioNamen.Add("BEM02");
            ScenarioNamen.Add("BEM05");
            ScenarioNamen.Add("BEM10");
            ScenarioNamen.Add("BEM16");
            ScenarioNamen.Add("BEM31");
            ScenarioNamen.Add("BEM47");
            ScenarioNamen.Add("BEM62");
            ScenarioNamen.Add("ONB00");
            ScenarioNamen.Add("ONB01");
            ScenarioNamen.Add("ONB02");
            ScenarioNamen.Add("ONB05");
            ScenarioNamen.Add("ONB10");
            ScenarioNamen.Add("ONB16");
            ScenarioNamen.Add("ONB31");
            ScenarioNamen.Add("ONB47");
            ScenarioNamen.Add("ONB62");

            //LscenWsrpbu = new MyCollection();
            //LscenDuur = new double[Xsize, Xsize, Xsize, Xsize, Xsize];
            //LscenFase = new double[Xsize, Xsize, Xsize];
            //LscenWsos11 = new MyCollection();
            //LscenWsn = new double[Xsize, Xsize, Xsize, Xsize, Xsize];
            //LscenSluit = new double[Xsize, Xsize];
            //LscenWr = new MyCollection();
            //LwrWsn = new double[Xsize, Xsize, Xsize, Xsize, Xsize];

            #region init globals

            ResetGlobals();

            #endregion


            Precision = 1;
        }


        /// <summary>
        /// Represents the globals in the prototype to be reset..
        /// </summary>
        public void ResetGlobals()
        {            
            Overschrijding = new double[MaxOverschrijding];
            Peil = new double[MaxPeil];

            /// Represents a 18 x 100 Matrix.
            LscenWsrpbu = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenWsrpbu[i] = new double[ConstLscenWsrpbu];
            }
            /// Represents a 18 x 5 Matrix.
            LscenDuur = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenDuur[i] = new double[ConstLscenDuur];
            }
            /// Represents a 18 x 3 Matrix.
            LscenFase = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenFase[i] = new double[ConstLscenFase];
            }
            /// Represents a 18 x 100 Matrix.
            LscenWsos11 = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenWsos11[i] = new double[ConstLscenWsos11];
            }
            /// Represents a 18 x 5 Matrix.
            LscenWsn = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenWsn[i] = new double[ConstLscenWsn];
            }
            /// Represents a 18 x 5 Matrix.
            LwrWsn = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LwrWsn[i] = new double[ConstLwrWsn];
            }
            /// Represents a 18 x 2 Matrix.
            LscenSluit = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenSluit[i] = new double[ConstLscenSluit];
            }
            /// Represents a 18 x 12 Matrix.
            LscenWr = new double[Xsize][];
            for (int i = 0; i < Xsize; i++)
            {
                LscenWr[i] = new double[ConstLscenWr];
            }
        }

        /// <summary>
        /// Private object instantiated with private constructor
        /// </summary>
        static readonly InitData Instance = new InitData();
        /// <summary>
        /// Dit representeert de unieke instantie van de data. Deze kan nooit meer dan 2x voorkomen
        /// in de heap.
        /// </summary> 
        public static InitData UniqueInstance
        {
            get { return Instance; }
        }

        /// <summary>
        /// Geeft filenaam terug.
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public string File(FileTypeRetPok fileType)
        {
            string file = "";
            string path = DeploymentDirectory;//Utility.GetAssemblyRootPath();

            switch (fileType)
            {
                case FileTypeRetPok.Onbekend:
                    file = string.Empty;
                    break;
                case FileTypeRetPok.KeringScenario:
                    file = path + @"\Scenariokansen.txt";
                    break;
                case FileTypeRetPok.TnoTabel:
                    file = path + @"\TNOkansen.txt";
                    break;
                case FileTypeRetPok.CorImplic:
                    file = path+ @"\Correcties_implic.dat";
                    break;
                default:
                    file = "";
                    break;
            }
            return file;
        }

        public ArrayList LocatieNamen { get; private set; }
        /// <summary>
        /// De onderstaande waarden zijn de prestatiepeilen dd 04-06-2009 
        /// </summary> 
        public int[] PrestatiePeilen { get; private set; }
        public double[] Tpeilen { get; private set; }
        public int Nimplic { get; private set; }
        public int Nscen { get; private set; }

        public ArrayList ImplicNamen { get; private set; }
        public ArrayList ScenarioNamen { get; private set; }

        /// <summary>
        /// This is the dir containing the subdirs for output.
        /// </summary>
        public string ResultDir { get; set; }
        /// <summary>
        /// This is the subdir where the files are saved, relative to Resultdir.
        /// </summary>
        public string OutputDir { get; set; }
        /// <summary>
        /// This is the root where the output files are deployed.
        /// </summary>
        public string DeploymentDirectory {get;set;}

        /// <summary>
        /// This is the root dir for the adapted version of the prototype.
        /// </summary>
        public string RootDirectory { get; set; }

        public string ImplicDirectory
        {
            get ; 
            set ; 
        }

        public string FaalkansDirectory
        {
            get;
            set;
        }

        public string TnoDirectory
        {
            get;
            set;
        }

        public string ToetsPeilenDirectory
        {
            get;
            set;
        }

        public double Precision { get; set; }

        #region Globals die gebruikt zijn.

        ///global overschrijding;
        ///global peil;
        ///global tpeilen;

        ///global LscenWsrpbu;
        ///global LscenFase;
        ///global LscenDuur;
        ///global LscenWr;
        ///global LscenWsn;
        ///global LscenWsos11;
        ///global LscenSluit;
        ///global LwrWsn;

        /// Represents a 1000 x 1 matrix
        public double[] Overschrijding { get; set; }

        /// Represents a 1000 x 1 matrix
        public double[] Peil { get; set; }
        /// <summary>
        /// Represents a 18 x 100 Matrix.
        /// </summary>
        public double[][] LscenWsrpbu { get; set; }
        /// <summary>
        /// Represents a 18 x 3 Matrix.
        /// </summary>
        public double[][] LscenFase { get; set; }
        /// <summary>
        /// Represents a 18 x 5 Matrix.
        /// </summary>
        public double[][] LscenDuur { get; set; }
        /// <summary>
        /// Represents a 18 x 12 Matrix.
        /// </summary>
        public double[][] LscenWr { get; set; }
        /// <summary>
        /// Represents a 18 x 5 Matrix.
        /// </summary>
        public double[][] LscenWsn { get; set; }
        /// <summary>
        /// Represents a 18 x 100 Matrix.
        /// </summary>
        public double[][] LscenWsos11 { get; set; }
        /// <summary>
        /// Represents a 18 x 2 Matrix.
        /// </summary>
        public double[][] LscenSluit { get; set; }
        /// <summary>
        /// Represents a 12 x 5 Matrix.
        /// </summary>
        public double[][] LwrWsn { get; set; }
        #endregion

    }

    public enum FileTypeRetPok
    {
        Onbekend = 0,
        KeringScenario =1,
        TnoTabel = 2,
        CorImplic = 3
    }

 
}
