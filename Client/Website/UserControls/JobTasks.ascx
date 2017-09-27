<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobTasks.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.JobTasks" %>
<%@ Import Namespace="Electracraft.Framework.DataObjects" %>
    <script src="../Scripts/jquery-1.7.min.js"></script>
 <script type="text/javascript">
     function GetTaskList() {
            // var jobId = 5;       
            $.getJSON("/api/Tasks/TaskByJobID/"+"<%#Job.JobID%>",
            function (data) {
                $('#tasksList').empty(); // Clear the table body.

                // Loop through the list of products.
                $.each(data, function (key, val) {
                    // Add a table row for the product.
                    var row = '<td>' + val.TaskName + '</td>';
                    $('<tr/>', { text: row })  // Append the name.
                        .appendTo($('#tasksList'));
                });
            });
        }
          <%-- //var pageInfo = [{ jobId: ' <%# Job.JobID %> ', sessionId:  ' '}];--%>
        $(document).ready(GetTaskList);
     $.ajax({
         url: urlString,
         type: 'POST',
         data: <%# ParentPage.CurrentSessionContext.CurrentTask %>,
         dataType: 'json',
         success: function (data) { console.log(data); }
     });
 </script>
<%--<asp:UpdatePanel runat="server">
    <ContentTemplate>--%>
<%--        <div class="row section" > --%>
            <asp:Label runat="server" ID="Error" CssClass="error"></asp:Label> 
            <div class="small-10 medium-5 columns">
                <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnAddTask_Click"><i class="fi-plus"></i></asp:LinkButton><!--added jared--><br /> --%>   
            </div>
<%--        </div>--%>
<asp:Repeater ID="rpTaskList" runat="server">
    <HeaderTemplate>
       
        <div class="row section">
        
            <div class="small-12 columns">
                
    </HeaderTemplate>
    <ItemTemplate>
      
