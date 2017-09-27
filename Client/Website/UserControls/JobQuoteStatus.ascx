<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobQuoteStatus.ascx.cs" Inherits="Electracraft.Client.Website.UserControls.JobQuoteStatus" %>
<%--            <asp:PlaceHolder ID="phQuotes" runat="server">
                <div class="row">
                    <div class="small-12 columns">
                        <h4>
                            Quotes</h4>
                        <controls:Quotes ID="Quotes" runat="server"></controls:Quotes>
                    </div>
                </div>
            </asp:PlaceHolder>
--%>
            <asp:PlaceHolder ID="phQuoteStatus" runat="server">
                <div class="row">
                <div class="small-12 columns">
                    Quote Status: <asp:Literal ID="litQuoteStatus" runat="server"></asp:Literal> (<asp:Literal ID="litQuoteStatusDate" runat="server"></asp:Literal>).<br />
                    Total quoted amount: <asp:Literal runat="server" ID="litQuoteAmountWithMarginStatus"></asp:Literal><br />
                    Terms and conditions:
                    <div class="tandc">
                        <asp:Literal ID="litTandCStatus" runat="server"></asp:Literal>
                    </div>
                </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phQuoteApprove" runat="server">
                <div class="row">
                <div class="small-12 columns">
                    <h4>Quote Submitted</h4>                    
                    Total quoted amount: <asp:Literal runat="server" ID="litQuoteAmountWithMarginApprove"></asp:Literal><br />
                    <asp:Button ID="btnAcceptQuote" runat="server" Text="Accept Quote" OnClick="btnApproveQuote_Click" />
                    
                  <asp:Button ID="btnDeclineQuote" Visible="false" runat="server" Text="Decline Quote" OnClick="btnDeclineQuote_Click" />

                    Terms and conditions:
                    <div class="tandc">
                        <asp:Literal ID="litTandCApprove" runat="server"></asp:Literal>
                    </div>
                </div>
                </div>
            </asp:PlaceHolder>


            <asp:PlaceHolder ID="phQuotedTasks" runat="server">
                <div class="row">
                <div class="small-12 columns">
                    <h4>Accepted Quoted Tasks</h4>

                    There are <asp:Literal ID="litQuotedTaskCount" runat="server"></asp:Literal> accepted quoted tasks for a total quoted value of <asp:Literal ID="litQuoteTotalAmount" runat="server"></asp:Literal>.<br />
                    Margin to apply to quote (%): <asp:TextBox ID="txtQuoteMarginPercent" runat="server"></asp:TextBox><br />
                    Terms and Conditions:<br />
                    <asp:TextBox ID="txtTermsAndConditions" runat="server" TextMode="MultiLine">
                    </asp:TextBox>
                    <asp:Button ID="btnSubmitQuote" runat="server" OnClick="btnSubmitQuote_Click" Text="Submit Quote" />
                    <ajaxToolkit:ConfirmButtonExtender ID="cbeSubmitQuote" runat="server" 
                    ConfirmText="This job has tasks with quotes that have not been accepted. Do you still want to submit the quote?" TargetControlID="btnSubmitQuote">
                    </ajaxToolkit:ConfirmButtonExtender>
                </div>
                </div>
            </asp:PlaceHolder>
