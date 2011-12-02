using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.UnitTests
{
    public class OrderBean
    {
        // Fields
        protected string m_customerId = "";
        protected string m_productNumber = "";
        protected int m_quantity = 0;

        // Properties
        public string CustomerId
        {
            get
            {
                return this.m_customerId;
            }
            set
            {
                this.m_customerId = value;
            }
        }

        public string ProductNumber
        {
            get
            {
                return this.m_productNumber;
            }
            set
            {
                this.m_productNumber = value;
            }
        }

        public int Quantity
        {
            get
            {
                return this.m_quantity;
            }
            set
            {
                this.m_quantity = value;
            }
        }
    }

}