</Table>
        <div class="row site-address">
            <div class="small-10 medium-5 columns" >
                 <%-- ADD CODE HERE TO REPEAT WITH ALL TASK BUTTONS --%>
                <asp:LinkButton ID="btnSelectJob" runat="server" CssClass=<%# Eval("taskClass") %>  OnClick="btnViewTask_Click" CommandArgument='<%# Eval("Task.TaskID").ToString() %>' Width="100%">
                    <span style="display: block; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; font-weight: bold;"><%# "<i class=\"fi-play\"></i>&nbsp;&nbsp;#"+JobNumberAuto+"&nbsp;-&nbsp;"+"&nbsp;&nbsp;"+Eval("Task.TaskNumber").ToString().PadLeft(3, '0')+"&nbsp;&nbsp;" + Eval("Task.TaskName") %> &nbsp;<%--<%#  Eval("Status").ToString()%>--%></span><%--<%# Eval("Status").ToString() == "Complete" ? "&nbsp;(Complete)" : "" %>--%>
                    <span style="font-size: 0.8em; margin-left: 1.3em;visibility:hidden; display: block;" class="show-for-medium-up">Starts:&nbsp;<%# Eval("taskStartDate") %>&nbsp;&nbsp;|&nbsp;&nbsp;Ends:&nbsp;<%#Eval("taskEndDate") %></span>

                    <%-- Contractor and TradeCategoryName --%>
                      
                    <span style="font-size: 0.8em;visibility:hidden; display: block; margin-left: 1.3em;" class="show-for-medium-up">Contractor:&nbsp;<%# GetContactName(Guid.Parse(Eval("Task.ContractorID").ToString()), Guid.Parse(Eval("Task.TaskID").ToString())) %></span>
                    <span style="font-size: 0.8em;visibility:hidden; margin-left: 1.3em; display: block;" class="show-for-medium-up">Trade Category Name:&nbsp;<%# GetTradeCategoryName(Guid.Parse(Eval("Task.TradeCategoryID").ToString())) %></span>
                    </asp:LinkButton>
                 </div>
            <%-- HISTORY BUTTON --%>
            <div class="small-12 medium-4 columns site-details show-for-medium-up" style="width: 30%;border: none !important;"><span style="display: block; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; margin-bottom: 0.4em; "><%# Eval("Task.Description").ToString().Replace("\r\n", "<br />") %></span>    
                 <asp:PlaceHolder runat="server" Visible='<%# !ShowFull && HistoryVisible((Guid)Eval("Task.TaskID")) %>'>
                    <asp:Button ID="btnHistory" runat="server" Text="Task History" CssClass="radius button small" OnClick="btnViewTaskHistory_Click" CommandArgument='<%# Eval("Task.TaskID").ToString() %>' />   
                </asp:PlaceHolder>  
                
            </div> 
            
            <div class="small-2 columns" style="padding-top:2%;padding-left:1%;">
               
              
             <%-- <asp:LinkButton runat="server" ID="CompleteIconBtnMobileOnly" OnClick="CompleteIconBtn_Click"  Visible='<%# Eval("enabledCompBtn")%>'
                    Font-Size="X-Large" CssClass='<%# bool.Parse(Eval("visible").ToString())==false?"fi-x show-for-small-only":"fi-check show-for-small-only" %>' 
                    CommandArgument='<%# Eval("Task.TaskID") %>'>
             </asp:LinkButton>--%>
        
             <span><asp:LinkButton runat="server" ID="CompleteIconBtnMobileOnly" OnClick="CompleteIconBtn_Click"  Visible='<%# Eval("enabledCompBtn")%>'
                    Font-Size="X-Large" CssClass='<%# bool.Parse(Eval("visible").ToString())==false?"fi-x show-for-small-only":"fi-check show-for-small-only" %>' 
                    CommandArgument='<%# Eval("Task.TaskID") %>'>
             </asp:LinkButton>
               <%-- <asp:LinkButton runat="server" ID="lbAddMaterials" OnClick="btnAddMaterials_Click" text="fjkl">  <i class="fi-plus"></i>--%>
          <%--   </asp:LinkButton>--%>
            </span>



              </div>
             <div class="small-12 medium-3 columns site-details show-for-medium-up">
                                  <div class="medium-10 columns" style="float:right;">
                                      <div class="medium-10 columns" style="margin-bottom:0px;">
                <asp:Button runat="server" ID="Complete_btn" Height="80%" Text="Complete" Width="80%" Visible='<%# Eval("visible")  %>' OnClick="Complete_Btn_Click" 
                    Enabled='<%# Eval("enabledCompBtn")%>' CssClass='<%# bool.Parse(Eval("enabledCompBtn").ToString())==true?"button radius tiny":"button secondary radius tiny" %>' 
                    CommandArgument='<%# Eval("Task.TaskID") %>'/>
                </div>
                                   <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
<%--                   <i class='<%# GetIconForComplete_btn((Guid)Eval("TaskID")) %>' ></i>--%>
                                           <i class='<%# Eval("iconComp")%>' ></i>
              </div>
            <%--     <div class="medium-10 columns"  >
                 <asp:Button runat="server" ID="Invoiced_btn" Text="Invoiced" Width="80%" Visible='<%#IfMobile() %>' Enabled='<%# Eval("enabledInvBtn") %>' CssClass='<%# bool.Parse(Eval("enabledInvBtn").ToString())==true?"button radius tiny":"button secondary radius tiny"%>' OnClick="Invoiced_btn_Click"  CommandArgument='<%# Eval("Task.TaskID") %>'/>
                </div>
                    <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
                <i class='<%# Eval("iconInv") %>' ></i>
              </div>
               <div class="medium-10 columns" >
                 <asp:Button runat="server" ID="Paid_btn" Text="Paid" Width="80%" Visible='<%#IfMobile() %>' Enabled='<%# Eval("enabledPaidBtn") %>' CssClass='<%# bool.Parse(Eval("enabledPaidBtn").ToString())==true?"button radius tiny":"button secondary radius tiny" %>' OnClick="Paid_btn_Click" CommandArgument='<%# Eval("Task.TaskID") %>'/>
                  </div>
                    <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
                    <i class='<%# Eval("iconPaid") %>' ></i>
              </div>--%>
             </div>
            </div>
            <hr />
            </div>
    </ItemTemplate>
     <FooterTemplate>
        <asp:PlaceHolder runat="server" Visible='<%# rpTaskList.Items.Count == 0 %>'>No tasks currently listed.
        </asp:PlaceHolder>
            </div>
        </div>
    </FooterTemplate>
