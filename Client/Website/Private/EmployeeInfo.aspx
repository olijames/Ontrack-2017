<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="EmployeeInfo.aspx.cs" Inherits="Electracraft.Client.Website.Private.EmployeeInfo" %>

<%@ Register Src="~/UserControls/PrivateContactMenu.ascx" TagName="ContactMenu" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="RegisterEmployee" TagPrefix="controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Employee Info
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <%--<controls:ContactMenu ID="ContactMenu1" runat="server"></controls:ContactMenu>
    --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Employee Info</h3>
            <asp:PlaceHolder ID="litCompanyName" runat="server">
                <h4>
                    <%# Company != null ? Company.DisplayName : "" %></h4>
            </asp:PlaceHolder>
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="phPage">

    <div class="row">
        <div class="small-12 columns">

            <h4>Active Employees</h4>
            <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvEmployees_OnRowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Name" DataField="DisplayName" />
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <%# Eval("Address1") %><br />
                            <%# Eval("Address2") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Phone" DataField="Phone" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnEdit" CommandArgument='<%# Eval("ContactCompanyID").ToString() %>' Text="Edit" OnClick="btnEdit_Click" />
                            <asp:Button runat="server" ID="btnRemove" CommandArgument='<%# Eval("ContactCompanyID").ToString() %>' Text="Remove" OnClick="btnRemove_Click" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnRemove" ConfirmText="Are you sure you want to remove this employee from your company?"></ajaxToolkit:ConfirmButtonExtender>
                            <%# Eval("ContactCompanyID").ToString() %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <h4>Pending / Declined employees</h4>
            <asp:GridView ID="gvEmployeesNotActive" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="Name" DataField="DisplayName" />
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <%# Eval("Address1") %><br />
                            <%# Eval("Address2") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Phone" DataField="Phone" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# GetStatusString(Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# (!(bool)Eval("ContactCompanyActive") || (bool)Eval("ContactCompanyPending")) %>'>
                                <asp:Button runat="server" ID="btnApprove" CommandArgument='<%# Eval("ContactCompanyID").ToString() %>' Text="Approve" OnClick="btnApprove_Click" />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" Visible='<%# ((bool)Eval("ContactCompanyActive")) %>'>
                                <asp:Button runat="server" ID="btnDecline" CommandArgument='<%# Eval("ContactCompanyID").ToString() %>' Text="Decline" OnClick="btnRemove_Click" />
                            </asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            
            <input type="button" value="Add Employee" onclick="$('#employee_details').slideDown()" />
            <div id="employee_details" style="display:none; ">
                <controls:RegisterEmployee ID="RE1" runat="server" />
                <div style="margin-left:50%">
                <asp:Button ID="btnAddEmployee" runat="server" Text="Save" CssClass="button radius" OnClick="btnAddEmployee_Click" />
            </div>
                </div>
        </div>
    </div>
    </asp:PlaceHolder>
    <script runat="server">
        private string GetStatusString(object item)
        {
            Electracraft.Framework.DataObjects.DOContactEmployee ce = item as Electracraft.Framework.DataObjects.DOContactEmployee;
            if (item == null) return string.Empty;
            if (ce.ContactCompanyActive && !ce.ContactCompanyPending)
            {
                return "Approved";
            }
            else if (ce.ContactCompanyActive && ce.ContactCompanyPending)
            {
                return "Pending";
            }
            else
            {
                return "Removed";
            }
        }
        </script>
</asp:Content>
