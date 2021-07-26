<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="MiscReports.aspx.vb" Inherits="CPS2016.MiscReports" %>

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
                Text="Miscellaneous Reports/ Process" style="font-size: small">
                </dx:ASPxLabel>

        </div>
        <div style="padding-left: 10px; padding-top: 10px; height: 93px;">
            <table style="width: 100%; ">
                <tr valign="top">
                    <td style="width: 397px; padding-bottom: 10px;">
                        1.
                        <asp:LinkButton ID="lbQCYieldReport" runat="server">QC Yield Report</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px">13.
                        <asp:LinkButton ID="lbModifyLaserData" runat="server">Modify FirstName/ LastName/ Address (laser data)</asp:LinkButton>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 397px; padding-bottom: 10px; height: 36px;">
                        2.
                        <asp:LinkButton ID="lbInventoryReport" runat="server">Inventory Report</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px; height: 36px;">14.
                        <asp:LinkButton ID="lbAcknowledgeFile" runat="server">Extract UBP acknowledgement file</asp:LinkButton>
                    </td>                    
                </tr>
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        3.
                        <asp:LinkButton ID="lbRejectReport" runat="server">Reject Report - Daily</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px">15.
                        <asp:LinkButton ID="lbResponseFile" runat="server">Extract UBP Response file</asp:LinkButton>
                    </td>                                        
                </tr>
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        4.
                        <asp:LinkButton ID="lbExtractLaserData" runat="server">Extract Laser Engraving Data</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px">16.
                        <asp:LinkButton ID="lbACMLFile" runat="server">Extract ACML file</asp:LinkButton>
                    </td>                                        
                </tr>
                 <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        5.
                        <asp:LinkButton ID="lbExtractLaserData0" runat="server">Extract Laser Engraving Data (by Barcode/ Back OCR)</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px">&nbsp;</td>                                        
                </tr>  
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        6.
                        <asp:LinkButton ID="lbExtractMuhlbauerData" runat="server">Extract Muhlbauer Data</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px">&nbsp;</td>                                        
                </tr>   
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        7.
                        <asp:LinkButton ID="lbExtractMuhlbauerData0" runat="server">Extract Muhlbauer Data (by Barcode/ Back OCR)</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>  
                 <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        8.
                        <asp:LinkButton ID="lbExtractRejectCard" runat="server">Extract Reject Card(s)</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>    
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        9.
                        <asp:LinkButton ID="lbDeliveryReceipt2" runat="server">Extract Delivery Receipt 2</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>   
                
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        10.
                        <asp:LinkButton ID="lbDeliveryReceiptManualExtraction" runat="server">Delivery Receipt Manual Extraction (by list)</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>
                <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        11.
                        <asp:LinkButton ID="lbDR_PDF" runat="server">Delivery Receipt PDF</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>
                 <tr valign="top">
                <td style="width: 397px; padding-bottom: 10px;">
                        12.
                        <asp:LinkButton ID="lbExtractCubaoBranchData" runat="server">Extract SSS Cubao Branch Data</asp:LinkButton>
                    </td>
                    <td style="padding-bottom: 15px"></td>                                        
                </tr>
            </table>           
        <div style="float: left; width: 791px;">
<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/RoleModule.aspx" EnableTheming="True" 
                HeaderText="Miscellaneous Report" ShowMaximizeButton="True" 
                Theme="Office2010Blue"></dx:ASPxPopupControl></div>
             
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>