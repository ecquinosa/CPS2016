<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AcknowledgeFile.aspx.vb" Inherits="CPS2016.AcknowledgeFile" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
    <script type="text/javascript">
    function ShowProcessingPanel() {
        Callback.PerformCallback();
        ProcessingPanel.Show();
    }
    </script>
    <form id="form1" runat="server">
    <div style="padding-bottom: 15px">            
            <asp:Label ID="lblHeader" runat="server" Text="Generate Acknowledgement File"></asp:Label>
            </div>
    <div style="float: left; padding-bottom: 10px; width: 536px;" id="divOutput" 
        runat="server">
    <div style="float:left; width: 116px; font-family: Arial, Helvetica, sans-serif;">
        CDFR</div>
        <div style="float:left; width: 400px; height: 60px;">
             <asp:DropDownList ID="cboCDFR" runat="server" Width="350px">
             </asp:DropDownList>
             <br />
             <br />

            <dx:ASPxButton ID="btnSubmit" runat="server" Height="30px" 
                Text="Generate" Width="100px" 
                Theme="Office2010Blue">
                
            </dx:ASPxButton>

        &nbsp;<asp:Label ID="lblStatus" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        

          

        </div>            
    </div>
   
            <br />
   <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue" Height="38px" 
                Width="121px">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>
    </form>
</body>
</html>
