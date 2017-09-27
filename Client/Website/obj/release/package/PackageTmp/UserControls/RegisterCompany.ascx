<%@ Control Language="C#" AutoEventWireup="true" Inherits="Electracraft.Client.Website.UserControls.RegisterBase" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Web" %>
<script runat="server">
	public override bool HasData()
	{
		bool data = !string.IsNullOrEmpty(txtCompanyName.Text);
		if (!string.IsNullOrEmpty(txtEmail.Text)) data = true;
		if (!string.IsNullOrEmpty(txtPhone.Text)) data = true;
		if (!string.IsNullOrEmpty(txtAddress1.Text)) data = true;
		return data;
	}

     public void Page_PreRender(object sender, EventArgs e)
    {
         if (!IsPostBack)
            {
                Loadregions();
                LoadDistrict();
                LoadSuburbs();
            }

    }
	public override void LoadForm(DOContact contact)
	{
		if (!IsPostBack)
		{
			Loadregions();
			LoadDistrict();
			LoadSuburbs();
		}

		txtEmail.Text = contact.Email;
		txtAddress1.Text = contact.Address1;
		txtPhone.Text = contact.Phone;
		chkCustomerExclude.Checked = contact.CustomerExclude;
		string region = contact.Address4;
		foreach (ListItem item in RegionDD.Items)
			item.Selected = false;
		RegionDD.Items.FindByText(region).Selected = true;
		RegionDD.SelectedIndex = RegionDD.Items.IndexOf(RegionDD.Items.FindByText(region));
		RegionDD.DataBind();
		RegionDD.PreRender += (object sender, EventArgs e) =>
		{

			foreach (ListItem item in RegionDD.Items)
				item.Selected = false;
			RegionDD.Items.FindByText(region).Selected = true;
			RegionDD.SelectedIndex = RegionDD.Items.IndexOf(RegionDD.Items.FindByText(region));
			RegionDD.DataBind();
		};

		LoadDistrict();
		string district = contact.Address3;
		foreach (ListItem item in District_DDL.Items)
			item.Selected = false;
		District_DDL.Items.FindByText(district).Selected = true;
		District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
		District_DDL.DataBind();
		District_DDL.PreRender += (object sender, EventArgs e) =>
		{
			foreach (ListItem item in District_DDL.Items)
				item.Selected = false;
			District_DDL.Items.FindByText(contact.Address3).Selected = true;
			District_DDL.SelectedIndex = District_DDL.Items.IndexOf(District_DDL.Items.FindByText(district));
			District_DDL.DataBind();
		};

		LoadSuburbs();
		string suburb = contact.Address2;
		foreach (ListItem item in SuburbDD.Items)
			item.Selected = false;
		SuburbDD.Items.FindByText(suburb).Selected = true;
		SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
		SuburbDD.DataBind();
		SuburbDD.PreRender += (object sender, EventArgs e) =>
		{
			foreach (ListItem item in SuburbDD.Items)
				item.Selected = false;
			SuburbDD.Items.FindByText(contact.Address2).Selected = true;
			SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText(suburb));
			SuburbDD.DataBind();
		};
	}

	public override void SaveForm(DOContact contact)
	{
		contact.CompanyName = txtCompanyName.Text;
		contact.Email = txtEmail.Text;
		contact.Phone = txtPhone.Text;
		contact.Address1 = txtAddress1.Text;
		contact.Address2 = SuburbDD.SelectedItem.Text;
		contact.Address3 = District_DDL.SelectedItem.Text;
		contact.Address4 = RegionDD.SelectedItem.Text;
		contact.CustomerExclude = chkCustomerExclude.Checked;
	}
	protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadDistrict();
		LoadSuburbs();

	}
	public void LoadDistrict()
	{

		PageBase pb = new PageBase();
		List<DOBase> districts = pb.CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
		District_DDL.DataSource = districts;
		District_DDL.DataTextField = "DistrictName";
		District_DDL.DataValueField = "DistrictID";
		District_DDL.DataBind();

	}
	protected void LoadSuburbs()
	{
         District_DDL.SelectedIndex = 2;
		PageBase pb = new PageBase();
		List<DOBase> suburbs = pb.CurrentBRSuburb.SelectSuburbs(Guid.Parse(District_DDL.SelectedValue));
		SuburbDD.DataSource = suburbs;
		SuburbDD.DataTextField = "SuburbName";
		SuburbDD.DataValueField = "SuburbID";
		SuburbDD.DataBind();

	}
	public void Loadregions()
	{
        RegionDD.SelectedIndex = 2;
		PageBase pb = new PageBase();
		List<DOBase> regions = pb.CurrentBRRegion.SelectRegions();
		RegionDD.DataSource = regions;
		RegionDD.DataTextField = "RegionName";
		RegionDD.DataValueField = "RegionID";
		RegionDD.DataBind();
	}
	protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadSuburbs();
	}

</script>
<div class="row">
	<div class="small-12 medium-4 columns" style="padding-left: 18%;">
		Company Name
	</div>
	<div class="small-12 medium-8 columns" style="padding-right: 20%;">
		<asp:TextBox ID="txtCompanyName" Font-Size="Small" runat="server" Style="margin-bottom: 0;"></asp:TextBox>
		<asp:CheckBox ID="chkCustomerExclude" runat="server" Text="Exclude this company from being selected as a customer" />
	</div>
</div>
<div class="row">
	<div class="small-12 medium-4 columns" style="padding-left: 18%;">
		Email Address
	</div>
	<div class="small-12 medium-8 columns" style="padding-right: 20%;">
		<asp:TextBox ID="txtEmail" Font-Size="Small" runat="server"></asp:TextBox>
	</div>
</div>
<div class="row">
	<div class="small-12 medium-4 columns" style="padding-left: 18%;">
		Phone
	</div>
	<div class="small-12 medium-8 columns" style="padding-right: 20%;">
		<asp:TextBox ID="txtPhone" Font-Size="Small" runat="server"></asp:TextBox>
	</div>
</div>
<div class="row">
	<div class="small-12 medium-4 columns" style="padding-left: 18%;">
		Street no. *
	</div>

	<div class="small-12 medium-8 columns" style="padding-right: 20%;">

		<asp:TextBox ID="txtAddress1" Font-Size="Small" runat="server"></asp:TextBox>
	</div>
</div>
<asp:UpdatePanel ID="AddressUpdatePanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
	<ContentTemplate>
		<div class="row">
			<div class="small-12 medium-4 columns" style="padding-left: 18%;">
				Region *
			</div>
			<div class="medium-8 columns" style="padding-right: 20%;">
				<asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False"
					AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged" Font-Size="Small">
				</asp:DropDownList>
			</div>

		</div>
		<div class="row">
			<div class=" medium-4 columns" style="padding-left: 18%;">
				District *
			</div>
			<div class="medium-8 columns" style="padding-right: 20%;">
				<asp:DropDownList ID="District_DDL" runat="server" EnableTheming="False"
					AutoPostBack="true" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged" Font-Size="Small">
				</asp:DropDownList>
			</div>

		</div>
		<div class="row">
			<div class="small-12 medium-4 columns" style="padding-left: 18%;">
				Suburb *
			</div>
			<div class="medium-8 columns" style="padding-right: 20%;">
				<asp:DropDownList ID="SuburbDD" runat="server" EnableTheming="False"
					AutoPostBack="true" Font-Size="Small">
				</asp:DropDownList>
			</div>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>
