using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.UnitTests
{
    public class PersonBean
    {
        // Fields
        protected string m_Address1 = "";
        protected string m_Address2 = "";
        protected int m_Age = 0;
        protected string m_City = "";
        protected string m_FirstName = "";
        protected string m_LastName = "";
        protected string m_Phone = "";
        protected string m_State = "";
        protected string m_Zip = "";

        // Properties
        public string Address1
        {
            get
            {
                return this.m_Address1;
            }
            set
            {
                this.m_Address1 = value;
            }
        }

        public string Address2
        {
            get
            {
                return this.m_Address2;
            }
            set
            {
                this.m_Address2 = value;
            }
        }

        public int Age
        {
            get
            {
                return this.m_Age;
            }
            set
            {
                this.m_Age = value;
            }
        }

        public string City
        {
            get
            {
                return this.m_City;
            }
            set
            {
                this.m_City = value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.m_FirstName;
            }
            set
            {
                this.m_FirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.m_LastName;
            }
            set
            {
                this.m_LastName = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.m_Phone;
            }
            set
            {
                this.m_Phone = value;
            }
        }

        public string State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
            }
        }

        public string Zip
        {
            get
            {
                return this.m_Zip;
            }
            set
            {
                this.m_Zip = value;
            }
        }
    }



}