</asp:Repeater>
<%--<asp:Repeater ID="rpTasks" runat="server" OnItemDataBound="rpTasks_ItemDataBound">
    <HeaderTemplate>
        <div class="row section">
            <div class="small-12 columns">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row site-address">
            <div class="small-12 medium-5 columns" >
                <asp:LinkButton ID="btnSelectJob" runat="server" CssClass=<%# GetTaskClass(Eval("TaskID").ToString()) %>  OnClick="btnViewTask_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' Width="100%"><span style="display: block; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; font-weight: bold;"><%# "<i class=\"fi-play\"></i>&nbsp;&nbsp;#"+JobNumberAuto+"&nbsp;-&nbsp;"+GetTaskNumber((DOTask)Container.DataItem)+"&nbsp;&nbsp;" + Eval("TaskName") %> &nbsp;<%--<%#  Eval("Status").ToString()%>--%></span><%--<%# Eval("Status").ToString() == "Complete" ? "&nbsp;(Complete)" : "" %>--%>
      <%-- %>          <span style="font-size: 0.8em; margin-left: 1.3em; display: block;">Starts:&nbsp;<%# ParentPage.CurrentBRJob.GetTaskStartTimeText((DOTask)Container.DataItem) %>&nbsp;&nbsp;|&nbsp;&nbsp;Ends:&nbsp;<%# ParentPage.CurrentBRJob.GetTaskEndTimeText((DOTask)Container.DataItem) %></span>
                    <%-- Contractor and TradeCategoryName --%>
        <%-- %>            <span style="font-size: 0.8em; margin-left: 1.3em; display: block;">Contractor:&nbsp;<%# GetContactName(((DOTask)Container.DataItem).ContractorID, ((DOTask)Container.DataItem).TaskID) %></span>
                    <span style="font-size: 0.8em; margin-left: 1.3em; display: block;">Trade Category Name:&nbsp;<%# GetTradeCategoryName(((DOTask)Container.DataItem).TradeCategoryID) %></span>
                </asp:LinkButton>
            </div>
            <div class="small-12 medium-4 columns site-details" style="border:0px"><span style="display: block; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; margin-bottom: 0.4em;"><%# Eval("Description").ToString().Replace("\r\n", "<br />") %></span> 
                <asp:PlaceHolder runat="server" Visible='<%# !ShowFull && HistoryVisible((Guid)Eval("TaskID")) %>'>
                    <asp:Button ID="btnHistory" runat="server" Text="Task History" CssClass="radius button small" OnClick="btnViewTaskHistory_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' />
                </asp:PlaceHolder>  
                </div>
            <div class="small-12 medium-3 columns site-details">
                               <div class="medium-10 columns" style="float:right;">
               <div class="medium-10 columns" style="margin-bottom:0px;">
<%--                 <asp:Button runat="server" ID="Complete_btn" Height="80%" Text="Complete" Width="80%" Visible='<%# IsCompleteVisible((Guid)Eval("TaskID")) %>' OnClick="Complete_Btn_Click" Enabled='<%# IfCompleted_btn_enabled((Guid)Eval("TaskID"))%>' CssClass='<%# IfCompleted_btn_enabled((Guid)Eval("TaskID"))==true?"button radius tiny":"button secondary radius tiny" %>' CommandArgument='<%# Eval("TaskID") %>'/>--%>
<%--                                    <asp:Button runat="server" ID="Complete_btn" Height="80%" Text="Complete" Width="80%" Visible='<%# IsCompleteVisible((DOTask)Container.DataItem)  %>' OnClick="Complete_Btn_Click" Enabled='<%# IfCompleted_btn_enabled((Guid)Eval("TaskID"))%>' CssClass='<%#  IfCompleted_btn_enabled((Guid)Eval("TaskID"))==true?"button radius tiny":"button secondary radius tiny" %>' CommandArgument='<%# Eval("TaskID") %>'/>--%>
              <%-- %>     </div>
                 <%--   <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
