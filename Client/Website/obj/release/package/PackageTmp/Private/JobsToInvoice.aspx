<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
CodeBehind="JobsToInvoice.aspx.cs" Inherits="Electracraft.Client.Website.Private.JobsToInvoice"  EnableEventValidation="true" %>


<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Accounts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <%--<controls:ContactMenu ID="ContactMenu1" runat="server"></controls:ContactMenu>
    --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
     
    Jobs with items to invoice
     <asp:GridView ID="gvParent" runat="server" AutoGenerateColumns="false" Width="100%"  CssClass="Grid" EnableModelValidation="True" Visible="true"
          OnRowDataBound="gvParent_RowDataBound"  DataKeyNames="ContactID" OnPreRender="gvParent_PreRender">
          <Columns>
            


              <asp:TemplateField>
                  <ItemTemplate>
                      
                      <img class="panal-hider"  alt="" style="cursor: pointer" src="../image/enter.png" width="50" height="50" border="0"/>
                            <asp:Panel ID="Panel1" runat="server" class="hide toggle-hide">
                            <asp:GridView ID="gvChild"  runat="server" AutoGenerateColumns="false" DataKeyNames="TaskID, TaskName, JobID, TaskNumber, JobName, JobNumberAuto, Status,TaskCustomerID, TotalMaterial, TotalLabour" OnPreRender="gvChild_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                          <HeaderTemplate>
                                              <asp:label ID="lblHeader" runat="server" Text="Select"></asp:label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:checkbox id="chkBody" runat="server" text=""></asp:checkbox>
                                          </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblJobNumberHeader" runat="server" Text="Job Number"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblJobNumberBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblJobNameHeader" runat="server" Text="Job Name"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblJobNameBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblTaskNumberHeader" runat="server" Text="Task Number"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblTaskNumberBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblTaskNameHeader" runat="server" Text="Task Name"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblTaskNameBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblTaskStatusHeader" runat="server" Text="Status"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblTaskStatusBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                          <HeaderTemplate>
                                              <asp:Label ID="lblTaskValueHeader" runat="server" Text="Value ($) ex gst"></asp:Label>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <asp:label id="lblTaskValueBody" runat="server" text=""></asp:label>
                                          </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="10%">
                                          <ItemTemplate>
                                              <asp:button id="btnGotToTaskBody" runat="server" text="Go to task" OnClick="btnGotToTaskBody_Click"></asp:button>
                                          </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice" HeaderStyle-Width="10%">
                                          <ItemTemplate>
                                              <asp:button id="btnInvoice" runat="server" text="Invoice" OnClick="btnInvoice_Click"></asp:button>
                                          </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                </asp:GridView>

                        </asp:Panel>
                  </ItemTemplate>
              </asp:TemplateField>
              <%--  <asp:TemplateField>
                  <HeaderTemplate>
                      <asp:Label ID="lblCustomerNameheader" runat="server" Text="Customer"></asp:Label>
                  </HeaderTemplate>
                  <ItemTemplate>
                      <asp:Label ID="lblCustomerNameBody" runat="server" Text=""></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>

              <asp:TemplateField>
                  <HeaderTemplate>
                      <asp:Label ID="lblCustomerTotalheader" runat="server" Text="Total outstanding ($) ex GST"></asp:Label>
                  </HeaderTemplate>
                  <ItemTemplate>
                      <asp:Label ID="lblCustomerTotalBody" runat="server" Text=""></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>--%>
              <asp:BoundField DataField="DisplayName" HeaderText="Customer"/>
              <asp:BoundField DataField="TotalJobValue" headertext="Total outstanding ($) ex GST" Visible="true" DataFormatString="{0:C2}"/>
              <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20%">
                    <%--<HeaderTemplate>
                        <asp:button ID="btnHeader" runat="server" Text="Invoice"></asp:button>
                    </HeaderTemplate>--%>
                    <ItemTemplate>
                        <asp:button id="btnBody" runat="server" text="Invoice..." OnClick="btnBody_Click"></asp:button>
                    </ItemTemplate>
               </asp:TemplateField>

          </Columns>
      </asp:GridView>
      <asp:Button runat="server" ID="btnClearFault" Text="Clear ContractorCustomer Fault" OnClick="clearFault_Click" visible="false"/>


    <%--   <script type="text/javascript">

        $(".panal-hider").click(function () {
            var panal = $(this).next();
            if(panal.hasClass("hide")) {
                var row = $(this).parent().parent();
                panal.removeClass("hide");
                panal.addClass("open");
                var tableRow = '<tr><td></td><td class="insert" colspan="999">' + panal.html() + '</td></tr>';
                panal.remove();
                $(tableRow).insertAfter(row);
                
            } else {
                var panal = $(this).parent().parent().next().find(".open");
                    panal.addClass("hide");
                    panal.removeClass("open");
                    var row = $(this);
                    panal.insertAfter(row);

                $(this).parent().parent().next().remove();
            }
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

</asp:Content>
