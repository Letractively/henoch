using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Reflection;

namespace Telerik.QuickStart
{
    public class TreeNavigation : WebControl
    {
        private string _contentFile = string.Empty;
        private string _collapsedImageUrl = string.Empty;
        private string _expandedImageUrl = string.Empty;
        private string _newExampleImageUrl = string.Empty;
        private string _updatedExampleImageUrl = string.Empty;
        private string _net35ExampleImageUrl = string.Empty;
        private string _net40ExampleImageUrl = string.Empty;
        private readonly ArrayList categories = new ArrayList();
        private XmlNode defaultCategory;
        private string selectedExample;

        public string ContentFile
        {
            get
            {
                if (ViewState["ContenFile"] != null)
                {
                    return (string)ViewState["ContenFile"];
                }
                return _contentFile;
            }
            set { ViewState["ContenFile"] = _contentFile = value; }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        public string NewExampleImageUrl
        {
            get
            {
                if (ViewState["NewExampleImageUrl"] != null)
                {
                    return (string)ViewState["NewExampleImageUrl"];
                }
                return _newExampleImageUrl;
            }
            set { ViewState["NewExampleImageUrl"] = _newExampleImageUrl = value; }
        }

        public string UpdatedExampleImageUrl
        {
            get
            {
                if (ViewState["UpdatedExampleImageUrl"] != null)
                {
                    return (string)ViewState["UpdatedExampleImageUrl"];
                }
                return _updatedExampleImageUrl;
            }
            set { ViewState["UpdatedExampleImageUrl"] = _updatedExampleImageUrl = value; }
        }

        public string Net35ExampleImageUrl
        {
            get
            {
                if (ViewState["Net35ExampleImageUrl"] != null)
                {
                    return (string)ViewState["Net35ExampleImageUrl"];
                }
                return _net35ExampleImageUrl;
            }
            set { ViewState["Net35ExampleImageUrl"] = _net35ExampleImageUrl = value; }
        }

        public string Net40ExampleImageUrl
        {
            get
            {
                if (ViewState["Net40ExampleImageUrl"] != null)
                {
                    return (string)ViewState["Net40ExampleImageUrl"];
                }
                return _net40ExampleImageUrl;
            }
            set { ViewState["Net40ExampleImageUrl"] = _net40ExampleImageUrl = value; }
        }

        public string ExpandedImageUrl
        {
            get
            {
                if (ViewState["ExpandedImageUrl"] != null)
                {
                    return (string)ViewState["ExpandedImageUrl"];
                }
                return _expandedImageUrl;
            }
            set { ViewState["ExpandedImageUrl"] = _expandedImageUrl = value; }
        }

        public string CollapsedImageUrl
        {
            get
            {
                if (ViewState["CollapsedImageUrl"] != null)
                {
                    return (string)ViewState["CollapsedImageUrl"];
                }
                return _collapsedImageUrl;
            }
            set { ViewState["CollapsedImageUrl"] = _collapsedImageUrl = value; }
        }

        public bool SingleExpand
        {
            get
            {
                if (ViewState["SingleExpand"] != null)
                {
                    return (bool)ViewState["SingleExpand"];
                }
                return true;
            }
            set { ViewState["SingleExpand"] = value; }
        }

        protected bool IsNet35
        {
            get
            {
                try
                {
                    string smAssemblyName = Assembly.GetAssembly(typeof(ScriptManager)).FullName;
                    return smAssemblyName.IndexOf("3.5") != -1 || smAssemblyName.IndexOf("4.0") != -1;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            LoadContentFile();
            Populate();
            StringBuilder clientObjectInitScript = BuildClientScript();
            Controls.Add(new LiteralControl(clientObjectInitScript.ToString()));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter output = new StringWriter();
            HtmlTextWriter html = new HtmlTextWriter(output);
            base.Render(html);
            writer.Write(output.GetStringBuilder().ToString());
        }

        private StringBuilder BuildClientScript()
        {
            StringBuilder clientObjectInitScript = new StringBuilder();
            clientObjectInitScript.Append("<script type=\"text/javascript\">");
            clientObjectInitScript.AppendFormat("var {0} = new TreeNavigator('{1}', '{2}', '{3}');",
                ClientObject, ClientID,
                Page.ResolveUrl(CollapsedImageUrl),
                Page.ResolveUrl(ExpandedImageUrl)
                );
            clientObjectInitScript.AppendFormat("{0}.SingleExpand = {1};",
                ClientObject, SingleExpand.ToString().ToLower());
            clientObjectInitScript.Append("</script>");
            return clientObjectInitScript;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType(), "TreeNavigatorScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "TreeNavigatorScript",
                    string.Format("<script src='{0}' type='text/javascript'></script>",
                        ResolveUrl("~/Common/TreeNavigator.js"))
                    );
            }
        }

        private string ClientObject
        {
            get { return ClientID + "_tree"; }
        }

        private XmlDocument _xmlDoc = null;
        private XmlDocument xmlDoc
        {
            get
            {
                if (_xmlDoc == null)
                {
                    _xmlDoc = (XmlDocument)Context.Cache["TreeNavigatorXmlDocument"];
                    if (_xmlDoc == null)
                    {
                        if (string.IsNullOrEmpty(ContentFile))
                        {
                            throw new ApplicationException("ContentFile cannot be empty.");
                        }
                        _xmlDoc = new XmlDocument();
                        _xmlDoc.Load(Context.Server.MapPath(ContentFile));
                        Context.Cache["TreeNavigatorXmlDocument"] = _xmlDoc;
                    }
                }
                return _xmlDoc;
            }
        }

        private void LoadContentFile()
        {
            XmlNode examples = xmlDoc.SelectSingleNode("/examples");
            if (examples.Attributes["singleExpand"] != null)
            {
                SingleExpand = Convert.ToBoolean(examples.Attributes["singleExpand"].Value);
            }
            InitSelectedExample();
            InitDefaultCategory();
        }

        private void InitDefaultCategory()
        {
            defaultCategory = FindCurrentCategory(selectedExample);
        }

        private XmlNode FindCurrentCategory(string examplePath)
        {
            XmlNodeList allExamples = xmlDoc.SelectNodes("//example");
            foreach (XmlNode example in allExamples)
            {
                if (example.NodeType != XmlNodeType.Comment)
                {
                    XmlAttribute name = example.Attributes["name"];
                    if (name != null)
                    {
                        if (name.Value.ToLower() == examplePath.ToLower())
                        {
                            if (!string.IsNullOrEmpty(Context.Request.QueryString["product"]))
                            {
                                if (example.Attributes["product"] != null)
                                {
                                    if (example.Attributes["product"].Value.ToLower() ==
                                        Context.Request.QueryString["product"].ToLower())
                                    {
                                        return example.ParentNode;
                                    }
                                }

                                continue;
                            }

                            return example.ParentNode;
                        }
                    }
                }
            }
            return null;
        }

        private void InitSelectedExample()
        {
            selectedExample = PageUtility.LocationInWebApp(Page);

            if (string.IsNullOrEmpty(selectedExample))
            {
                XmlNode defaultExample = xmlDoc.SelectSingleNode("//example[@default = 'true']");
                selectedExample = defaultExample.Attributes["name"].Value;
            }
        }

        private void BuildCategories(Control parentControl, XmlNode parentNode)
        {
            HtmlGenericControl container = new HtmlGenericControl("ul");

            parentControl.Controls.Add(container);
            categories.Add(container.ClientID);

            if (parentNode == defaultCategory)
            {
                ExpandCurrentExample(container);
            }

            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == "category")
                {
                    HtmlGenericControl exampleElement = new HtmlGenericControl("li");
                    container.Controls.Add(exampleElement);

                    exampleElement.Attributes["class"] = "collapsed";

                    HyperLink exampleLink = new HyperLink();
                    exampleLink.Text = node.Attributes["text"].Value;
                    exampleElement.Controls.Add(exampleLink);
                    //Build hierarchy
                    BuildCategories(exampleElement, node);
                    exampleLink.NavigateUrl = "javascript:void(0)";
                    if (node.Attributes["isNew"] != null)
                    {
                        exampleLink.Text += "<img src=\"" + ResolveUrl(NewExampleImageUrl) + "\" alt=\"new\" />";
                    }
                    if (node.Attributes["isUpdated"] != null)
                    {
                        exampleLink.Text += "<img src=\"" + ResolveUrl(UpdatedExampleImageUrl) + "\" alt=\"updated\" />";
                    }
                }
                else if (node.NodeType != XmlNodeType.Comment)
                {
                    if (!IsNet35 && node.Attributes["isNET35"] != null && Convert.ToBoolean(node.Attributes["isNET35"].Value))
                        continue;

                    HtmlGenericControl exampleElement = new HtmlGenericControl("li");
                    container.Controls.Add(exampleElement);

                    AddExampleLink(node, exampleElement, string.Empty,
                        (node.Attributes["isNew"] != null && Convert.ToBoolean(node.Attributes["isNew"].Value)),
                        (node.Attributes["isNET35"] != null && Convert.ToBoolean(node.Attributes["isNET35"].Value)),
                        (node.Attributes["isNET40"] != null && Convert.ToBoolean(node.Attributes["isNET40"].Value)),
                        (node.Attributes["isUpdated"] != null && Convert.ToBoolean(node.Attributes["isUpdated"].Value)));
                }
            }
            return;
        }

