<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewPOMatProcess.aspx.vb" Inherits="CPS2016.NewPOMatProcess" %>

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
    <div style="padding-bottom: 10px">
            <dx:ASPxButton ID="btnSubmit" runat="server" Height="30px" 
                Text="Submit" Width="100px" 
                Theme="Office2010Blue">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to submit process and continue?');    
}" />
                
            </dx:ASPxButton>
        <asp:Label ID="lblResult" runat="server" 
            style="font-family: Tahoma" Font-Size="Small"></asp:Label>        
    </div>
    <div>
    
        <asp:Label ID="Label2" runat="server" Text="Available Materials" 
                
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif;"></asp:Label>
    
            <dx:ASPxGridView ID="xGridMaterial" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="MaterialID" 
                Width="501px">
                <Columns>
                    <dx:GridViewCommandColumn 
                        VisibleIndex="6" Width="10px" ShowEditButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataButtonEditColumn Caption="Material" FieldName="Material" 
                        VisibleIndex="1" Width="200px" ReadOnly="True">
                        <PropertiesButtonEdit ClientInstanceName="txtUsername" Width="200px">
                        

                            
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

                        

                            
</PropertiesButtonEdit>
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />

<EditFormSettings RowSpan="1" VisibleIndex="1"></EditFormSettings>
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataButtonEditColumn>
                    <dx:GridViewDataTextColumn 
                        VisibleIndex="0" Caption="ID" FieldName="MaterialID" ReadOnly="True" 
                        Width="20px">
<EditFormSettings RowSpan="1" VisibleIndex="0"></EditFormSettings>

                        <PropertiesTextEdit Width="20px">
                            
                            <Style HorizontalAlign="Center">
                            </Style>
                            
                        </PropertiesTextEdit>
                        <EditFormSettings Visible="False" RowSpan="1" VisibleIndex="0" />

                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Balance" FieldName="FinalEndQty" 
                        ReadOnly="True" VisibleIndex="2" Width="120px">
                        <PropertiesTextEdit DisplayFormatString="{0:N0}" DisplayFormatInEditMode="True" 
                            Width="120px">
                            
                            <Style HorizontalAlign="Right">
                            </Style>
                            
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Alloted" FieldName="AlltdQty" 
                        VisibleIndex="3" Width="100px">
                        <PropertiesTextEdit DisplayFormatInEditMode="True" DisplayFormatString="{0:N0}" 
                            Width="100px">
                            
                            <Style HorizontalAlign="Right">
                            </Style>
                            
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Used" FieldName="UsdQty" VisibleIndex="4" 
                        Width="100px">
                        <PropertiesTextEdit DisplayFormatInEditMode="True" DisplayFormatString="{0:N0}" 
                            Width="100px">
                            
                            <Style HorizontalAlign="Right">
                            </Style>
                            
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Spoiled" FieldName="SpoiledQty" 
                        VisibleIndex="5" Width="100px">
                        <PropertiesTextEdit DisplayFormatInEditMode="True" DisplayFormatString="{0:N0}" 
                            Width="100px">
                            
                            <Style HorizontalAlign="Right">
                            </Style>
                            
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>

                <SettingsEditing Mode="Inline" EditFormColumnCount="1">
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
            <br />
        <asp:Label ID="Label3" runat="server" Text="Purchase Order(s) for Allotment" 
                
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif;"></asp:Label>
            <br />    
        <dx:ASPxGridView ID="xGridForAllotment" runat="server" AutoGenerateColumns="False" 
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
    <div style="padding-top: 10px;">
        <asp:Label ID="Label1" runat="server" Text="Total: 0" 
                
            style="font-size: medium; font-weight: 700; font-family: Arial, Helvetica, sans-serif;"></asp:Label>
    </div>
    </form>
</body>
</html>
