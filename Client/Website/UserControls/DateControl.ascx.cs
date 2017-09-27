using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Electracraft.Framework.Web;
using Electracraft.Framework.Utility;
using Html5Asp;

namespace Electracraft.Client.Website.UserControls
{
    public partial class DateControl : UserControlBase
    {
        protected string DatePickerFormatString
        {
            get { return "yyyy-MM-dd"; }
        }

        bool ios;
        protected override void OnLoad(EventArgs e)
        {
            string UserAgent = Request.UserAgent.ToLower();
            if (UserAgent.Contains("ipad") || UserAgent.Contains("iphone"))
                ios = true;

           txtDateControlDate.Visible = !ios;
            dateDateControlInput.Visible = ios;

            base.OnLoad(e);
        }

        private bool _Enabled = true;
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                SetControlsEnabled();
            }
        }

        private void SetControlsEnabled()
        {
            txtDateControlDate.Enabled = _Enabled;
            dateDateControlInput.Enabled = _Enabled;
        }


        public void SetDate(DateTime Date)
        {
            if (Date > DateAndTime.NoValueDate)
            {
                if (ios)
                    dateDateControlInput.Text = Date.ToString(DatePickerFormatString);
                else
                    txtDateControlDate.Text = Date.ToString(DateAndTime.ShortDateFormat);
            }
            else
            {
                if (ios)
                    dateDateControlInput.Text = string.Empty;
                else
                    txtDateControlDate.Text = string.Empty;
            }

        }

        public DateTime GetDate()
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                if (ios)
                {
                    if (string.IsNullOrEmpty(dateDateControlInput.Text)) return DateAndTime.NoValueDate;
                    return DateTime.ParseExact(dateDateControlInput.Text, DatePickerFormatString, provider);
                }
                else
                {
                    if (string.IsNullOrEmpty(txtDateControlDate.Text)) return DateAndTime.NoValueDate;
                    return DateTime.ParseExact(txtDateControlDate.Text, DateAndTime.ShortDateFormat, provider);
                }
            }
            catch
            {
                return DateAndTime.NoValueDate;
            }
        }
    }
}