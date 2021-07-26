<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Help.aspx.vb" Inherits="CPS2016.Help" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
            <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div> 
        <div style="padding-left: 5px; padding-top: 10px; height: 36px;">

                        <dx:ASPxLabel ID="lblStatus" runat="server" 
                Text="Help" style="font-size: small">
                </dx:ASPxLabel>

        </div>
        <div style="padding-left: 10px; padding-top: 10px; height: 93px;">
            <table style="width: 100%; ">
                <tr valign="top">
                    <td style="width: 329px">
                        1.
                        <asp:LinkButton ID="lb1" runat="server">Viewer</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px">
                    
                        &nbsp;</td>
                </tr>
                <tr valign="top">
                    <td style="width: 329px">
                        2.
                        <asp:LinkButton ID="lb2" runat="server">Indigo</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>
                <tr valign="top">
                    <td style="width: 329px">
                        3.
                        <asp:LinkButton ID="lb3" runat="server">Assembly</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>
                <tr valign="top">
                    <td style="width: 329px">
                        4.
                        <asp:LinkButton ID="lb4" runat="server">Quality Control</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>
                <tr valign="top">
                    <td style="width: 329px">
                        5.
                        <asp:LinkButton ID="lb5" runat="server">Personalization</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>               
               <tr valign="top">
                    <td style="width: 329px">
                        6.
                        <asp:LinkButton ID="lb6" runat="server">Inventory Admin</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>  
                <tr valign="top">
                    <td style="width: 329px">
                        7.
                        <asp:LinkButton ID="lb7" runat="server">CPS Activity/ Status Sequence</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 5px;">
                        &nbsp;</td>
                </tr>  
            </table>           
        <div style="float: left; width: 791px;">
<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/RoleModule.aspx" EnableTheming="True" 
                HeaderText="Help" ShowMaximizeButton="True" 
                Theme="Office2010Blue">
                 </dx:ASPxPopupControl></div>
             
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>