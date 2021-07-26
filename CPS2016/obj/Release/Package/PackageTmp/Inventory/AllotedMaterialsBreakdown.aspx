<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AllotedMaterialsBreakdown.aspx.vb" Inherits="CPS2016.AllotedMaterialsBreakdown" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" /></div>
    <div>
    
           <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    ClientInstanceName="xGrid" EnableTheming="True" KeyFieldName="MaterialID" 
                    Theme="Office2010Blue">
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />

<SettingsBehavior FilterRowMode="OnClick" ConfirmDelete="True"></SettingsBehavior>

                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                    <Columns>
                        <dx:GridViewCommandColumn ShowApplyFilterButton="True" 
                            ShowClearFilterButton="True" Visible="False" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="MaterialID" ReadOnly="True" 
                            VisibleIndex="1" Width="50px" Visible="False">
                            <PropertiesTextEdit ClientInstanceName="TxnID">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0"></EditFormSettings>

                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Material" FieldName="Material" 
                            VisibleIndex="2" Width="250px">
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4" />
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="5"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="5" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Alloted Quantity" FieldName="AlltdQty" 
                            VisibleIndex="3" Width="50px">
                            <EditFormSettings Visible="False" />
                            <PropertiesTextEdit NullText="-Select Item-">
                            </PropertiesTextEdit>
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="4" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="UsdQty" Width="110px" 
                            Caption="Used Quantity" VisibleIndex="4">

<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
    <HeaderStyle HorizontalAlign="Center" />
    <CellStyle HorizontalAlign="Center">
    </CellStyle>
</dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                            VisibleIndex="7">
                            <EditFormSettings Visible="False" />
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Spoiled Quantity" FieldName="SpoiledQty" 
                            VisibleIndex="6" Width="200px">
                            <EditFormSettings Visible="False" RowSpan="1" VisibleIndex="3" />
                            <PropertiesTextEdit ClientInstanceName="txtSubModule" Height="100px" 
                                NullText="enter Submodule">
                            </PropertiesTextEdit>
<Settings AutoFilterCondition="Contains"></Settings>

<EditFormSettings Visible="False"></EditFormSettings>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings Visible="False" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quantity" FieldName="PO_Quantity" 
                            VisibleIndex="9">
                            <PropertiesTextEdit ClientInstanceName="txtActivity" Height="100px" 
                                NullText="enter Activity">
                            </PropertiesTextEdit>
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="True" VisibleIndex="4" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date/ Time Posted" FieldName="DateTimePosted_PO" 
                            VisibleIndex="17">
                            <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <SettingsPager PageSize="1000">
                    </SettingsPager>

                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

                    <SettingsPopup>
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
<EditForm HorizontalAlign="Center" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter"></CustomizationWindow>
                    </SettingsPopup>
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
                        AllowInsert="False" />
                </dx:ASPxGridView>
    
    </div>
        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>
    </form>
</body>
</html>
