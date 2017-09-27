<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateControl.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.DateControl" %>
<%@ Register Assembly="Html5Asp" Namespace="Html5Asp" TagPrefix="controls" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:TextBox ID="txtDateControlDate" runat="server"></asp:TextBox>
<ajaxToolkit:CalendarExtender ID="ceDate" runat="server"  TargetControlID="txtDateControlDate" Format="dd/MM/yyyy" />
<controls:DateInput ID="dateDateControlInput" runat="server" />