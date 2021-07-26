<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Inventory.aspx.vb" Inherits="CPS2016.Inventory" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>

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
        <div style="padding-left: 5px; padding-top: 5px;">
            <dx:ASPxGridView ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2010Blue" 
                    Width="900px" style="font-size: 9pt" KeyFieldName="MaterialID">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Material" FieldName="Material" 
                            VisibleIndex="2" Width="500px">
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
                         <dx:GridViewDataHyperLinkColumn VisibleIndex="13" Caption=" " Width="200px">
                        <DataItemTemplate>
                            <asp:linkbutton ID="LinkButton1" runat="server" Text="View Detail" OnClientClick="ShowProcessingPanel()" onclick="LinkButton1_Click" ForeColor="#FF6600"></asp:linkbutton>
                        </DataItemTemplate>
                    </dx:GridViewDataHyperLinkColumn>
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
            <br />
    <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid"></dx:ASPxGridViewExporter>

            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/Inventory/AllotedMaterialsBreakdown.aspx" EnableTheming="True" 
                HeaderText="Alloted Materials" ShowMaximizeButton="True" 
                Theme="Office2010Blue">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                    </dx:PopupControlContentControl>
                </ContentCollection>
                 </dx:ASPxPopupControl>
        </div>  
</asp:Content>