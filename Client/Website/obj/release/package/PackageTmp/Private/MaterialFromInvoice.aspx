<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
CodeBehind="MaterialFromInvoice.aspx.cs" Inherits="Electracraft.Client.Website.MaterialFromInvoice" %>


<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Assign Invoices   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">

    
 <%--   <asp:Button ID="btnImport" runat="server" Text="Vehcile" OnClick="LoadgvVehicle" />--%>

   <%-- 

    
    <asp:FileUpload id="FileUploadControl" runat="server"/>
    <asp:Button runat="server" id="UploadButton" text="Up load" onclick="UploadButton_Click" />
   
    <br /><br />
    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
    <br />
    <asp:Label runat="server" id="label5" Text="-------------------------below is from the task page--------------------------------" />
    <br /><br />--%>
  
             
  <h3> 
    <asp:Label ID="lblTitle" runat="server" Text="Task details" />
  </h3>
    <div class="row">
        <div class="small-2 columns">
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
        <div class="small-5 columns">
            <asp:CheckBox ID="chkFilterAssigned" runat="server" Text="Display assigned invoices?" onchange="chk_Changed" AutoPostBack="true" Width="100%"/>
        </div>
        <div class="small-5 columns">
            <asp:CheckBox ID="chkFilterDeleted" runat="server" Text="Display Deleted invoices?" onchange="chk_Changed" AutoPostBack="true"  Width="100%"/>
        </div>
    </div>

    <div class="row">
        <div class="small-6 large-6 columns">
            <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" />
        </div>
        <div class="small-6 large-6 columns">
            <asp:TextBox ID="txtUntilDate" runat="server" TextMode="Date" />
        </div>
    </div>
    <div class="row">
        <div class="small-6 columns">
            Enter job number <asp:TextBox ID="txtSearchJobNumber" runat="server" Height="10"></asp:TextBox>
        </div>
        <div class="small-6 columns">
            <asp:Button ID="btnSearchJobNumber" runat="server"  Text="Go to job" OnClick="btnSearchJobNumber_Click"/>
        </div>
        
    </div>
    
    
    <!--Parent//  -->
    <asp:GridView ID="gvParent" runat="server" AutoGenerateColumns="false" Width="100%"  CssClass="Grid" EnableModelValidation="True"
    visible="true" OnRowDataBound="gvParent_OnRowDataBound"  DataKeyNames="SupplierReference, SupplierInvoiceID">
     
        
        <Columns>
             
            <asp:TemplateField>
                <ItemTemplate>
                                      
                        <%--<img class="panal-hider" alt = "" style="cursor: pointer" src="images/plus.png" />--%>
                        <img class="panal-hider"  alt="" style="cursor: pointer" src="../image/enter.png" width="50" height="50" border="0"/>
                        <asp:Panel ID="Panel1" runat="server" class="hide toggle-hide">
                                           
                                <!-- Child -->
                                <asp:GridView ID="gvChild"  runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid" 
                                ShowFooter = "true" allowpaging="false" DataKeyNames="SupplierReference, SupplierInvoiceMaterialID, OldSupplierInvoiceMaterialID, 
                                    Qty, MaterialID, SupplierInvoiceID, QtyRemainingToAssign, MaterialName, TaskMaterialID, MatchID, VehicleID, sellprice">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                          <HeaderTemplate>
                                                <asp:Button ID="btnSelect" Text="Select All/None" runat="server"  OnClick="btnSelect_Click"/><br />
                                             
                                               
                                          </HeaderTemplate>
                                        
                                        
                                         <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked="false"
                                              AutoPostBack="false" />
                                        </ItemTemplate>

                                       
    
                                    </asp:TemplateField>
                                    

                                     <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQtyHeader" Text="Assigned / Total" runat="server" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyBody" Text="" runat="server" forecolor="Blue"/>
                                            </ItemTemplate>
                                            
                                    </asp:TemplateField>   


                                     <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQtyToAssignHeader" Text="Quantity to assign" runat="server" />
                                                <asp:Button runat="server" ID="btnAddToTask" onclick="btnAdd_Click" text="Add materials to this task"/> 
                                                <asp:Button runat="server" ID="btnAddToVehicle" onclick="btnAdd_Click" text="Add materials to my Vehicle"/> 
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                 <asp:TextBox  ID="TextBox2" runat="server" Text=""  OnTextChanged="TextBox2_TextChanged" ForeColor="Blue"  onkeydown="return checkIfKeyIsNumber(event)"></asp:TextBox>  
                                                 <asp:Label ID="lblTextBox2" Text="" runat="server"  Visible="false" forecolor="Blue"/>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                              <asp:Button runat="server" ID="btnAddToTask" onclick="btnAdd_Click" text="Add materials to this task"/> 
                                              <asp:Button runat="server" ID="btnAddToVehicle" onclick="btnAdd_Click" text="Add materials to my Vehicle"/> 
                                              <asp:Button runat="server" ID="btnMatchReturn" onclick="btnAdd_Click" text="Match Returns"/> 
                                    </FooterTemplate>
                                    </asp:TemplateField>   
                                    <asp:BoundField DataField="MaterialName"  headertext="Material name"   ItemStyle-ForeColor="Blue"/>

                                    
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnRedo" onclick="btnRedo_Click" text="Redo materials"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Assigned to (taskname/vehiclename/siteaddress1)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                              <asp:Label runat="server" id="lblAssignedTo" Text ="" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                                             
                                  
                                    
                                </Columns>
                            </asp:GridView>

                        </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField datafield="ContractorReference" HeaderText="Our reference" />
            <asp:BoundField DataField="totalexgst" HeaderText="Total ex GST" />
            <asp:BoundField datafield="InvoiceDate" HeaderText="InvoiceDate" DataFormatString="{0:dd/MM/yy}"/>
            <asp:BoundField datafield="SupplierName" HeaderText="Supplier" />
            <asp:BoundField datafield="createdDate" HeaderText="CreatedDate" DataFormatString="{0:dd/MM/yy}"/>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button Text="Delete" runat="server" id="btnDelete" onclick="btnDelete_Click"/>
                    <%--<asp:BoundField datafield="supplierinvoiceid" HeaderText="supplierinvoiceid" />--%>
                    </ItemTemplate>
            </asp:TemplateField>
            
           
            
        </Columns>
    </asp:GridView>

   
    <br /><br />

  
    
   
    
    
    
    
  <%--<%--  <h3> Product from my vehicle:  </h3>




















    <%--VEHICLE


    
       
    <asp:GridView ID="gvVehicle"  runat="server" AutoGenerateColumns="false" CssClass = "Grid" OnPreRender="gvVehicle_PreRender"
                                ShowFooter = "true" allowpaging="false" DataKeyNames="Qty, MaterialName, Vehicle, CostPrice, SellPrice,
                                RRP, UOM, MaterialID, SupplierName, QtyRemainingToAssign, SupplierInvoiceMaterialID, SupplierInvoiceID"> 
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
  
     

    <br /><br />

  <h3> Product from my sites:  </h3>--%>
 
