using System.Collections;
using MyDataConsumer;
namespace DataResource
{
    public partial interface IGraphInfo
    {
        ICollection OverschrijdingsKansen { get; set; }
        IDataPointBase ToetsPeil { get; set; }
        IDataPointBase PrestatiePeil { get; set; }
    }

    public class GraphInfo<TDataConsumer> : IGraphInfo
        where TDataConsumer : MyDataConsumer.IDataConsumer, new() 
    {

        public ICollection OverschrijdingsKansen { get;  set; }
        public IDataPointBase ToetsPeil { get;  set; }
        public IDataPointBase PrestatiePeil { get;  set; }

        public  GraphInfo()
        {
            //disallow it.
        }
        public GraphInfo(ICollection overschrijdingsKansen, IDataPointBase toetsPeil, IDataPointBase prestatiePeil)
        {
            OverschrijdingsKansen = overschrijdingsKansen;
            ToetsPeil = toetsPeil;
            PrestatiePeil = prestatiePeil;
        }

    }
    public class GraphInfoSimple: IGraphInfo
    {

        public ICollection OverschrijdingsKansen { get;  set; }
        public IDataPointBase ToetsPeil { get;  set; }
        public IDataPointBase PrestatiePeil { get;  set; }

        public GraphInfoSimple()
        {
            //disallow it.
        }
        public GraphInfoSimple(ICollection overschrijdingsKansen, IDataPointBase toetsPeil, IDataPointBase prestatiePeil)
        {
            OverschrijdingsKansen = overschrijdingsKansen;
            ToetsPeil = toetsPeil;
            PrestatiePeil = prestatiePeil;
        }

    }
}