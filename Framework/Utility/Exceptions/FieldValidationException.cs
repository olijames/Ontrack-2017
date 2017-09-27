using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.Utility.Exceptions
{
    public class FieldValidationException : Exception
    {
        public new string Message { get; set; }
        public FieldValidationException(string Message)
            : base()
        {
            this.Message = Message;
        }
    }
}
