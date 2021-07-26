<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="ProcessedPOMat.aspx.vb" Inherits="CPS2016.ProcessedPOMat" %>

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
               
        </div>      
        <div style="padding-left: 5px; padding-top: 5px;">                      
            <dx:ASPxGridView 
                    ID="xGrid" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2010Blue" 
                    Width="1000px" style="font-size: 9pt">
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="1" ShowApplyFilterButton="True" 
                            ShowClearFilterButton="True">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="POMaterialID" 
                            VisibleIndex="2" Width="50px">
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Purchase Order(s)" FieldName="PurchaseOrders" 
                            UnboundType="Boolean" VisibleIndex="7" Width="500px">
                            <Settings AutoFilterCondition="Contains" />
                            <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Date Posted" 
                            FieldName="DateTimePosted" VisibleIndex="3" 
                            Width="150px">
                            <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Processed By" FieldName="UserCompleteName" 
                            VisibleIndex="11" Width="150px">
                            <PropertiesTextEdit DisplayFormatString="{0:N0}">
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>
                         <dx:GridViewDataHyperLinkColumn VisibleIndex="13" Caption=" ">
                             <PropertiesHyperLinkEdit>                                 
                             </PropertiesHyperLinkEdit>                                                
                        <DataItemTemplate>
                            <asp:linkbutton ID="LinkButton1" runat="server" Text="Cancel"
                                onclick="LinkButton1_Click" OnClientClick="return confirm('Are you sure you want to cancel and continue?');" ForeColor="#FF6600" 
                        ></asp:linkbutton>
                        </DataItemTemplate>
                    </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataHyperLinkColumn Caption=" " VisibleIndex="12">
                            <DataItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" ForeColor="#FF6600" 
                                    onclick="LinkButton2_Click"                                     
                                    Text="View"></asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <SettingsPopup>
                        <EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" />
                        <CustomizationWindow HorizontalAlign="Center" VerticalAlign="WindowCenter" />
                    </SettingsPopup>
                    <SettingsDataSecurity AllowDelete="False" 
                        AllowInsert="False" AllowEdit="False" />
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