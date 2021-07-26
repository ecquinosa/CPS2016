<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="CPS2016.Login" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Allcard Card Production System - SSS</title>

    <link rel="stylesheet" href="css/reset.css">
	<link rel="stylesheet" href="css/animate.css">
	<link rel="stylesheet" href="css/styles.css">
    <style type="text/css">
        .style1
        {
            color: #FF9933;
        }
        .style2
        {
            color: #FF9933;
            text-decoration: underline;
        }
        .style3
        {
            font-size: x-small;
        }
        .style4
        {
            font-size: small;
        }
    </style>
</head>
<body>
    
    <div id="container">
		
		<form id="form1" runat="server">		
        <div style="padding-bottom: 5px; padding-left: 5px;"><strong><em><span class="style2">C</span>ard <span class="style2">P</span>roduction
            <span class="style2">S</span>ystem </em></strong><span class="style3"><strong>
            <em class="style4">v1.8</em></strong></span></div>
           <label for="name">Username:</label>			
    <dx:ASPxButtonEdit runat="server" Width="280px" ID="txtUsername"
                        Font-Size="Medium" Height="30px" 
            ClientInstanceName="txtUsername" CssClass="username" 
            NullText="enter username" >
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
                        <ValidationSettings ErrorText="">                            
                            <RegularExpression ErrorText="" />
                            <RequiredField IsRequired="True" ErrorText="" />
                        </ValidationSettings>                                    
                        <InvalidStyle BackColor="#FFFFCC" />
                            <BackgroundImage Repeat="NoRepeat" />
                    </dx:ASPxButtonEdit>
		<label for="username">Password:</label>
		
		<p><a href="#" class="style1" style="visibility: hidden">Forgot your password?</a>		
        <dx:ASPxButtonEdit ID="txtPassword" runat="server" ClientInstanceName="txtPassword" 
                            Font-Size="Medium" Height="30px" NullText="enter password" 
                            Password="True" Width="280px" CssClass="password">
                            <ClientSideEvents ButtonClick="function(s, e) {
	txtPassword.SetText('');
	txtPassword.Focus();	
}" GotFocus="function(s, e) {
	var button = txtPassword.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtPassword.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtPassword.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                            <Buttons>
                                <dx:EditButton>
                                    <Image IconID="actions_cancel_16x16">
                                    </Image>
                                </dx:EditButton>
                            </Buttons>
                            <ButtonEditEllipsisImage IconID="actions_cancel_16x16">
                            </ButtonEditEllipsisImage>
                            <ValidationSettings ErrorText="">
                                <RequiredField IsRequired="True" ErrorText="" />
                            </ValidationSettings>
                            <InvalidStyle BackColor="#FFFFCC" />
                        </dx:ASPxButtonEdit>

		<div id="lower">
		
		<!--<input type="checkbox"><label class="check" for="checkbox">Keep me logged in</label> -->
		
		    
                            <asp:Button ID="loginSubmit" runat="server" Height="25px" 
                Width="120px" Text="Login" />
        <div style="padding-top: 40px; padding-left: 5px;">
            <asp:Label ID="Label1" runat="server" ForeColor="#FF9933" Font-Size="Small"></asp:Label>
            <dx:ASPxCheckBox ID="chkEndSession" runat="server" Font-Bold="False" 
                Font-Size="8pt" Text="End existing system session?" Theme="Office2010Blue" 
                Visible="False">
            </dx:ASPxCheckBox>
            <br />
            </div>
		</div>
		
		</form>
		
	</div>
</body>
</html>
