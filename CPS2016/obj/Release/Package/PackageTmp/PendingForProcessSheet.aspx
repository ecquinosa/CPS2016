<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="PendingForProcessSheet.aspx.vb" Inherits="CPS2016.PendingForProcessSheet" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxDataView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function DoProcessEnterKey(htmlEvent, editName) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
                if (editName) {
                    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                } else {
                    btn.DoClick();
                    //var front = document.getElementById('<%=txtFrontSheet.ClientID%>').value;
                    //var back = document.getElementById('<%=txtBackSheet.ClientID%>').value;
                    //if (front==back) {
                    //document.getElementById('<%=Label1.ClientID%>').innerHTML = 'MATCHED';
                //} else {
                    //document.getElementById('<%=Label1.ClientID%>').innerHTML = 'NOT MATCHED';
                //}
                    txtFrontSheet.Focus();
                }
            }
        }

        function DoProcessEnterKeyFrontSheet(htmlEvent, editName) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
                if (editName) {
                    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                } else {
                    txtBackSheet.Focus();
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
        <div style="padding-bottom: 5px; float: left; padding-top: 5px; padding-left: 5px;">
        <div style="float: left;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="523px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent runat="server">
<div style="float: left; width: 510px; position: relative; top: 0px; left: 0px; padding-bottom: 15px;">
    <dx:ASPxButton ID="btnSubmitCards" runat="server" BackColor="#339933" 
        ForeColor="#CCFFFF" Height="30px" Text="Submit Card/s" Theme="Youthful" 
        Width="100px">
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
    <dx:ASPxLabel ID="lblTotal" runat="server" style="font-weight: 700" 
        Text="Total Record(s): 0">
    </dx:ASPxLabel>
    </div>
    <div style="float: left; width: 507px; position: relative; top: 0px; left: 0px; padding-bottom: 15px;">
                            <div style="width: 95px; float: left; height: 21px;">Front Sheet</div>
                            <div id="divSearch0" style="width: 410px; float: right;">
                                <dx:ASPxButtonEdit ID="txtFrontSheet" runat="server" ClientInstanceName="txtFrontSheet" 
                                    Height="20px" NullText="enter/ scan front sheet" Width="300px">
                                    <ClientSideEvents ButtonClick="function(s, e) {
	txtFrontSheet.SetText('');
	txtFrontSheet.Focus();	
}" GotFocus="function(s, e) {
	var button = txtFrontSheet.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtFrontSheet.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtFrontSheet.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" KeyDown="function(s, e) {	
    DoProcessEnterKeyFrontSheet(e.htmlEvent, '');
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
    <div style="float: left; width: 507px; position: relative; top: 0px; left: 0px; padding-bottom: 15px;">
                            <div style="width: 95px; float: left; height: 21px;">Back Sheet</div>
                            <div id="div1" style="width: 410px; float: right;">
                                <dx:ASPxButtonEdit ID="txtBackSheet" runat="server" ClientInstanceName="txtBackSheet" 
                                    Height="20px" NullText="enter/ scan back sheet" Width="300px">
                                    <ClientSideEvents ButtonClick="function(s, e) {
	txtBackSheet.SetText('');
	txtBackSheet.Focus();	
}" GotFocus="function(s, e) {
	var button = txtBackSheet.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtBackSheet.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
	var button = txtBackSheet.GetButton(0);
	button.style.visibility = &quot;hidden&quot;;
}" KeyDown="function(s, e) {
    DoProcessEnterKey(e.htmlEvent, '');	
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
    <div style="float: left; width: 511px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
        
        <dx:ASPxButton ID="btn" runat="server" AutoPostBack="False" 
            ClientInstanceName="btn" Height="20px" OnClick="btn_Click" Text="Submit" 
            Width="91px">
            <ClientSideEvents Click="function(s, e) {

}" />
        </dx:ASPxButton>
        <dx:ASPxLabel ID="lblStatus" runat="server" Text="Ready">
        </dx:ASPxLabel>
    </div>
    
                                </dx:PanelContent>
</PanelCollection>
                        </dx:ASPxRoundPanel>
                        </div>
                        <div style="float: right; padding-left: 15px; padding-top: 10px;">
                            <asp:Label ID="Label1" runat="server" style="font-size: xx-large"></asp:Label></div>
        </div>
        <div style="padding-left: 5px; padding-top: 5px;">
        <div id="divDV" style="padding-bottom: 5px; float: left;">
            <dx:ASPxDataView ID="xDataView" runat="server" 
                SettingsTableLayout-RowsPerPage="2" Width="850px" PagerAlign="Justify" 
                ItemSpacing="5px" ColumnCount="4" RowPerPage="1" Theme="Office2010Blue" 
                Height="190px" Visible="False" EnableTheming="False">
        <ItemTemplate>
            <div id="itCont" style="width: 439px">
            
                <table style="width:98%; font-size: 12px;">
                    <tr>
                        <td rowspan="7" valign="middle" style="width: 126px" align="left">
                            <dx:ASPxBinaryImage ID="ASPxBinaryImage1" runat="server" Height="140px" 
                                Value='<%# GetPhotoImage(DataBinder.Eval(Container.DataItem, "Barcode"))%>' Width="122px">
                            </dx:ASPxBinaryImage>
                        </td></tr>
                    <tr>
                        <td style="width: 110px">
                            Batch</td>
                        <td style="font-weight: 700">
                            <%# Eval("Batch")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Barcode</td>
                        <td style="font-weight: 700">
                            <%# Eval("Barcode")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            CRN</td>
                        <td style="font-weight: 700">
                            <%# Eval("CRN")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Member&#39;s Name</td>
                        <td style="font-weight: 700"><%# GetFullName(DataBinder.Eval(Container.DataItem, "FName"), DataBinder.Eval(Container.DataItem, "MName"), DataBinder.Eval(Container.DataItem, "LName"))%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Back OCR</td>
                        <td style="font-weight: 700">
                            <%# Eval("BackOCR")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Page <b><%# Eval("CurrentPage")%></b></td>
                        <td>
                            Series <b><%# Eval("CurrentSeries")%></b></td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
<SettingsFlowLayout ItemsPerPage="4"></SettingsFlowLayout>

<SettingsTableLayout RowsPerPage="1" ColumnCount="4"></SettingsTableLayout>

        <PagerSettings ShowNumericButtons="true" Position="Top" Visible="False">
            <AllButton Visible="False" />
            <Summary Visible="false" />
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </PagerSettings>
    </dx:ASPxDataView>
    </div>
    <div id="divGrid" style="float: left;">
           <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    ClientInstanceName="xGrid" EnableTheming="True" KeyFieldName="CardID" 
                    Theme="Office2010Blue">
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <Settings ShowFilterRow="True" />
                    <Columns>
                        <dx:GridViewCommandColumn 
                            ShowClearFilterButton="True" VisibleIndex="21" 
                            SelectAllCheckboxMode="Page" ShowSelectCheckbox="True">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="CardID" ReadOnly="True" 
                            VisibleIndex="0" Width="50px">
                            <PropertiesTextEdit ClientInstanceName="TxnID">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Series" FieldName="CurrentSeries" 
                            UnboundType="Boolean" VisibleIndex="19" Width="50px">
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="5" />
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="5" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Batch" Width="50px" 
                            Caption="Batch" VisibleIndex="2">
<PropertiesTextEdit NullText="-Select Item-"></PropertiesTextEdit>

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
    <HeaderStyle HorizontalAlign="Center" />
    <CellStyle HorizontalAlign="Center">
    </CellStyle>
</dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CRN" FieldName="CRN" VisibleIndex="3" 
                            Width="110px">
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="First" FieldName="FName" VisibleIndex="6">
                            <EditFormSettings Visible="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Back OCR" FieldName="BackOCR" 
                            VisibleIndex="17" Width="80px">
                            <PropertiesTextEdit ClientInstanceName="txtStartTime">
                                <MaskSettings Mask="hh:mm tt" />
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Page" FieldName="CurrentPage" 
                            VisibleIndex="18" Width="50px">
                            <PropertiesTextEdit ClientInstanceName="txtEndTime">
                                <MaskSettings Mask="hh:mm tt" />
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Center">
                            </EditCellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Barcode" FieldName="Barcode" 
                            VisibleIndex="5" Width="200px">
                            <EditFormSettings Visible="False" />
                            <PropertiesTextEdit ClientInstanceName="txtSubModule" Height="100px" 
                                nulltext="enter Submodule">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" VisibleIndex="3" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Middle" FieldName="MName" VisibleIndex="8">
                            <PropertiesTextEdit ClientInstanceName="txtActivity" Height="100px" 
                                NullText="enter Activity">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="True" VisibleIndex="4" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last" FieldName="LName" VisibleIndex="16">
                            <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="ActivityDesc" 
                            VisibleIndex="20" Width="100px">
                            <HeaderStyle HorizontalAlign="Left" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowGroupPanel="False" />
                    <SettingsPopup>
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
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

                </div>
        </div>     
</asp:Content>