using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using System.Runtime.InteropServices;

namespace TestParallelPatterns
{
	public class Timing: TestContext
	{
        //[ DllImport( "Kernel32.dll" )]
        //private static extern int GetTickCount( );

	    public static double Duration { get; private set; }
	    private	static double m_Start;

        public long Begin { get; private set; }
	    public Timing()
	    {
            Begin = Environment.TickCount;
            m_Start = Begin;
	    }
        public override void EndTimer(string timerName)
        {

            double stop = Environment.TickCount; //GetTickCount();
            double duration = (stop - Begin) / 10000000;
            WriteLine("*****************************************");
            WriteLine("Duration " + duration + "(s).");
            WriteLine("*****************************************");
            ///return duration;
        }

        public override void AddResultFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public override void BeginTimer(string timerName)
        {
            //BeginTimer(timerName);
            m_Start = Environment.TickCount;
        }

        public override System.Data.Common.DbConnection DataConnection
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Data.DataRow DataRow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Collections.IDictionary Properties
        {
            get { throw new NotImplementedException(); }
        }

        public override void WriteLine(string format, params object[] args)
        {
            Console.Out.WriteLine(format, args);
        }
    }

	
}