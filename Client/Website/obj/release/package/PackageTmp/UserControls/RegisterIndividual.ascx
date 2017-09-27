<%@ Control Language="C#" AutoEventWireup="true" Inherits="Electracraft.Client.Website.UserControls.RegisterBase" %>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility.Exceptions" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>
<%@ Import Namespace="Electracraft.Client.Website.Private.Admin" %>
<%@ Import Namespace="Electracraft.Framework.Web" %>
<script runat="server">
    public void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["email"]))
        {
            txtEmail.Text = Request.QueryString["email"];
        }

        if(IsPostBack)
        {
            if (!(String.IsNullOrEmpty(txtPassword.Text.Trim())))
            {
                txtPassword.Attributes["value"]= txtPassword.Text;
            }
            if (!(String.IsNullOrEmpty(txtConfirmPassword.Text.Trim())))
            {
                txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
            }
        }
        if (!IsPostBack)
        {

            Loadregions();
            LoadDistrict();
            LoadSuburbs();
        }
    }
    public override void SaveForm(DOContact contact)
    {
        if (string.IsNullOrEmpty(txtPassword.Text))
            throw new FieldValidationException("Password is required.");
        if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            throw new FieldValidationException("Please confirm your password.");
        if (txtPassword.Text != txtConfirmPassword.Text)
        {
            throw new FieldValidationException("The passwords you have entered do not match.");
        }
        if(SuburbDD.SelectedItem==null || SuburbDD.SelectedItem.ToString() == "-Select-")
        {
            throw new FieldValidationException("Please select Suburb");
        }
        contact.PasswordHash = PasswordHash.CreateHash(txtPassword.Text);
        contact.UserName = txtEmail.Text.Trim();
        contact.FirstName = txtFirstName.Text.Trim();
        contact.LastName = txtLastName.Text.Trim();
        contact.Email = txtEmail.Text.Trim();
        contact.Phone = txtPhone.Text;
        contact.Address1 = txtAddress1.Text;
        contact.Address2 = SuburbDD.SelectedItem.Text;
        contact.Address3 = District_DDL.SelectedItem.Text;
        contact.Address4=RegionDD.SelectedItem.Text;
    }

    public void Loadregions()
    {
         RegionDD.SelectedIndex = 2;
        PageBase pb = new PageBase();
        List<DOBase> Regions = pb.CurrentBRRegion.SelectRegions();
        RegionDD.DataSource = Regions;
        RegionDD.DataTextField = "RegionName";
        RegionDD.DataValueField = "RegionID";
        RegionDD.DataBind();
    }
    public void LoadDistrict()
    {
        PageBase pb = new PageBase();
        List<DOBase> Districts = pb.CurrentBRDistrict.SelectDistricts(Guid.Parse(RegionDD.SelectedValue));
        District_DDL.DataSource = Districts;
        District_DDL.DataTextField = "DistrictName";
        District_DDL.DataValueField = "DistrictID";
        District_DDL.DataBind();
        District_DDL.SelectedIndex = 2;

    }
    protected void LoadSuburbs()
    {

        PageBase pb = new PageBase();
        List<DOBase> Suburbs = pb.CurrentBRSuburb.SelectSuburbs(Guid.Parse(District_DDL.SelectedValue));
        SuburbDD.DataSource = Suburbs;
        SuburbDD.DataTextField = "SuburbName";
        SuburbDD.DataValueField = "SuburbID";
        SuburbDD.DataBind();

    }
    protected void RegionDD_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDistrict();
        LoadSuburbs();
        // SuburbDD.Focus();

    }


    protected void District_DDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSuburbs();
        // SuburbDD.Focus();
    }

    protected void SuburbDD_SelectedIndexChanged(object sender, EventArgs e)
    {
        // SuburbDD.Focus();
    }
    //Form foreground color
    string getForeColor()
    {
        string color;
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (url.Contains("employeeinfo"))
            color= "black";
        else
            color= "white";
        return color;
    }

    protected void SuburbDD_PreRender(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            SuburbDD.Items.Insert(0, new ListItem("-Select-", ""));
            foreach (ListItem item in SuburbDD.Items)
                item.Selected = false;
            SuburbDD.SelectedIndex = SuburbDD.Items.IndexOf(SuburbDD.Items.FindByText("-Select-"));
        }
    }
</script>
<div style="color:<%# getForeColor() %>;">
<div class="row">
<%--    <div class="small-12 medium-4 columns" style="padding-left: 3.3em;">--%>
   <br />
        
        <div class=" medium-4 columns" style="padding-left:18%;">
        Email *
            
    </div>
 <%--   <div class="small-12 medium-8 columns">--%>
 
         <div class="medium-8 columns" style="padding-right:20%;">
        <asp:TextBox ID="txtEmail" runat="server" Font-Size="Small"></asp:TextBox>
             </div>
  </div>


<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Password *
    </div>
   <div class="medium-8 columns" style="padding-right:20%;">
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Font-Size="Small"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Confirm Password *
    </div>
    <div class="medium-8 columns" style="padding-right:20%;">
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Font-Size="Small"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        First Name *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
        <asp:TextBox ID="txtFirstName" runat="server" Font-Size="Small"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Last Name *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
        <asp:TextBox ID="txtLastName" runat="server" Font-Size="Small"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Phone *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
    
        <asp:TextBox ID="txtPhone" runat="server" Font-Size="Small"></asp:TextBox>
   
    </div>
</div>
  
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Street no. *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
     
        <asp:TextBox ID="txtAddress1" runat="server" Font-Size="Small"></asp:TextBox>
    </div>
</div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
                <div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
       Region *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
       <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False"
    AutoPostBack="true" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged" Font-Size="Small">
</asp:DropDownList>
    </div>
        
</div>
     <div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
      District *
    </div>
  <div class="medium-8 columns" style="padding-right:20%;">
       <asp:DropDownList ID="District_DDL" runat="server" EnableTheming="False"
    AutoPostBack="true" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged" Font-Size="Small">
</asp:DropDownList>
    </div>
        
</div>
<div class="row">
    <div class=" medium-4 columns" style="padding-left:18%;">
        Suburb *
    </div>
        <div class="medium-8 columns" style="padding-right:20%;">
        <asp:DropDownList ID="SuburbDD" runat="server" EnableTheming="False" AutoPostBack="true" Font-Size="Small" 
            OnSelectedIndexChanged="SuburbDD_SelectedIndexChanged" OnPreRender="SuburbDD_PreRender">
  
</asp:DropDownList>
            </div>
  
</div>
            </ContentTemplate>
</asp:UpdatePanel>
    </div>