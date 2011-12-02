using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.Lib
{
    public class Net
    {
        // Fields
        private static string _DOMAIN_CHUNK = ("(" + ST.ALPHA + ST.OR + ST.ANY_DIGIT + ST.OR + "-)" + ST.ONE_OR_MORE);
        private static string _URI = (ST.WORD_BOUNDARY + ST.ALPHA + ST.ANY_COUNT + "://" + ST.NON_WHITESPACE + ST.ANY_COUNT);
        public static string DOMAIN = (_DOMAIN_CHUNK + "(" + DOT + _DOMAIN_CHUNK + ")" + ST.ANY_COUNT);
        public static string DOT = @"\.";
        public static string EMAIL = ("(" + EMAIL_AT_DOMAIN + ST.OR + EMAIL_AT_IP + ")");
        public static string EMAIL_AT_DOMAIN = (@"[^\s@]" + ST.ONE_OR_MORE + "@" + DOMAIN);
        public static string EMAIL_AT_IP = (@"[^\s@]" + ST.ONE_OR_MORE + "@" + DOMAIN);
        public static string FTP = "ftp://";
        public static string HTTP = "http://";
        public static string IP = (OCTET + "(" + DOT + OCTET + ")" + ST.REPEAT(3));
        public static string OCTET = ("([0-2][0-5][0-5]" + ST.OR + "[0-1]" + ST.ZERO_OR_ONE + ST.ANY_DIGIT + ST.REPEAT(2) + ST.OR + ST.ANY_DIGIT + ST.REPEAT(2) + ST.OR + ST.ANY_DIGIT + ")");
        public static string TELNET = "telnet://";
        public static Constraint URL = new Constraint(_URI, "", "");
    }




}
