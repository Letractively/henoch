<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="WebApplication1.Companies" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="AdjustRadWidow();>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
            <Scripts>
                <%--Needed for JavaScript IntelliSense in VS2010--%>
                <%--For VS2008 replace RadScriptManager with ScriptManager--%>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />

            </Scripts>
        </telerik:RadScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        Skin="Black" />
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function openWin2() {
            var parentPage = GetRadWindow().BrowserWindow;
            var parentRadWindowManager = parentPage.GetRadWindowManager();
            var oWnd2 = parentRadWindowManager.open("Dialog2.aspx", "RadWindow2");
            window.setTimeout(function () {
                oWnd2.setActive(true);
            }, 0);
        }

        function populateCityName(arg) {
            var cityName = document.getElementById("cityName");
            cityName.value = arg;
        }

        function AdjustRadWidow() {
            var oWindow = GetRadWindow();
            setTimeout(function () { oWindow.autoSize(true); if ($telerik.isChrome || $telerik.isSafari) ChromeSafariFix(oWindow); }, 500);
        }

        //fix for Chrome/Safari due to absolute positioned popup not counted as part of the content page layout
        function ChromeSafariFix(oWindow) {
            var iframe = oWindow.get_contentFrame();
            var body = iframe.contentWindow.document.body;

            setTimeout(function () {
                var height = body.scrollHeight;
                var width = body.scrollWidth;

                var iframeBounds = $telerik.getBounds(iframe);
                var heightDelta = height - iframeBounds.height;
                var widthDelta = width - iframeBounds.width;

                if (heightDelta > 0) oWindow.set_height(oWindow.get_height() + heightDelta);
                if (widthDelta > 0) oWindow.set_width(oWindow.get_width() + widthDelta);
                oWindow.center();

            }, 310);
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();

            //get the city's name 
            //oArg.cityName = document.getElementById("cityName").value;

            //get the selected date from RadDatePicker
            //var datePicker = $find("<%= Datepicker1.ClientID %>");


            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();




            //Close the RadWindow and send the argument to the parent page
            if (oArg.selDate && oArg.cityName) {
                oWnd.close(oArg);
            }
            else {
                alert("Please fill both fields");
            }
        }
    </script>
    <div style="width: 268px; height: 193px;">
        <telerik:RadGrid ID="RadGrid1" runat="server" Skin="Black">
        </telerik:RadGrid>
        <div style="margin-top: 4px; text-align: right;">
            <button title="Submit" id="close" onclick="returnToParent(); return false;">
                Submit</button>
        </div>
    </div>
    </form>
</body>
</html>
