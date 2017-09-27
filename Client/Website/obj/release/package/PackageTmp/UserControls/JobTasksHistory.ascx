<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobTasksHistory.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.JobTasksHistory" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
<%@ Import Namespace="Electracraft.Framework.Utility" %>

<asp:Repeater ID="rpTasks" runat="server">
    <HeaderTemplate>
        <div class="row section">
            <div class="small-12 columns">
            <h2>Tasks</h2>

    </HeaderTemplate>
    <ItemTemplate>
        <ItemTemplate>
            <div class="row ">
                <div class="small-12 columns">
                    <h4><%# Eval("TaskName") %></h4>
                    <p><%# Eval("Status").ToString() == "Complete" ? "Complete" : "" %></p>
                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>Task Type:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# ((DOTask)Container.DataItem).TaskType.ToString() %></p>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="<%# ((DOTask)Container.DataItem).AmendedBy != Constants.Guid_DefaultUser %>">
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                    <p>Amended:</p>
                            </div>
                            <div class="small-12 medium-8 columns">
                                <p><%# GetContactName(((DOTask)Container.DataItem).AmendedBy, ((DOTask)Container.DataItem).TaskID) %> - <%# ((DOTask)Container.DataItem).AmendedDate.ToString("dd/MM/yyyy hh:mmtt") %></p>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>Contractor:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# GetContactName(((DOTask)Container.DataItem).ContractorID, ((DOTask)Container.DataItem).TaskID) %></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>Requested By:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# GetContactName(((DOTask)Container.DataItem).CreatedBy, ((DOTask)Container.DataItem).TaskID) + " (" + GetContactName(((DOTask)Container.DataItem).TaskOwner, ((DOTask)Container.DataItem).TaskID) + ")"%></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>Start Date:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# ParentPage.CurrentBRJob.GetTaskStartTimeText((DOTask)Container.DataItem) %></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>End Date:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# ParentPage.CurrentBRJob.GetTaskEndTimeText((DOTask)Container.DataItem) %></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="small-12 columns">
                            <p><%# Eval("Description").ToString().Replace("\r\n", "<br />") %></p>
                        </div>
                    </div>
                    <asp:PlaceHolder runat="server" ID="phTaskQuote" Visible="<%# QuoteVisible(((DOTask)Container.DataItem)) %>">
                        <div class="row">
                            <div class="small-12 medium-4 columns">
                                <p>Quote (<%# GetQuoteStatus(((DOTask)Container.DataItem))%>):</p>
                            </div>
                            <div class="small-12 medium-8 columns">
                                <p>
                                    <asp:PlaceHolder runat="server" Visible="<%# LMSplitVisible(((DOTask)Container.DataItem)) %>">Labour: <%# GetQuoteLabour(((DOTask)Container.DataItem)).ToString("C") %><br />
                                        Materials: <%# GetQuoteMaterial(((DOTask)Container.DataItem)).ToString("C") %><br />
                                    </asp:PlaceHolder>
                                    Total: <%# GetQuoteTotal(((DOTask)Container.DataItem)).ToString("C") %><br />

                                    <asp:PlaceHolder runat="server" Visible="<%# CanAccept(((DOTask)Container.DataItem)) %>">Margin (%):&nbsp;<input type="text" name="txtTaskMargin<%# Eval("TaskID").ToString() %>" id="txtTaskMargin<%# Eval("TaskID").ToString() %>" /><br />
                                        <asp:LinkButton ID="btnAcceptQuote" runat="server" Text="Accept Quote" OnClick="btnAcceptQuote_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder runat="server" Visible="<%# CanDecline(((DOTask)Container.DataItem)) %>">

                                        <%--                            <asp:Button ID="btnDeclineQuote" runat="server" Text="Decline Quote" OnClick="btnDeclineQuote_Click" CommandArgument='<%# Eval("TaskID").ToString() %>'  />
                                        --%>
                                    </asp:PlaceHolder>
                                </p>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div style="margin: 0.625em 0">
                        
                        <asp:LinkButton ID="btnViewTask" runat="server" Text="View Task" OnClick="btnViewTask_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </ItemTemplate>
    <FooterTemplate>
        <asp:PlaceHolder runat="server" Visible='<%# rpTasks.Items.Count == 0 %>'>No tasks currently listed.
        </asp:PlaceHolder>
            </div>
        </div>
    </FooterTemplate>

</asp:Repeater>
    <asp:PlaceHolder runat="server" ID="phNoTasks">No tasks selected.</asp:PlaceHolder>


