<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="DeliveryReceiptByScan.aspx.vb" Inherits="CPS2016.DeliveryReceiptByScan" %>

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
                            
<dx:PanelContent runat="server">

    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
        Purchase Order<dx:ASPxComboBox ID="cboPO" runat="server" ClientInstanceName="cboPO" IncrementalFilteringMode="Contains"
                DropDownRows="30" DropDownStyle="DropDown" NullText="select field to search" 
                style="font-size: small" Theme="Office2010Blue" Width="340px">
                <ClientSideEvents ButtonClick="function(s, e) {
	cboPO.SetText('');
	cboPO.Focus();	
}" GotFocus="function(s, e) {
	var button = cboPO.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = cboPO.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = cboPO.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                <Buttons>
                    <dx:EditButton>
                        <Image IconID="actions_cancel_16x16">
                        </Image>
                    </dx:EditButton>
                </Buttons>
            </dx:ASPxComboBox><br />
        <div style="width: 374px; float: left; height: 21px;">
            List of Barcode/ s or Back OCR</div>
            <br />

    </div>
    <div style="float: left; width: 471px; position: relative; top: 0px; left: 0px;">
                                    <asp:TextBox ID="txtSource" runat="server" Height="303px" Width="470px" 
                                        TextMode="MultiLine"></asp:TextBox>
    </div>
    <div style="float: left; width: 431px; position: relative; top: 0px; left: 0px; padding-bottom: 5px; padding-top: 5px;">
        <dx:ASPxButton runat="server" Text="Submit" Theme="Office2010Blue" 
            Width="100px" Height="30px" ID="btnSearch"></dx:ASPxButton>

        <dx:ASPxLabel runat="server" ID="lblStatus"></dx:ASPxLabel>

        

    </div>
    
                                
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

            <div style="padding-top: 10px; padding-bottom: 10px"><asp:Label ID="lblTotal" runat="server" 
            style="font-weight: 700; font-size: 10pt; font-family: Arial, Helvetica, sans-serif" 
            Text="Total: 0"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkCompleteDR" runat="server" Text="Complete DR" />
&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" Visible="False">EXTRACT</asp:LinkButton>
            &nbsp;<asp:LinkButton ID="lbViewDownloadTXT" runat="server" Visible="False">View/ Download TXT</asp:LinkButton>
            &nbsp;<asp:LinkButton ID="lbViewGenerateSummaryPDF" runat="server" Visible="False">View/ Generate Summary PDF</asp:LinkButton>    
            &nbsp;<asp:LinkButton ID="lbViewDownloadPDF" runat="server" Visible="False">View/ Download PDF</asp:LinkButton>
            &nbsp;<asp:LinkButton ID="lbExcluded" runat="server" Visible="False">Barcode(s) Excluded</asp:LinkButton>
            </div>          

            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="Barcode" 
                Width="900px">

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="1" FieldName="Barcode" Width="150px" Caption="Barcode">
                        <HeaderStyle HorizontalAlign="Center" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" FieldName="ChipSerialNo" 
                        Width="350px" Caption="Chip Serial No.">
                        <HeaderStyle HorizontalAlign="Center" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CardDate" VisibleIndex="3" Width="150px" 
                        Caption="Card Date">
                        <HeaderStyle HorizontalAlign="Center" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Posted" 
                        FieldName="DateTimePosted" VisibleIndex="5" Width="220px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="BackOCR" FieldName="BackOCR" 
                        VisibleIndex="6" Width="80px">
                    <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>

                <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1">
                </SettingsEditing>

                <Settings ShowFilterRow="True" />

                <SettingsPopup>
                    <EditForm Modal="True" HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
                </SettingsPopup>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>
                 <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" 
            GridViewID="xGrid"></dx:ASPxGridViewExporter>
            <em>  
             <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/ReportD_DeliveryReceipt.aspx" EnableTheming="True" 
                HeaderText="Report" ShowMaximizeButton="True" 
                Theme="Office2010Blue">
                 </dx:ASPxPopupControl>     
        </em>
        <br />
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>