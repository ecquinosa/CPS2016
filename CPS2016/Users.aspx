<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Users.aspx.vb" Inherits="CPS2016.Users" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
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
        <div style="padding-left: 5px; padding-top: 5px; height: 28px;">
        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid">
            </dx:ASPxGridViewExporter>
            <div style="width: 241px; float: left;">
                <dx:ASPxCheckBox ID="chkAllUser" runat="server" AutoPostBack="True" 
                    Text="View All User" Theme="Office2010Blue">
                </dx:ASPxCheckBox>
            </div>            
        </div>      
        <div style="padding-left: 5px; padding-top: 5px;">                      
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="UserID" 
                Width="655px">
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" 
                        VisibleIndex="0" ShowEditButton="True" ShowNewButtonInHeader="True" 
                        ShowApplyFilterButton="True" ShowDeleteButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Status" FieldName="IsActive" 
                        VisibleIndex="7">
                        <EditFormSettings VisibleIndex="3" RowSpan="1" Visible="False" />
<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="3"></EditFormSettings>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Date Added" FieldName="DateTimePosted" 
                        ReadOnly="True" UnboundType="DateTime" VisibleIndex="8">
                        <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy">
                        </PropertiesDateEdit>
                        <EditFormSettings RowSpan="1" Visible="False" />

<EditFormSettings RowSpan="1" Visible="False"></EditFormSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Username" FieldName="Username" 
                        VisibleIndex="2">
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
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataButtonEditColumn Caption="First" 
                        FieldName="FName" VisibleIndex="3">
                        <PropertiesButtonEdit ClientInstanceName="txtEmpName">
                        <ClientSideEvents ButtonClick="function(s, e) {
	txtEmpName.SetText('');
	txtEmpName.Focus();	
}" GotFocus="function(s, e) {
	var button = txtEmpName.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtEmpName.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtEmpName.GetButton(0);
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

<EditFormSettings RowSpan="1" VisibleIndex="2"></EditFormSettings>
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataComboBoxColumn Caption="Edit Type" 
                        FieldName="EditType" VisibleIndex="10" 
                        UnboundType="String" Visible="False">
                        <EditFormSettings Visible="True" />
<EditFormSettings Visible="True"></EditFormSettings>
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataButtonEditColumn FieldName="MName" 
                        VisibleIndex="4" Caption="Middle">
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="11" Caption=" ">
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#0066CC" 
                                onclick="LinkButton1_Click">Role</asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="UserID" FieldName="UserID" ReadOnly="True" 
                        Visible="False" VisibleIndex="1">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Last" FieldName="LName" 
                        VisibleIndex="6">
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn Caption="Login Cntr" FieldName="LogInAttmptCntr" 
                        VisibleIndex="9">
                        <EditFormSettings Visible="False" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
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
                AllowResize="True" ContentUrl="~/UserRole.aspx" EnableTheming="True" 
                HeaderText="User Role(s)" ShowMaximizeButton="True" Theme="Office2010Blue">
                 </dx:ASPxPopupControl>
                 <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>