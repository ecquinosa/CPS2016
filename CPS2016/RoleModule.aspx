<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RoleModule.aspx.vb" Inherits="CPS2016.RoleModule" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-bottom: 10px"><asp:Button ID="Button1" runat="server" 
            Text="Save Changes" BackColor="#006600" BorderColor="White" BorderStyle="Solid" 
            BorderWidth="1px" Font-Names="Tahoma" ForeColor="White" Height="29px" 
            Width="104px" />
        <asp:Label ID="lblResult" runat="server" 
            style="font-family: Arial, Helvetica, sans-serif" Font-Size="Small"></asp:Label>        
    </div>
    <div style="padding-bottom: 10px">
        <asp:Label ID="lblRole" runat="server" Text="Role: [Role]" 
            style="font-family: Arial, Helvetica, sans-serif"></asp:Label>        
    </div>
    <div>
    
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="ModuleID" 
                Width="560px">

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

<Settings ShowFilterRow="True"></Settings>

                <Columns>
                    <dx:GridViewCommandColumn 
                        VisibleIndex="5" Width="10px" SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Module Description" FieldName="ModuleDesc" 
                        VisibleIndex="2" Width="450px">
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
                        <EditFormSettings RowSpan="1" VisibleIndex="2" />

<EditFormSettings RowSpan="1" VisibleIndex="1"></EditFormSettings>
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="0" Caption="ModuleID" FieldName="ModuleID" ReadOnly="True" 
                        Width="20px">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Module" FieldName="Module" VisibleIndex="1" 
                        Width="150px">
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Page" 
                        FieldName="Page" VisibleIndex="3" Width="200px">
                        <EditFormSettings RowSpan="1" VisibleIndex="3" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

                <SettingsPager PageSize="20" Mode="ShowAllRecords">
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
    
    </div>
    </form>
</body>
</html>
