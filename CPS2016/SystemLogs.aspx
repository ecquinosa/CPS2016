<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="SystemLogs.aspx.vb" Inherits="CPS2016.SystemLogs" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        </div>                        
        <div style="padding-left: 5px; padding-top: 5px;">
        <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="ID" Theme="Office2010Blue" 
                Width="1115px">
                <Columns>
                    <dx:GridViewCommandColumn ShowApplyFilterButton="True" 
                        ShowClearFilterButton="True" VisibleIndex="0">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="SystemLogID" 
                        VisibleIndex="1" Width="80px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Description" FieldName="SystemLogDesc" 
                        VisibleIndex="2" Width="500px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Process" FieldName="Process" 
                        VisibleIndex="3" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date Created" FieldName="DateTimePosted" 
                        VisibleIndex="4" Width="160px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Submitted By" FieldName="UserCompleteName" 
                        VisibleIndex="5" Width="150px">
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                <SettingsPager PageSize="15">
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
            <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
        </div>  
</asp:Content>