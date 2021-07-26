<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="IndigoExtract.aspx.vb" Inherits="CPS2016.IndigoExtract" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
       <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        
        
        <script type="text/javascript">
            function ShowProcessingPanel() 
            {
                Callback.PerformCallback();
                ProcessingPanel.Show();
            }

            function grid_SelectionChanged(s, e) {
                s.GetSelectedFieldValues("Quantity", GetSelectedFieldValuesCallback);
            }

            function GetSelectedFieldValuesCallback(values) {
                var total = 0;
                var divisibleBy21 = 0;
                var quotient = '';
                var fordrop = 0;                

                for (var i = 0; i < values.length; i++) {
                    total += parseInt(values[i]);
                }

                //total = 5;
                if (total == 0)
                { }
                else {
                    if (total < 21) {
                        if (document.forms[0].elements['<%=chkPrintAll.ClientID%>'].checked == true) {
                            divisibleBy21 = total;
                        }
                        else {
                            fordrop = total;
                        }
                    }
                    else {
                        if (document.getElementById('<%=chkPrintAll.ClientID%>').checked == true) {
                            divisibleBy21 = total;
                        }
                        else {
                            quotient = (total / 21).toString();
                            if (quotient.indexOf('.') > 0) {
                                var arr = quotient.split('.');
                                var divisor = arr[0];
                                divisibleBy21 = 21 * parseInt(divisor);
                                fordrop = total - divisibleBy21;
                            }
                            else {
                                divisibleBy21 = total;
                            }
                        }
                    }
                }

                document.getElementById('<%=Label1.ClientID%>').innerHTML = 'Total: ' + total.format() + '<br><br> Printable: ' + divisibleBy21.format() + ' For Drop: ' + fordrop.format();
                //document.getElementById('<%=Hidden1.ClientID%>').innerHTML = 'Total: ' + total.format() + '<br><br> Printable: ' + divisibleBy21.format() + ' For Drop: ' + fordrop.format();
                $('#<%= Hidden1.ClientID %>').value('Total: ' + total.format() + '<br><br> Printable: ' + divisibleBy21.format() + ' For Drop: ' + fordrop.format());
            }

            Number.prototype.format = function (n, x) {
                var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
                return this.toFixed(Math.max(0, ~ ~n)).replace(new RegExp(re, 'g'), '$&,');
            };
    </script>
        </div>                       
        <div style="padding-left: 5px; padding-top: 5px;">
        <div style="padding-left: 5px; padding-top: 5px; height: 36px; padding-bottom: 5px;">        
            <dx:ASPxButton ID="btnSubmit" runat="server" Height="30px" 
                Text="Submit" Width="100px" 
                Theme="Office2010Blue">
                <ClientSideEvents Click="function(s, e) {
	e.processOnServer = confirm('Are you sure you want to process selected PO and continue?');
    Callback.PerformCallback();
    ProcessingPanel.Show();
}" />
                
            </dx:ASPxButton>
&nbsp;<dx:ASPxButton ID="btnViewData" runat="server" Height="30px" ClientInstanceName="btnViewData" 
                Text="View Data" Width="100px" 
                Theme="Office2010Blue">
                
            </dx:ASPxButton>            
            &nbsp;<dx:ASPxLabel ID="lblStatus" runat="server">
                </dx:ASPxLabel>            
            </div>
            <div style="padding-bottom: 5px">
                <dx:ASPxLabel ID="lblStatus0" runat="server" Text="Select: ">
                </dx:ASPxLabel>            
            &nbsp;<asp:DropDownList ID="cboSelect" runat="server" Width="200px" AutoPostBack="True">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem Selected="True">Regular SSS</asp:ListItem>
                    <asp:ListItem>SSS UBP</asp:ListItem>
                </asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkPrintAll" runat="server" 
                    Text="  Print All (including drop)" />
            </div>
        <div style="float: left; padding-bottom: 10px;">
        <dx:ASPxGridView ID="xGrid" ClientInstanceName="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" 
                Width="815px" KeyFieldName="POID">                
                <ClientSideEvents SelectionChanged="function(s, e){grid_SelectionChanged(s,e);  }" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="2" Width="250px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Posted" FieldName="DateTimePosted" 
                        VisibleIndex="4" Width="170px">
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="7" Width="30px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Batch" FieldName="Batch" VisibleIndex="3" 
                        Width="80px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="POID" VisibleIndex="1" 
                        Width="60px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                        ShowSelectCheckbox="True" VisibleIndex="9" ShowClearFilterButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Type" FieldName="Type" VisibleIndex="8" 
                        Width="80px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DataType" Visible="False" VisibleIndex="10" Width="0px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                <SettingsPager Visible="False" Mode="ShowAllRecords" AlwaysShowPager="True">
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
            <br />
            &nbsp;<asp:Label ID="Label1" runat="server" Text="Total: 0" 
                style="font-size: small; font-weight: 700"></asp:Label>
            <input id="Hidden1" runat=server type="hidden" />
            </div>
            <br />
            <div style="float: left; visibility: hidden;">
                <strong>*Note: Order of data when extracted</strong><br />
            1. New Batches ONLY with multiple selection - sorted by ID<br />
            2. New Batches with Replacement - sorted by Replacement then New Batches (sorted by ID)
            </div>
            <div style="float: left;">
            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>

            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/IndigoDataForPrinting.aspx" EnableTheming="True" 
                HeaderText="FOR PRINTING" ShowMaximizeButton="True" Theme="Office2010Blue">
                 </dx:ASPxPopupControl>
            </div>
            <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
        </div>  
</asp:Content>