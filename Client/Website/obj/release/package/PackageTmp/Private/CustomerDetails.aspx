<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
	CodeBehind="CustomerDetails.aspx.cs" Inherits="Electracraft.Client.Website.Private.CustomerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
	Customer Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
	<div class="row">
		<div class="small-12 columns">
			<asp:PlaceHolder ID="phNew" runat="server">
				<h2>Add New Customer of</h2>
				<p style="color: #f00;">
					<asp:Literal ID="litContactName" runat="server"></asp:Literal>.</p>
			</asp:PlaceHolder>
		</div>
	</div>
    <%-- mandeeps copy of sln --%>
	<asp:Panel ID="pnlCustomerExists" runat="server">
		<div class="row">
			<div class="small-12 columns">
				<asp:PlaceHolder ID="phMultipleExisting" runat="server">
					<strong>
						<asp:Literal ID="litExistingEmail" runat="server"></asp:Literal></strong>
					<asp:Label ID="Linked_lbl" Visible="False" runat="server" Text="has following company/individual linked to it:-"></asp:Label>
					<br />
					<asp:Button ID="btnNoMultiple" Visible="False" runat="server" CssClass="button radius tiny" Text="My customer is not listed below" OnClick="btnNo_Click" /><br />
					<asp:Repeater ID="RepCustomersList" runat="server">
						<ItemTemplate>
							<div style="margin-top: 1rem">
								<%# Eval("type") %>
								<asp:Label runat="server"></asp:Label>
								&nbsp;&nbsp;            
                                <asp:Button ID="btnYesExisting" runat="server" CssClass="button radius tiny" Text="Use this customer" CommandArgument='<%# Eval("Contact.ContactID").ToString() %>' OnClick="btnSaveFromMulti_Click" />
								<br />
								<asp:Label runat="server" Text=' <%# Eval("Contact.DisplayName") %>'></asp:Label>
								<br />
								<asp:Label runat="server" Text=' <%# Eval("Contact.Address1") %>'></asp:Label>
								<br />
								<asp:Label runat="server" Text=' <%# Eval("Contact.Address2") %>'></asp:Label>
								<br />
								<asp:Label runat="server" Text=' <%# Eval("Contact.Address3") %>'></asp:Label>
								<br />
								<asp:Label runat="server" Text=' <%# Eval("Contact.Phone") %>'></asp:Label>
								<br />
								<br />
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</asp:PlaceHolder>
			</div>
		</div>
	</asp:Panel>
	<asp:Panel ID="pnlCustomerDetails" runat="server">
		<div class="row section">
			<div class="small-12 columns">
				<h4 class="underline"><i class="fi-torso"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="pnl_lbl_forCustomerOrOwner" Text="Customer Details"></asp:Label>
				</h4>
				<asp:Panel ID="Cust_name" runat="server">
					<div class="row">
						<div class="small-12 medium-4 columns">
							First Name *
						</div>
						<div class="small-12 medium-8 columns">
							<asp:TextBox ID="txtCustomerFirstName" Font-Size="Small" runat="server"></asp:TextBox>
						</div>
					</div>
					<div class="row">
						<div class="small-12 medium-4 columns">
							Last Name *
						</div>
						<div class="small-12 medium-8 columns">
							<asp:TextBox ID="txtCustomerLastName" Font-Size="Small" runat="server"></asp:TextBox>
						</div>
					</div>
				</asp:Panel>
				<%--Jared 16/1/17
                    
                    <asp:Panel runat="server" ID="companyName">
					<div class="row">
						<div class="small-12 medium-4 columns">
							Company Name 
						</div>
						<div class="small-12 medium-8 columns">
							<asp:TextBox ID="txtCustomerCompanyName" Font-Size="Small" runat="server"></asp:TextBox>
						</div>
					</div>
				</asp:Panel>--%>

				<div class="row">
					<div class="small-12 medium-4 columns">
                        Street no. *
					</div>
					<div class="small-12 medium-8 columns">
						<asp:TextBox ID="txtCustomerAddress1" Font-Size="Small" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="row">
					<div class="small-12 medium-4 columns">Region
					</div>
					<div class="small-12 medium-8 columns">
						<asp:DropDownList ID="DDL_Address4" runat="server" EnableTheming="False"
							AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged" Font-Size="Small">
						</asp:DropDownList>
					</div>
				</div>
				<div class="row">
					<div class="small-12 medium-4 columns">
						<%--  Customer Address 3--%> District
					</div>
					<div class="small-12 medium-8 columns">
						<%--  <asp:TextBox ID="TB_Address3" runat="server"></asp:TextBox>--%>
						<asp:DropDownList ID="DDL_Address3" runat="server" EnableTheming="False"
							AutoPostBack="true" OnSelectedIndexChanged="DDL_Address3_SelectedIndexChanged" OnPreRender="DDL_Address3_PreRender" Font-Size="Small">
						</asp:DropDownList>
					</div>
				</div>
				<div class="row">
					<div class="small-12 medium-4 columns">
						<%-- Customer Address 4--%> Suburb *
					</div>
					<div class="small-12 medium-8 columns">
						<%--  <asp:TextBox ID="TB_Address4" runat="server"></asp:TextBox>--%>
						<asp:DropDownList ID="DDL_Add2" runat="server" EnableTheming="False"
							Font-Size="Small" CausesValidation="True" OnPreRender="DDL_Add2_PreRender" AppendDataBoundItems="False" Visible="True">
						</asp:DropDownList>
					</div>
				</div>
				<div class="row">
					<div class="small-12 medium-4 columns">
						Email *
					</div>
					<div class="small-12 medium-8 columns">
						<asp:TextBox ID="txtCustomerEmail" Font-Size="Small" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="row">
					<div class="small-12 medium-4 columns">
						Phone *
					</div>
					<div class="small-12 medium-8 columns">
						<asp:TextBox ID="txtCustomerPhone" Font-Size="Small" runat="server"></asp:TextBox>
						<%-- <asp:RegularExpressionValidator ID="PhoneNumbervalidator" runat="server" ControlToValidate="txtCustomerPhone" 
                            ErrorMessage="Please enter correct phone number" ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"
                             ForeColor="red"  ValidateRequestMode="Enabled"></asp:RegularExpressionValidator>
						--%>
					</div>
				</div>
			</div>
		</div>

	</asp:Panel>
	<div class="row">
		<div class="small-12 columns" style="padding-bottom: 1em; float: left">
			<asp:Panel runat="server" ID="SaveCancelPnl">
				<asp:Button ID="btnSave_Notify" Visible="False" runat="server" Text="Save and Notify customer" CssClass="button radius tiny" OnClick="btnSaveCompanyOnClick" CommandArgument="Yes" />
				<asp:Button ID="btnEditSave" runat="server" Visible="False" Text="Save" CssClass="button radius tiny" OnClick="btnSave_Click" />
				<asp:Button ID="btnSaveIndividual" Visible="False" runat="server" Text="Save" CssClass="button radius tiny" OnClick="btnSave_Click" />
				<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button radius tiny" OnClick="btnSave_OnClick" CommandArgument="No" />
				<%--                    <asp:Button ID="btnSaveAddSite" runat="server" Text="Apply & add site info" OnClick="btnSaveAddSite_Click" />--%>
				<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button radius tiny" OnClick="btnCancel_Click" />
			</asp:Panel>
		</div>
	</div>
	<div class="row">
		<div class="small-12 columns" style="padding-bottom: 1em; float: left">
			<asp:Panel ID="AskIfCompanyOfCustomer" runat="server" Visible="False">
				Do you want to create a company for 
                <asp:Label runat="server" Font-Bold="True" Text="<%#txtCustomerFirstName.Text%>" />? 
         <asp:Button ID="CompanyYes" runat="server" Text="Yes" CssClass="button radius tiny" OnClick="CompanyYes_OnClick" />
				<asp:Button ID="CompanyNo" runat="server" Text="No" CssClass="button radius tiny" OnClick="CompanyNo_OnClick" />
			</asp:Panel>
			<asp:Panel runat="server" ID="companyName_pnl" ClientIDMode="Static" Visible="False">
				<asp:Label runat="server" Text="Company Name"></asp:Label>
				<asp:TextBox runat="server" ID="companyName_txt"></asp:TextBox>
			</asp:Panel>
			<asp:Panel runat="server" ID="IsOwner_pnl" Visible="False">
				How is
                <asp:Label runat="server" Text="<%#txtCustomerFirstName.Text %> " Font-Bold="True" />
				linked to the company?
                 <asp:Button ID="Owner_btn" runat="server" Text="Owner" CssClass="button radius tiny" OnClick="Owner_btn_OnClick" />
				<asp:Button ID="Emp_btn" runat="server" Text="Employee" CssClass="button radius tiny" OnClick="Emp_btn_OnClick" />
				<asp:Button runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="button radius tiny" />
			</asp:Panel>
			<asp:Panel runat="server" ID="pnl_CompanyOwnerDetails" Visible="False">
				Do you have company owner's details?
                <asp:Button ID="btn_OwnerYes" runat="server" Text="Yes" pnl_CompanyOwnerDetails="Yes" CssClass="button radius tiny" OnClick="btn_OwnerYes_OnClick" />
				<asp:Button ID="btn_OwnerNo" runat="server" Text="No" CssClass="button radius tiny" OnClick="btn_OwnerNo_OnClick" />
			</asp:Panel>
			<asp:Panel runat="server" ID="pnl_CompanyOwnerEmailID" Visible="False">
				<div class="row">
					<div class="small-12 medium-4 columns">
						Enter owner's email id *
					</div>
					<div class="small-12 medium-8 columns">
						<asp:TextBox ID="txt_emailId" Font-Size="Small" runat="server"></asp:TextBox>
					</div>
					<div class="small-12 medium-8 columns">
						<asp:Button runat="server" ID="SubmitEmailID" Text="Submit" OnClick="SubmitEmailID_OnClick" CssClass="button radius tiny" />
						<asp:Button runat="server" ID="cancelBtn" Text="Cancel" OnClick="btnCancel_Click" CssClass="button radius tiny" />
					</div>
				</div>
			</asp:Panel>
			<asp:Panel runat="server" ID="pnl_CompanyOwnerSave" Visible="False">
				<div class="small-12 medium-8 columns">
					<asp:Button runat="server" ID="btn_CompanyOwnerSave" Text="Add" OnClick="btn_CompanyOwner_OnClick" CssClass="button radius tiny" />
					<asp:Button runat="server" ID="btn_Cancel" Text="Cancel" OnClick="btnCancel_Click" CssClass="button radius tiny" />
				</div>
			</asp:Panel>
		</div>
	</div>
</asp:Content>
