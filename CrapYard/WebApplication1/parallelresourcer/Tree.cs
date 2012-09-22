using System;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

namespace ParallelResourcer
{
    public class Tree<TKeyValue> : IEnumerable
    {

        // 1) Define a delegate type.
        public delegate void TreeHandler(string msgForCaller);
        // 2) Define a member variable of this delegate.
        private static TreeHandler _listOfHandlers;
        // 3) Add registration function for the caller.
        public void RegisterWithTree(TreeHandler methodToCall)
        {
            //multicasting support
            _listOfHandlers += methodToCall;
        }
        public void UnRegisterWithTree(TreeHandler methodToCall)
        {
            _listOfHandlers -= methodToCall;
        }

        public static XDocument XDoc;
        public static ConcurrentQueue<TKeyValue> Queue;
        public static ConcurrentStack<IList<XElement>> StackNodes;
        public static ConcurrentQueue<XElement> QueueNodes;
        public Tree<TKeyValue> Left, Right;
        public IList<Tree<TKeyValue>> NTree;
        public TKeyValue Data;

        /// <summary>
        /// Indicates whether the node is unique.
        /// </summary>
        public bool IsUnique { get; private set; }
        /// <summary>
        /// List of all nodes keyvalues.
        /// </summary>
        public static ConcurrentDictionary<TKeyValue, TKeyValue> Nodes { get; private set; }

        private TKeyValue _Key;

