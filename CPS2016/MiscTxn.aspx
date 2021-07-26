<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="MiscTxn.aspx.vb" Inherits="CPS2016.MiscTxn" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
            <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div> 
        <div style="padding-left: 5px; padding-top: 10px; height: 36px;">

                        <dx:ASPxLabel ID="lblStatus" runat="server" 
                Text="Miscellaneous Reports" style="font-size: small">
                </dx:ASPxLabel>

        </div>
        <div style="padding-left: 10px; padding-top: 10px; height: 93px;">
            <table style="width: 100%; ">
                <tr valign="top">
                    <td style="width: 377px; padding-bottom: 10px;">
                        1.
                        Upload Purchase Order folder</td>
                    <td style="padding-bottom: 15px">
            <dx:ASPxButtonEdit runat="server" NullText="enter Purchase Order" Width="300px" 
                            Height="20px" ClientInstanceName="txtUploadPO" ID="txtUploadPO">
<ClientSideEvents ButtonClick="function(s, e) {
	txtUploadPO.SetText(&#39;&#39;);
	txtUploadPO.Focus();	
}" GotFocus="function(s, e) {
	var button = txtUploadPO.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtUploadPO.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtUploadPO.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmitUploadPO" Text="Submit" Theme="Youthful" 
                            Width="100px" Height="30px" BackColor="#339933" ForeColor="#CCFFFF" 
                            ID="btnSubmitUploadPO">
<ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm(&#39;Are you sure you want to upload PO?&#39;);   
    Callback.PerformCallback();
    ProcessingPanel.Show();
}"></ClientSideEvents>
</dx:ASPxButton>

                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 377px; padding-bottom: 10px;">
                        2.
                        Upload barcode/ data missing</td>
                    <td style="padding-bottom: 15px">
            <dx:ASPxButtonEdit runat="server" NullText="enter folder path" Width="300px" Height="20px" 
                            ClientInstanceName="txtBarcodeMissing" ID="txtBarcodeMissing">
<ClientSideEvents ButtonClick="function(s, e) {
	txtBarcodeMissing.SetText(&#39;&#39;);
	txtBarcodeMissing.Focus();	
}" GotFocus="function(s, e) {
	var button = txtBarcodeMissing.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtBarcodeMissing.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtBarcodeMissing.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

    <dx:ASPxButton runat="server" ClientInstanceName="btnSubmitMissing" Text="Submit" Theme="Youthful" 
                            Width="100px" Height="30px" BackColor="#339933" ForeColor="#CCFFFF" 
                            ID="btnSubmitMissing">
<ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm(&#39;Are you sure you want to submit?&#39;);   
    Callback.PerformCallback();
    ProcessingPanel.Show();
}"></ClientSideEvents>
</dx:ASPxButton>

                    </td>                    
                </tr>
                <tr valign="top">
                    <td style="width: 377px; padding-bottom: 10px;">
                        3.
                        Return purchase order status to new upload/ indigo extractable</td>
                    <td style="padding-bottom: 15px">
            <dx:ASPxButtonEdit runat="server" NullText="enter purchase order" Width="300px" Height="20px" 
                            ClientInstanceName="txtPOStatusReturn" ID="txtPOStatusReturn">
<ClientSideEvents ButtonClick="function(s, e) {
	txtPOStatusReturn.SetText(&#39;&#39;);
	txtPOStatusReturn.Focus();	
}" GotFocus="function(s, e) {
	var button = txtPOStatusReturn.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtPOStatusReturn.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtPOStatusReturn.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

    <dx:ASPxButton runat="server" ClientInstanceName="btnPOStatusReturn" Text="Submit" Theme="Youthful" 
                            Width="100px" Height="30px" BackColor="#339933" ForeColor="#CCFFFF" 
                            ID="btnPOStatusReturn">
<ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm(&#39;Are you sure you want to submit?&#39;);   
    Callback.PerformCallback();
    ProcessingPanel.Show();
}"></ClientSideEvents>
</dx:ASPxButton>

                    </td>                    
                </tr>
                <tr valign="top">
                <td style="padding-bottom: 10px;" colspan="2">
        <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="250px" Width="800px" 
                Visible="False">
            <Border BorderStyle="None" />
            </dx:ASPxMemo>

                        </td>
                </tr>               
            </table>           
        <div style="float: left; width: 791px;">

            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue" Height="38px" 
                Width="121px">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>
            </div>
             
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>