        private void ExpandCurrentExample(HtmlGenericControl parentDiv)
        {
            Control current = parentDiv;
            while (current is HtmlGenericControl)
            {
                HtmlGenericControl control = (HtmlGenericControl)current;
                if (control.TagName == "ul")
                {
                    control.Style["display"] = "block";
                }
                if (control.Attributes["class"] == "collapsed")
                {
                    control.Attributes["class"] = "expanded";
                }

                current = current.Parent;
            }
        }

        private void Populate()
        {
            HtmlTable tableNavigation = BuildTreeViewTable();
            XmlNode rootNode = xmlDoc.SelectSingleNode("/examples");
            if (rootNode.Attributes["Type"] != null && rootNode.Attributes["Type"].Value.ToLower() == "controls")
            {
                //get only the selected category(assuming InitSelectedExample has been already called)
                //use case insensitive search since the example URL is not case sensitive
                //contains(lower-case(identity), lower-case('mYiD'))

                XmlNode currentExample =
                    rootNode.SelectSingleNode(
                        "//example[translate(@name, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') = translate('" +
                            selectedExample.ToLower() + "', 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')]");
                //If a product is specified (using one example in two products)

                if (!string.IsNullOrEmpty(Context.Request.QueryString["product"]))
                {
                    currentExample = rootNode.SelectSingleNode(
                        "//example[translate(@product, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') = translate('" +
                            Context.Request.QueryString["product"].ToLower() +
                                "', 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') and translate(@name, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') = translate('"
                                    + selectedExample.ToLower() +
                                        "', 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')]");
                }

                if (currentExample == null)
                {
                    //the current example is not in the XML file??
                    //just skip the navigation then
                    return;
                }


                XmlNode rootCategory = currentExample.ParentNode;
                while (rootCategory != null && rootCategory.ParentNode.Name != "examples")
                    rootCategory = rootCategory.ParentNode;
                if (rootCategory == null)
                    throw new ApplicationException("Cannot find the main category for this example");

                string rootCategoryControlName = rootCategory.Attributes["text"].Value;
                tableNavigation.Rows[0].Cells[1].Controls.Add(new LiteralControl(String.Format(@"<input type=""button"" class=""Img{0}"" value="" "" alt=""{0}"" />", rootCategoryControlName)));
                tableNavigation.Rows[0].Cells[1].Controls.Add(new LiteralControl(rootCategoryControlName));

                //build the root category examples only
                BuildCategories(tableNavigation.Rows[1].Cells[1], rootCategory);
                //build the list of root categories 
                HtmlTable productsTable = BuildProductsTable();
                HtmlGenericControl container = new HtmlGenericControl("ul");
                productsTable.Rows[1].Cells[0].Controls.Add(container);
                foreach (XmlNode categoryNode in this.xmlDoc.SelectNodes("examples/category"))
                {
                    //find the default example for that category
                    XmlNode defaultExample = categoryNode.SelectSingleNode("descendant::example[@default = 'true']");
                    if (defaultExample == null)
                    {
                        if (!IsNet35)
                        {
                            defaultExample = categoryNode.SelectSingleNode("descendant::example[@isNet35 != 'true']");
                        }
                        else
                        {
                            defaultExample = categoryNode.SelectSingleNode("descendant::example");
                        }
                    }
                    //if no examples at all, skip the category
                    if (defaultExample != null)
                    {
                        HtmlGenericControl categoryElement = new HtmlGenericControl("li");
                        container.Controls.Add(categoryElement);

                        //left image icon (if available)
                        //if (categoryNode.Attributes["IconUrl"] != null)
                        //{
                        //    HtmlImage newIcon = new HtmlImage();
                        //    newIcon.Src = categoryNode.Attributes["IconUrl"].Value;
                        //    newIcon.Attributes["alt"] = "icon";
                        //    categoryElement.Controls.Add(newIcon);
                        //}

                        //add the link to that category
                        bool isNew = false;
                        bool isUpdated = false;
                        if (categoryNode.Attributes["isNew"] != null)
                        {
                            isNew = Convert.ToBoolean(categoryNode.Attributes["isNew"].Value);
                        }
                        if (categoryNode.Attributes["isUpdated"] != null)
                        {
                            isUpdated = Convert.ToBoolean(categoryNode.Attributes["isUpdated"].Value);
                        }
                  
                        AddExampleLink(defaultExample, categoryElement, categoryNode.Attributes["text"].Value, isNew, (categoryNode.Attributes["IconUrl"] != null), false, false, isUpdated);
                    }
                }
                if (container.Controls.Count == 0)
                {
                    //no categories, no need for a list
                    Controls.Remove(productsTable);
                }
                else
                {
                    container.Attributes.Add("id", "categoryList");
                    container.Attributes.Add("style", "display:block");
                }
            }
            else
            {
                tableNavigation.Rows[0].Cells[1].InnerHtml = "&nbsp;";
                BuildCategories(tableNavigation.Rows[1].Cells[1], xmlDoc.SelectSingleNode("/examples"));
            }

            categories.RemoveAt(0);
        }

