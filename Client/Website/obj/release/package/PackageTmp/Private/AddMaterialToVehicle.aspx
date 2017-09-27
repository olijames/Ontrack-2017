<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="AddMaterialToVehicle.aspx.cs" Inherits="Electracraft.Client.Website.AddMaterialToVehicle" %>

<%@ Register Src="~/UserControls/DateControl.ascx" TagName="DateControl" TagPrefix="controls" %>

<%--<%@ Register Src="~/UserControls/MaterialForm.ascx" TagName="MaterialForm" TagPrefix="controls" %>
<%@ Register Src="~/UserControls/RegisterIndividual.ascx" TagName="UserDetails" TagPrefix="controls" %>--%>

<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentTitle" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBody" runat="server">
    
<h3>Add materials to my vehicle</h3>

 <div class = "column" style = "background-color: lightyellow">
        <asp:Label ID="label3" runat="server" text="Vehicle Driver"/> 
        <asp:DropDownList ID="ddCompanyContacts" DataTextField="" runat="server"></asp:DropDownList>
      </div>

    <div class="small-12 medium-4 columns">

        <asp:DropDownList ID="ddlMaterialCategory" runat="server" onchange="GetMaterials($(this).val()); CheckShowNewMaterial()"></asp:DropDownList>


    </div>




 <script type="text/javascript">
        $(document).ready(function () {

            $('#dateDateControlInput').on('change', function (e) { checkAppointmentEnabled(); });
            $('#txtDateControlDate').on('change', function (e) { checkAppointmentEnabled(); });

            checkAppointmentEnabled();
            GetMaterials($('[id$=ddlMaterialCategory]').val());

            CheckShowNewMaterial();
        });

        function GetMaterials(cat) {
            if (cat != '') {
                $.ajax({
                    type: "POST",
                    url: "TaskDetails.aspx/GetMaterialCategoryItems",
                    data: "{'MaterialCategoryID':'" + cat + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var res = jQuery.parseJSON(msg.d);
                        var catDDL = $('.ddl-materials');
                        catDDL.empty();
                        $.each(res, function (key, val) {
                            catDDL.append($('<option></option>').val(key).html(val));
                        });
                    }
                });
            }
        }

        function CheckShowNewMaterial() {
            var cat = $('[id$=ddlMaterialCategory]').val();
            if (cat == 'ffffffff-ffff-ffff-ffff-ffffffffffff' || cat == 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF') {
                $('#new-material-form').slideDown();
            }
            else {
                $('#new-material-form').slideUp();
            }
        }
    </script>
    </asp:Content>