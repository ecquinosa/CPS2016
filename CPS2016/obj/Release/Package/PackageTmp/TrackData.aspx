<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="TrackData.aspx.vb" Inherits="CPS2016.TrackData" %>

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
    <div style="padding-left: 5px; padding-top: 10px; height: 93px;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="443px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent runat="server">
<div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
    <div style="width: 97px; float: left; height: 21px;">Search</div>
    <div id="divSearch" style="width: 218px; float: right;">
    <dx:ASPxComboBox ID="cboSearch" runat="server" ClientInstanceName="cboSearch" 
        NullText="select field to search" SelectedIndex="0" style="font-size: small" 
        Theme="Office2010Blue" Width="200px">
        <clientsideevents buttonclick="function(s, e) {
	cboSearch.SetText('');
	cboSearch.Focus();	
}" gotfocus="function(s, e) {
	var button = cboSearch.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" init="function(s, e) {
	var button = cboSearch.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" lostfocus="function(s, e) {
	var button = cboSearch.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" />
<ClientSideEvents ButtonClick="function(s, e) {
	cboSearch.SetText(&#39;&#39;);
	cboSearch.Focus();	
}" GotFocus="function(s, e) {
	var button = cboSearch.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = cboSearch.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = cboSearch.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
        <Items>
            <dx:ListEditItem Selected="True" Text="Purchase Order" Value="Purchase Order" />
            <dx:ListEditItem Text="Barcode" Value="Barcode" />
            <dx:ListEditItem Text="CRN" Value="CRN" />
        </Items>
        <buttons>
            <dx:EditButton>
                <image iconid="actions_cancel_16x16">
                </image>
            </dx:EditButton>
        </buttons>
    </dx:ASPxComboBox></div>
    </div>
    <div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 10px;">
                            <div style="width: 97px; float: left; height: 21px;">Value</div>
                            <div id="divValue" style="width: 218px; float: right;"><dx:ASPxButtonEdit ID="txtValue" runat="server" 
                                    ClientInstanceName="txtValue" NullText="enter value to search" Width="300px">
                                <clientsideevents buttonclick="function(s, e) {
	txtValue.SetText('');
	txtValue.Focus();	
}" gotfocus="function(s, e) {
	var button = txtValue.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" init="function(s, e) {
	var button = txtValue.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" lostfocus="function(s, e) {
	var button = txtValue.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" KeyDown="function(s, e) {
	DoProcessEnterKey(e.htmlEvent, '');
}" />
                                <buttons>
                                    <dx:EditButton>
                                        <image iconid="actions_cancel_16x16">
                                        </image>
                                    </dx:EditButton>
                                </buttons>
                                </dx:ASPxButtonEdit>
                                
                                </div>
                                
    </div>
    <div style="float: left; width: 416px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
        <dx:ASPxButton ID="btn" runat="server" AutoPostBack="False" 
            ClientInstanceName="btn" Height="20px" OnClick="btn_Click" Text="Submit" 
            Width="91px">
            <ClientSideEvents Click="function(s, e) {

}" />
        </dx:ASPxButton>
        <dx:ASPxLabel ID="lblStatus" runat="server">
        </dx:ASPxLabel>
    </div>
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>

            <br />

            <dx:ASPxGridView ID="xGridPO" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="RoleID" 
                Width="457px" Visible="False">

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <Columns>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="1" Caption="Activity" FieldName="ActivityDesc" ReadOnly="True" 
                        Width="300px">
                        <EditFormSettings Visible="True" RowSpan="1" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" VisibleIndex="2" 
                        FieldName="Quantity" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
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
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>

            <dx:ASPxGridView ID="xGridBarcodeCRN" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="RoleID" 
                Width="1200px" Visible="False">

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <Columns>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="0" FieldName="PurchaseOrder" Width="300px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="1" FieldName="Barcode" Width="110px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CRN" VisibleIndex="2" Width="60px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="3" 
                        Width="330px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="UserCompleteName" VisibleIndex="4" 
                        Caption="User" Width="230px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Posted" 
                        FieldName="DateTimePosted" VisibleIndex="5" Width="150px">
                        <HeaderStyle HorizontalAlign="Center" />
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
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>