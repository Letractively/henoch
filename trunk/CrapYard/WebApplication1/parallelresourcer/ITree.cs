using System;
using Dictionary.System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace Dictionary.System
{
    public interface ITree<TKeyValue>
    {
        Tree<TKeyValue> CreateNTree(IList<XElement> outerTrack, TKeyValue rootValue, 
                                    IDictionary<TKeyValue, IList<TKeyValue>> outerdictionary, 
                                    Func<TKeyValue, IDictionary<TKeyValue, IList<TKeyValue>>, IList<TKeyValue>> GetRelations, 
                                    Action<TKeyValue, TKeyValue, IList<TKeyValue>> TransFormXsubTree, 
                                    Func<TKeyValue, IList<TKeyValue>, IList<XElement>> CreateXML);
        IList<TKeyValue> GetRoots(TKeyValue virtualRoot, TKeyValue node, IDictionary<TKeyValue, 
                                    IList<TKeyValue>> dictionary);
        TKeyValue Key { get; set; }
        ConcurrentDictionary<TKeyValue, TKeyValue> Nodes { get; }
        string UID { get; }
        int Weight { get; set; }
    }
}
