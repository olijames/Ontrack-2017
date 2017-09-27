<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master"
    AutoEventWireup="true" CodeBehind="PromoteBusiness.aspx.cs"
    Inherits="Electracraft.Client.Website.Private.PromoteBusiness" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <br />
    <br />
    <div class="row large-text-center">
        <h2>Promote Business</h2>

    </div>
    <br />
    <br />
     <div class="row">
         <asp:PlaceHolder id="searchable" runat="server" >
           <div class="row" >
        <div class="small-12 medium-5 large-5 columns" style="margin-left:0%;margin-bottom:0px;">
        <strong>Do you want to be searchable as a contractor?</strong> 
            </div>
               <div class="small-12 medium-7 large-7 columns">
<%--<asp:Button runat="server" ID="Yes" Text="Yes" CssClass="button radius tiny" OnClick="Yes_Click"/>
                 &nbsp;  <asp:Button  runat="server" ID="No" Text="No" CssClass="button radius tiny" OnClick="No_Click"/>--%>
                   <asp:CheckBox runat="server" ID="SearchTick" /> &nbsp; &nbsp; &nbsp;
                     <asp:Button runat="server" ID="Button1" Text="Save" CssClass="button radius tiny" OnClick="Yes_Click"/>
               </div>
        </div>
             <%--<div class="row" >
                  <div class="small-12 medium-7 large-7 columns">
                      <asp:Button runat="server" ID="Yes" Text="Save" CssClass="button radius tiny" OnClick="Yes_Click"/>
                      </div>
             </div>--%>
             </asp:PlaceHolder>
         <hr />
    <div class="row">
         <div class="small-12 medium-12 columns">
              <div class="medium-text-center">
          <asp:Label runat="server" Font-Bold="true" ClientIDMode="Static" ID="StatusText" Visible="False"></asp:Label>
              </div>
          </div>
        <br />
        <div class="small-12 medium-4 large-4 columns" style="margin-left:0%;margin-bottom:0px;">
         People can search you for Trade Categories
        </div>
  <div class="small-12 medium-8 large-8 columns">
      <asp:BulletedList ID="ExistTradCat" runat="server">
      </asp:BulletedList>
      </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
        <div class="row">
        <div class="small-12 medium-4 large-4 columns" style="margin-left:0%;margin-bottom:0px;">
         Add New
            </div>
            <div class="small-12 medium-8 large-8 columns">
     <asp:DropDownList ID="TradeCategories_DDL" runat="server" OnPreRender="TradeCategories_DDL_PreRender" ></asp:DropDownList>
            <%--    OnSelectedIndexChanged="TradeCategories_DDL_SelectedIndexChanged"AutoPostBack="true"--%>
  </div>
        </div>
    </div>
    <div class="row" style="display:none;">
        <div class="small-12 medium-4 large-5 columns" style="margin-bottom:0px;">
         Select SubTradeCategory
        </div>
         <div class="small-12 medium-8 columns">
                   <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                        ID="SubtradeCategoryRdBtn" CssClass="radio-list" OnSelectedIndexChanged="SubtradeCategoryRdBtn_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                    </asp:RadioButtonList> 
        <div id="SUbTradeCategory_div" visible="false" runat="server" class="small-12 medium-8 large-12 column" 
            style="OVERFLOW-Y:scroll;height:250px;margin-top:0px;margin-bottom:10px">
         <div class="small-12 large-5 large-push-1">
             <asp:CheckBoxList ID="SubTradeCategories_CBList"
           RepeatLayout="Flow"
           TextAlign="Right"
           runat="server"
           Checked="checked">
       </asp:CheckBoxList>
             </div>
             </div>
             </div>
    </div>
      <div class="row" style="margin-left:55%;">
            <asp:Button ID="btn_SaveTradeCategories" runat="server" Text="Save" CssClass="button radius small" OnClick="btn_SaveTradeCategories_Click" />
        </div>
    <hr class="style-four row" />

    <div class="row large-text-center">
        <h2>Areas of Operation</h2>

    </div>
     <div class="row">
              <div class="medium-text-center">
          <asp:Label runat="server" CssClass="alert" ClientIDMode="Static" ID="SuburbStatusText" Visible="False"></asp:Label>
              </div>
          </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            <asp:Label runat="server">Region</asp:Label>
        </div>
        <div class="small-12 medium-8 columns">
            <asp:DropDownList ID="RegionDD" runat="server" EnableTheming="False" AutoPostBack="true"
                Font-Size="Large" OnSelectedIndexChanged="RegionDD_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 columns">
            <asp:Label runat="server">District</asp:Label>
        </div>
        <div class="small-12 medium-8 columns">
            <asp:DropDownList ID="District_DDL" runat="server" EnableTheming="False" AutoPostBack="true"
                Font-Size="Large" OnSelectedIndexChanged="District_DDL_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="small-12 medium-4 large-4 columns"  >
            <asp:Label runat="server">Suburb</asp:Label>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>        
                <div class="small-12 medium-8 columns">
                <asp:RadioButtonList AutoPostBack="true" RepeatDirection="Horizontal" runat="server"
                        ID="SuburbSelection_rdbtn" CssClass="radio-list" OnSelectedIndexChanged="SuburbSelection_rdbtn_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                    </asp:RadioButtonList>
       <div id="Suburb_cbl_div" runat="server" visible="false" class="small-12 medium-8 large-12 columns" 
            style="OVERFLOW-Y:scroll;height:250px;margin-top:0px;margin-bottom:10px">
               <%-- <asp:ListBox ID="ListBox_Suburb" runat="server" SelectionMode="Multiple"></asp:ListBox>--%>
           <div class="small-12 large-5 large-push-1">
            <asp:CheckBoxList ID="Suburb_CBList"  RepeatLayout="Flow"   TextAlign="Right" runat="server"></asp:CheckBoxList>
        </div>
           </div>
                    </div>
                </ContentTemplate>
 
            </asp:UpdatePanel>
    </div>
    
   
    <br />

    <div class="row" style="margin-left:55%;">
        <asp:Button ID="btn_SaveOperatingSites" runat="server" Text="Save" CssClass="button radius" 
        OnClick="btn_SaveOperatingSites_Click"   />
    </div>
</asp:Content>
