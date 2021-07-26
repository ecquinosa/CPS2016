<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Inspection_QC_YieldReportForm.aspx.vb" Inherits="CPS2016.Inspection_QC_YieldReportForm" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>
<script type="text/javascript">
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
}" />
                
            </dx:ASPxButton>
        <asp:Label ID="lblStatus" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        
    </div>
    <div style="padding-bottom: 5px">
    <div style="float:left; width: 109px; font-family: Arial, Helvetica, sans-serif;">Report Date</div>
        <div style="float:left; width: 178px;">
        <dx:ASPxDateEdit ID="dateEdit" ClientInstanceName="dateEdit" runat="server" 
                DisplayFormatString="MM/dd/yyyy">
        <ClientSideEvents ButtonClick="function(s, e) {
	dateEdit.SetText(&#39;&#39;);
	dateEdit.Focus();	
}" GotFocus="function(s, e) {
	var button = dateEdit.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = dateEdit.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = dateEdit.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
            <Buttons>
                <dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
            </Buttons>
            </dx:ASPxDateEdit></div><div style="float:left;"></div>            
            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Submit">
            </dx:ASPxButton>
    </div>
    <div style="padding-top: 10px">
    
        <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="POID" Theme="Office2010Blue" 
                Width="728px" ClientInstanceName="xGrid">
            <ClientSideEvents SelectionChanged="function(s, e){grid_SelectionChanged(s,e);  }" />
            <Columns>
                <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True" VisibleIndex="7">
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="1" Width="400px">
                    <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Date/ Time Posted" FieldName="DateTimePosted" 
                        VisibleIndex="3" Width="170px">
                    <EditFormSettings RowSpan="1" VisibleIndex="1" />
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="6" Width="30px">
                    <PropertiesTextEdit DisplayFormatString="{0:N0}">
                    </PropertiesTextEdit>
                    <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Right">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="POID" FieldName="POID" Visible="False" 
                        VisibleIndex="8">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Batch" Visible="False" VisibleIndex="9">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
            </SettingsEditing>
            <SettingsPopup>
                <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
            </SettingsPopup>
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
        </dx:ASPxGridView>
        
    </div>
    <div style="padding-top: 10px; padding-bottom: 10px;">
        <asp:Label ID="Label1" runat="server" Text="Total: 0" 
                
            
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif;" 
            Visible="False"></asp:Label>
    </div>
    <div style="padding-top: 10px; padding-bottom: 10px; background-color: #999999;" 
        id="divCertificateHeader" runat="server">
        <asp:Label ID="Label4" runat="server" Text="CERTIFICATE OF DESTRUCTION PORTION" 
                
            
            
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif; color: #FFFFCC;"></asp:Label>

    </div>
    <div style="padding-top: 10px;" id="divExecutedBy"  runat="server">
        <asp:Label ID="Label2" runat="server" Text="Executed By:" 
                
            
            style="font-size: small; font-weight: 700; font-family: Arial, Helvetica, sans-serif;" 
            Visible="False" Font-Size="Smaller"></asp:Label>
            <dx:ASPxButtonEdit runat="server" NullText="enter executed by" 
            Width="300px" Height="20px" ClientInstanceName="txtExecutedBy" 
            ID="txtExecutedBy">
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
    <div style="padding-top: 10px;" id="divWitnessedBy"  runat="server">
        <asp:Label ID="Label3" runat="server" Text="Witnessed By:" 
                
            
            style="font-size: small; font-weight: 700; font-family: Arial, Helvetica, sans-serif;" 
            Visible="False"></asp:Label>
            <dx:ASPxButtonEdit runat="server" NullText="enter witnessed by" 
            Width="300px" Height="20px" ClientInstanceName="txtWitnessedBy" 
            ID="txtWitnessedBy">
<ClientSideEvents ButtonClick="function(s, e) {
	txtWitnessedBy.SetText(&#39;&#39;);
	txtWitnessedBy.Focus();	
}" GotFocus="function(s, e) {
	var button = txtWitnessedBy.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" LostFocus="function(s, e) {
	var button = txtWitnessedBy.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" Init="function(s, e) {
	var button = txtWitnessedBy.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}"></ClientSideEvents>
<Buttons>
<dx:EditButton>
<Image IconID="actions_cancel_16x16"></Image>
</dx:EditButton>
</Buttons>
</dx:ASPxButtonEdit>

    </div>
    </form>
</body>
</html>
