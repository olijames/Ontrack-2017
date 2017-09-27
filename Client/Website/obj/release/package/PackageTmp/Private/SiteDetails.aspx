<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="SiteDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.SiteDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
    Site Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <%-- Tony modified 11/3/2016--%>
    <div class="row">
        <div class="small-12 medium-4 columns">
            <asp:Button ID="CancelAddSite" runat="server" OnClick="CancelAddSite_Click" CssClass="radius button small" Text="Cancel" Width="100%" />
        </div>
    </div>
   
    <asp:Panel ID="pnlSiteDetails" runat="server">
        <div class="row">
            <div class="row">
                <asp:Label runat="server" CssClass="error" Visible="false" ID="Error"></asp:Label>
            </div>
            <%--<asp:PlaceHolder ID="phCustomer" runat="server">
               
            </asp:PlaceHolder>--%>
<%--Commented Jared 2017.2.15 
    Tony modified 11/3/2016--%>
       <%--  <div class="row">
            <div class="small-12 medium-4 small-push-1 columns">
                Share with
            </div>
            <div class="small-12 medium-8 columns">
              <%--  <asp:TextBox ID="txtSiteAdd4" Font-Size="Small" runat="server"></asp:TextBox>
                <asp:DropDownList ID="ContactDD" runat="server" EnableTheming="False"
    AutoPostBack="true" Font-Size="Small" Height="30px" Width="575px">
</asp:DropDownList>&nbsp;<asp:Button ID="btnShare" runat="server" OnClick="btnShare_Click" Text="Share" Width="112px" />
            </div>
        </div>--%>