        private HtmlTable BuildTreeViewTable()
        {
            HtmlTable firstTable = new HtmlTable();
            Controls.Add(firstTable);
            firstTable.Attributes.Add("summary", "navigation table");
            firstTable.CellPadding = 0;
            firstTable.CellSpacing = 0;
            firstTable.Attributes.Add("class", "controlMenu");
            firstTable.Rows.Add(new HtmlTableRow());
            firstTable.Rows.Add(new HtmlTableRow());
            firstTable.Rows.Add(new HtmlTableRow());
            firstTable.Rows[0].Cells.Add(new HtmlTableCell());
            firstTable.Rows[0].Cells.Add(new HtmlTableCell());
            firstTable.Rows[0].Cells.Add(new HtmlTableCell());
            firstTable.Rows[1].Cells.Add(new HtmlTableCell());
            firstTable.Rows[1].Cells.Add(new HtmlTableCell());
            firstTable.Rows[1].Cells.Add(new HtmlTableCell());
            firstTable.Rows[2].Cells.Add(new HtmlTableCell());
            firstTable.Rows[2].Cells.Add(new HtmlTableCell());
            firstTable.Rows[2].Cells.Add(new HtmlTableCell());

            firstTable.Rows[0].Cells[0].InnerHtml = "&nbsp;";
            firstTable.Rows[0].Cells[2].InnerHtml = "&nbsp;";
            firstTable.Rows[1].Cells[0].InnerHtml = "&nbsp;";
            firstTable.Rows[1].Cells[2].InnerHtml = "&nbsp;";
            firstTable.Rows[2].Cells[0].InnerHtml = "&nbsp;";
            firstTable.Rows[2].Cells[1].InnerHtml = "&nbsp;";
            firstTable.Rows[2].Cells[2].InnerHtml = "&nbsp;";


            firstTable.Rows[0].Cells[0].Attributes.Add("class", "leftCellTop");
            firstTable.Rows[0].Cells[1].Attributes.Add("class", "controlMenuHeaderCell");
            firstTable.Rows[0].Cells[2].Attributes.Add("class", "rightCellTop");
            firstTable.Rows[1].Cells[0].Attributes.Add("class", "leftCellMiddle");
            firstTable.Rows[1].Cells[1].Attributes.Add("class", "menuContainer");
            firstTable.Rows[1].Cells[2].Attributes.Add("class", "rightCellMiddle");
            firstTable.Rows[2].Cells[0].Attributes.Add("class", "leftCellBottom");
            firstTable.Rows[2].Cells[1].Attributes.Add("class", "middleCellBottom");
            firstTable.Rows[2].Cells[2].Attributes.Add("class", "rightCellBottom");


            return firstTable;
        }