        /// <summary>
        /// The uniqueness of the key will be also set.
        /// </summary>
        public TKeyValue Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
                //if (Nodes == null) Nodes = new LinkedList<TKeyValue>();
                if (Nodes.TryAdd(_Key, _Key))
                    IsUnique = true;
                else
                    IsUnique = false;
            }
        }

        private string _UID;

        public string UID
        {
            get
            {
                if (string.IsNullOrEmpty(_UID))
                    _UID = Guid.NewGuid().ToString();

                return _UID;
            }
        }

        public int Weight { get; set; }

        static Tree()
        {
            InitializeObjects();
        }

        private static void InitializeObjects()
        {
            Queue = new ConcurrentQueue<TKeyValue>();
            StackNodes = new ConcurrentStack<IList<XElement>>();
            QueueNodes = new ConcurrentQueue<XElement>();
            Nodes = new ConcurrentDictionary<TKeyValue, TKeyValue>();
            XDoc = new XDocument();
        }
        public Tree()
        {
            IsUnique = true;

            Nodes = new ConcurrentDictionary<TKeyValue, TKeyValue>();
        }
        public static IEnumerable<Tree<TKeyValue>> Iterate(Tree<TKeyValue> head)
        {
            for (Tree<TKeyValue> i = head; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static IEnumerable<TKeyValue> Iterate<TKeyValue>(
            Func<TKeyValue> initialization, Func<TKeyValue, bool> condition, Func<TKeyValue, TKeyValue> update)
        {
            for (TKeyValue i = initialization(); condition(i); i = update(i))
            {
                yield return i;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (Tree<TKeyValue> i = this; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static void WalkParallel<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action, bool waitAll = false)
        {
            if (root == null) return;
            //LRW wandeling in parallel!
            var t2 = Task.Factory.StartNew(() => WalkParallel(root.Left, action)
                , TaskCreationOptions.AttachedToParent);
            var t3 = Task.Factory.StartNew(() => WalkParallel(root.Right, action)
                , TaskCreationOptions.AttachedToParent);
            var t1 = Task.Factory.StartNew(() => action(root.Data)
                , TaskCreationOptions.AttachedToParent);
            if (waitAll) Task.WaitAll(t1, t2, t3);
        }
        /// <summary>
        /// Walk the tree and do an action for each node
        /// </summary>
        /// <typeparam name="TKeyValue"></typeparam>
        /// <param name="root"></param>
        /// <param name="action"></param>
        /// <param name="waitAll"></param>
        public static void WalkParallelNTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action, bool waitAll = false)
        {
            if (root == null) return;

            if (root.NTree == null)
            {
                var t0 = Task.Factory.StartNew(() => action(root.Data)
                        , TaskCreationOptions.AttachedToParent);
                if (waitAll) Task.WaitAll(t0);
                return;
            }

            int countNodes = root.NTree.Count;
            Task[] tasks = new Task[countNodes + 1];
            tasks[countNodes] = Task.Factory.StartNew(() => action(root.Data)
                , TaskCreationOptions.AttachedToParent);

            Parallel.For(0, countNodes,
                (i) =>
                {
                    tasks[i] = Task.Factory.StartNew(() => WalkParallelNTree(root.NTree[i], action, waitAll));
                }
                );

            if (waitAll) Task.WaitAll(tasks);
        }
        /// <summary>
        /// Walk the tree and do an action for each node
        /// </summary>
        /// <typeparam name="TKeyValue"></typeparam>
        /// <param name="root"></param>
        /// <param name="action"></param>
        /// <param name="waitAll"></param>
        public static void WalkParallelNTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue, IList<TKeyValue>> action, bool waitAll = false)
        {
            if (root == null) return;

            if (root.NTree == null)
            {
                var t0 = Task.Factory.StartNew(() => action(root.Data, null)
                        , TaskCreationOptions.AttachedToParent);
                if (waitAll) Task.WaitAll(t0);
                return;
            }

            int countNodes = root.NTree.Count;
            Task[] tasks = new Task[countNodes];
            IList<TKeyValue> children = new List<TKeyValue>();

            Parallel.For(0, countNodes,
                (i) =>
                {
                    tasks[i] = Task.Factory.StartNew(() => WalkParallelNTree(root.NTree[i], action, waitAll));
                    children.Add(root.NTree[i].Data);
                }
                );

            if (waitAll) Task.WaitAll(tasks);

            var task = Task.Factory.StartNew(() => action(root.Data, children), TaskCreationOptions.AttachedToParent);
            Task.WaitAll(task);

        }

        public static void WalkNaryTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue, IList<TKeyValue>> action)
        {
            if (root == null)
                return;

            IList<TKeyValue> children = new List<TKeyValue>();
            if (root.NTree == null)
            {
                action(root.Data, null);
                return;
            }
            int countNodes = root.NTree.Count;

            for (int i = 0; i < countNodes; i++)
            {
                //travelsal of children.
                WalkNaryTree(root.NTree[i], action);
                children.Add(root.NTree[i].Data);
            }
            action(root.Data, children);
        }

        public static Tree<string> CreateParseTree(XElement parent, Action<XElement> action)
        {
            if (parent == null)
                return null;

            if (parent.Elements() != null && parent.Elements().Count() == 0)
            {
                action(parent);
                Tree<string> leaf = new Tree<string>() { Data = parent.Attribute("Text").Value };
                return leaf;
            }
            int countNodes = parent.Elements().Count();
            Tree<string> parseTree = new Tree<string>() { Data = parent.Attribute("Text").Value};
            // create subtrees
            parseTree.NTree = new List<Tree<string>>();

            for (int i = 0; i < countNodes; i++)
            {
                XElement child = parent.Elements().ToList()[i];
                action(parent);
                //travelsal of children.

                parseTree.NTree.Add(CreateParseTree(child, action));
            }

            return parseTree;
        }
        public static void WalkNaryTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action)
        {
            if (root == null)
                return;

            if (root.NTree == null)
            {
                action(root.Data);
                return;
            }
            int countNodes = root.NTree.Count;
            action(root.Data);

            for (int i = 0; i < countNodes; i++)
            {
                //travelsal of children.
                WalkNaryTree(root.NTree[i], action);
            }

        }
        public static void WalkClassic<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left, action);
            WalkClassic(root.Right, action);
            action(root.Data);
        }
        public static void WalkClassic<TKeyValue>(Tree<TKeyValue> root)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left);
            WalkClassic(root.Right);
            _listOfHandlers(root.Data.ToString());
        }
        public static IList<TKeyValue> GetParents(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary)
        {
            var parents = from pair in dictionary
                          where pair.Value != null && pair.Value.Where(val => val.Equals(node)).FirstOrDefault() != null
                          select pair.Key;

            return parents.ToList<TKeyValue>();
        }

        public TKeyValue GetRoot(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary)
        {
            TKeyValue parent = node;

            var parents = GetParents(node, dictionary);
            if (parents!= null && parents.Count>0)
            {
                parent = GetRoot(parents[0], dictionary);
            }

            return parent;
        }


        public static IList<XElement> GetParents(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary,
                                                     Func<TKeyValue, IList<TKeyValue>, IList<XElement>> CreateXElements)
        {
            var parents = GetChildren (node, dictionary);

            var list = CreateXmlElementsTopDown(node, parents);

            return list;
        }
        public static IList<TKeyValue> GetChildren(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary)
        {
            #region return null for root values : null, empty
            string defaultVar = default(string);
            if (node == null)
                return null;
            else
            {
                defaultVar = node.ToString();
                if (string.IsNullOrEmpty(defaultVar))
                    return null;
            }
            #endregion

            IList<TKeyValue> list;

            dictionary.TryGetValue(node, out list);
            if (list == null)
                list = new List<TKeyValue>();

            return list;
        }
        public static Tree<TKeyValue> CreateParellelNTree(TKeyValue rootValue, IDictionary<TKeyValue, IList<TKeyValue>> outerdictionary,
                                            Func<TKeyValue, IDictionary<TKeyValue, IList<TKeyValue>>, IList<TKeyValue>> GetRelations,
                                            Action<TKeyValue, TKeyValue, IList<TKeyValue>> TransFormXsubTree)
        {
            var parents = GetRelations(rootValue, outerdictionary);

            Tree<TKeyValue> nTree = new Tree<TKeyValue>();
            nTree.Key = rootValue;
            nTree.Data = rootValue;

            int parentCount = parents.Count;

            if (parentCount > 0)
            {
                // create subtrees
                nTree.NTree = new List<Tree<TKeyValue>>();

                int countNodes = parents.Count();
                var t1 = Task.Factory.StartNew(() => Parallel.For(0, countNodes,
                    (i) =>
                    {
                        var t2 = Task.Factory.StartNew(() =>
                            nTree.NTree.Add(CreateParellelNTree(parents[i], outerdictionary, GetRelations, TransFormXsubTree)));

                        Task.WaitAll(t2);

                        TransFormXsubTree(rootValue, parents[i], parents);
                    }
                    )
                );
                Task.WaitAll(t1);
                CreateOneSubTree(parents.Count());
            }
            else
            {
                //push LEAF on stack
                StackNodes.Push(new List<XElement>() 
                    {  
                        new XElement("Node",
                            new XAttribute("Text", rootValue.ToString()),
                            new XAttribute("Font-Italic", "False"),
                            new XAttribute("Expanded", "True"))
                    });
            }

            return nTree;
        }
        /// <summary>
        /// Creates  a N-ary tree. A node has a key-valued pair 
        /// </summary>
        /// <param name="rootValue"></param>
        /// <param name="dictionary"></param>
        /// <param name="GetRelations"></param>
        /// <returns></returns>
        public Tree<TKeyValue> CreateNTree(IList<XElement> outerTrack, TKeyValue rootValue, IDictionary<TKeyValue, IList<TKeyValue>> outerdictionary,
                                                    Func<TKeyValue, IDictionary<TKeyValue, IList<TKeyValue>>, IList<TKeyValue>> GetRelations,
                                                    Action<TKeyValue, TKeyValue, IList<TKeyValue>> TransFormXsubTree)
        {
            var parents = GetRelations(rootValue, outerdictionary);

            Tree<TKeyValue> nTree = new Tree<TKeyValue>();
            nTree.Key = rootValue;
            nTree.Data = rootValue;

            int parentCount = parents.Count;

            if (parentCount > 0)
            {
                // create subtrees
                nTree.NTree = new List<Tree<TKeyValue>>();
                var sortedParents = parents.OrderByDescending(p => p.ToString());
                foreach (var parent in sortedParents)
                {

                    var track = GetParents(rootValue, outerdictionary, CreateXmlElementsTopDown);
                    var outerTrackEndNode = outerTrack.Descendants().Where(e => e.Attribute("Text").Value == rootValue.ToString());
                    var descendants = track.Where(e => e.Descendants().Any(a => a.Attribute("Text").Value == parent.ToString()));
                    
                    bool isVisitedBefore = outerTrack.Descendants().Any(d => d.Attribute("Text").Value == parent.ToString());
                    if (nTree.IsUnique && !isVisitedBefore)
                    {
                        outerTrackEndNode.First().Add(descendants.Descendants());
                        nTree.NTree.Add(CreateNTree(outerTrack, parent, outerdictionary, GetRelations, TransFormXsubTree));

                        TransFormXsubTree(rootValue, parent, parents);

                        var subTreeTrack = outerTrack.Descendants().Where(d => d.Attribute("Text").Value == nTree.Key.ToString());
                        subTreeTrack.First().Elements().Remove();

                    }
                    else
                    {
                        var parentXNode = new XElement("Node",
                                new XAttribute("Text", parent.ToString()),
                                new XAttribute("Font-Italic", "False"),
                                new XAttribute("Expanded", "True"),
                                new XAttribute("BackColor", "Red"));
                        #region removed add children
                        //var childrenParent = GetChildren(parent, outerdictionary);

                        //foreach (var child in childrenParent)
                        //{
                        //    parentXNode.Add(new XElement("Node",
                        //        new XAttribute("Text", child.ToString()),
                        //        new XAttribute("Font-Italic", "False"),
                        //        new XAttribute("Expanded", "True"),
                        //        new XAttribute("BackColor", "Red")));
                        //}
                        #endregion
                        //push node with LEAFs on stack
                        StackNodes.Push(new List<XElement>() {parentXNode});

                        TransFormXsubTree(rootValue, parent, parents);
                    }


                }
                if (parentCount > 0)
                    CreateOneSubTree(parentCount);
            }
            else
            {
                //push LEAF on stack
                StackNodes.Push(new List<XElement>() 
                    {  
                        new XElement("Node", 
                            new XAttribute("Text", rootValue.ToString()),
                            new XAttribute("Font-Italic", "False"),
                            new XAttribute("Expanded", "True"))
                    });
            }

            return nTree;
        }
        /// <summary>
        /// The number of subtrees is equal to the number of the last stackitems pushed in current recursion depth.
        /// Every subtree has a root which has one child (injection).
        /// </summary>
        /// <param name="countParents"></param>
        public static void CreateOneSubTree(int countParents)
        {
            IList<XElement> stackItem;
            IList<string> duplicateCandidates = new List<string>();

            Tree<string>.StackNodes.TryPop(out stackItem);
            duplicateCandidates.Add(stackItem.Elements().First().Attribute("Text").Value);

            for (int i = 0; i < (countParents - 1); i++)
            {
                IList<XElement> nextStackItem;
                Tree<string>.StackNodes.TryPop(out nextStackItem);

                bool isLeaf = nextStackItem.Elements().Count() == 0;
                if (!isLeaf)
                {
                    duplicateCandidates.Add(nextStackItem.Elements().First().Attribute("Text").Value);
                    stackItem.First().Add(nextStackItem.Descendants().First());
                }
                else
                {
                    Debug.Assert(isLeaf);
                    stackItem.Add(nextStackItem.First());
                }
            }

            ///Try to disable the duplicates in any subtree
            foreach (var candidate in duplicateCandidates)
            {
                var duplicates = stackItem.Elements().Descendants().Where(d => d.Attribute("Text").Value == candidate);

                foreach (var duplicate in duplicates)
                {
                    duplicate.Attribute("Expanded").Value = "False";//ForeColor="#FF8000" 
                    duplicate.Attribute("Font-Italic").Value = "True";//ForeColor="#FF8000" 
                }
            }
            Tree<string>.StackNodes.Push(stackItem);
        }

        /// <summary>
        /// Expand each child's relationship in parent's subtree with the current relation of the parent.
        /// The node pushed on stack has only one child: injection.
        /// </summary>
        /// <param name="rootValue"> parent R rootValue </param>
        /// <param name="parents">Collection of parents in any relation current tree depth</param>
        /// <param name="parent" > parent R rootValue </param>
        public static void TransFormXSubTreeBottomUp(TKeyValue rootValue, TKeyValue parent, IList<TKeyValue> parents)
        {
            IList<XElement> listSubtreeParents;
            IList<XElement> newListxElt = new List<XElement>();
            //pop the root value of current subtree from stack
            StackNodes.TryPop(out listSubtreeParents);

            ///relationships of node with rootValue in current depth
            IList<XElement> listCurDepthRelations = CreateXmlElementsBottomUp(rootValue, parents);
            var childrenOfP1InSubtree = listSubtreeParents.Descendants().Where(d => d.Attribute("Text").Value == parent.ToString());
            var childOfP1 = listCurDepthRelations.Where(d => d.Attribute("Text").Value == parent.ToString());

            if (childrenOfP1InSubtree.Count() > 0)
            {
                for (int i = 0; i < childrenOfP1InSubtree.Count(); i++)
                {
                    childrenOfP1InSubtree.ToList()[i].Add(childOfP1.FirstOrDefault().Elements());
                };
                StackNodes.Push(listSubtreeParents);
            }
            else
                StackNodes.Push(childOfP1.ToList());
        }
        /// <summary>
        /// Every child of the rootValue gets the children of the child´s relation in its subtree.
        /// The node pushed on stack has only one child: injection.
        /// </summary>
        /// <param name="rootValue">rootValue R parent  </param>
        /// <param name="parents">Collection of parents in any relation current tree depth</param>
        /// <param name="parent" > rootValue R parent </param>
        public static void TransFormXSubTreeTopDown(TKeyValue rootValue, TKeyValue parent, IList<TKeyValue> parents)
        {
            IList<XElement> listSubtreeParents = new List<XElement>();
            IList<XElement> newListxElt = new List<XElement>();
            //pop the root value of current subtree from stack
            StackNodes.TryPop(out listSubtreeParents);

            ///relationships of node with rootValue in current depth
            IList<XElement> listCurDepthRelations = CreateXmlElementsTopDown(rootValue, parents);
            var childrenOfP1InSubtree = listSubtreeParents.Where(d => d.Attribute("Text").Value == parent.ToString());
            var childOfP1 = listCurDepthRelations.Descendants().Where(d => d.Attribute("Text").Value == parent.ToString());

            if (childrenOfP1InSubtree.Count() > 0)
            {
                for (int i = 0; i < childrenOfP1InSubtree.Count(); i++)
                {
                    if (childrenOfP1InSubtree.FirstOrDefault() != null)
                        childOfP1.First().Add(childrenOfP1InSubtree.ToList()[i].Elements());
                };
                var res = listCurDepthRelations.Where(elt => elt.Elements().Any(e => e.Attribute("Text").Value == parent.ToString()));
                StackNodes.Push(res.ToList());
            }
            else
                StackNodes.Push(childOfP1.ToList());
        }

        public static IList<XElement> CreateXmlElementsBottomUp(TKeyValue parent, IList<TKeyValue> children)
        {

            var nodeParent = new XElement("Node", 
                new XAttribute("Font-Italic", "False"));
            nodeParent.Add(new XAttribute("Text", parent));
            nodeParent.Add(new XAttribute("Expanded", "True"));
            var list = new List<XElement>();

            if (children != null && children.Count > 0)
                foreach (var child in children)
                {
                    var nodeChild = new XElement("Node", new XAttribute("Font-Italic", "False"));
                    nodeChild.Add(new XAttribute("Text", child));
                    nodeChild.Add(new XAttribute("Expanded", "True"));

                    nodeChild.Add(nodeParent);
                    //StackNodes.Push(nodeChild);
                    list.Add(nodeChild);
                }
            else
            //StackNodes.Push(nodeParent);
            {

                list.Add(nodeParent);

            }
            return list;
        }
        public static IList<XElement> CreateXmlElementsTopDown(TKeyValue parent, IList<TKeyValue> children)
        {
            var list = new List<XElement>();

            if (children != null && children.Count > 0)
                foreach (var child in children)
                {
                    var nodeParent = new XElement("Node", new XAttribute("Font-Italic", "False"));
                    nodeParent.Add(new XAttribute("Text", parent));
                    nodeParent.Add(new XAttribute("Expanded", "True"));

                    var nodeChild = new XElement("Node", new XAttribute("Font-Italic", "False"));
                    nodeChild.Add(new XAttribute("Text", child));
                    nodeChild.Add(new XAttribute("Expanded", "True"));

                    nodeParent.Add(nodeChild);
                    //StackNodes.Push(nodeChild);
                    list.Add(nodeParent);
                }
            else
            //StackNodes.Push(nodeParent);
            {
                var nodeParent = new XElement("Node", new XAttribute("Font-Italic", "False"));
                nodeParent.Add(new XAttribute("Text", parent));
                nodeParent.Add(new XAttribute("Expanded", "True"));

                list.Add(nodeParent);

            }
            return list;

        }

        public static Tree<string> CreateTestNaryTree()
        {
            var testTree = new Tree<string>
            {
                Data = "root",
                #region subtree root
                NTree = new Tree<string>[]
                {
                    new Tree<string>
                    { 
                        Data = "S11",
                        NTree = new Tree<string>[]
                        {
                            new Tree<string>{ Data = "S211"},
                            new Tree<string>{ Data = "S221"}
                        }

                    },
                    new Tree<string>
                    { 
                        Data = "S211",
                        NTree = new Tree<string>[]
                        {
                          new Tree<string>
                          {
                              Data = "S22",
                              NTree = new Tree<string>[]
                             {
                                new Tree<string>{ Data = "S41",
                                NTree = new Tree<string>[]
                                {
                                    new Tree<string>{ Data = "S51"},
                                    new Tree<string>{ Data = "S52"}
                                }},
                                new Tree<string>{ Data = "S42"}
                             }
                          }
                        }
                    },
                    new Tree<string>
                    { 
                        Data = "S221",
                        NTree = new Tree<string>[]
                        {
                            new Tree<string>{ Data = "S21"},
                            new Tree<string>{ Data = "m1"},
                            new Tree<string>{ Data = "Sp"},
                        }
                    },
                    new Tree<string>{ Data = "S41"}
                }
                #endregion
            };

            return testTree;
        }
        /// <summary>
        /// Ambiguous
        /// </summary>
        /// <param name="root"></param>
        /// <param name="treeHandler"></param>
        /// <param name="waitAll"></param>
        //public static void WalkParallel(Tree<TKeyValue> root, TreeHandler treeHandler, bool waitAll = false)
        //{
        //    if (root == null) return;

        //    //LRW wandeling in parallel!
        //    var t2 = Task.Factory.StartNew(() => WalkParallel(root.Left, treeHandler)
        //        , TaskCreationOptions.AttachedToParent);
        //    var t3 = Task.Factory.StartNew(() => WalkParallel(root.Right, treeHandler)
        //        , TaskCreationOptions.AttachedToParent);
        //    var t1 = Task.Factory.StartNew(() => _listOfHandlers(string.Format("Handling node {0}", root.Data))
        //        , TaskCreationOptions.AttachedToParent);
        //    if (waitAll) Task.WaitAll(t1, t2, t3);
        //}
    }
    /// <summary>
    /// Taskduration uses DateTime.
    /// Taskduration2 uses Stopwatch.
    /// </summary>
    public struct TaskInfo
    {
        public long Taskduration
        {
            get
            {
                return TaskEnded - TaskStarted;
            }
        }
        public long Taskduration2
        {
            get
            {
                return TaskEnded2 - TaskStarted2;
            }
        }
        public string TaskDescription;

        public long TaskEnded;
        public long TaskStarted;

        public long TaskStarted2;
        public long TaskEnded2;
    }
}