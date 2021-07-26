<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectPurchaseOrder.aspx.vb" Inherits="CPS2016.SelectPurchaseOrder" %>

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
    
    function grid_SelectionChanged(s, e) {
        s.GetSelectedFieldValues("Quantity", GetSelectedFieldValuesCallback);
    }

    function GetSelectedFieldValuesCallback(values) {
        var total = 0;
       
       for (var i = 0; i < values.length; i++) {
            total += parseInt(values[i]);
        }

        document.getElementById('<%=Label1.ClientID%>').innerHTML = 'Total: ' + total.format();
    }

    Number.prototype.format = function (n, x) {
        var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
        return this.toFixed(Math.max(0, ~ ~n)).replace(new RegExp(re, 'g'), '$&,');
    };
    </script>
    <form id="form1" runat="server">
    <div style="padding-bottom: 15px">
            <dx:ASPxButton ID="btnGenerate" runat="server" Height="30px" 
                Text="Generate" Width="100px" 
                Theme="Office2010Blue">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit process and continue?'); 
    Callback.PerformCallback();
    ProcessingPanel.Show();   
}" />
                
            </dx:ASPxButton>
        <asp:Label ID="lblStatus" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        
    &nbsp;
            <asp:LinkButton ID="LinkButton1" runat="server" Visible="False">Download</asp:LinkButton>
    </div>
    <div style="float: left; padding-bottom: 10px; width: 1059px;" id="divOutput" 
        runat="server">
    <div style="float:left; width: 109px; font-family: Arial, Helvetica, sans-serif;">
        Output Folder</div>
        <div style="float:left; width: 591px;">
            <dx:ASPxButtonEdit runat="server" NullText="enter executed by" 
            Width="300px" Height="20px" ClientInstanceName="txtExecutedBy" 
            ID="txtOutput">
<ClientSideEvents ButtonClick="function(s, e) {
	txtExecutedBy.SetText(&#39;&#39;);
	txtExecutedBy.Focus();	
}" GotFocus="function(s, e) {
	var button = txtExecutedBy.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtExecutedBy.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtExecutedBy.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

        </div>            
    </div>
    <div style="float: left; padding-top: 10px; width: 1061px;">
    
        <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="POID" Theme="Office2010Blue" 
                Width="728px" ClientInstanceName="xGrid">
            <ClientSideEvents SelectionChanged="function(s, e){grid_SelectionChanged(s,e);  }" />
            <Columns>
                <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True" VisibleIndex="7" 
                    ShowClearFilterButton="True">
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="1" Width="400px">
                    <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Date/ Time Posted" FieldName="DateTimePosted" 
                        VisibleIndex="2" Width="170px">
                    <EditFormSettings RowSpan="1" VisibleIndex="1" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="3" Width="30px">
                    <PropertiesTextEdit DisplayFormatString="{0:N0}">
                    </PropertiesTextEdit>
                    <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Right">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="POID" FieldName="POID" Visible="False" 
                        VisibleIndex="8">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
            </SettingsEditing>
            <Settings ShowFilterRow="True" />
            <SettingsPopup>
                <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
            </SettingsPopup>
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
        </dx:ASPxGridView>
        
    </div>
    <div style="float: left; padding-top: 10px; padding-bottom: 10px;">
        <asp:Label ID="Label1" runat="server" Text="Total: 0" 
                
            
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif;" 
            Visible="False"></asp:Label>
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
