using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Collections;
using System.Text;

namespace Built.Text
{
    [Serializable]
    public class Core
    {
        // Fields
        protected int lastFound = -1;
        public RegexAdapter Rx = null;
        protected StringBuilder textBuffer = new StringBuilder();
        protected bool wasTouched = false;

        // Methods
        protected Core()
        {
        }

        override public int GetHashCode()
        {
            return this.textBuffer.GetHashCode();
        }

        public void Append(StringTheory val)
        {
            this.Append(val.ToString());
        }

        public void Append(char val)
        {
            this.Paste(this.Length, val);
        }

        public void Append(char[] val)
        {
            this.Paste(this.Length, val);
        }

        public void Append(ICollection list)
        {
            this.Paste(this.Length, list);
        }

        public void Append(object[] list)
        {
            this.Paste(this.Length, list);
        }

        public void Append(object val)
        {
            this.Append(val.ToString());
        }

        public void Append(string val)
        {
            this.Paste(this.Length, val);
        }

        public bool BeginsWith(StringTheory phrase)
        {
            return this.BeginsWith(phrase.ToString());
        }

        public bool BeginsWith(object phrase)
        {
            return this.BeginsWith(phrase.ToString());
        }

        public bool BeginsWith(string phrase)
        {
            return this.textBuffer.ToString().StartsWith(phrase);
        }

        protected void ClearBuffer()
        {
            this.textBuffer.Length = 0;
            this.touch();
        }

        public bool Contains(StringTheory phrase)
        {
            return this.Contains(phrase.ToString());
        }

        public bool Contains(object phrase)
        {
            return this.Contains(phrase.ToString());
        }

        public bool Contains(string phrase)
        {
            return (this.textBuffer.ToString().IndexOf(phrase) > -1);
        }

        public string Copy(int start, int count)
        {
            start = this.LimitToZero(start);
            if (this.ensureSectionIsInRange(start, count) > 0)
            {
                return this.textBuffer.ToString(start, count);
            }
            return "";
        }

        public string CopyAfter(object phrase)
        {
            string str = "";
            int num = this.Find(phrase);
            if (num > -1)
            {
                int start = num + phrase.ToString().Length;
                str = this.Copy(start, this.Length - start);
            }
            return str;
        }

        public string CopyAfterLast(object phrase)
        {
            string str = "";
            int num = this.RevFind(phrase);
            if (num > -1)
            {
                int start = num + phrase.ToString().Length;
                str = this.Copy(start, this.Length - start);
            }
            return str;
        }

        public string CopyBefore(object phrase)
        {
            return this.Copy(0, this.LimitToZero(this.Find(phrase)));
        }

        public string CopyBeforeLast(object phrase)
        {
            string str = "";
            int count = this.RevFind(phrase);
            if (count > -1)
            {
                str = this.Copy(0, count);
            }
            return str;
        }

        public string CopyFrom(object phrase)
        {
            string str = "";
            int start = this.Find(phrase);
            if (start > -1)
            {
                str = this.Copy(start, this.Length - start);
            }
            return str;
        }

        public string CopyFromLast(object phrase)
        {
            string str = "";
            int start = this.RevFind(phrase);
            if (start > -1)
            {
                int length = phrase.ToString().Length;
                str = this.Copy(start, this.Length - start);
            }
            return str;
        }

        public string CopyThru(object phrase)
        {
            string str = "";
            int num = this.Find(phrase);
            if (num > -1)
            {
                str = this.Copy(0, num + phrase.ToString().Length);
            }
            return str;
        }

        public string CopyThruLast(object phrase)
        {
            string str = "";
            int num = this.RevFind(phrase);
            if (num > -1)
            {
                str = this.Copy(0, num + phrase.ToString().Length);
            }
            return str;
        }

        protected char[] CreateCharBufferOfLength(int len, char c)
        {
            len = this.LimitToZero(len);
            char[] chArray = new char[len];
            for (int i = 0; i < len; i++)
            {
                chArray[i] = c;
            }
            return chArray;
        }

        public string Cut(int start, int count)
        {
            int num = start + count;
            string str = this.Copy(0, start);
            string str2 = this.Copy(start, count);
            string str3 = this.Copy(num, this.textBuffer.Length - num);
            this.ClearBuffer();
            this.textBuffer.Append(str);
            this.textBuffer.Append(str3);
            this.touch();
            return str2.ToString();
        }

