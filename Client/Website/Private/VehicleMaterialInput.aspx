<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleMaterialInput.aspx.cs" Inherits="Electracraft.Client.Website.Private.VehicleMaterialInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="DDVehicleSelect" runat="server">

        </asp:DropDownList>

         <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" Width="100%"  CssClass="Grid" EnableModelValidation="True"
            visible="true" OnRowDataBound="gvParent_OnRowDataBound"  DataKeyNames="SupplierReference">

             </asp:GridView>

    </div>
    </form>
</body>
</html>
