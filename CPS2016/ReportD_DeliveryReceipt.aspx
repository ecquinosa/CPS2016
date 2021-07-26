<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportD_DeliveryReceipt.aspx.vb" Inherits="CPS2016.ReportD_DeliveryReceipt" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>

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
            </div>
    <div style="padding-top: 10px">
    
        <dx:ASPxRoundPanel ID="groupPanel" runat="server" Theme="Office2010Blue" 
                            Width="490px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent runat="server">
<div style="float: left; width: 510px; top: 0px; left: 0px; padding-bottom: 15px;">
    <asp:Button ID="Button1" runat="server" Enabled="False" Height="30px" 
        Text="Submit" Width="78px" />
    <dx:ASPxLabel ID="lblStatus" runat="server">
    </dx:ASPxLabel>
    <dx:ASPxButton ID="btnSubmit" runat="server" BackColor="#339933" 
        ClientInstanceName="btnSubmit" ForeColor="#CCFFFF" Height="30px" Text="Submit" 
        Theme="Youthful" Visible="False" Width="100px">
        <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit?');   
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
    </dx:ASPxButton>
    </div>
    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">Purchase Order</div>
                            <div id="divSearch0" style="width: 410px; float: right;">
                                <dx:ASPxComboBox ID="cboPO" runat="server" 
                                    OnSelectedIndexChanged="cboPO_SelectedIndexChanged" ClientInstanceName="cboPO" 
                                    DropDownRows="30" DropDownStyle="DropDown" NullText="select field to search" 
                                    style="font-size: small" Theme="Office2010Blue" Width="340px" 
                                    AutoPostBack="True">
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
}" SelectedIndexChanged="function(s, e) {
	e.processOnServer = true;	
}" />
                                    <Buttons>
                                        <dx:EditButton>
                                            <Image IconID="actions_cancel_16x16">
                                            </Image>
                                        </dx:EditButton>
                                    </Buttons>
                                </dx:ASPxComboBox>
                                
                                </div>
                                
    </div>
    
    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
            PO Date</div>
        <div ID="divSearch2" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtPODate" runat="server" ClientInstanceName="txtPODate" 
                Height="20px" NullText="enter po date" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtPODate.SetText('');
	txtPODate.Focus();	
}" GotFocus="function(s, e) {
	var button = txtPODate.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtPODate.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtPODate.GetButton(0);
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
    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                Quantity Ordered</div>
        <div ID="divSearch3" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtQtyOrdered" runat="server" ClientInstanceName="txtQtyOrdered" 
                Height="20px" NullText="enter records received" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtQtyOrdered.SetText('');
	txtQtyOrdered.Focus();	
}" GotFocus="function(s, e) {
	var button = txtQtyOrdered.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtQtyOrdered.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtQtyOrdered.GetButton(0);
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

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                Quantity Reprinted</div>
        <div ID="div1" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtQtyReprinted" runat="server" ClientInstanceName="txtQtyReprinted" 
                Height="20px" NullText="enter invalid records" Width="300px" Text="0">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtQtyReprinted.SetText('');
	txtQtyReprinted.Focus();	
}" GotFocus="function(s, e) {
	var button = txtQtyReprinted.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtQtyReprinted.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtQtyReprinted.GetButton(0);
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

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                Total # of Cards Delivered</div>
        <div ID="div2" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtTotalCardsDelivered" runat="server" ClientInstanceName="txtTotalCardsDelivered" 
                Height="20px" NullText="enter valid records" Width="300px" Text="0">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtTotalCardsDelivered.SetText('');
	txtTotalCardsDelivered.Focus();	
}" GotFocus="function(s, e) {
	var button = txtTotalCardsDelivered.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtTotalCardsDelivered.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtTotalCardsDelivered.GetButton(0);
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

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                Total # of Boxes</div>
        <div ID="div3" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtBoxes" runat="server" ClientInstanceName="txtBoxes" 
                Height="20px" NullText="enter reprint" Width="300px" Text="0">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtBoxes.SetText('');
	txtBoxes.Focus();	
}" GotFocus="function(s, e) {
	var button = txtBoxes.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtBoxes.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtBoxes.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                <Buttons>
                    <dx:EditButton>
                        <Image IconID="actions_cancel_16x16">
                        </Image>
                    </dx:EditButton>
                </Buttons>
            </dx:ASPxButtonEdit>
            <asp:Button ID="btnCompute" runat="server" Text="Compute" 
                Width="72px" />
        </div>
    </div>
   

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
            Description</div>
        <div ID="div6" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtDescription" runat="server" ClientInstanceName="txtDescription" 
                Height="20px" NullText="enter description" Width="415px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtDescription.SetText('');
	txtDescription.Focus();	
}" GotFocus="function(s, e) {
	var button = txtDescription.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtDescription.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtDescription.GetButton(0);
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

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                DR Number</div>
        <div ID="div4" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtDRNumber" runat="server" ClientInstanceName="txtDRNumber" 
                Height="20px" NullText="enter dr number" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtDRNumber.SetText('');
	txtDRNumber.Focus();	
}" GotFocus="function(s, e) {
	var button = txtDRNumber.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtDRNumber.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtDRNumber.GetButton(0);
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
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

    </div>
    </form>
</body>
</html>
