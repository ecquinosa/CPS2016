<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="QAGoodRejectTagging.aspx.vb" Inherits="CPS2016.QAGoodRejectTagging" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxMenu" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
     <script type="text/javascript">
         function DoProcessEnterKey(htmlEvent, editName) {
             if (htmlEvent.keyCode == 13) {
                 ASPxClientUtils.PreventEventAndBubble(htmlEvent);
                 if (editName) {
                     ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                 } else {
                     btn.DoClick();
                 }
             }
         }
    </script>
       <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div> 
        <div style="padding-left: 5px; padding-top: 5px; height: 36px;">

            <dx:ASPxButton ID="btnSubmitCards" runat="server" Height="30px" 
                Text="Submit Card/s" Width="100px" 
                Theme="Youthful" BackColor="#339933" ForeColor="#CCFFFF">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit the card(s) and continue?');
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
                
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnDelete" runat="server" Height="30px" 
        Text="Delete Selected" Theme="SoftOrange" Width="100px">
        <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to remove the card(s) and continue?');
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
    </dx:ASPxButton>

            <dx:ASPxButton ID="btnBack" runat="server" Height="30px" 
        Text="Cancel/ Back" Theme="Office2010Blue" Width="100px" EnableTheming="True">
    </dx:ASPxButton>

        </div>
        <div style="padding-left: 10px; padding-top: 10px; height: 93px;">
            <table style="width: 100%; ">
                <tr valign="top">
                    <td style="width: 127px">
                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Purchase Order" 
                Theme="Office2010Blue" style="font-size: small">
            </dx:ASPxLabel>
                    </td>
                    <td style="padding-bottom: 5px">
                    
                        <dx:ASPxButtonEdit runat="server" NullText="enter purchase order" Width="300px" 
                            Height="20px" ClientInstanceName="txtPO" ID="txtPO">
<ClientSideEvents ButtonClick="function(s, e) {
	txtPO.SetText(&#39;&#39;);
	txtPO.Focus();	
}" KeyDown="function(s, e) {
	DoProcessEnterKey(e.htmlEvent, &#39;&#39;);
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
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

                                
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 127px">
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Barcode" 
                            style="font-size: small" Theme="Office2010Blue">
            </dx:ASPxLabel>
                    </td>
                    <td style="padding-bottom: 5px;">
                        <dx:ASPxButtonEdit runat="server" NullText="enter barcode" Width="300px" 
                            Height="20px" ClientInstanceName="txtBarcode" ID="txtBarcode">
<ClientSideEvents ButtonClick="function(s, e) {
	txtBarcode.SetText(&#39;&#39;);
	txtBarcode.Focus();	
}" KeyDown="function(s, e) {
	DoProcessEnterKey(e.htmlEvent, &#39;&#39;);
    txtBarcode.Focus();	
}" GotFocus="function(s, e) {
	var button = txtBarcode.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtBarcode.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtBarcode.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

                                
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 127px">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="CRN" 
                            style="font-size: small" Theme="Office2010Blue">
            </dx:ASPxLabel>
                    </td>
                    <td style="padding-bottom: 5px;">
                        <dx:ASPxButtonEdit runat="server" NullText="enter crn" Width="300px" 
                            Height="20px" ClientInstanceName="txtCRN" ID="txtCRN">
<ClientSideEvents ButtonClick="function(s, e) {
	txtCRN.SetText(&#39;&#39;);
	txtCRN.Focus();	
}" KeyDown="function(s, e) {
	DoProcessEnterKey(e.htmlEvent, &#39;&#39;);
    txtCRN.Focus();
}" GotFocus="function(s, e) {
	var button = txtCRN.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtCRN.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtCRN.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

                                
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 127px">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Tag" 
                            style="font-size: small" Theme="Office2010Blue">
            </dx:ASPxLabel>
                    </td>
                    <td style="padding-bottom: 5px;">
                        <dx:ASPxComboBox ID="cboTag" runat="server" SelectedIndex="0" 
                            ClientInstanceName="cboTag" AutoPostBack="True">
                            <Items>
                                <dx:ListEditItem Selected="True" Text="-Select-" Value="-Select-" />
                                <dx:ListEditItem Text="Good" Value="Good" />
                                <dx:ListEditItem Text="Reject" Value="Reject" />
                            </Items>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 127px">
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Reject Type" 
                            style="font-size: small" Theme="Office2010Blue" 
                            ClientInstanceName="lblRejectType">
            </dx:ASPxLabel>
                    </td>
                    <td style="padding-bottom: 5px;">
                        <dx:ASPxComboBox ID="cboRejectType" runat="server" 
                            ClientInstanceName="cboRejectType">
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-bottom: 10px; width: 127px;">                      
                <dx:ASPxButton ID="btn" runat="server" AutoPostBack="False" 
                    ClientInstanceName="btn"
                        OnClick="btn_Click" Text="Submit" Width="91px" Height="30px">
                        <ClientSideEvents Click="function(s, e) {

}" />
                    </dx:ASPxButton>            
                    </td>
                    <td style="padding-bottom: 5px;">
                        <dx:ASPxLabel ID="lblStatus" runat="server" Text="Ready">
                </dx:ASPxLabel>
                    </td>
                </tr>
                 <tr>
                    <td colspan="2" valign="top"> 
                    <dx:ASPxLabel ID="lblTotal" runat="server" Text="Total Record(s): 0" 
                style="font-weight: 700">
                </dx:ASPxLabel>                     
                        <br />
                        <br />
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="QAGRID" 
                Width="705px">

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <Columns>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="1" FieldName="Barcode" 
                        Width="120px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" 
                        FieldName="CRN" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PurchaseOrder" VisibleIndex="3" 
                        Width="250px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Tag" VisibleIndex="4" Width="10px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="RejectTypeDesc" VisibleIndex="5" 
                        Width="10px" Caption="Reject Type">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="RejectTypeID" Visible="False" 
                        VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True" VisibleIndex="7">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="QAGRID" VisibleIndex="0">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>

                <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1">
                </SettingsEditing>

                <SettingsPopup>
                    <EditForm Modal="True" HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
                </SettingsPopup>
                <SettingsDataSecurity AllowDelete="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>

            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>

                    </td>
                </tr>
            </table>           
        <div style="float: left; width: 791px;">
&nbsp;</div>
        <div style="float: left; width: 789px;">
            
        </div>
        <div style="float: left;"></div>
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>