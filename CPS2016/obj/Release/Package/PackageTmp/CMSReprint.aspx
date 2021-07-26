<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="CMSReprint.aspx.vb" Inherits="CPS2016.CMSReprint" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
       <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
                
        <script type="text/javascript">
            function ShowProcessingPanel(msg) {
                var result = confirm(msg);
                if (result) {
                    Callback.PerformCallback();
                    ProcessingPanel.Show();
                }
            }
    </script>
        </div>                       
        <div style="padding-left: 5px; padding-top: 5px;">
        <div style="padding-bottom: 10px">
            <dx:ASPxButton runat="server" Text="New Upload" Theme="Office2010Blue" 
                Width="50px" Height="30px" ID="btnNewUpload"></dx:ASPxButton>
            </div>
        <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="ID" Theme="Office2010Blue" 
                Width="1300px">
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Source File" FieldName="SourceFile" 
                        VisibleIndex="2" Width="300px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date Posted" FieldName="DateTimePosted" 
                        VisibleIndex="7" Width="170px">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn VisibleIndex="12" Caption=" " Visible="False">
                        <DataItemTemplate>
                            <asp:linkbutton ID="LinkButton1" runat="server" Text="Upload" OnClientClick="ShowProcessingPanel('Are you sure you want to upload PO?')" onclick="LinkButton1_Click" ForeColor="#FF6600"></asp:linkbutton>
                        </DataItemTemplate>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn VisibleIndex="13" Caption=" " Visible="False">
                        <DataItemTemplate>
                            <asp:linkbutton ID="LinkButton2" runat="server" Text="Delete" OnClientClick="ShowProcessingPanel('Are you sure you want to delete PO?')" onclick="LinkButton2_Click" ForeColor="#FF6600"></asp:linkbutton>
                        </DataItemTemplate>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="4" Width="400px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="PO Qty." FieldName="PO_Qty" 
                        VisibleIndex="5">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="PO Reprint" FieldName="PO_Reprint" 
                        VisibleIndex="6">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" 
                        VisibleIndex="11" Width="300px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
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
        <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="250px" Width="800px" 
                Visible="False">
            <Border BorderStyle="None" />
            </dx:ASPxMemo>

            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue" Height="38px" 
                Width="121px">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>
    <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
        </div>  
</asp:Content>