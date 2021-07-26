<%@ Page Language="VB" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Home_CPS.aspx.vb" Inherits="CPS2016.Home_CPS" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallback" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
       <div style="height: 19px; background-color: #D2DCE3; padding-top: 2px; padding-left: 2px; padding-bottom: 2px;">
        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/refresh.png" Width="16px" />
            <asp:ImageButton ID="ImageButton2" runat="server" Height="16px" 
            ImageUrl="~/Content/Images/xls.png" Width="16px" Visible="False" />
        </div> 
        <div id="divBody" style="padding-left: 5px">
        <div id="divOpenBatch" 
                style="float: left; padding-top: 5px; padding-bottom: 15px; width: 1450px;">
        <div style="font-size: small; padding-bottom: 5px;">
            <strong style="font-family: Arial, Helvetica, sans-serif">Open Batch/es</strong><asp:Button 
                ID="Button1" runat="server" Text="Button" Visible="False" />
            <asp:LinkButton ID="lbOpenBatchRefresh" runat="server">Refresh</asp:LinkButton>
            <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="TextBox3" runat="server" Visible="False"></asp:TextBox>
            </div>
        <dx:ASPxGridView ID="xGridOpenBatch" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" KeyFieldName="ID" Theme="Office2010Blue" 
                Width="1042px" style="font-size: 9pt" Font-Size="9pt">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="2" Width="300px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Uploaded" FieldName="DateTimePosted" 
                        VisibleIndex="6" Width="150px">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yy hh:mm tt">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" VisibleIndex="1" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="4" Width="30px">
                        <PropertiesTextEdit DisplayFormatString="{0:N0}">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Batch" FieldName="Batch" VisibleIndex="3" 
                        Width="50px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Date/ Time Extracted" 
                        FieldName="DateTimeExtracted" VisibleIndex="7" Width="150px">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yy hh:mm tt">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " FieldName="Remark" VisibleIndex="9" 
                        Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="8" 
                        Width="500px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="SSS-UBP Qty" FieldName="SSSUBP_Qty" Name="SSSUBP_Qty" VisibleIndex="5" Width="30px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" FilterRowMode="OnClick" />
                <SettingsPager AlwaysShowPager="True" PageSize="100">
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

        <div id="divLower" style="float: left; width: 100%;">
        <div id="divLowerLeft" style="float: left; width: 40%; visibility: hidden;">
        <div style="font-size: small; padding-bottom: 5px;">
            <strong style="font-family: Arial, Helvetica, sans-serif">Today's For Delivery&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lbForDelivery" runat="server">Refresh</asp:LinkButton>
            </strong></div>
        <dx:ASPxGridView ID="xGridForDelivery" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" 
                Width="615px" style="font-size: 9pt">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Purchase Order" FieldName="PurchaseOrder" 
                        VisibleIndex="2" Width="400px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="7" Width="50px">
                        <PropertiesTextEdit DisplayFormatString="{0:N0}">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Batch" FieldName="Batch" VisibleIndex="3" 
                        Width="80px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center">
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
        <div id="divLowerRight" style="float: right:">
        <div style="font-size: small; padding-bottom: 5px;">
            <strong style="font-family: Arial, Helvetica, sans-serif">Quantity Per Status</strong> 
            <em><span style="font-size: x-small">(last updated 
            <asp:Label
                ID="lbStatusCounterTimestamp" runat="server" Text="Label"></asp:Label></span>)
            <asp:LinkButton ID="lbStatusCounter" runat="server">Refresh</asp:LinkButton>
            </div>
        <dx:ASPxGridView ID="xGridStatus" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" 
                Width="494px" style="font-size: 9pt">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Status" FieldName="ActivityDesc" 
                        VisibleIndex="2" Width="250px">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" 
                        UnboundType="Boolean" VisibleIndex="6" Width="10px">
                        <PropertiesTextEdit DisplayFormatString="{0:N0}">
                        </PropertiesTextEdit>
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="6" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Right">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ActivityID" FieldName="ActivityID" 
                        VisibleIndex="3" Width="20px" Visible="false">
                        <EditFormSettings RowSpan="1" Visible="False" VisibleIndex="0" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="7" Width="150px">
                        <DataItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#FF6600" onclick="LinkButton1_Click">View By PO</asp:LinkButton>                            
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption=" " VisibleIndex="8" Width="150px">
                        <DataItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server" ForeColor="#FF6600" onclick="LinkButton2_Click">View By Card</asp:LinkButton>
                        </DataItemTemplate>
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
            &nbsp;</div>  
             <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
                AllowResize="True" ContentUrl="~/StatusCardByPO.aspx" EnableTheming="True" 
                HeaderText="Cards" ShowMaximizeButton="True" 
                Theme="Office2010Blue">
                 </dx:ASPxPopupControl>     
        </div>
        
        </div>
        
                               
        
</asp:Content>