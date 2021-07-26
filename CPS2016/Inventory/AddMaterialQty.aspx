<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddMaterialQty.aspx.vb" Inherits="CPS2016.AddMaterialQty" %>

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
    <div style="padding-bottom: 10px">
        <asp:Label ID="lblMaterial" runat="server" Text="Material: [Material]" 
            style="font-family: Arial, Helvetica, sans-serif; font-weight: 700;"></asp:Label>        
    </div>
    <div>
    
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="AddtlMaterialID" 
                Width="408px">
                <Columns>
                    <dx:GridViewCommandColumn 
                        VisibleIndex="0" Width="10px" ShowDeleteButton="True" 
                        ShowNewButtonInHeader="True" ShowEditButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Added Quantity" FieldName="AddedQty" 
                        VisibleIndex="2" Width="100px">
                        <PropertiesButtonEdit ClientInstanceName="txtUsername" 
                            DisplayFormatString="{0:N0}" Width="150px">
                        

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
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="1" Caption="ID" FieldName="AddtlMaterialID" ReadOnly="True" 
                        Width="50px">
                        <PropertiesTextEdit Width="150px">
                        </PropertiesTextEdit>
                        <EditFormSettings Visible="False" RowSpan="1" VisibleIndex="0" />
<EditFormSettings RowSpan="1" Visible="True" VisibleIndex="0"></EditFormSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Date Posted" FieldName="DateTimePosted" 
                        VisibleIndex="3" Width="150px">
                        <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy" Width="150px">
                        </PropertiesDateEdit>
                        <EditFormSettings Visible="True" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataDateColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1">
                </SettingsEditing>

                <SettingsPopup>
                    <EditForm Modal="True" HorizontalAlign="Center" VerticalAlign="WindowCenter" 
                        Width="150px" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
                </SettingsPopup>
            </dx:ASPxGridView>
    
    </div>
    </form>
</body>
</html>
