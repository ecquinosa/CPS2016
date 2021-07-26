<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="ChangeStatusByList.aspx.vb" Inherits="CPS2016.ChangeStatusByList" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
    <script type="text/javascript" language="javascript">
        function ReplaceString() {
          var find = '<br>' 
          var re = new RegExp(find, 'g');
          str = document.getElementById("txtSource").value.replace(find, '');

          document.getElementById("txtSource").value = str;
         }
    </script> 


    <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div>
    <div style="padding-left: 5px; padding-top: 10px; height: 93px;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="676px" HeaderText="CHANGE STATUS TO INDIGO DOWNLOAD" Height="508px">
                            <HeaderStyle Font-Bold="True" />
                            <PanelCollection>
                            
<dx:PanelContent runat="server">

    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
        <br />
        <div style="width: 374px; float: left; height: 21px;">
            Copy &#39;<strong>Process Log</strong>&#39; content (remove &lt;br&gt; if present)</div>
            <br />

    </div>
    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px;">
                                    <asp:TextBox ID="txtSource" runat="server" Height="387px" Width="643px" 
                                        TextMode="MultiLine"></asp:TextBox>
    </div>
    <div style="float: left; width: 431px; position: relative; top: 0px; left: 0px; padding-bottom: 5px; padding-top: 5px;">
        <dx:ASPxButton runat="server" Text="Submit" Theme="Office2010Blue" 
            Width="100px" Height="30px" ID="btnSubmit"></dx:ASPxButton>

        <dx:ASPxLabel runat="server" ID="lblStatus"></dx:ASPxLabel>

        

    </div>
    
                                
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

            <div style="padding-top: 10px; padding-bottom: 10px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>          

        <br />
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>