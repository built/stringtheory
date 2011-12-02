using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Built.Text.Lib;

namespace Built.Text
{
    [Serializable]
    public class RegexAdapter
    {
        // Fields
        protected StringTheory hostString;
        protected int lastFound = -1;
        private Match latestMatch = null;

        // Methods
        public string Copy()
        {
            string str = "";
            if (this.latestMatch != null)
            {
                str = this.hostString.Copy(this.latestMatch.Index, this.latestMatch.Length);
            }
            return str;
        }

        public string Cut()
        {
            string str = "";
            if (this.latestMatch != null)
            {
                str = this.hostString.Cut(this.latestMatch.Index, this.latestMatch.Length);
            }
            return str;
        }

        public void Filter(string unwanted)
        {
            this.Replace(unwanted, "");
        }

        public int Find(object pattern)
        {
            int index = -1;
            this.latestMatch = this.getPatternMatcher(pattern);
            this.latestMatch.NextMatch();
            if (this.latestMatch.Success)
            {
                index = this.latestMatch.Index;
            }
            return index;
        }

        public int FindNext(object pattern)
        {
            if (this.latestMatch == null)
            {
                this.latestMatch = this.getPatternMatcher(pattern);
            }
            else
            {
                this.latestMatch = this.latestMatch.NextMatch();
            }
            return this.latestMatch.Index;
        }

        protected Match getPatternMatcher(object pattern)
        {
            Regex regex = new Regex(pattern.ToString());
            return regex.Match(this.hostString.ToString());
        }

        public bool Matches(Constraint pattern)
        {
            return Regex.IsMatch(this.hostString.ToString(), pattern.ToString());
        }

        public bool Matches(string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(this.hostString.ToString());
        }

        public void Paste(object value)
        {
            if (this.latestMatch != null)
            {
                this.hostString.Paste(this.latestMatch.Index, value);
            }
        }

        public void PatternReplace(Match match)
        {
        }

        public void RegisterString(StringTheory hostString)
        {
            this.hostString = hostString;
        }

        public void Replace(object pattern, object content)
        {
            this.latestMatch = this.getPatternMatcher(pattern);
            Regex regex = new Regex(pattern.ToString());
            this.hostString.PasteOver(regex.Replace(this.hostString.ToString(), content.ToString()));
        }

        public void Reset()
        {
            this.lastFound = -1;
            this.latestMatch = null;
        }
    }


}
