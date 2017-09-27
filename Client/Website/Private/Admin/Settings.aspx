<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="Settings.aspx.cs" Inherits="Electracraft.Client.Website.Private.Admin.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Settings
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>Settings</h3>
        </div>
    </div>
    <asp:PlaceHolder runat="server" Visible="false">
        <div class="row">
            <div class="small-12 columns">
                <asp:Button ID="btnDeleteAll" runat="server" OnClick="btnDeleteAll_Click" Text="Delete All Data" />
                <ajaxToolkit:ConfirmButtonExtender runat="server" TargetControlID="btnDeleteAll"
                    ConfirmText="This will delete all data from the database. Are you sure you want to proceed?">
                </ajaxToolkit:ConfirmButtonExtender>
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="small-12 medium-5 columns">
            Email recipient for testing (leave blank to send emails to actual recipients):
        </div>
        <div class="small-12 medium-7 columns">
            <asp:TextBox ID="txtTestEmailRecipient" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="small-12 medium-5 columns">
            Website base path (must start with http)
        </div>
        <div class="small-12 medium-7 columns">
            <asp:TextBox ID="txtWebsiteBasePath" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="small-12 large-3 columns right">
            <asp:Button ID="btnSave" Width="100%" CssClass="button radius" runat="server" Text="Save Changes" OnClick="btnSave_Click" />
        </div>


    </div>
    <br />
    <br />
    <%--  //Trade Categories--%>
    <div class="row">
        <asp:Panel ID="TradeCatgories" runat="server">
            <div class="row">
                <div class="small-12 columns">
                    <h3 align="center">Trade Categories</h3>
                </div>
                <br />
                <div class="row">
                    <div class="info medium-text-center small-12 columns">
                        <asp:Label runat="server" CssClass="warning" Font-Bold="true" ClientIDMode="Static"
                            ID="StatusText" Visible="False"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="small-12 large-3 medium-3 columns">
                        <asp:Label runat="server" Width="90%" Font-Size="Large"> Add Trade Category</asp:Label>
                    </div>
                    <div class="small-12 large-6 columns">
                        <asp:TextBox ID="TradeCategoryTextBox" Width="90%" onkeypress="HideLabel()" placeholder="<Trade Category Name>" runat="server" Font-Size="Smaller"></asp:TextBox>
                    </div>
                    <div class="small-12 large-3 columns">
                        <asp:Button CssClass="button radius" Width="90%" runat="server" Text="Add" OnClick="AddTradeCategory_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 large-3 medium-3 columns">
                        <asp:Label runat="server" Width="90%" Font-Size="Large"> Add SubTradeCategory</asp:Label>
                    </div>
                    <div class="small-12 medium-3 columns">
                        <asp:Label runat="server" Width="90%" Font-Size="Large"> Select Trade Category</asp:Label>
                    </div>

                    <div class="small-12 medium-4 large-6 medium-centered columns">
                        <asp:DropDownList ID="TradeCategories_DDL" runat="server" Width="90%"></asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="small-12 medium-3 large-6 columns large-push-3">
                        <asp:TextBox ID="SubTradeCategory_txt" Width="90%" onkeypress="HideLabel()" placeholder="<Sub-Trade Category Name>" runat="server" Font-Size="Smaller"></asp:TextBox>
                    </div>
                    <div class="small-12 large-3 columns">
                        <asp:Button ID="AddSubTradeCategory" CssClass="button radius" Width="90%" runat="server" Text="Add" OnClick="AddSubTradeCategory_Click"/>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="small-12 medium-3 columns">
                        <asp:Label runat="server" Font-Size="Large">Add Region</asp:Label>
                    </div>

                    <div class="small-12 medium-4 large-6 columns">

                        <asp:TextBox ID="Txt_NewRegion" Width="90%" onkeypress="HideLabel()" runat="server" Font-Size="Smaller" Placeholder="<Enter new region>">
                        </asp:TextBox>
                    </div>
                    <div class="small-12 medium-5 large-3 columns">
                        <asp:Button CssClass="button radius" Width="90%" runat="server" Text="Add"
                            OnClick="AddRegion_Click" />
                    </div>
                </div>

                <div class="row">
                    <br />
                    <div class="small-12 medium-3 columns">
                        <asp:Label runat="server" Font-Size="Large">Add District</asp:Label>
                    </div>
                    <div class="small-12 medium-2 columns">
                        Select Region
                    </div>
                    <br />
                    <div class="small-12 medium-4 large-6 columns">
                        <asp:DropDownList ID="Ddl_RegionDD" runat="server" Width="90%" EnableTheming="False"
                            OnSelectedIndexChanged="RegionDD_SelectedIndexChanged"
                            OnDataBound="RegionDD_DataBound" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="small-12 medium-3 large-6 columns large-push-3">

                        <asp:TextBox ID="TxtBx_District" Width="90%" onkeypress="HideLabel()" runat="server" Font-Size="Smaller" Placeholder="<Enter new District>">
                        </asp:TextBox>
                    </div>
                    <div class="small-12 medium-5 large-3 columns">
                        <asp:Button ID="Add_District_btn" CssClass="button radius" Width="90%" runat="server" Text="Add"
                            OnClick="AddDistrict_Click" />
                    </div>
                </div>



                <br />
                <div class="row">
                    <div class="info medium-text-center small-12 columns">
                        <asp:Label runat="server" CssClass="warning" Font-Bold="true" ClientIDMode="Static"
                            ID="SuburbStatusText" Visible="False"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-3 columns">

                        <asp:Label runat="server" Font-Size="Large">Add Suburb</asp:Label>
                    </div>
                    <div class="small-12 medium-2 columns">
                        Select Region
                    </div>
                    <br />
                    <div class="small-12 medium-4 large-6 columns">
                        <asp:DropDownList ID="RegionDD" runat="server" Width="90%" EnableTheming="False"
                            OnSelectedIndexChanged="RegionDD_SelectedIndexChanged"
                            OnDataBound="RegionDD_DataBound" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="small-12 medium-8 columns medium-push-3">
                        Select District
                    </div>
                </div>

                <div class="row">
                    <div class="small-12 medium-4 large-6 columns medium-push-3">
                        <asp:DropDownList ID="Districts_DDL" runat="server" Width="90%">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="small-12 medium-3 columns">
                    <asp:Label runat="server" Font-Size="Large" Style="display: block; color: white">Add Region</asp:Label>
                </div>
                <div class="small-12 medium-3 large-6 columns">

                    <asp:TextBox ClientIDMode="Static" ID="Txt_NewSuburb" Width="90%" runat="server" Font-Size="Smaller"
                        Placeholder="<Enter new suburb>" onkeypress="HideLabel()">
                    </asp:TextBox>
                </div>
                <div class="small-12 medium-5 large-3 columns">
                    <asp:Button ID="ADD_Suburb_btn" Width="90%" CssClass="button radius" runat="server" Text="Add" OnClick="AddSuburb_Click" />
                </div>
            </div>
            <%-- <div class="info medium-text-center">
          <asp:Label runat="server" ClientIDMode="Static" ID="SuccessText" Visible="False">Trade Category added successfully!</asp:Label>
              </div>
          <div class="errorText">
          <asp:Label runat="server" ClientIDMode="Static" ID="ErrorText" Visible="False" ForeColor="Red">Trade Category already exists!</asp:Label>
              </div>
            --%>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div class="row large-text-center">
                <%--   <h3 align="center">Link Trade Categories to Companies</h3> 
                --%>
                <div class="row">
                </div>
            </div>
    
            <asp:GridView ID="gvSuburb" runat="server" AutoGenerateColumns="false" Width="100%"  CssClass="Grid" EnableModelValidation="True"
                                                                                                     >
                <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%"  CssClass="Grid" EnableModelValidation="True"
                                                                                                     OnRowDataBound="gvSuburb_OnRowDataBound">--%>



            </asp:GridView>



                   
     
    </asp:Panel>
        </div>
    <script language="javascript" type="text/javascript">
        function HideLabel() {
            document.getElementById('StatusText').style.display = 'none';


        }
    </script>
</asp:Content>