<%-- Tony modified 11/3/2016--%>

            <div class="row ">
                <div class="small-12 medium-5 columns">
                    <h5><b>Site Address</b></h5>
                </div>
                <br />
                <br />
                <div class="small-12 medium-4 small-push-1 columns">
                    Street no.
                </div>
                <div class="small-12 medium-8 columns">
                    <asp:TextBox ID="txtSiteAddress1" Font-Size="Small" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 small-push-1 columns">
                    Region
                </div>
                <div class="small-12 medium-8 columns">
                    <%--  <asp:TextBox ID="txtSiteAdd4" Font-Size="Small" runat="server"></asp:TextBox>--%>
                    <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False"
                        AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged" Font-Size="Small">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4 small-push-1 columns">
                    District *
                </div>
                <div class="small-12 medium-8 columns">
                    <%--<asp:TextBox ID="txtSiteAddress2" Font-Size="Small" runat="server"></asp:TextBox>--%>
                    <asp:DropDownList ID="District_DDL" runat="server" EnableTheming="False"
                        AutoPostBack="true" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged" Font-Size="Small">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="small-12 medium-4  small-push-1 columns">
                    Suburb *
                </div>
                <div class="small-12 medium-8 columns">
                    <%--  <asp:TextBox ID="txtSiteAdd3" Font-Size="Small" runat="server"></asp:TextBox>--%>
                    <asp:DropDownList ID="SuburbDD" runat="server" EnableTheming="False"
                        AutoPostBack="true" Font-Size="Small" OnPreRender="SuburbDD_PreRender" CausesValidation="True" AppendDataBoundItems="False" Visible="True">
                    </asp:DropDownList>
                </div>
            </div>
             <div class="row">
                <div class="small-12 columns">
                <h2>Site Details</h2>
                </div>
            </div>
             <div class="row">
                    <div class="small-12 small-push-1 medium-4 columns">
                        Select site owner
                    </div>
                    <div class="small-12 medium-8 columns">
                        <%--<asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlCustomer" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            <br />



            <asp:Panel ID="SiteAddBtns" runat="server">
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:label runat="server" ID="lbl1" Width="100%"/>
                    </div>
                </div>

                <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:Button runat="server" ID="Btn_CustomerOwn" BorderWidth="0px" Width="100%" Font-Size="Small" CssClass="customerBtn button radius small" OnClick="Btn_CustomerOwn_Click" text="Use selected site owner"/>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:Button runat="server" ID="btnNoDetails" BorderWidth="0px" Width="100%" Font-Size="Small" CssClass="customerBtn button radius small" OnClick="Btn_NoDetails_Click" text="Add the site owner later"/>
                    </div>
                </div>
               <%-- <div class="row">
                    <div class="small-12 medium-4 columns">
                        <asp:TextBox ID="txtNewCustomerEmail" runat="server" placeholder="Email Address" Font-Size="Small" Width="100%" Visible="false"></asp:TextBox>
                    </div>

                    <div class="small-12 medium-4 columns">
                        <asp:Button ID="btnAddNewCustomer" CssClass="customerBtn button radius small" runat="server" Width="100%" OnClick="btnAddNewCustomer_Click" Text="Add another customer" visible="false"/>
                    </div>
                </div>--%>


            </asp:Panel>
            <asp:PlaceHolder ID="EditSiteBtns" runat="server" Visible="false">
                <div class="row">
                    <div class="small-12 medium-12 columns push-7">
                        <asp:Button ID="Button1" runat="server" OnClick="btnSave_Click" CssClass="radius button small" Text="Save" />
                        &nbsp;<asp:Button ID="Button3" runat="server" OnClick="CancelAddSite_Click" CssClass="radius button small" Text="Cancel" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <br />
            <asp:PlaceHolder ID="phNew" Visible="false" runat="server">
                <div class="row">
                    <h2>Add New Customer</h2>
                    <p style="color: #f00;">
                        The new customer will be added to
                    <asp:Literal ID="litContactName" runat="server"></asp:Literal>.
                    </p>
                </div>
            </asp:PlaceHolder>

            <asp:Panel ID="pnlCustomerExists" runat="server" Visible="false">
                <div class="row">
                    <div class="small-12 columns">
                        <asp:PlaceHolder ID="phOneExisting" runat="server">Is this the customer? 
                    <asp:Button ID="btnYes" runat="server" Text="yes" CssClass="radius button tiny" OnClick="btnSave_Click" />&nbsp;
     <%-- <asp:Button ID="btnYes" runat="server" Text="Yes"  /> 
     --%>
                            <asp:Button ID="btnNo" runat="server" Text="No" CssClass="radius button tiny" OnClick="btnNo_Click" />
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phMultipleExisting" runat="server">Multiple contacts were found with the email address
                    <asp:Literal ID="litExistingEmail" runat="server"></asp:Literal>.<br />
                            <asp:Button ID="btnNoMultiple" runat="server" CssClass="button radius small" Text="My customer is not listed below" OnClick="btnNo_Click" /><br />
                            <asp:Repeater ID="rpExistingCustomers" runat="server">
                                <ItemTemplate>
                                    <div style="margin-top: 1rem">
                                        <%# (Electracraft.Framework.DataObjects.DOContact.ContactTypeEnum)Eval("ContactType") == Electracraft.Framework.DataObjects.DOContact.ContactTypeEnum.Individual ? Eval("DisplayName") + "<br />": "" %>
                                        <%# String.IsNullOrEmpty(Eval("CompanyName").ToString()) ? "" : Eval("CompanyName") + "<br />" %>
                                        <%# Eval("Address1") %><br />
                                        <%# Eval("Address2") %><br />
                                        <%# Eval("Phone") %><br />
                                        <asp:Button ID="btnYesExisting" runat="server" CssClass="button radius small" Text="This is my customer" CommandArgument='<%# Eval("ContactID").ToString() %>' OnClick="btnSaveFromMulti_Click" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlCustomerDetails" Visible="false" runat="server">
                <div class="row">
                    <asp:Label runat="server" CssClass="error" Visible="false" ID="ErrorCustomer"></asp:Label>
                </div>
                <div class="row section">
                    <div class="small-12 columns">
                        <h4 class="underline"><i class="fi-torso"></i>&nbsp;&nbsp;Customer Details</h4>

                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Customer First Name *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <asp:TextBox ID="Txt_FirstName" Font-Size="Small" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Customer Last Name *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <asp:TextBox ID="Txt_LastName" Font-Size="Small" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Customer Company Name (if applicable)
                            </div>
                            <div class="small-12 medium-8 columns">
                                <asp:TextBox ID="txtCustomerCompanyName" Font-Size="Small" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="small-12 medium-4 columns">
                            Street no.*
                        </div>
                        <div class="small-12 medium-8 columns">
                            <asp:TextBox ID="Txt_Add1" Font-Size="Small" runat="server"></asp:TextBox>
                        </div>
                        <%--<div class="row">
                    <div class="small-12 medium-4 columns">
                        Customer Address 1
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="Txt_Add1" runat="server"></asp:TextBox>
                    </div>
                </div>--%>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Region
                            </div>
                            <div class="small-12 medium-8 columns">
                                <%--  <asp:TextBox ID="txtSiteAdd4" Font-Size="Small" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="DDL_Address4" runat="server" EnableTheming="False"
                                    AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged" Font-Size="Small">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                District *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <%--<asp:TextBox ID="txtSiteAddress2" Font-Size="Small" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="DDL_Address3" runat="server" EnableTheming="False"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDL_Address3_SelectedIndexChanged" Font-Size="Small">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Suburb *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <%--  <asp:TextBox ID="txtSiteAdd3" Font-Size="Small" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="DDL_Add2" runat="server" EnableTheming="False"
                                    AutoPostBack="true" Font-Size="Small" OnSelectedIndexChanged="DDL_Add2_SelectedIndexChanged" OnPreRender="DDL_Add2_PreRender" CausesValidation="True" AppendDataBoundItems="False" Visible="True">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%--<div class="row">
                    <div class="small-12 medium-4 columns">
                        Customer Address 2
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="Txt_Add2" runat="server"></asp:TextBox>
                    </div>
                </div>--%>
                        <%-- <div class="row">
                    <div class="small-12 medium-4 columns">
                        Customer Address 3
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="TB_Address3" runat="server"></asp:TextBox>
                    </div>
                </div>--%>
                        <%--  <div class="row">
                    <div class="small-12 medium-4 columns">
                        Customer Address 4
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="TB_Address4" runat="server"></asp:TextBox>
                    </div>
                </div>--%>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Customer Email *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <asp:TextBox ID="Txt_Email" Font-Size="Small" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                Customer Phone *
                            </div>
                            <div class="small-12 medium-8 columns">
                                <asp:TextBox ID="Txt_Phone" Font-Size="Small" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

            </asp:Panel>
            <asp:PlaceHolder ID="phOwner" Visible="false" runat="server">
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner First Name
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerFirstName" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner Last Name
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerLastName" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner Address 1
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerAddress1" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner Address 2
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerAddress2" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner Email
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerEmail" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-4 columns">
                        Owner Phone
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="txtCustomerPhone" runat="server"></asp:TextBox>
                    </div>
                </div>
            </asp:PlaceHolder>
            <br />
            <asp:PlaceHolder ID="NewCustomerButtons" runat="server" Visible="false">
                <div class="row">
                    <div class="small-12 medium-12 columns push-7">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="radius button small" Text="Save" />
                        &nbsp;
                        <asp:Button ID="btnSaveAddJob" runat="server" OnClick="btnSaveAddJob_Click" CssClass="radius button small" Text="Save and add job details" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="radius button small" Text="Cancel" />
                    </div>
                </div>
            </asp:PlaceHolder>
        </div>
    </asp:Panel>
    <%--<div class="row">
                    <div class="small-12 medium-4 columns">
                        Customer Address 2
                    </div>
                    <div class="small-12 medium-8 columns">
                        <asp:TextBox ID="Txt_Add2" runat="server"></asp:TextBox>
                    </div>
                </div>--%>
</asp:Content>
