<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditNamesAddress.aspx.vb" Inherits="CPS2016.EditNamesAddress" %>


<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
    <script type="text/javascript">
    function ShowProcessingPanel() {
        Callback.PerformCallback();
        ProcessingPanel.Show();
    }
    </script>
    <form id="form1" runat="server">
    <div style="padding-bottom: 15px">
            <strong>EDIT FIRST NAME/ LAST NAME/ ADDRESS FOR LASER DATA</strong></div>
    <div style="float: left; padding-bottom: 10px; width: 1059px;" id="divOutput" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Barcode/ CRN</div>
        <div style="float:left; width: 591px; height: 60px;">
            <dx:ASPxButtonEdit runat="server" NullText="enter barcode or csn" 
            Width="300px" Height="20px" ClientInstanceName="txtCSNCRN.SetText" 
            ID="txtCSNCRN">
<ClientSideEvents ButtonClick="function(s, e) {
	txtCSNCRN.SetText('');
	txtCSNCRN.Focus();	
}" GotFocus="function(s, e) {
	var button = txtCSNCRN.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtCSNCRN.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtCSNCRN.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

            <dx:ASPxButton ID="btnSubmit" runat="server" Height="30px" 
                Text="Submit" Width="100px" 
                Theme="Office2010Blue">
                
            </dx:ASPxButton>

        &nbsp;<asp:Label ID="lblStatus" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        

        </div>            
    </div>
         <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div4" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Purchase Order</div>
        <div style="float:left; width: 591px;">
            <dx:ASPxButtonEdit runat="server" 
            Width="400px" Height="20px" ClientInstanceName="txtPO" 
            ID="txtPO">
<ClientSideEvents ButtonClick="function(s, e) {
	txtPO.SetText('');
	txtPO.Focus();	
}" GotFocus="function(s, e) {
	var button = txtPO.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtPO.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtPO.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
</dx:ASPxButtonEdit>

        </div>            
    </div>
        <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div5" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Back OCR</div>
        <div style="float:left; width: 591px;">
            <dx:ASPxButtonEdit runat="server" 
            Width="400px" Height="20px" ClientInstanceName="txtBackOCR" 
            ID="txtBackOCR">
<ClientSideEvents ButtonClick="function(s, e) {
	txtBackOCR.SetText('');
	txtBackOCR.Focus();	
}" GotFocus="function(s, e) {
	var button = txtBackOCR.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtBackOCR.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtBackOCR.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
</dx:ASPxButtonEdit>

        </div>            
    </div>
        <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div1" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        First Name</div>
        <div style="float:left; width: 591px;">
            <asp:TextBox ID="txtFName" runat="server" Width="392px"></asp:TextBox>

        </div>            
    </div>
        <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div2" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Last Name</div>
        <div style="float:left; width: 591px;">
            <asp:TextBox ID="txtLName" runat="server" Width="392px"></asp:TextBox>

        </div>            
    </div>
        <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div6" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Suffix</div>
        <div style="float:left; width: 591px;">
            <asp:TextBox ID="txtSuffix" runat="server" Width="392px"></asp:TextBox>

        </div>            
    </div>
        <div style="float: left; padding-bottom: 10px; width: 1059px;" id="div3" 
        runat="server">
    <div style="float:left; width: 148px; font-family: Arial, Helvetica, sans-serif;">
        Address</div>
        <div style="float:left; width: 591px;">
            <asp:TextBox ID="txtAddress" runat="server" Height="61px" TextMode="MultiLine" Width="392px"></asp:TextBox>

        </div>            
    </div>
    <div style="float: left; padding-top: 10px; width: 1061px; height: 52px;">
    
            <dx:ASPxButton ID="btnSave" runat="server" Height="30px" 
                Text="Save Changes" Width="100px" 
                Theme="Office2010Blue" Visible="False">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit changes?'); 
    Callback.PerformCallback();
    ProcessingPanel.Show();   
}" />
                
            </dx:ASPxButton>

    &nbsp;<asp:Label ID="lblStatus2" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        

    </div>
   
            <br />
   <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue" Height="38px" 
                Width="121px">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>
    </form>
</body>
</html>
