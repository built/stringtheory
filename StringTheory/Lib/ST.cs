using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Text.Lib
{
    public sealed class ST
    {
        // Fields
        public static string ALPHA = "[A-Za-z]";
        public static string ANY_CHAR = ".";
        public static string ANY_COUNT = "*";
        public static string ANY_DIGIT = @"\d";
        public static string ANY_LETTER = "[A-Za-z]";
        public static string ANY_LINE_END = (WIN_LINE_END + OR + UNIX_LINE_END + OR + MAC_LINE_END);
        public static string ANY_NON_DIGIT = @"\D";
        public static string ANY_NON_WORD = @"\W";
        public static string ANY_SPACES = @"\s*";
        public static string ANY_WORD = @"\w";
        public static string ANYTHING = (ANY_CHAR + ANY_COUNT);
        public static string END = "$";
        public static string MAC_LINE_END = "\r";
        public static string NON_WHITESPACE = @"\S";
        public static string NON_WORD_BOUNDARY = @"\B";
        public static string ONE_OR_MORE = "+";
        public static string OR = "|";
        public static string STARTS_WITH = "^";
        public static string TRAILING_LINE_END = (ANY_LINE_END + "$");
        public static string UNIX_LINE_END = "\n";
        public static string WHITESPACE = @"\s";
        public static string WIN_LINE_END = "\r\n";
        public static string WORD_BOUNDARY = @"\b";
        public static string ZERO_OR_ONE = "?";

        // Methods
        public static string REPEAT(int times)
        {
            return ("{" + times + "}");
        }
    }




}
