using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.Lib
{
    public class Constraint
    {
        // Fields
        protected string Filter;
        protected string Format;
        protected string Test;

        // Methods
        public Constraint()
        {
            this.Test = "";
            this.Format = "";
            this.Filter = "";
        }

        public Constraint(string test, string format, string filter)
        {
            this.Test = "";
            this.Format = "";
            this.Filter = "";
            this.Test = test;
            this.Format = format;
            this.Filter = filter;
        }

        public string getFilter()
        {
            return this.Filter;
        }

        public string getFormat()
        {
            return this.Format;
        }

        public string getTest()
        {
            return this.Test;
        }

        public bool hasFilter()
        {
            return (this.Filter.Length > 0);
        }

        public override string ToString()
        {
            return this.getTest();
        }
    }

 
}
