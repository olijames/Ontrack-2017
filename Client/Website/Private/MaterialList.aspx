<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="MaterialList.aspx.cs" Inherits="Electracraft.Client.Website.Private.MaterialList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Material List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h2>
                Material List</h2>
                <h4><asp:Literal ID="litContactName" runat="server"></asp:Literal></h4>
       </div>
    </div>
     <div class="row">
        <div class="small-12 columns">
            <asp:Button ID="btnAddMaterial" runat="server" Text="Add New Material" OnClick="btnAddMaterial_Click" />
            <asp:Button ID="btnCategories" runat="server" Text="Manage Material Categories" OnClick="btnCategory_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
             <asp:DropDownList ID="ddVehicleSelect" runat="server" AutoPostBack="true"></asp:DropDownList> <!--Jared-->
            <asp:Button ID="btnAddToContainer" runat="server" Text="Add material(s) to container" OnClick="btnAddToContainer_Click" visible="false"/>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
            
           


            <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false"  OnPreRender="gvMaterials_PreRender" DataKeyNames="MaterialID">
                <Columns>


                    <asp:BoundField HeaderText="MaterialName" DataField="MaterialName" />
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <%# GetCategoryName((Guid)Eval("MaterialCategoryID")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Cost Price" DataField="CostPrice" />
                    <asp:BoundField HeaderText="Sell Price" DataField="SellPrice" />
                    <asp:BoundField HeaderText="Description" DataField="Description" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditMaterial" CommandArgument='<%# Eval("MaterialID").ToString() %>'
                                OnClick="btnEdit_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField>
                          <Headertemplate>
                                <asp:Label runat="server" Text="Add to container"></asp:Label>
                               
                        </Headertemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtAdd" height="10px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                      
                    <asp:TemplateField>
                          <Headertemplate>
                                <asp:Label id="lblVehicleDriverName" runat="server" Text="In vehicle"></asp:Label>
                               
                        </Headertemplate>
                        <ItemTemplate>
                            <asp:Label id="lblInVehicle" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                          <Headertemplate>
                                <asp:Label id="lblCompanyName" runat="server" Text="In company"></asp:Label>
                        </Headertemplate>
                        <ItemTemplate>
                            <asp:Label id="lblInCompany" runat="server" Text="N/A"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                          
                        <ItemTemplate>
                             <asp:Button ID="btnMore" runat="server" Text="More"  />
                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
            </asp:GridView>
        </div>
    </div>
   
      
</asp:Content>
