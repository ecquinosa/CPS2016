<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusCardBreakdown.aspx.vb" Inherits="CPS2016.StatusCardBreakdown" %>

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
    <script type="text/javascript">
        function ShowProcessingPanel() {
            Callback.PerformCallback();
            ProcessingPanel.Show();
        }
    </script>
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
    </div>
    <div style="padding-bottom: 10px">
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
                                        Purchase Order</div>
    </div>
    <div style="float: left; width: 431px; position: relative; top: 0px; left: 0px; padding-bottom: 5px; padding-top: 5px;">
        <dx:ASPxButton runat="server" Text="Submit" Theme="Office2010Blue" 
            Width="100px" Height="30px" ID="btnSearch"></dx:ASPxButton>

        <dx:ASPxLabel runat="server" ID="lblStatus0"></dx:ASPxLabel>

    </div>
    
                                
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

    </div>
    <div>
    
           <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    ClientInstanceName="xGrid" EnableTheming="True" KeyFieldName="CardID" 
                    Theme="Office2010Blue">
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                    <Columns>
                        <dx:GridViewCommandColumn ShowApplyFilterButton="True" 
                            ShowClearFilterButton="True" VisibleIndex="19" 
                            SelectAllCheckboxMode="Page" ShowSelectCheckbox="True">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="CardID" ReadOnly="True" 
                            VisibleIndex="0" Width="50px">
                            <PropertiesTextEdit ClientInstanceName="TxnID">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0"></EditFormSettings>

                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PurchaseOrder" FieldName="PurchaseOrder" 
                            VisibleIndex="1" Width="250px">
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4" />
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Batch" Width="50px" 
                            Caption="Batch" VisibleIndex="2">
<PropertiesTextEdit NullText="-Select Item-"></PropertiesTextEdit>

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
    <HeaderStyle HorizontalAlign="Center" />
    <CellStyle HorizontalAlign="Center">
    </CellStyle>
</dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CRN" FieldName="CRN" VisibleIndex="3" 
                            Width="110px">
                            <EditFormSettings Visible="False" />
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="First" FieldName="FName" VisibleIndex="6">
                            <EditFormSettings Visible="False" />
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Back OCR" FieldName="BackOCR" 
                            VisibleIndex="17" Width="80px">
                            <PropertiesTextEdit ClientInstanceName="txtStartTime">
                                <MaskSettings Mask="hh:mm tt" />
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Barcode" FieldName="Barcode" 
                            VisibleIndex="5" Width="200px">
                            <EditFormSettings Visible="False" />

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="3"></EditFormSettings>
                            <PropertiesTextEdit ClientInstanceName="txtSubModule" Height="100px" 
                                nulltext="enter Submodule">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" VisibleIndex="3" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Middle" FieldName="MName" VisibleIndex="8">

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="True" VisibleIndex="4"></EditFormSettings>
                            <PropertiesTextEdit ClientInstanceName="txtActivity" Height="100px" 
                                NullText="enter Activity">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="True" VisibleIndex="4" />

                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last" FieldName="LName" VisibleIndex="16">
                            <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="POID" FieldName="POID" Visible="False" 
                            VisibleIndex="20">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <SettingsPager PageSize="1000" Mode="ShowAllRecords">
                    </SettingsPager>

                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

                    <SettingsPopup>
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
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
