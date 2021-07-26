<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="POReports.aspx.vb" Inherits="CPS2016.POReports" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
      <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        </div> 
        <div style="padding-left: 5px; padding-top: 5px;">                      
        <div style="padding-top: 5px; padding-bottom: 5px"><dx:ASPxLabel ID="lblStatus" 
                runat="server">
        </dx:ASPxLabel></div>        
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="POReportID" 
                Width="1100px">
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" 
                        VisibleIndex="0" ShowEditButton="True" ShowNewButtonInHeader="True" 
                        ShowApplyFilterButton="True" ShowDeleteButton="True" Width="10px" 
                        Visible="False">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Report" FieldName="ReportType" 
                        VisibleIndex="2" Width="400px">
                        <PropertiesButtonEdit ClientInstanceName="txtUsername">
                        <ClientSideEvents ButtonClick="function(s, e) {
	txtUsername.SetText('');
	txtUsername.Focus();	
}" GotFocus="function(s, e) {
	var button = txtUsername.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtUsername.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtUsername.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                            <Buttons>
                                <dx:EditButton>
                                    <Image IconID="actions_cancel_16x16">
                                    </Image>
                                </dx:EditButton>
                            </Buttons>

                        </PropertiesButtonEdit>
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />

<EditFormSettings RowSpan="1" VisibleIndex="1"></EditFormSettings>
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="1" Caption="Purchase Order" FieldName="PurchaseOrder" ReadOnly="True" 
                        Width="270px">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
<EditFormSettings RowSpan="1" Visible="True" VisibleIndex="0"></EditFormSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="8">
                        <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                        <DataItemTemplate>
                            <asp:LinkButton ID="lbGenerate" runat="server" ForeColor="#FF6600" onclick="lbGenerate_Click"><%# DataBinder.Eval(Container.DataItem, "Button_Generate")%></asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="9">
                        <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                        <DataItemTemplate>
                            <asp:LinkButton ID="lbView" runat="server" ForeColor="#FF6600" onclick="lbView_Click"><%# DataBinder.Eval(Container.DataItem, "Button_View")%></asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Generated" 
                        FieldName="DateTimePosted" VisibleIndex="3" Width="120px">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy hh:mm tt">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="POID" FieldName="POID" Visible="False" 
                        VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Document Cntr." FieldName="DocCntr" 
                        VisibleIndex="5" Width="100px">
                        <Settings AutoFilterCondition="Equals" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ReportTypeID" FieldName="ReportTypeID" 
                        Visible="False" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="POReportID" FieldName="POReportID" 
                        Visible="False" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1">
                </SettingsEditing>
                <Settings ShowFilterRow="True" />

<Settings ShowFilterRow="True"></Settings>

                <SettingsPopup>
                    <EditForm Modal="True" HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
                </SettingsPopup>
            </dx:ASPxGridView>

            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/Rpt/PDFViewer.aspx" EnableTheming="True" 
                HeaderText="CPS Report" ShowMaximizeButton="True" 
                Theme="Office2010Blue">                 
</dx:ASPxPopupControl>
        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
            <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
            <asp:TextBox ID="TextBox1" runat="server" Width="718px" Visible="False"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Button" Visible="False" />
            <asp:Button ID="Button3" runat="server" Text="Button" Visible="False" />
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>