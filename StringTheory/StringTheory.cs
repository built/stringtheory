using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Collections;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Built.Text.Lib;

namespace Built.Text
{
    [Serializable]
    public class StringTheory : Core
    {
        // Fields
        private const string VERSION = "1.3.11";

        // Methods
        public StringTheory()
        {
            this.init();
        }

        public StringTheory(StringTheory initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(double initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(int initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(long initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(char[] initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(object initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(float initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public StringTheory(string initialValue)
        {
            this.init();
            base.textBuffer.Append(initialValue);
        }

        public int AsBase(int numericBase)
        {
            int num = 0;
            int num2 = numericBase;
            if (((num2 != 2) && (num2 != 8)) && (num2 != 0x10))
            {
                return num;
            }
            return Convert.ToInt32(base.textBuffer.ToString(), numericBase);
        }

        public int AsBase36()
        {
            return this.AsBase(0x24);
        }

        public int AsBase64()
        {
            return -1;
        }

        public int AsHex()
        {
            return this.AsBase(0x10);
        }

        public int AsOct()
        {
            return this.AsBase(8);
        }

        protected string buildTag(string fieldname, string Format)
        {
            StringTheory theory = new StringTheory(Format);
            theory.Replace("*", fieldname);
            return theory.ToString();
        }

        public void Clean(Built.Text.Lib.Constraint desiredPattern)
        {
            base.Rx.Filter(desiredPattern.getFilter());
        }

        public void Contract(int size)
        {
            base.PasteOver(base.Cut(base.Start(), size));
        }

        public int CountPhrase(object phrase)
        {
            return this.Replace(phrase.ToString(), phrase.ToString());
        }

        public void Ellipsize(int size)
        {
            if (size < base.Length)
            {
                this.Contract(size - 3);
                base.Paste(base.Length, "...");
            }
        }

        public void Escape()
        {
            this.Replace(@"\", @"\\");
            this.Replace("\"", "\\\"");
        }

        public void Expand(int size)
        {
            this.Expand(size, ' ');
        }

        public void Expand(int size, char ch)
        {
            this.Pad(size - base.Length, ch);
        }

        public void Fill(int start, int len, char ch)
        {
            base.PasteOver(start, len, new string(ch, len));
        }

        public void Filter(string badChars)
        {
            for (int i = 0; i < badChars.Length; i++)
            {
                this.Replace(badChars[i], "");
            }
        }

        public void FilterRx(string unwanted)
        {
            base.Rx.Filter(unwanted);
        }

        public int FindNextRx(object pattern)
        {
            return base.Rx.FindNext(pattern);
        }

        public int FindRx(object pattern)
        {
            return base.Rx.Find(pattern);
        }

        public bool Fix(Built.Text.Lib.Constraint goal)
        {
            if (!base.Rx.Matches(goal))
            {
                base.Rx.Filter(goal.getFilter());
                this.Format(goal.getFormat());
            }
            return base.Rx.Matches(goal);
        }

        public void Format(string FormatString)
        {
            if (FormatString.Length > 0)
            {
                StringTheory theory = new StringTheory();
                StringTheory theory2 = new StringTheory(this);
                theory2.Flip();
                for (int i = 0; i < FormatString.Length; i++)
                {
                    if (FormatString[i] == '#')
                    {
                        theory.Push(theory2.Pop());
                    }
                    else
                    {
                        theory.Push(FormatString[i]);
                    }
                }
                base.Renew(theory);
            }
        }

        protected void init()
        {
            base.Rx = new RegexAdapter();
            base.Rx.RegisterString(this);
        }

        public void Join(ICollection list, char joinChar)
        {
            this.Join(list, joinChar.ToString());
        }

        public void Join(ICollection coll, object joinString)
        {
            IEnumerator enumerator = coll.GetEnumerator();
            int num = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    base.Append(enumerator.Current);
                    if (num < (coll.Count - 1))
                    {
                        base.Append(joinString);
                    }
                    num++;
                }
            }
        }

        public void Join(object[] list, char joinChar)
        {
            this.Join(list, joinChar.ToString());
        }

        public void Join(object[] list, object joinString)
        {
            for (int i = 0; i < list.Length; i++)
            {
                object val = (string)list[i];
                if (val != null)
                {
                    base.Append(val);
                    if (i < (list.Length - 1))
                    {
                        base.Append(joinString);
                    }
                }
            }
        }

        public void LeftJustify()
        {
            for (int i = 0; ((base.First() == ' ') || (base.First() == '\t')) && (i < base.Length); i++)
            {
                string val = base.Cut(0, 1);
                base.Paste(base.Length, val);
            }
        }

        public int LineCount()
        {
            return this.Lines().Length;
        }

        public string[] Lines()
        {
            string[] sourceArray = Regex.Split(this.ToString(), ST.ANY_LINE_END);
            int index = sourceArray.Length - 1;
            if (sourceArray[index].Length < 1)
            {
                string[] destinationArray = new string[sourceArray.Length - 1];
                Array.Copy(sourceArray, 0, destinationArray, 0, sourceArray.Length - 1);
                sourceArray = destinationArray;
            }
            return sourceArray;
        }

        public bool LoadFile(string filename)
        {
            bool flag = false;
            try
            {
                StreamReader reader = new StreamReader(filename);
                base.Append(reader.ReadToEnd());
                reader.Close();
                flag = true;
            }
            catch (IOException exception)
            {
                this.stifle(exception);
            }
            return flag;
        }

        public static void main(string[] args)
        {
            StringTheory theory = new StringTheory("StringTheory v%VERSION% (c) 2003 Built Software.\nSee http://www.builtsoftware.com/products/StringTheory/ for more info.");
            theory.Replace("%VERSION%", Version);
            Console.WriteLine(theory);
        }

        public bool Matches(object pattern)
        {
            return base.Rx.Matches(pattern.ToString());
        }

        public void Pad(int count)
        {
            this.PadRight(count);
        }

        public void Pad(int count, char c)
        {
            this.PadRight(count, c);
        }

        public void PadEnds(int count)
        {
            this.PadEnds(count, ' ');
        }

        public void PadEnds(int count, char c)
        {
            this.PadLeft(count, c);
            this.PadRight(count, c);
        }

        public void PadLeft(int count)
        {
            this.PadLeft(count, ' ');
        }

        public void PadLeft(int count, char c)
        {
            base.Prepend(base.CreateCharBufferOfLength(count, c));
        }

        public void PadRight(int count)
        {
            this.PadRight(count, ' ');
        }

        public void PadRight(int count, char c)
        {
            base.Append(base.CreateCharBufferOfLength(count, c));
        }

        public void Populate(Hashtable hash)
        {
            this.Populate(hash, "*");
        }

        public void Populate(IDataReader results)
        {
            this.Populate(results, "*");
        }

        public void Populate(object bean)
        {
            this.Populate(bean, "*");
        }

        public void Populate(Hashtable values, string tagFormat)
        {
            StringTheory phrase = new StringTheory();
            foreach (string str in values.Keys)
            {
                phrase.Renew(tagFormat);
                phrase.Replace("*", str);
                this.Replace(phrase, values[str]);
            }
        }

        public void Populate(IDataReader row, string tagFormat)
        {
            Console.WriteLine("There are {0} columns to work with.", row.FieldCount);
            Console.WriteLine("There are {0} rows to work with.", row.GetSchemaTable().Rows.Count);
            foreach (DataRow row2 in row.GetSchemaTable().Rows)
            {
                string name = row2["ColumnName"].ToString();
                this.populateSingleTag(name, tagFormat, row[name].ToString());
            }
        }

        public void Populate(object bean, string tagFormat)
        {
            Type type = bean.GetType();
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (info.CanRead)
                {
                    this.populateSingleTag(info.Name, tagFormat, type.InvokeMember(info.Name, BindingFlags.GetProperty, null, bean, new object[0]).ToString());
                }
            }
        }

        public void PopulateExact(object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                this.PopulateExact(list[i]);
            }
        }

        public void PopulateExact(object bean)
        {
            this.PopulateExact(bean, "*");
        }

        public void PopulateExact(object bean, string tagFormat)
        {
            Type type = bean.GetType();
            string str = type.Namespace + "." + type.Name + ".";
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (info.CanRead)
                {
                    this.populateSingleTag(str + info.Name, tagFormat, type.InvokeMember(info.Name, BindingFlags.GetProperty, null, bean, new object[0]).ToString());
                }
            }
        }

        public void PopulateExact(object[] list, string tagFormat)
        {
            for (int i = 0; i < list.Length; i++)
            {
                this.PopulateExact(list[i], tagFormat);
            }
        }

        protected void populateSingleTag(string name, string Format, string data)
        {
            this.Replace(this.buildTag(name, Format), data);
        }

        public void Proper()
        {
            base.Lower();
            base.Upper(base.Start());
        }

        public void ProperWords()
        {
            bool flag = true;
            for (int i = 0; i < base.Length; i++)
            {
                if (flag)
                {
                    base.Upper(i);
                }
                flag = (base[i] == ' ') || (base[i] == '\t');
            }
        }

        public void Quote()
        {
            this.Quote("\"");
        }

        public void Quote(char ch)
        {
            base.Paste(base.Start(), ch.ToString());
            base.Paste(base.Length, ch.ToString());
        }

        public void Quote(object value)
        {
            base.Paste(base.Start(), value);
            base.Paste(base.Length, value);
        }

        public void Repeat(int times)
        {
            string val = base.textBuffer.ToString();
            for (int i = 0; i < times; i++)
            {
                base.Append(val);
            }
        }

        public int Replace(char character, char replacement)
        {
            return this.Replace(character.ToString(), replacement.ToString());
        }

        public int Replace(char character, object replacement)
        {
            return this.Replace(character.ToString(), replacement.ToString());
        }

        public int Replace(object character, char replacement)
        {
            return this.Replace(character.ToString(), replacement.ToString());
        }

        public int Replace(object phrase, object value)
        {
            return this.Replace(phrase.ToString(), value.ToString());
        }

        public int Replace(string phrase, string val)
        {
            int num = 0;
            int index = base.Find(phrase);
            while ((phrase.Length > 0) && base.inBounds(index))
            {
                base.PasteOver(index, phrase.Length, val);
                index = base.Find(phrase, index + val.Length);
                num++;
            }
            return num;
        }

        public void ReplaceRx(object pattern, object value)
        {
            base.Rx.Replace(pattern, value);
        }

        public void RightJustify()
        {
            for (int i = 0; ((base.Last() == ' ') || (base.Last() == '\t')) && (i < base.Length); i++)
            {
                string val = base.Cut(base.End(), 1);
                base.Paste(0, val);
            }
        }

        public void SingleQuote()
        {
            this.Quote("'");
        }

        public void SingleSpace()
        {
            bool flag = false;
            for (int i = 0; i < base.Length; i++)
            {
                if ((base[i] == ' ') || (base[i] == '\t'))
                {
                    if (flag)
                    {
                        base.Cut(i, 1);
                        i--;
                    }
                    else
                    {
                        base[i] = ' ';
                    }
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
        }

        public void SpacesToTabs(int num)
        {
            this.Replace(new string(' ', base.LimitToZero(num)), "\t");
        }

        public ArrayList Split(char delimiter)
        {
            return this.Split(delimiter.ToString());
        }

        public ArrayList Split(string phrase)
        {
            ArrayList list = new ArrayList();
            int startIndex = 0;
            int count = 0;
            int index = 0;
            while ((index > -1) && (index < base.Length))
            {
                index = base.textBuffer.ToString().IndexOf(phrase, startIndex);
                if (index < 0)
                {
                    count = base.Length - startIndex;
                }
                else
                {
                    count = index - startIndex;
                }
                list.Add(new StringTheory(base.Copy(startIndex, count)));
                startIndex = index + phrase.Length;
            }
            return list;
        }

        public string[] SplitToStrings(char delimiter)
        {
            return this.SplitToStrings(delimiter.ToString());
        }

        public string[] SplitToStrings(string phrase)
        {
            ArrayList list = this.Split(phrase);
            string[] strArray = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strArray[i] = list[i].ToString();
            }
            return strArray;
        }

        protected void stifle(Exception e)
        {
        }

        public void Strip()
        {
            this.StripTabs();
            this.StripSpaces();
        }

        public void StripRange(int start, int count)
        {
            StringTheory theory = new StringTheory(base.Cut(start, count));
            theory.Replace(" ", "");
            theory.Replace("\t", "");
            base.Paste(base.Start(), theory.ToString());
        }

        public int StripSpaces()
        {
            return this.Replace(" ", "");
        }

        public int StripTabs()
        {
            return this.Replace("\t", "");
        }

        public void TabsToSpaces(int num)
        {
            this.Replace("\t", new string(' ', base.LimitToZero(num)));
        }

        public int TiltBack()
        {
            return this.Replace("/", @"\");
        }

        public int TiltForward()
        {
            return this.Replace(@"\", "/");
        }

        public double ToDouble()
        {
            double num = 0.0;
            try
            {
                num = double.Parse(base.textBuffer.ToString());
            }
            catch (FormatException exception)
            {
                this.stifle(exception);
            }
            return num;
        }

        public float ToFloat()
        {
            float num = 0f;
            try
            {
                num = float.Parse(base.textBuffer.ToString());
            }
            catch (FormatException exception)
            {
                this.stifle(exception);
            }
            return num;
        }

        public int ToInt()
        {
            int num = 0;
            try
            {
                num = int.Parse(base.textBuffer.ToString());
            }
            catch (FormatException exception)
            {
                this.stifle(exception);
            }
            return num;
        }

        public long ToLong()
        {
            long num = 0L;
            try
            {
                num = long.Parse(base.textBuffer.ToString());
            }
            catch (FormatException exception)
            {
                this.stifle(exception);
            }
            return num;
        }

        public void Trim()
        {
            this.TrimLeft();
            this.TrimRight();
        }

        public void TrimLeft()
        {
            while ((base.First() == ' ') || (base.First() == '\t'))
            {
                base.Cut(base.Start(), 1);
            }
        }

        public void TrimLineEnds()
        {
            while ((base.Last() == '\n') || (base.Last() == '\r'))
            {
                base.Cut(base.End(), 1);
            }
        }

        public void TrimRight()
        {
            while ((base.Last() == ' ') || (base.Last() == '\t'))
            {
                base.Cut(base.End(), 1);
            }
        }

        public int WordCount()
        {
            return this.Words().Length;
        }

        public string[] Words()
        {
            Regex regex = new Regex(ST.WHITESPACE);
            return regex.Split(base.textBuffer.ToString());
        }

        // Properties
        public static string Version
        {
            get
            {
                return "1.3.11";
            }
        }
    }



}
