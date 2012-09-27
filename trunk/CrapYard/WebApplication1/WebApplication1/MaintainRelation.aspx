

<%@ Register TagPrefix="qsf" Namespace="Telerik.QuickStart" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaintainRelation.aspx.cs" Inherits="WebApplication1.MaintainRelation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">

            <script type="text/javascript">        
                /* <![CDATA[ */                                    
                var gridId = "<%= RadGrid1.ClientID %>";
                function isMouseOverGrid(target)
                {
                    parentNode = target;
                    while (parentNode != null)
                    {                    
                        if (parentNode.id == gridId)
                        {
                            return parentNode;
                        }
                        parentNode = parentNode.parentNode;
                    }
         
                    return null;
                }
         
                function onNodeDragging(sender, args)
                {
                    var target = args.get_htmlElement();    
                    
                    if(!target) return;
                    
                    if (target.tagName == "INPUT")
                    {        
                        target.style.cursor = "hand";
                    }

                    var grid = isMouseOverGrid(target)
                    if (grid)
                    {
                        grid.style.cursor = "hand";
                    }
                }
         
                function dropOnHtmlElement(args)
                {                    
                 if(droppedOnInput(args))
                 return;
                 
                 if(droppedOnGrid(args))
                 return;                    
                }
                
                function droppedOnGrid(args)
                {
                 var target = args.get_htmlElement();
                 
                 while(target)
                 {
                 if(target.id == gridId)
                 {
                 args.set_htmlElement(target);
                 return;                                                   
                 }
                 
                 target = target.parentNode;
                 }
                 args.set_cancel(true);
                }
                
                function droppedOnInput(args)
                {
                 var target = args.get_htmlElement();
                    if (target.tagName == "INPUT")
                    {
                        target.style.cursor = "default";
                        target.value = args.get_sourceNode().get_text();        
                        args.set_cancel(true);
                        return true;
                    }                          
                }
         
                function dropOnTree(args)
                {
                    var text = "";

                    if(args.get_sourceNodes().length)
                    {    
                        var i;
                        for(i=0; i < args.get_sourceNodes().length; i++)
                        {
                            var node = args.get_sourceNodes()[i];
                            text = text + ', ' +node.get_text();
                        }
                    }
                }
                
                function clientSideEdit(sender, args)
                {
                 var destinationNode = args.get_destNode();                                  
                 
                 if(destinationNode)
                 {             
                 var firstTreeView = $find('RadTreeView1');
                 var secondTreeView = $find('RadTreeView2');
                 
                        firstTreeView.trackChanges();
                        secondTreeView.trackChanges();
                        var sourceNodes = args.get_sourceNodes();
                        var dropPosition = args.get_dropPosition();

                        //Needed to preserve the order of the dragged items
                        if (dropPosition == "below") {
                            for (var i = sourceNodes.length - 1; i >= 0; i--)
                            {
                                var sourceNode = sourceNodes[i];
                                sourceNode.get_parent().get_nodes().remove(sourceNode); 
                            
                                insertAfter(destinationNode, sourceNode);
                            }
                        }
                        else {
                            for (var i = 0; i < sourceNodes.length; i++)
                            {
                                var sourceNode = sourceNodes[i];
                                sourceNode.get_parent().get_nodes().remove(sourceNode); 
                            
                                if (dropPosition == "over")
                                    destinationNode.get_nodes().add(sourceNode); 
                                if (dropPosition == "above")
                                    insertBefore(destinationNode, sourceNode);
                            }
                        }
                 destinationNode.set_expanded(true);
                 firstTreeView.commitChanges();
                 secondTreeView.commitChanges();
                 }
                }
                
                function insertBefore(destinationNode, sourceNode)
                {
                 var destinationParent = destinationNode.get_parent();
                 var index = destinationParent.get_nodes().indexOf(destinationNode);
                 destinationParent.get_nodes().insert(index, sourceNode);
                }
                
                function insertAfter(destinationNode, sourceNode)
                {
                 var destinationParent = destinationNode.get_parent();
                 var index = destinationParent.get_nodes().indexOf(destinationNode);
                 destinationParent.get_nodes().insert(index+1, sourceNode);
                }                
                
                function onNodeDropping(sender, args)
                {            
                 var dest = args.get_destNode();
                    if (dest)
                    { 
                     var clientSide = document.getElementById('ChbClientSide').checked;
     
                 if(clientSide)
                 {
                 clientSideEdit(sender, args);                 
                 args.set_cancel(true);
                 return;
                 }
                    
                        dropOnTree(args);
                    }
                    else
                    { 
                        dropOnHtmlElement(args);
                    }
                }
        /* ]]> */
            </script>

        </telerik:RadScriptBlock>
        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ConfigurationPanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="Panel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <asp:Panel runat="server" ID="Panel1">
<%--            <qsf:ConfiguratorPanel runat="server" ID="ConfigurationPanel1" Expanded="true">
                <ul>
                    <li style="border-left: none"><span class="title">Drag &amp; drop options:</span>
                        <asp:CheckBox ID="ChbClientSide" runat="server" Checked="true" Text="Client-side drag &amp;amp; drop" /></li>
                    <li>
<asp:CheckBox ID="ChbMultipleSelect" runat="server" Text="Multiple node selection" Checked="True"
AutoPostBack="True" OnCheckedChanged="ChbMultipleSelect_CheckedChanged"></asp:CheckBox></li>
                    <li>
<asp:CheckBox ID="ChbBetweenNodes" runat="server" Text="Drag and drop between nodes" Checked="True"
AutoPostBack="True" OnCheckedChanged="ChbBetweenNodes_CheckedChanged"></asp:CheckBox></li>
                </ul>
            </qsf:ConfiguratorPanel>--%>
            <div style="background: url(Img/bg.gif) no-repeat; padding: 115px 0px 0px 15px;">
                <div style="width: 180px; float: left;">
                    <span class="label">RadTreeView1</span>
                    <telerik:RadTreeView ID="RadTreeView1" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        OnClientNodeDropping="onNodeDropping" OnClientNodeDragging="onNodeDragging" MultipleSelect="true" EnableDragAndDropBetweenNodes="true">
                    </telerik:RadTreeView>
                </div>
                <div style="width: 180px; float: left;">
                    <span class="label">RadTreeView2</span>
                    <telerik:RadTreeView ID="RadTreeView2" runat="server" EnableDragAndDrop="True" OnNodeDrop="RadTreeView1_HandleDrop"
                        OnClientNodeDropping="onNodeDropping" OnClientNodeDragging="onNodeDragging" MultipleSelect="true" EnableDragAndDropBetweenNodes="true">
                    </telerik:RadTreeView>
                </div>
                <div style="width: 110px; float: left">
                    <span class="label">TextBox</span>
                    <asp:TextBox runat="server" ID="NodeText" Style="width: 82px;"></asp:TextBox>
                </div>
                <div style="float: left">
                    <span class="label">RadGrid</span>
                    <telerik:RadGrid runat="server" ID="RadGrid1" Width="220px">
                    </telerik:RadGrid>
                    <asp:Label CssClass="textr" runat="server" ID="Label1"></asp:Label>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </asp:Panel>

</asp:Content>