        private HtmlTable BuildProductsTable()
        {
            HtmlTable secondTable = new HtmlTable();
            Controls.Add(secondTable);
            secondTable.Attributes.Add("summary", "controls table");
            secondTable.CellPadding = 0;
            secondTable.CellSpacing = 0;
            secondTable.Attributes.Add("class", "allProducts");
            secondTable.Rows.Add(new HtmlTableRow());
            secondTable.Rows.Add(new HtmlTableRow());
            secondTable.Rows.Add(new HtmlTableRow());
            secondTable.Rows[0].Cells.Add(new HtmlTableCell());
            secondTable.Rows[1].Cells.Add(new HtmlTableCell());
            secondTable.Rows[2].Cells.Add(new HtmlTableCell());

            secondTable.Rows[0].Cells[0].InnerHtml = "&nbsp;";
            secondTable.Rows[2].Cells[0].InnerHtml = "&nbsp;";

            secondTable.Rows[0].Cells[0].Attributes.Add("class", "allProductsHeader");
            secondTable.Rows[1].Cells[0].Attributes.Add("class", "allProductsMainCell");
            secondTable.Rows[2].Cells[0].Attributes.Add("class", "allProductsFooter");

            return secondTable;
        }

        private void AddExampleLink(XmlNode exampleNode, HtmlGenericControl placeHolder, string productName, bool isNewExample,
                                    bool isNet35Example,bool isNet40Example, bool isUpdatedExample)
        {
            AddExampleLink(exampleNode, placeHolder, productName, isNewExample, false, isNet35Example,isNet40Example, isUpdatedExample);
        }

