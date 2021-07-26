<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Role.aspx.vb" Inherits="CPS2016.Role" %>

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
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="RoleID" 
                Width="500px">
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" 
                        VisibleIndex="0" ShowEditButton="True" ShowNewButtonInHeader="True" 
                        ShowApplyFilterButton="True" ShowDeleteButton="True" Width="10px">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Role Description" FieldName="RoleDesc" 
                        VisibleIndex="2" Width="500px">
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
                        VisibleIndex="1" Caption="RoleID" FieldName="RoleID" ReadOnly="True" 
                        Width="50px">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="3">
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#FF6600" onclick="LinkButton1_Click">Module</asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="4">
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <asp:LinkButton ID="lbActivity" runat="server" ForeColor="#FF6600" onclick="lbActivity_Click">Activity</asp:LinkButton>
                        </DataItemTemplate>
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
                AllowResize="True" ContentUrl="~/RoleModule.aspx" EnableTheming="True" 
                HeaderText="Role Module(s)" ShowMaximizeButton="True" 
                Theme="Office2010Blue">
                 </dx:ASPxPopupControl>
        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>