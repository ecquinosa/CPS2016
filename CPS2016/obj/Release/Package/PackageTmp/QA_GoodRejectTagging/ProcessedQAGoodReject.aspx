<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="ProcessedQAGoodReject.aspx.vb" Inherits="CPS2016.ProcessedQAGoodReject" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>
    <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" />
        </div>          
        <div style="padding-left: 5px; padding-top: 5px; height: 28px; padding-bottom: 5px;"><dx:ASPxButton ID="btnNew" runat="server" Text="New Process" Height="30px" 
                            Theme="Office2010Blue" Width="100px">
                        </dx:ASPxButton>
               
            <dx:ASPxButton ID="btnSearch" runat="server" Text="Search" Height="30px" 
                            Theme="Office2010Blue" Width="100px">
                        </dx:ASPxButton>
               
        </div>      
        <div style="padding-left: 5px; padding-top: 5px;">                      
            <dx:ASPxGridView 
                    ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2010Blue" 
                    Width="900px" style="font-size: 9pt" ClientInstanceName="xGrid" 
                KeyFieldName="DateTimePosted">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Total Quantity" FieldName="Processed" 
                            VisibleIndex="6" Width="150px" UnboundType="Boolean">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date/ Time Posted" 
                            FieldName="DateTimePosted" VisibleIndex="2" Width="200px">
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Processed By" 
                            FieldName="UserCompleteName" VisibleIndex="13" 
                            Width="200px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Good" 
                            VisibleIndex="11" Width="100px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Reject" VisibleIndex="12" Width="100px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn Caption=" " VisibleIndex="14">
                            <PropertiesHyperLinkEdit>                                
                            </PropertiesHyperLinkEdit>
                            <DataItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#FF6600" 
                                    onclick="LinkButton1_Click" Text="View"></asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
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

            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/Inventory/NewPOMatProcess.aspx" EnableTheming="True" 
                HeaderText="New Process" ShowMaximizeButton="True" Theme="Office2010Blue">
                 </dx:ASPxPopupControl>
        <dx:ASPxGridViewExporter ID="xGridExporter" runat="server" GridViewID="xGrid">
            </dx:ASPxGridViewExporter>
        </div>       
       
<%-- DXCOMMENT: Configure your datasource for ASPxGridView --%>

</asp:Content>