<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusCardByPO.aspx.vb" Inherits="CPS2016.StatusCardByPO" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
    <form id="form1" runat="server">
     <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" /></div>
    <div>    
    <div style="padding-bottom: 10px; padding-top: 10px;">
        <dx:ASPxButton ID="btnProcess" runat="server" Height="30px" 
                Text="Process" Width="100px" Theme="Office2010Blue">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to process selected record(s) and continue?');
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
                
            </dx:ASPxButton>
        <asp:Label ID="lblResult" runat="server" 
            style="font-family: Arial, Helvetica, sans-serif" Font-Size="Small"></asp:Label>        
        <asp:TextBox ID="TextBox1" runat="server" Width="764px" Visible="False"></asp:TextBox>
        <dx:ASPxButton ID="btnProcess0" runat="server" Height="30px" 
                Text="Process" Width="100px" Theme="Office2010Blue" Visible="False">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to process selected record(s) and continue?');
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
                
            </dx:ASPxButton>
    </div>
    <div style="padding-bottom: 10px; width: 689px;">
        <asp:Label ID="lblStatus" runat="server" Text="Status: [Status]" 
            style="font-family: Arial, Helvetica, sans-serif"></asp:Label>        
        <br />
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="507px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent runat="server">
<div style="float: left; width: 471px; position: relative; top: 0px; left: 0px;">
                                    <div ID="divSearch0" style="width: 350px; float: right;">
                                        <dx:ASPxComboBox ID="cboPO" runat="server" ClientInstanceName="cboPO" 
                                            NullText="select field to search" style="font-size: small" 
                                            Theme="Office2010Blue" Width="310px">
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
                                        </dx:ASPxComboBox>
                                    </div>
                                    <div style="width: 105px; float: left; height: 21px;">
                                        Next Status</div>
    </div>
    <div style="float: left; width: 431px; position: relative; top: 0px; left: 0px; padding-bottom: 5px; padding-top: 5px;">

    </div>
    
                                
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

    </div>
    <div style="width: 689px">
    
        <dx:ASPxGridView ID="xGrid" ClientInstanceName="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" 
                Width="600px" KeyFieldName="POID">                
                <ClientSideEvents SelectionChanged="function(s, e){grid_SelectionChanged(s,e);  }" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="2" Width="330px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="7" Width="30px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Batch" FieldName="Batch" VisibleIndex="3" 
                        Width="80px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="POID" VisibleIndex="1" 
                        Width="60px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True" VisibleIndex="9" ShowClearFilterButton="True" 
                        Width="60px">
                    </dx:GridViewCommandColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                <SettingsPager Visible="False" Mode="ShowAllRecords" AlwaysShowPager="True">
                </SettingsPager>
                <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                </SettingsEditing>
                <Settings ShowFilterRow="True" />
                <SettingsPopup>
                    <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                </SettingsPopup>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>
    
            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue" Height="38px" 
               Width="121px">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>
    <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
    
    </div>
    </form>
</body>
</html>
