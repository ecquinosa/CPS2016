<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="DownloadFile.aspx.vb" Inherits="CPS2016.DownloadFile" %>

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
            function ShowProcessingPanel() 
            {
                Callback.PerformCallback();
                ProcessingPanel.Show();
            }
    </script>
        </div>                       
        <div>
            <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All" Visible="False" />
    </div>
        <div style="padding-left: 5px; padding-top: 5px;">
        <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="DownloadFileID" Theme="Office2010Blue" 
                Width="1000px">
                <Columns>
                    <dx:GridViewCommandColumn ShowApplyFilterButton="True" 
                        ShowClearFilterButton="True" VisibleIndex="0">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" 
                        VisibleIndex="2" Width="500px">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/Time Posted" FieldName="DateTimePosted" 
                        VisibleIndex="4" Width="180px">
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Type" FieldName="Type" VisibleIndex="8" 
                        Width="150px">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn VisibleIndex="9" Caption=" ">
                        <DataItemTemplate>
                            <asp:linkbutton ID="LinkButton1" runat="server" Text="Download"
                                onclick="LinkButton1_Click" ForeColor="#FF6600" 
                        ></asp:linkbutton>
                        </DataItemTemplate>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="DFID" FieldName="DownloadFileID" VisibleIndex="1" 
                        Width="50px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="FilePath" FieldName="FilePath" 
                        Visible="False" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/Time Downloaded" 
                        FieldName="DateTimeExtracted" VisibleIndex="7" Width="180px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="DownloadCntr" FieldName="DownloadCntr" 
                        Visible="False" VisibleIndex="11">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                <SettingsPager PageSize="20">
                </SettingsPager>
                <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                </SettingsEditing>
                <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                <SettingsPopup>
                    <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                    <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                </SettingsPopup>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                    AllowInsert="False" />
            </dx:ASPxGridView>
            <br />

            <dx:ASPxLoadingPanel ID="ProcessingPanel" runat="server" ClientInstanceName="ProcessingPanel"
        Modal="True" Text="Processing&amp;hellip;" Theme="Office2010Blue">
    </dx:ASPxLoadingPanel>
             <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { ProcessingPanel.Hide(); }" />
    </dx:ASPxCallback>

        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>

        </div>  
</asp:Content>