<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserRole.aspx.vb" Inherits="CPS2016.UserRole" %>

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
        <asp:Label ID="lblUser" runat="server" Text="User: [User]" 
            style="font-family: Arial, Helvetica, sans-serif"></asp:Label>        
    </div>
    <div>
    
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="RoleID" 
                Width="316px">
                <Columns>
                    <dx:GridViewCommandColumn 
                        VisibleIndex="2" Width="10px" SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Role Description" FieldName="RoleDesc" 
                        VisibleIndex="1" Width="150px">
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
                        VisibleIndex="0" Caption="RoleID" FieldName="RoleID" ReadOnly="True" 
                        Width="20px">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
<EditFormSettings RowSpan="1" Visible="True" VisibleIndex="0"></EditFormSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>

                <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1">
                </SettingsEditing>

<Settings ShowFilterRow="True"></Settings>

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
