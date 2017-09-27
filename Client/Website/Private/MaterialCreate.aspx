<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialCreate.aspx.cs" Inherits="Electracraft.Client.Website.Private.MaterialCreate" MasterPageFile="~/Private/PrivatePage.Master" %>

<%--<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>--%>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Create and assign materials   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    <asp:GridView ID="gvVehicle"  runat="server" AutoGenerateColumns="false" CssClass = "Grid" OnPreRender="gvVehicle_PreRender"
                                ShowFooter = "true" allowpaging="false" DataKeyNames="MaterialName, CostPrice, SellPrice,
                                UOM, MaterialID, SupplierName, MaterialCategoryName"> 
                    <Columns>
                              <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                    <HeaderTemplate>
                                        <asp:Button ID="btnSelect" Text="Select All/None" runat="server"  OnClick="btnVehicleSelect_Click"/><br />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="false"
                                        AutoPostBack="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                                    

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQtyHeader" Text="Total" runat="server" />
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyBody" Text="" runat="server" forecolor="Blue"/>
                                    </ItemTemplate>
                                            
                                 </asp:TemplateField>   


                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQtyToAssignHeader" Text="Quantity to assign" runat="server" />
                                        <asp:Button runat="server" ID="btnAddToTask" onclick="btnAddFromVehicleToTask_Click" text="Add materials to this task"/>
                                               
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                            <asp:TextBox  ID="tbAdd" runat="server" OnTextChanged="TextBox2_TextChanged" ForeColor="Blue"  onkeydown="return checkIfKeyIsNumber(event)"></asp:TextBox>  
                                    </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button runat="server" ID="btnAddToTask" onclick="btnAddFromVehicleToTask_Click" text="Add materials to this task"/> 
                                        </FooterTemplate>
                                    </asp:TemplateField> 
            
              
                                    <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMNameHeader" Text="Material" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblMNameBody" runat="server" forecolor="Blue"/>
                                            </ItemTemplate>
                                            
                                    </asp:TemplateField>   
                                    <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCostHeader" Text="Cost price" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblCostBody" Text="" runat="server" forecolor="Blue"/>
                                            </ItemTemplate>
                                            
                                    </asp:TemplateField>   
                                    <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblSellHeader" Text="Sell Price" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblSellBody" Text="" runat="server" forecolor="Blue"/>
                                            </ItemTemplate>
                                            
                                    </asp:TemplateField>   
                                    <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRRPHeader" Text="RRP" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblRRPBody" Text="" runat="server" forecolor="Blue"/>
                                            </ItemTemplate>
                                            
                                    </asp:TemplateField>   
                                     <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblSupplierHeader" Text="Supplier" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierBody" Text="old text"  runat="server"  forecolor="Blue" />
                                                 
                                            </ItemTemplate>
                                          
                                    </asp:TemplateField>   
                          <asp:BoundField datafield="supplierinvoiceid" HeaderText="supplierinvoiceid" />
                    </Columns>
        </asp:GridView>




</asp:Content>
