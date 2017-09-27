<%@ Page Title="" Language="C#" MasterPageFile="~/Private/PrivatePage.Master" AutoEventWireup="true"
    CodeBehind="TimeSheetView.aspx.cs" Inherits="Electracraft.Client.Website.Private.TimeSheetView" %>

<%@ Import Namespace="Electracraft.Framework.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentTitle" runat="server">
    Time Sheet View
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentBody" runat="server">
    <div class="row">
        <div class="small-12 columns">
            <h3>
                Time Sheet View</h3>
            <h4>
                <%# Contact.DisplayName %></h4>
        </div>
    </div>
    <div class="row">
        <div class="small-12 columns">
              <%--<asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" ><i class="fi-arrow-left"></i> Back</asp:LinkButton>--%>
            <a href="<%# ResolveClientUrl(HttpContext.Current.Request.UrlReferrer.ToString()) %>"><i class="fi-arrow-left"></i> Back</a>
        </div>
    </div>
    <asp:PlaceHolder runat="server" Visible="<%# Authorised %>">
        <div class="row">
            <div class="small-12 columns">
                <asp:GridView ID="gvTimeSheet" runat="server" AutoGenerateColumns="false">
                    <Columns>
                         <asp:TemplateField HeaderText="Day">
                            <ItemTemplate>
                                <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                    <%# DateAndTime.DisplayDay((DateTime)Eval("LabourDate")) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                    <%# DateAndTime.DisplayShortDate((DateTime)Eval("LabourDate")) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job" DataField="JobName" />
                        <asp:TemplateField HeaderText="Number">
                            <ItemTemplate>
                                <a href="<%# ResolveClientUrl("~/private/JobSummary.aspx?jobid=" + Eval("JobID").ToString()) %>"><%# string.IsNullOrEmpty(Eval("JobNumberAuto").ToString()) ? "(none)" : Eval("JobNumberAuto") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Task" DataField="TaskName" />
                        <asp:BoundField HeaderText="Description" DataField="Description" />
                        <asp:TemplateField HeaderText="Hours">
                            <ItemTemplate>
                                <div class='<%# (bool)Eval("Active") ? "" : "x-out" %>'>
                                    <%# DateAndTime.DisplayShortTimeString(((int)Eval("EndMinute")) - ((int)Eval("StartMinute"))) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chargeable">
                            <ItemTemplate>
                                <%# ((bool)Eval("Chargeable")) ? "Yes" : "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input type="button" value="More..." onclick="showmore('<%# Eval("TaskLabourID").ToString() %>')" />
                                <div id='more<%# Eval("TaskLabourID").ToString() %>' class="tv_more" style="display: none;
                                    position: fixed; top: 0; left:0; width: 100%; height: 100%; z-index:1;
                                    background: rgba(0,0,0,0.7)"  onclick="hidemore()">
                                            <div style="background-color: #fff; max-width:970px; margin: 3rem auto; padding:1rem;">
                                                Job Number:
                                                <%# Eval("JobNumber") %><br />
                                                Customer:
                                                <%# Eval("Customer") %><br />
                                                <%# Eval("JobDescription").ToString().Replace("/r/n", "<br />") %><br />
                                                <input type="button" value="Close" onclick='hidemore()' />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:PlaceHolder>

  <%--  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                    <Columns>
                         <asp:BoundField HeaderText="Day" DataField="Day" />
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <div class='<%# (bool)Eval("timeSheet.Active") ? "" : "x-out" %>'>
                                    <%# DateAndTime.DisplayShortDate((DateTime)Eval("timeSheet.LabourDate")) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job" DataField="timeSheet.JobName" />
                        <asp:TemplateField HeaderText="Number">
                            <ItemTemplate>
                                <a href="<%# ResolveClientUrl("~/private/JobSummary.aspx?jobid=" + Eval("timeSheet.JobID").ToString()) %>"><%# string.IsNullOrEmpty(Eval("timeSheet.JobNumberAuto").ToString()) ? "(none)" : Eval("timeSheet.JobNumberAuto") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Task" DataField="timeSheet.TaskName" />
                        <asp:BoundField HeaderText="Description" DataField="timeSheet.Description" />
                        <asp:TemplateField HeaderText="Hours">
                            <ItemTemplate>
                                <div class='<%# (bool)Eval("timeSheet.Active") ? "" : "x-out" %>'>
                                    <%# DateAndTime.DisplayShortTimeString(((int)Eval("timeSheet.EndMinute")) - ((int)Eval("timeSheet.StartMinute"))) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chargeable">
                            <ItemTemplate>
                                <%# ((bool)Eval("timeSheet.Chargeable")) ? "Yes" : "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input type="button" value="More..." onclick="showmore('<%# Eval("timeSheet.TaskLabourID").ToString() %>')" />
                                <div id='more<%# Eval("timeSheet.TaskLabourID").ToString() %>' class="tv_more" style="display: none;
                                    position: fixed; top: 0; left:0; width: 100%; height: 100%; z-index:1;
                                    background: rgba(0,0,0,0.7)"  onclick="hidemore()">
                                            <div style="background-color: #fff; max-width:970px; margin: 3rem auto; padding:1rem;">
                                                Job Number:
                                                <%# Eval("timeSheet.JobNumber") %><br />
                                                Customer:
                                                <%# Eval("timeSheet.Customer") %><br />
                                                <%# Eval("timeSheet.JobDescription").ToString().Replace("/r/n", "<br />") %><br />
                                                <input type="button" value="Close" onclick='hidemore()' />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
   <%-- <div class="row">
        <div class="small-12 columns">
          <%--  <a href="<%# ResolveClientUrl("~/private/timesheets.aspx") %>">Back</a>--%>
             <%-- <a href="<%# ResolveClientUrl(HttpContext.Current.Request.UrlReferrer.ToString()) %>">Back</a>
        </div>
    </div>--%>


    <script type="text/javascript">
        function showmore(id) {
            $('#more' + id).show();
        }
        function hidemore() {
            $('.tv_more').hide();
        }
    </script>
</asp:Content>
