using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using Electracraft.Framework.DataAccess;
using Electracraft.Framework.DataObjects;
using Electracraft.Framework.Utility;

namespace Electracraft.Framework.BusinessRules
{
    public class BRGeneral : BRBase
    {

        private DAGeneral _CurrentDAGeneral;
        private DAGeneral CurrentDAGeneral
        {
            get
            {
                if (_CurrentDAGeneral == null)
                    _CurrentDAGeneral = new DAGeneral(ConnectionString);
                return _CurrentDAGeneral;
            }
        }
        public decimal ApplyGST(decimal BaseAmount)
        {
            return (BaseAmount * 1.15m);
        }
        public void SendConfirmationEmail(string From, string to, string Subject, StringBuilder body) // !here!
        {
                MailMessage msg = new MailMessage(From, to, Subject, body.ToString());
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["emailClient"]);
                client.Port = 587;
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPassword"]);
                client.Send(msg);
        }
        public void SendEmail(string From, string To, string Subject, string Body) // !here!
        {
            if (Constants.DISABLE__EMAIL) return;
            //if (Subject == "Please verify your email for OnTrack") //return;//inserted by Jared
                List<string> Recipients = new List<string>();

            DOGeneralSetting TestEmailAddress = SelectGeneralSetting(Constants.Setting_TestEmailRecipient);
            if (TestEmailAddress != null && !string.IsNullOrEmpty(TestEmailAddress.SettingValue))
            {
                
                foreach (string email in TestEmailAddress.SettingValue.Split(new char[] { ';' }))
                {                    
                    Recipients.Add(email);
                }
            }
            //Recipients.Add(To);//mandeep added to send to actual recipient
         //   else
         //   {
         ////       Recipients.Add(To);
         //   }

            foreach (string Recipient in Recipients)
            {
                MailMessage msg = new MailMessage(From, Recipient, Subject, Body);
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["emailClient"]);
                client.Port = 587;
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPassword"]);
                client.Send(msg);

                if (Recipients.Count > 1)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Creates a general setting.
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public DOGeneralSetting CreateGeneralSetting(string SettingName)
        {
            DOGeneralSetting Setting = new DOGeneralSetting();
            Setting.SettingID = Guid.NewGuid();
            Setting.SettingName = SettingName;
            Setting.CreatedBy = Guid.Empty;
            return Setting;
        }

        /// <summary>
        /// Selects a general setting.
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public DOGeneralSetting SelectGeneralSetting(string SettingName)
        {
            return CurrentDAGeneral.SelectObject(typeof(DOGeneralSetting), "SettingName = {0}", SettingName) as DOGeneralSetting;
        }

        /// <summary>
        /// Saves a general setting. The setting name must be unique.
        /// </summary>
        /// <param name="Setting"></param>
        public void SaveGeneralSetting(DOGeneralSetting Setting)
        {
            CurrentDAGeneral.SaveObject(Setting);
        }

        /// <summary>
        /// Selects the website base path .
        /// </summary>
        /// <returns></returns>
        public string SelectWebsiteBasePath()
        {
            DOGeneralSetting basepath = SelectGeneralSetting(Constants.Setting_WebsiteBasePath);
            return basepath.SettingValue;
        }

        /// <summary>
        /// Generates a temporary password.
        /// </summary>
        /// <returns></returns>
        public string GenerateTempPassword()
        {
            Guid g = Guid.NewGuid();
            string Pass = g.ToString().Substring(0, 13).Replace("-", "");
            return Pass;
        }

        public void DeleteAllData()
        {
            CurrentDAGeneral.DeleteAllData();
        }


        /// <summary>
        /// Sends an email to company manager notifying that someone wants to join the company.
        /// </summary>
        /// <param name="CompanyContact"></param>
        /// <param name="PendingContact"></param>
        public void SendContactCompanyPendingEmail(DOContact CompanyContact, DOContact PendingContact)
        {
            string Subject = PendingContact.DisplayName + " wants to join your company on Ontrack";
            string Body = PendingContact.DisplayName + " wants to join your company on Ontrack.<br />";
            Body += "<br /><a href=\"" + SelectWebsiteBasePath() + "/private/EmployeeInfo.aspx?company=" + CompanyContact.ContactID.ToString() + "\">Click here</a> to approve or decline this request.";
            if (Constants.EMAIL__TESTMODE)
            {
                Body += "<br/><br/>";
                Body += "Sent By: " + PendingContact.DisplayName + "<br />";
                Body += "Sent To: " + CompanyContact.DisplayName + " (" + CompanyContact.Email + ")";
            }

            SendEmail("no-reply@ontrack.co.nz", CompanyContact.Email, PendingContact.DisplayName + " wants to join " + CompanyContact.DisplayName + " on Ontrack", Body);

        }

        /// <summary>
        /// Returns true if using local database connection string.
        /// </summary>
        /// <returns></returns>
        public bool IsLocalDatabase()
        {
            //System.Diagnostics.Debug.WriteLine("BRGeneral.cs");
            return ConnectionString.ToLower().Contains("sqlexpress");
        }
    }
}
