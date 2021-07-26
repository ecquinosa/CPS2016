<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportA_CreatedPrintOrderForm.aspx.vb" Inherits="CPS2016.ReportA_CreatedPrintOrderForm" %>

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
    <dx:ASPxButton ID="btnSubmit" runat="server" BackColor="#339933" 
        ForeColor="#CCFFFF" Height="30px" Text="Submit" Theme="Youthful" 
        Width="100px" ClientInstanceName="btnSubmit">
        <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit?');   
    Callback.PerformCallback();
    ProcessingPanel.Show();
}"></ClientSideEvents>
    </dx:ASPxButton>
    <dx:ASPxLabel ID="lblStatus" runat="server">
    </dx:ASPxLabel>
    </div>
    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">Purchase Order</div>
                            <div id="divSearch0" style="width: 410px; float: right;">
                                <dx:ASPxButtonEdit ID="txtPurchaseOrder" runat="server" ClientInstanceName="txtPurchaseOrder" 
                                    Height="20px" NullText="enter purchase order" Width="300px">
                                    <ClientSideEvents ButtonClick="function(s, e) {
	txtPurchaseOrder.SetText('');
	txtPurchaseOrder.Focus();	
}" GotFocus="function(s, e) {
	var button = txtPurchaseOrder.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtPurchaseOrder.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtPurchaseOrder.GetButton(0);
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
            Requested By</div>
        <div ID="divSearch1" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtRequestedBy" runat="server" ClientInstanceName="txtRequestedBy" 
                Height="20px" NullText="enter requested by" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtRequestedBy.SetText('');
	txtRequestedBy.Focus();	
}" GotFocus="function(s, e) {
	var button = txtRequestedBy.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtRequestedBy.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtRequestedBy.GetButton(0);
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
            Record(s) received</div>
        <div ID="divSearch3" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtRecordsReceived" runat="server" ClientInstanceName="txtRecordsReceived" 
                Height="20px" NullText="enter records received" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtRecordsReceived.SetText('');
	txtRecordsReceived.Focus();	
}" GotFocus="function(s, e) {
	var button = txtRecordsReceived.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtRecordsReceived.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtRecordsReceived.GetButton(0);
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
            Invalid record(s)</div>
        <div ID="div1" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtInvalidRecords" runat="server" ClientInstanceName="txtInvalidRecords" 
                Height="20px" NullText="enter invalid records" Width="300px" Text="0">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtInvalidRecords.SetText('');
	txtInvalidRecords.Focus();	
}" GotFocus="function(s, e) {
	var button = txtInvalidRecords.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtInvalidRecords.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtInvalidRecords.GetButton(0);
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
            Valid record(s)</div>
        <div ID="div2" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtValidRecords" runat="server" ClientInstanceName="txtValidRecords" 
                Height="20px" NullText="enter valid records" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtValidRecords.SetText('');
	txtValidRecords.Focus();	
}" GotFocus="function(s, e) {
	var button = txtValidRecords.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtValidRecords.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtValidRecords.GetButton(0);
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
            Reprint(s)</div>
        <div ID="div3" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtReprints" runat="server" ClientInstanceName="txtReprints" 
                Height="20px" NullText="enter reprint" Width="300px" Text="0">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtReprints.SetText('');
	txtReprints.Focus();	
}" GotFocus="function(s, e) {
	var button = txtReprints.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtReprints.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtParam.GetButton(0);
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
            Card(s) for printing</div>
        <div ID="div4" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtCardForPrinting" runat="server" ClientInstanceName="txtCardForPrinting" 
                Height="20px" NullText="enter card for printing" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtCardForPrinting.SetText('');
	txtCardForPrinting.Focus();	
}" GotFocus="function(s, e) {
	var button = txtCardForPrinting.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtCardForPrinting.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtCardForPrinting.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
                <Buttons>
                    <dx:EditButton>
                        <Image IconID="actions_cancel_16x16">
                        </Image>
                    </dx:EditButton>
                </Buttons>
            </dx:ASPxButtonEdit>
            <asp:Button ID="btnCompute" runat="server" Text="Compute" Visible="False" 
                Width="72px" />
        </div>
    </div>

    <div style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
            Status</div>
        <div ID="div5" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtStatus" runat="server" ClientInstanceName="txtStatus" 
                Height="20px" NullText="enter status" Width="300px" Text="PO Created">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtStatus.SetText('');
	txtStatus.Focus();	
}" GotFocus="function(s, e) {
	var button = txtStatus.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtStatus.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" KeyDown="function(s, e) {
	DoProcessEnterKey(e.htmlEvent, '');
}" LostFocus="function(s, e) {
	var button = txtStatus.GetButton(0);
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
            Description</div>
        <div ID="div6" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtDescription" runat="server" ClientInstanceName="txtDescription" 
                Height="20px" NullText="enter description" Width="300px">
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

    <div id="divJVDate" style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                <asp:Label ID="Label1" runat="server" Text="JV Confirmed Date"></asp:Label>
                            </div>
        <div ID="div7" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtJVConfirmedDate" runat="server" ClientInstanceName="txtJVConfirmedDate" 
                Height="20px" NullText="enter confirmed date" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtJVConfirmedDate.SetText('');
	txtJVConfirmedDate.Focus();	
}" GotFocus="function(s, e) {
	var button = txtJVConfirmedDate.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtJVConfirmedDate.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtJVConfirmedDate.GetButton(0);
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

    <div id="divJVName" style="float: left; width: 535px; top: 0px; left: 0px; padding-bottom: 5px;">
                            <div style="width: 125px; float: left; height: 21px;">
                                <asp:Label ID="Label2" runat="server" Text="JV Confirmed By"></asp:Label>
                            </div>
        <div ID="div9" style="width: 410px; float: right;">
            <dx:ASPxButtonEdit ID="txtJVConfirmedBy" runat="server" ClientInstanceName="txtJVConfirmedBy" 
                Height="20px" NullText="enter confirmed by" Width="300px">
                <ClientSideEvents ButtonClick="function(s, e) {
	txtJVConfirmedBy.SetText('');
	txtJVConfirmedBy.Focus();	
}" GotFocus="function(s, e) {
	var button = txtJVConfirmedBy.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtJVConfirmedBy.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtJVConfirmedBy.GetButton(0);
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
                            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" 
                                ClientInstanceName="ProcessingPanel" Height="38px" Modal="True" 
                                Text="Processing&amp;hellip;" Theme="Office2010Blue" Width="121px">
                            </dx:ASPxLoadingPanel><dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
<ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }"></ClientSideEvents>
    </dx:ASPxCallback>
    </div>

    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

    </div>
    </form>
</body>
</html>
