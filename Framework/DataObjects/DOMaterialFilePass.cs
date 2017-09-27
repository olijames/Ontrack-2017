using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Electracraft.Framework.DataObjects
{
    public class DOMaterialFilePass
    {
        public System.IO.Stream F;
        private int m_RowCount;
        public int RowCount
        {
            get
            {
                return m_RowCount;
            }
            set
            {
                m_RowCount = value;
            }
        }

        //public string[,] Materials;
        public int intChar;
        public char c;

        private int m_ColCount;
        public int ColCount
        {
            get
            {
                return m_ColCount;
            }
            set
            {
                m_ColCount = value;
            }
        }

        private int m_CommaCount;
        public int CommaCount
        {
            get
            {
                return m_CommaCount;
            }
            set
            {
                m_CommaCount = value;
            }
        }

        
    }
}
