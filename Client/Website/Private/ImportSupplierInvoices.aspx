<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportSupplierInvoices.aspx.cs" Inherits="Electracraft.Client.Website.ImportSupplierInvoices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
    <asp:FileUpload id="FileUploadControl" runat="server"/>
    <asp:Button runat="server" id="UploadButton" text="Up load" onclick="UploadButton_Click" />
    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
    </div>
    </form>
</body>
</html>
