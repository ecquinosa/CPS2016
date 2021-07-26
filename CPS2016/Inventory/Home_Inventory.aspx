<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Home_Inventory.aspx.vb" Inherits="CPS2016.Home_Inventory" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
       <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        </div> 
        <div id="divBody" style="padding-left: 5px; width: 100%;">
        <div id="divUpper" style="float: left; width: 100%; padding-top: 10px;">
            <div id="divUpperLeft" style="float: left; width: 60%;">
            <div style="font-size: small; padding-bottom: 5px;">
                <strong style="font-family: Arial, Helvetica, sans-serif">Inventory</strong></div>
            <dx:ASPxGridView ID="xGridInventory" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2010Blue" 
                    Width="755px" style="font-size: 9pt">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Material" FieldName="Material" 
                            VisibleIndex="2" Width="400px">
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Alltd.Qty." FieldName="AlltdQty" 
                            UnboundType="Boolean" VisibleIndex="7" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Beg.Qty." FieldName="BegQty" VisibleIndex="3" 
                            Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Usd.Qty." FieldName="UsdQty" 
                            VisibleIndex="8" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Spoiled Qty." FieldName="SpoiledQty" 
                            VisibleIndex="9" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="End Qty." FieldName="EndQty" 
                            VisibleIndex="10" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Added Qty." FieldName="AddedQty" 
                            VisibleIndex="11" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Final Balance" FieldName="FinalEndQty" 
                            VisibleIndex="12" Width="50px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
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
            <div id="divUpperRight" style="float: right; width: 40%;">
            <div style="font-size: small; padding-bottom: 5px;">
                <strong style="font-family: Arial, Helvetica, sans-serif">Processed Purchase 
                Order/s</strong></div>
            <dx:ASPxGridView 
                    ID="xGridProcessed" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2010Blue" 
                    Width="600px" style="font-size: 9pt">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Purchase Order(s)" FieldName="PurchaseOrders" 
                            UnboundType="Boolean" VisibleIndex="7" Width="600px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date/ Time Posted" 
                            FieldName="DateTimePosted" VisibleIndex="3" 
                            Width="140px">
                            <PropertiesTextEdit DisplayFormatString="MM/dd/yy hh:mm tt">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Processed By" FieldName="UserCompleteName" 
                            VisibleIndex="12" Width="150px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <SettingsPopup>
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                    </SettingsPopup>
                    <SettingsDataSecurity AllowDelete="False" 
                        AllowInsert="False" />
                </dx:ASPxGridView>

                &nbsp;
                </div>       
        </div>
        
        <div id="divLower" style="float: left; width: 100%; padding-top: 10px;">
        <div id="divLowerLeft" style="float: left; width: 53%;">
        <div style="font-size: small; padding-bottom: 5px;">
            <strong style="font-family: Arial, Helvetica, sans-serif">For Allotment</strong></div>
        <dx:ASPxGridView ID="xGridForAllotment" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="ID" Theme="Office2010Blue" 
                Width="600px">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="2" Width="400px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Posted" FieldName="DateTimePosted" 
                        VisibleIndex="4" Width="180px">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyy hh:mm tt">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="7" Width="30px">
                        <PropertiesTextEdit DisplayFormatString="{0:N0}">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
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
        <div id="div3" style="float: right:">&nbsp;</div>       
        </div>            
        </div>
        
                               
        
</asp:Content>