<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });

    
</script>--%>
     <script type="text/javascript">

        $(".panal-hider").click(function () {
            var panal = $(this).parent().find(".toggle-hide");
            if (panal.hasClass("hide")) {
                var row = $(this).parent().parent();
                panal.removeClass("hide");
                var tableRow = '<tr><td></td><td class="insert" colspan="999"></td></tr>';
                
                $(tableRow).insertAfter(row);

                $(this).parent().parent().next().find(".insert").append(panal);



            } else {
                var panal = $(this).parent().parent().next().find(".toggle-hide");
                
                    panal.addClass("hide");
                    var row = $(this);
                    panal.insertAfter(row);

                $(this).parent().parent().next().remove();
            }
        });

    </script>
    <script type = "text/javascript">
        function checkIfKeyIsNumber(e) {
            if (
                !(e.keyCode >= 48 && e.keyCode <= 57) &&  // 0-9
                !(e.keyCode >= 96 && e.keyCode <= 105) // numpad 0-9

                // some more checking like arrows, delete, backspace, etc.
                && e.keyCode != 229 // Check for the 'placeholder keyCode'
                && e.keyCode != 8 //backspace
                && e.keyCode != 37 //left
                && e.keyCode != 39 //right
                && e.keyCode != 109 //-
                && e.keyCode != 189 //-
                && e.keyCode != 116 //F5
                && e.keyCode != 110 //.
                && e.keyCode != 190 //.
            ) {
                // This is not a valid key
                e.preventDefault();
            }
        }
     </script>
</asp:Content>


