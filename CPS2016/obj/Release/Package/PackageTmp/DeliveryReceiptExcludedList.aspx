<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DeliveryReceiptExcludedList.aspx.vb" Inherits="CPS2016.DeliveryReceiptExcludedList" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
    <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        </div>
    <div style="padding-left: 5px; padding-top: 10px; height: 93px;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="507px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent ID="PanelContent1" runat="server">

    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
        <dx:ASPxTextBox ID="txtPO" runat="server" Height="15px" 
                NullText="enter purchase order here for saving" Width="300px">
            </dx:ASPxTextBox><br />
        <div style="width: 374px; float: left; height: 21px;">
            List of Barcode/ s
        </div>
            <br />

    </div>
    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px;">
                                    <asp:TextBox ID="txtSource" runat="server" Height="303px" Width="470px" 
                                        TextMode="MultiLine"></asp:TextBox>
    </div>
    <div style="float: left; width: 431px; position: relative; top: 0px; left: 0px; padding-bottom: 5px; padding-top: 5px;">
        <dx:ASPxButton runat="server" Text="Submit" Theme="Office2010Blue" 
            Width="50px" Height="30px" ID="btnSearch"></dx:ASPxButton>

        <dx:ASPxButton ID="btnBack" runat="server" Height="30px" Text="Back" 
            Theme="Office2010Blue" Width="50px" Visible="False">
        </dx:ASPxButton>

        <dx:ASPxLabel runat="server" ID="lblStatus"></dx:ASPxLabel>

        

    </div>
    
                                
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

            <div style="padding-top: 10px; padding-bottom: 10px"><asp:Label ID="lblTotal" runat="server" 
            style="font-weight: 700; font-size: 10pt; font-family: Arial, Helvetica, sans-serif" 
            Text="Total: 0"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;</div>          

                 <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" 
            GridViewID="xGrid"></dx:ASPxGridViewExporter>
        <br />
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>    
    </form>
</body>
</html>