<%--                   <i class='<%# GetIconForComplete_btn((Guid)Eval("TaskID")) %>' ></i>--%>
                                         <%-- %>  <i class='<%# GetIconForComplete_btn((Guid)Eval("TaskID"))%>' ></i>

              </div>--%>
     <%-- %>      <div class="medium-10 columns"  >
                 <asp:Button runat="server" ID="Invoiced_btn" Text="Invoiced" Width="80%" Visible='<%#IfMobile() %>' Enabled='<%# IsInvoicedEnabled((Guid)Eval("TaskID")) %>' CssClass='<%# IsInvoicedEnabled((Guid)Eval("TaskID"))==true?"button radius tiny":"button secondary radius tiny" %>' OnClick="Invoiced_btn_Click"  CommandArgument='<%# Eval("TaskID") %>'/>
                </div>
                    <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
                <i class='<%# GetIconForInvoiced_btn((Guid)Eval("TaskID")) %>' ></i>
              </div>
               <div class="medium-10 columns" >
                 <asp:Button runat="server" ID="Paid_btn" Text="Paid" Width="80%" Visible='<%#IfMobile() %>' Enabled='<%# IfPaidEnabled((Guid)Eval("TaskID")) %>' CssClass='<%# IfPaidEnabled((Guid)Eval("TaskID"))==true?"button radius tiny":"button secondary radius tiny" %>' OnClick="Paid_btn_Click" CommandArgument='<%# Eval("TaskID") %>'/>
                  </div>
                    <div class="medium-2 pull-1 columns" style="padding-top:5%;" >
                    <i class='<%# GetIconForPaid_btn((Guid)Eval("TaskID")) %>' ></i>
              </div>
             </div>
            </div>
            <hr />
            </div>
    </ItemTemplate>
    <FooterTemplate>
        <asp:PlaceHolder runat="server" Visible='<%# rpTasks.Items.Count == 0 %>'>No tasks currently listed.
        </asp:PlaceHolder>
            </div>
        </div>
    </FooterTemplate>
</asp:Repeater>--%>

 <%-- </ContentTemplate>
</asp:UpdatePanel>--%>
<div class="jobtasks hide">
    <asp:PlaceHolder runat="server" ID="phNoTasks">No tasks selected.</asp:PlaceHolder>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <div class="row ">
                <div class="small-12 columns">
                    <div class="row">
                        <div class="small-12 medium-4 columns">
                            <p>Task Type:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# ((DOTask)Container.DataItem).TaskType.ToString() %></p>
                        </div>
                    </div>
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
                            <p>Contractor:</p>
                        </div>
                        <div class="small-12 medium-8 columns">
                            <p><%# GetContactName(((DOTask)Container.DataItem).TradeCategoryID, ((DOTask)Container.DataItem).TaskID) %></p>
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
                     <div class="row">
                        <div class="small-12 columns">
                            <p><%# ((DOTask)Container.DataItem).TradeCategoryID.ToString() %></p>
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
                                        <asp:Button ID="btnAcceptQuote" runat="server" Text="Accept Quote" OnClick="btnAcceptQuote_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' />
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
                        
                        <asp:Button ID="btnViewTask" runat="server" Text="View Task" OnClick="btnViewTask_Click" CommandArgument='<%# Eval("TaskID").ToString() %>' />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
     <h2>All Tasks</h2>
    <ul id="tasksList"></ul>
</div>

<%--<script type="text/javascript">
    var uri = 'api/Tasks';
    $(document)
        .ready(function() {
            //Send an AJAX request
            $.getJSON(uri)
                .done(function(data) {
                    //On success, 'data' contains a list of products.
                    $.each(data,
                        function(key, item) {
                            //Add a list item for the product.
                            $('<li>', { text: formatItem(item) }).appendTo($('#tasksList'));
                        });
                });
        });
    function formatItem(item) {
        return item.name + 'id:' + item.id;
    }
    //function find() {
    //    var id = $('#prodId').val();
    //    $.getJSON(uri + '/' + id)
    //        .done(function(data) {
    //            $('#products').text(formatItem(data));

    //        })
    //        .fail(function(jqXHR, textStatus, err) {
    //            $('#products').text('Error: ' + err);
    //        });
    //}
</script>--%>
   
<%--        <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.0.3.min.js"></script>--%>
