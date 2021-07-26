<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="CardControl.aspx.vb" Inherits="CPS2016.CardControl" %>

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
        <div style="float: left; width: 316px; position: relative; top: 0px; left: 0px; padding-bottom: 5px;">
                <div style="padding-left: 5px; padding-top: 5px; height: 364px; float: left;">
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Theme="Office2010Blue" 
                            Width="471px" HeaderText="">
                            <PanelCollection>
                            
<dx:PanelContent ID="PanelContent1" runat="server">
    <div style="float: left; width: 418px; position: relative; top: 0px; left: 0px; padding-bottom: 10px;">
                            <div style="width: 165px; float: left; height: 21px;">Barcode/ CRN/ BackOCR</div>
                            <div id="divValue" style="width: 218px; float: right;">
                                <dx:ASPxButtonEdit ID="txtParam" runat="server" 
                                    ClientInstanceName="txtValue" NullText="enter barcode or crn" 
                                    Width="250px">
                                <ClientSideEvents ButtonClick="function(s, e) {
	txtValue.SetText('');
	txtValue.Focus();	
}" GotFocus="function(s, e) {
	var button = txtValue.GetButton(0);
	button.style.visibility = &quot;&quot;;
}" Init="function(s, e) {
	var button = txtValue.GetButton(0);
    button.style.visibility = &quot;hidden&quot;;
}" LostFocus="function(s, e) {
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
            ClientInstanceName="btn" Height="30px" OnClick="btn_Click" Text="Submit" 
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
        <div id="divDV" style="padding-bottom: 5px; height: 311px; float: left;">
            <dx:ASPxDataView ID="xDataView" runat="server" 
                SettingsTableLayout-RowsPerPage="2" Width="850px" PagerAlign="Justify" 
                ItemSpacing="5px" ColumnCount="4" RowPerPage="1" Theme="Office2010Blue" 
                Height="257px" EnableTheming="False">
        <ItemTemplate>
            <div id="itCont" style="width: 654px">
            
                <table style="width:98%; font-size: 12px;">
                    <tr>
                        <td rowspan="11" valign="middle" style="width: 173px" align="left">
                            <dx:ASPxBinaryImage ID="ASPxBinaryImage1" runat="server" Height="200px" 
                                Value='<%# GetPhotoImage(DataBinder.Eval(Container.DataItem, "Barcode"))%>' Width="163px">
                            </dx:ASPxBinaryImage>
                            <dx:ASPxBinaryImage ID="ASPxBinaryImage2" runat="server" Height="57px" 
                                Value='<%# GetSignatureImage(DataBinder.Eval(Container.DataItem, "Barcode"))%>' Width="163px">
                            </dx:ASPxBinaryImage>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Purchase Order</td>
                        <td style="font-weight: 700">
                            <%# Eval("PurchaseOrder")%></td>
                    </tr>
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
                            <%# Eval("Barcode")%>
                            <div style="font-weight: bold; font-size: large; color: #FF9933"><%# CubaoBranchTag(DataBinder.Eval(Container.DataItem, "Barcode"))%></div>
                        </td>
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
                            Gender</td>
                        <td style="font-weight: 700">
                            <%# Eval("Sex")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Date of Birth</td>
                        <td style="font-weight: 700">
                            <%# Eval("DateOfBirth")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Address</td>
                        <td style="font-weight: 700">
                            <%# Eval("Address")%></td>
                    </tr>
                    <tr>
                        <td style="width: 110px">
                            Back OCR</td>
                        <td style="font-weight: 700">
                            <%# Eval("BackOCR")%>
                        </td>
                    </tr> 
                    <tr>
                        <td style="width: 110px">
                            Status</td>
                        <td style="font-weight: 700">
                            <%# GetActivityDesc(DataBinder.Eval(Container.DataItem, "ActivityID"))%>
                        </td>
                    </tr>                     
                </table>
            </div>
        </ItemTemplate>
<SettingsFlowLayout ItemsPerPage="2"></SettingsFlowLayout>

<SettingsTableLayout RowsPerPage="1" ColumnCount="2"></SettingsTableLayout>

        <PagerSettings ShowNumericButtons="true" Position="Top" Visible="False">
            <AllButton Visible="False" />
            <Summary Visible="false" />
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </PagerSettings>
    </dx:ASPxDataView>
    </div>   
        </div> 
        </div>
</asp:Content>