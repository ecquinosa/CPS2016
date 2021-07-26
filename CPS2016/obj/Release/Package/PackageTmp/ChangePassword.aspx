<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="ChangePassword.aspx.vb" Inherits="CPS2016.ChangePassword" %>

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
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div>
    <div style="padding-left: 5px; padding-top: 10px; height: 93px;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="443px" HeaderText="Change Password">
                            <PanelCollection>
                            
<dx:PanelContent runat="server">
<div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
    <div style="width: 97px; float: left; height: 21px;">Old</div>
    <div id="divSearch" style="width: 218px; float: right;">
        <dx:ASPxButtonEdit ID="txtOldPassword" runat="server" 
            ClientInstanceName="txtOldPassword" NullText="enter old password" 
            Width="300px" Password="True">
            <ClientSideEvents ButtonClick="function(s, e) {
	txtOldPassword.SetText('');
	txtOldPassword.Focus();	
}" GotFocus="function(s, e) {
	var button = txtOldPassword.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtOldPassword.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtOldPassword.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
            <Buttons>
                <dx:EditButton>
                    <Image IconID="actions_cancel_16x16">
                    </Image>
                </dx:EditButton>
            </Buttons>
        </dx:ASPxButtonEdit>
    </div>
    </div>
    <div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 10px;">
                            <div style="width: 97px; float: left; height: 21px;">New</div>
                            <div id="divValue" style="width: 218px; float: right;">
                                <dx:ASPxButtonEdit ID="txtNewPassword" runat="server" 
                                    ClientInstanceName="txtNewPassword" NullText="enter new password" 
                                    Width="300px" Password="True">
                                    <ClientSideEvents ButtonClick="function(s, e) {
	txtNewPassword.SetText('');
	txtNewPassword.Focus();	
}" GotFocus="function(s, e) {
	var button = txtNewPassword.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtNewPassword.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtNewPassword.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                                    <Buttons>
                                        <dx:EditButton>
                                            <Image IconID="actions_cancel_16x16">
                                            </Image>
                                        </dx:EditButton>
                                    </Buttons>
                                </dx:ASPxButtonEdit>
                                
                                </div>
                                
    </div>
    <div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 10px;">
                            <div style="width: 97px; float: left; height: 21px;">Confirm</div>
                            <div id="div1" style="width: 218px; float: right;">
                                <dx:ASPxButtonEdit ID="txtConfirm" runat="server" 
                                    ClientInstanceName="txtConfirm" NullText="confirm password" Width="300px" 
                                    Password="True">
                                    <ClientSideEvents ButtonClick="function(s, e) {
	txtConfirm.SetText('');
	txtConfirm.Focus();	
}" GotFocus="function(s, e) {
	var button = txtConfirm.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtConfirm.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtConfirm.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                                    <Buttons>
                                        <dx:EditButton>
                                            <Image IconID="actions_cancel_16x16">
                                            </Image>
                                        </dx:EditButton>
                                    </Buttons>
                                </dx:ASPxButtonEdit>
                                
                                </div>
                                
    </div>
    <div style="float: left; width: 416px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
    <dx:ASPxButton ID="btnSearch" runat="server" Height="30px" Text="Submit" 
                                    Theme="Office2010Blue" Width="100px">
                                </dx:ASPxButton>
        <dx:ASPxLabel ID="lblStatus" runat="server">
        </dx:ASPxLabel>
    </div>
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

            <br />

        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>