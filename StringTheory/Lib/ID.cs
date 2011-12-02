using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.Lib
{
    public class ID
    {
        // Fields
        public static string SSN = (US_SSN_PERFECT + ST.OR + US_SSN_IMPERFECT);
        protected static string US_SSN_IMPERFECT = (ST.ANY_DIGIT + ST.REPEAT(9));
        protected static string US_SSN_PERFECT = (ST.ANY_DIGIT + ST.REPEAT(3) + "-" + ST.ANY_DIGIT + ST.REPEAT(2) + "-" + ST.ANY_DIGIT + ST.REPEAT(4));
    }




}