        public string CutAfter(object phrase)
        {
            string str = "";
            int num = this.Find(phrase);
            if (num > -1)
            {
                int start = num + phrase.ToString().Length;
                str = this.Cut(start, this.Length - start);
            }
            return str;
        }

        public string CutAfterLast(object phrase)
        {
            string str = "";
            int num = this.RevFind(phrase);
            if (num > -1)
            {
                int start = num + phrase.ToString().Length;
                str = this.Cut(start, this.Length - start);
            }
            return str;
        }

        public string CutBefore(object phrase)
        {
            return this.Cut(0, this.LimitToZero(this.Find(phrase)));
        }

        public string CutBeforeLast(object phrase)
        {
            string str = "";
            int count = this.RevFind(phrase);
            if (count > -1)
            {
                str = this.Cut(0, count);
            }
            return str;
        }

        public string CutFrom(object phrase)
        {
            string str = "";
            int start = this.Find(phrase);
            if (start > -1)
            {
                str = this.Cut(start, this.Length - start);
            }
            return str;
        }

        public string CutFromLast(object phrase)
        {
            string str = "";
            int start = this.RevFind(phrase);
            if (start > -1)
            {
                int length = phrase.ToString().Length;
                str = this.Cut(start, this.Length - start);
            }
            return str;
        }

        public string CutThru(object phrase)
        {
            string str = "";
            int num = this.Find(phrase);
            if (num > -1)
            {
                str = this.Cut(0, num + phrase.ToString().Length);
            }
            return str;
        }

        public string CutThruLast(object phrase)
        {
            string str = "";
            int num = this.RevFind(phrase);
            if (num > -1)
            {
                str = this.Cut(0, num + phrase.ToString().Length);
            }
            return str;
        }

        public bool Differs(object value)
        {
            return !this.Equals(value);
        }

        public int End()
        {
            return this.LimitToZero(this.Length - 1);
        }

        public bool EndsWith(object phrase)
        {
            return this.ToString().EndsWith(phrase.ToString());
        }

        protected int ensureSectionIsInRange(int start, int len)
        {
            return Math.Min(len, this.Length - start);
        }

        public override bool Equals(object value)
        {
            return this.ToString().Equals(value.ToString());
        }

        public void Erase()
        {
            this.ClearBuffer();
        }

        public int Find(object phrase)
        {
            return this.textBuffer.ToString().IndexOf(phrase.ToString());
        }

        public int Find(object phrase, int start)
        {
            return this.textBuffer.ToString().IndexOf(phrase.ToString(), start);
        }

        public int FindNext(object phrase)
        {
            this.lastFound = this.textBuffer.ToString().IndexOf(phrase.ToString(), (int) (this.lastFound + 1));
            return this.lastFound;
        }

        public char First()
        {
            return this[0];
        }

        public void Flip()
        {
            StringTheory theory = new StringTheory(this);
            this.Erase();
            while (!theory.IsEmpty())
            {
                this.Push(theory.Pop());
            }
            this.touch();
        }

        protected bool inBounds(int index)
        {
            return ((index > -1) && (index < this.textBuffer.Length));
        }

        public bool IsEmpty()
        {
            return (this.textBuffer.Length < 1);
        }

        public bool IsLower()
        {
            return this.IsLower(0, this.Length);
        }

        public bool IsLower(int position)
        {
            return char.IsLower(this[position]);
        }

        public bool IsLower(int start, int len)
        {
            int num = this.ensureSectionIsInRange(start, len);
            for (int i = start; i < (start + num); i++)
            {
                if (char.IsLetter(this[i]) && !this.IsLower(i))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsUpper()
        {
            return this.IsUpper(0, this.Length);
        }

        public bool IsUpper(int position)
        {
            return char.IsUpper(this[position]);
        }

        public bool IsUpper(int start, int len)
        {
            int num = this.ensureSectionIsInRange(start, len);
            for (int i = start; i < (start + num); i++)
            {
                if (char.IsLetter(this[i]) && !this.IsUpper(i))
                {
                    return false;
                }
            }
            return true;
        }

        public char Last()
        {
            return this[this.End()];
        }

        protected int LimitToZero(int value)
        {
            return Math.Max(value, 0);
        }

        public void Lower()
        {
            string val = this.textBuffer.ToString().ToLower();
            this.ClearBuffer();
            this.Append(val);
        }

        public void Lower(int position)
        {
            this[position] = char.ToLower(this[position]);
        }

        public void Lower(int start, int len)
        {
            for (int i = 0; i < len; i++)
            {
                this.Lower(start + i);
            }
        }

        public void Paste(int position, char val)
        {
            this.textBuffer.Insert(this.LimitToZero(position), val);
            this.touch();
        }

        public void Paste(int position, char[] val)
        {
            this.textBuffer.Insert(this.LimitToZero(position), val);
            this.touch();
        }

        public void Paste(int position, ICollection list)
        {
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    this.Paste(this.End() + 1, enumerator.Current);
                }
            }
        }

