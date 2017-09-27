<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactCompanies.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.ContactCompanies" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Register Src="~/UserControls/RegisterCompany.ascx" TagName="RegisterCompany" TagPrefix="controls" %>

<script runat="server">
    string GetContactName(DOContact Contact)
    {
        if (Contact.ContactType == DOContact.ContactTypeEnum.Company)
            return Contact.CompanyName;
        else
            return string.Format("{0} {1}", Contact.FirstName, Contact.LastName);
    }
</script>
<div class="row">
    <div class="small-12 columns">

        <asp:Panel ID="pnlContactCompanies" runat="server">
            <asp:Repeater ID="rpContactCompanies" runat="server">
                <ItemTemplate>
                    <div class="row">
                        <div class="small-8 columns"><%# ((DOContact)Container.DataItem).DisplayName %></div>
                        <div class="small-4 columns text-right">
                            <asp:Button ID="btnView" runat="server" Text="View Details" OnClick="btnView_Click" CommandName="View" CommandArgument='<%# Eval("ContactID").ToString() %>' />
                            <asp:PlaceHolder Visible="<%# _Contact == null %>" runat="server">
                                <asp:Button ID="btnRemoveContactLink" runat="server" Text="Remove" OnClick="btnRemoveLink_Click" CommandName="RemoveLink" CommandArgument="<%# ((DOContact)Container.DataItem).ContactID.ToString() %>" />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder Visible="<%# _Contact != null %>" runat="server">
                                <a href="<%# ResolveClientUrl( "~/private/admin/companydetails.aspx?id=" + Eval("ContactID").ToString())%>">View Company</a>
                            </asp:PlaceHolder>
                            <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnRemoveContactLink" ConfirmText="Are you sure you want to remove this company?"></ajaxToolkit:ConfirmButtonExtender>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Literal ID="litNoCompanies" runat="server">No companies.</asp:Literal>

            <div style="border-top: 1px solid #d8d8d8; margin-top: 1em; padding-top: 0.625em;">
                <h6>Link a company</h6>
                <asp:RadioButtonList ID="rblToggle" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
                    <asp:ListItem onclick="toggleCC($(this).is(':checked'))" Text="Existing" Selected="True"></asp:ListItem>
                    <asp:ListItem onclick="toggleCC(!($(this).is(':checked')))"  Text="Add New"></asp:ListItem>
                </asp:RadioButtonList>
                <div class="row section" id="CC_Key">
                    <div class="small-12 medium-4 columns">
                        Enter Company Key
            
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCompanyKey" runat="server"></asp:TextBox>
                        <asp:Button ID="btnLinkCompany" runat="server" Text="Link Company" OnClick="btnLinkCompany_Click" />
                    </div>
                </div>
                <div class="row section" id="CC_Register" style="display:none">
                    <div class="small-12 columns">
                        <controls:RegisterCompany ID="rgCompany" runat="server" />
                    </div>
                    <div class="small-12 medium-4 columns">&nbsp;</div>
                    <div class="small-12 medium-8 columns"><asp:Button ID="btnAddCompany" runat="server" Text="Add & Link Company" OnClick="btnAddCompany_Click" /></div>
                </div>
            </div>
        </asp:Panel>

    </div>
</div>
<script type="text/javascript">
    function toggleCC(key) {
        var divKey = $('#CC_Key');
        var divReg = $('#CC_Register');
        if (key) {
            divKey.slideDown();
            divReg.slideUp();
        }
        else {
            divKey.slideUp();
            divReg.slideDown();
        }
    }

    $(document).ready(function () {
        toggleCC(!$('#CC_Register').is(':checked'));
    });

</script>