        private void AddExampleLink(XmlNode exampleNode, HtmlGenericControl placeHolder, string productName, bool isNewExample,
                                    bool isControlListItem, bool isNet35Example, bool isNet40Example, bool isUpdatedExample)
        {
            string exampleName = string.Empty;
            // Added in case the category node has isNew or isUpdated attributes
            if (exampleNode.Attributes["text"] == null)
            {
                return;
            }
            else
            {
                exampleName = exampleNode.Attributes["text"].Value;
            }
            if (exampleNode.Attributes["name"] != null)
            {
                exampleName =  exampleNode.Attributes["name"].Value;
            }

            HyperLink link = new HyperLink();
            if (exampleNode.Attributes["external"] != null &&
                exampleNode.Attributes["external"].Value.ToLower() == "true")
            {
                link.NavigateUrl = exampleName.ToLowerInvariant();
                if (productName.Length > 0)
                    link.Text = productName;
                else
                    link.Text = exampleNode.Attributes["text"].Value;

                string target = "_blank";
                if (exampleNode.Attributes["target"] != null)
                {
                    target = exampleNode.Attributes["target"].Value;
                }
                link.Target = target;
            }
            else
            {
                link.NavigateUrl = "~/" + exampleName.ToLowerInvariant() + "/default" + NavigationLanguage.ToLowerInvariant() +
                    ".aspx";

                if (exampleNode.Attributes["product"] != null)
                {
                    link.NavigateUrl += "?product=" + exampleNode.Attributes["product"].Value.ToLowerInvariant();
                }

                if (isControlListItem)
                {
                    link.Text = String.Format(@"<span class=""Icon{0}""><!-- --></span>", productName);
                }
                else
                {
                    link.Text = "";
                }

                if (productName.Length > 0)
                    link.Text += productName;
                else
                    link.Text += exampleNode.Attributes["text"].Value;
                if (exampleName.ToLower() == selectedExample.ToLower())
                {
                    placeHolder.Attributes["class"] = "tabSelected";
                }
                if (exampleName.ToLower() == selectedExample.ToLower())
                {
                    Context.Items["CurrentExample"] = exampleNode.Attributes["text"].Value;
                }
            }

            if (isNet35Example)
            {
                link.Text += "<img src=\"" + ResolveUrl(Net35ExampleImageUrl) + "\" alt=\"net35\" />";
            }

            if (isNet40Example)
            {
                link.Text += "<img src=\"" + ResolveUrl(Net40ExampleImageUrl) + "\" alt=\"net40\" />";
            }

            if (isNewExample)
            {
                link.Text += "<img class=\"qsfIsNew\" src=\"" + ResolveUrl(NewExampleImageUrl) + "\" alt=\"new\" />";
            }

            if (isUpdatedExample)
            {
                link.Text += "<img class=\"qsfIsUpdated\" src=\"" + ResolveUrl(UpdatedExampleImageUrl) + "\" alt=\"updated\" />";
            }

            placeHolder.Controls.Add(link);
        }

        public string NavigationLanguage
        {
            get
            {
                if (ViewState["NavigationLanguage"] == null)
                {
                    return "CS";
                }

                return (string)ViewState["NavigationLanguage"];
            }
            set { ViewState["NavigationLanguage"] = value; }
        }
    }
}