        public void Paste(int position, object[] list)
        {
            int num = position;
            for (int i = 0; i < list.Length; i++)
            {
                string val = list[i].ToString();
                if (val != null)
                {
                    this.Paste(num, val);
                    num += val.Length;
                }
            }
        }

        public void Paste(int position, object val)
        {
            this.textBuffer.Insert(this.LimitToZero(position), val);
            this.touch();
        }

        public void Paste(int position, string val)
        {
            this.textBuffer.Insert(this.LimitToZero(position), val);
            this.touch();
        }

        public void PasteOver(object value)
        {
            this.Erase();
            this.Paste(0, value);
        }

        public void PasteOver(int start, int count, object value)
        {
            this.Cut(start, count);
            this.Paste(start, value);
        }

        public char Pop()
        {
            return this.Cut(this.End(), 1)[0];
        }

        public void Prepend(char[] str)
        {
            this.Paste(0, str);
        }

        public void Prepend(StringTheory str)
        {
            this.Paste(0, str.ToString());
        }

        public void Prepend(ICollection list)
        {
            StringTheory str = new StringTheory();
            str.Join(list, "");
            this.Prepend(str);
        }

        public void Prepend(object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                object str = (string) list[i];
                if (str != null)
                {
                    this.Prepend(str);
                }
            }
        }

        public void Prepend(object str)
        {
            this.Paste(0, str.ToString());
        }

        public void Prepend(string str)
        {
            this.Paste(0, str);
        }

        public int Push(char value)
        {
            this.Append(value);
            return this.Length;
        }

        public void Renew()
        {
            this.Erase();
            this.Reset();
        }

        public void Renew(object obj)
        {
            this.Renew(obj.ToString());
        }

        public void Renew(string str)
        {
            this.Erase();
            this.Append(str);
            this.Reset();
        }

        public void Reset()
        {
            this.wasTouched = false;
            this.lastFound = -1;
            this.Rx.Reset();
        }

        public int RevFind(object phrase)
        {
            return this.textBuffer.ToString().LastIndexOf(phrase.ToString());
        }

        public int RevFind(object phrase, int start)
        {
            return this.textBuffer.ToString().LastIndexOf(phrase.ToString(), start);
        }

        public int RevFindNext(object phrase)
        {
            int lastFound = this.lastFound;
            if (lastFound < 0)
            {
                lastFound = this.End();
            }
            else
            {
                lastFound--;
            }
            this.lastFound = this.textBuffer.ToString().LastIndexOf(phrase.ToString(), lastFound);
            return this.lastFound;
        }

        public bool SameAs(object value)
        {
            return this.Equals(value);
        }

        public char Shift()
        {
            return this.Cut(this.Start(), 1)[0];
        }

        public int Start()
        {
            return 0;
        }

        public override string ToString()
        {
            return this.textBuffer.ToString();
        }

        protected void touch()
        {
            this.wasTouched = true;
        }

        public bool Touched()
        {
            return this.wasTouched;
        }

        public int Unshift(char replacement)
        {
            this.Prepend(replacement.ToString());
            return this.Length;
        }

        public void Upper()
        {
            string val = this.textBuffer.ToString().ToUpper();
            this.ClearBuffer();
            this.Append(val);
        }

        public void Upper(int position)
        {
            this[position] = char.ToUpper(this[position]);
        }

        public void Upper(int start, int len)
        {
            for (int i = 0; i < len; i++)
            {
                this.Upper(start + i);
            }
        }

        // Properties
        public char this[int index]
        {
            get
            {
                char ch = '\0';
                index = this.LimitToZero(index);
                if (index < this.textBuffer.Length)
                {
                    ch = this.textBuffer[index];
                }
                return ch;
            }
            set
            {
                this.touch();
                this.textBuffer[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.textBuffer.Length;
            }
        }

        public string Text
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this.Renew(value);
            }
        }
    }

 
 


}
