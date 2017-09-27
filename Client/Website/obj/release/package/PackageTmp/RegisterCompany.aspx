<%@ Page Title="" Language="C#" MasterPageFile="~/Default2.Master" AutoEventWireup="true" 
CodeBehind="RegisterCompany.aspx.cs" Inherits="Electracraft.Client.Website.RegisterCompany" %>





<%@ Register Src="~/UserControls/RegisterCompany.ascx" TagName="RegisterCompany" TagPrefix="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server">
Register - Step 2
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
 <br />
    <br />
    <br />
   
     <div class="row large-text-center">
      <h2>Company Details</h2>
      </div>
    <div class="row">
        <div class="medium-text-center" style="color:white;">
    
    <p>Do you want to link to a new or existing company?       </p>
            <p>If yes, check this box</p>
                <asp:CheckBox CssClass="rb_CompanyBelong" ID="chkCompany" runat="server" onclick="toggleCompany($(this).is(':checked'))" />
    
    
   
         </div>
        
    </div>

    <div id="divCompany" style="display:none">
        <div class="row">
            <div class="small-12 columns">
                <h4><asp:RadioButton CssClass="rb_Existing" ID="rbLinkCompany" AutoPostBack="false" runat="server" GroupName="company" Checked="true" onclick="toggleCC($(this).is(':checked'))" /> Join Existing Company</h4>
            </div>
            <div class="small-12 columns">
                <div id="CC_Key">
                    Enter Company Key: <asp:TextBox ID="txtCompanyKey" runat="server"></asp:TextBox><br />
                    Please note: you will not be linked to this company until you are approved by the company manager.
                </div>
            </div>
        </div>

        <div class="row">
            <div class="small-12 columns">
                <h4><asp:RadioButton ID="rbNewCompany" runat="server" GroupName="company" onclick="toggleCC(!($(this).is(':checked')))"  /> Register New Company</h4>
            </div>
            <div class="small-12 columns">
                <div id="CC_Register" style="display:none">
                   
                    <controls:RegisterCompany ID="rgCompany" runat="server"></controls:RegisterCompany>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="medium-4 columns" style="float:right">
            <asp:Button ID="btnRegister" runat="server" Font-Bold="true" CssClass="button radius" ForeColor="#007acc" BackColor="White" Text="Continue" OnClick="btnRegister_Click" />
        </div>
    </div>

<script type="text/javascript">
    function toggleCompany(v) {
       
        if (v){
            $('#divCompany').slideDown();
        }
        else {
            $('#divCompany').slideUp();
        }        
    }
    function showCompany()
    {
       
        document.getElementById('divCompany').style.display = "";
    }
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
        var checkPostBack = '<%=Page.IsPostBack ?"true":"false"%>';
        if (!checkPostBack)
            {
        toggleCC(!$('#CC_Register').is(':checked'));
        toggleCompany($('.rb-companybelong').is(':checked'));
        }
    });

</script>
</asp:Content>

