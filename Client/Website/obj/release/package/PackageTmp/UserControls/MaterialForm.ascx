<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialForm.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.MaterialForm" %>
<asp:HiddenField ID="hidCategoryContactID" runat="server" />
    <div class="row">
        <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">
            Material Name
        </div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">
            Category
        </div>
        <div class="small-12 medium-8 columns">
            <asp:DropDownList ID="ddlMaterialCategoryNew" ClientIDMode="Static" runat="server" onchange="shownew()"></asp:DropDownList>
            <div id="MaterialCategoryNew" style="display:none">
                <asp:TextBox ID="txtNewCategoryName" runat="server" placeholder="Category name..."></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">
            Cost Price</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtCostPrice" runat="server"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">
            Sell Price</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtSellPrice" runat="server"></asp:TextBox></div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">
            Description</div>
        <div class="small-12 medium-8 columns">
            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            shownew();
        });

        function shownew() {
            if ($('#ddlMaterialCategoryNew').val() == 'ffffffff-ffff-ffff-ffff-ffffffffffff') {
                $('#MaterialCategoryNew').slideDown();
            }
            else {
                $('#MaterialCategoryNew').slideUp();
            }
        }
    </script>
