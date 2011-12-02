using System;
using System.Collections;
using System.Text;
using System.Data.OleDb;
using NUnit.Framework;
using Built.Text.Lib;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Built.Text.UnitTests
{
    [TestFixture]
    public class StringTheoryTestSuite
    {
        // Fields
        private string Url = "driver={SQLServer};provider=SQLOLEDB;server=dev-1;database=BuiltDev;uid=sa;pwd=built";

        // Methods
        protected void AssertFalse(string msg, bool condition)
        {
            if (condition)
            {
                Assert.Fail(msg);
            }
        }

        protected OrderBean CreateOrderBean()
        {
            return new OrderBean { ProductNumber = "202-456-1", Quantity = 0x39 };
        }

        protected PersonBean CreatePersonBean()
        {
            return new PersonBean { FirstName = "George", LastName = "Bush", Address1 = "1600 Pennsylvania Ave.", Address2 = "(c/o Laura)", City = "Washington", State = "DC", Zip = "20500", Phone = "202-456-1414", Age = 0x39 };
        }

        public void DEFERRED_testAsteriskBasedCustomTemplatePopulationWithHashtable()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Phone: 202-456-1414Age: 57";
            StringTheory theory = new StringTheory("Name: ***FirstName*** ***LastName***Address: ***Address1*** ***Address2***, ***City***, ***State*** ***Zip***Phone: ***Phone***Age: ***Age***");
            Hashtable values = new Hashtable();
            values["FirstName"] = "George";
            values["LastName"] = "Bush";
            values["Address1"] = "1600 Pennsylvania Ave.";
            values["Address2"] = "(c/o Laura)";
            values["City"] = "Washington";
            values["State"] = "DC";
            values["Zip"] = "20500";
            values["Phone"] = "202-456-1414";
            values["Age"] = 0x39;
            theory.Populate(values, @"\*\*\**\*\*\*");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        protected void DumpArray(object[] list)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine(list[i].ToString() + "(" + list[i].ToString().Length);
                }
            }
        }

        protected OleDbConnection GetConnected()
        {
            OleDbConnection connection = new OleDbConnection(this.Url);
            connection.Open();
            return connection;
        }

        protected void PopulatingRecordCleanup(OleDbConnection conn)
        {
            this.RunQuery("DELETE FROM customers", conn).Close();
        }

        protected void PopulatingRecordCreate(OleDbConnection conn)
        {
            new OleDbCommand("INSERT INTO customers (FIRST_NAME, LAST_NAME, PHONE) VALUES ('George', 'Bush', '202-456-1414')", conn).ExecuteNonQuery();
        }

        protected OleDbDataReader PopulatingRecordRetrieve(OleDbConnection conn)
        {
            return this.RunQuery("SELECT * FROM customers", conn);
        }

        protected OleDbDataReader RunQuery(string query, OleDbConnection conn)
        {
            OleDbCommand command = new OleDbCommand(query, conn);
            return command.ExecuteReader();
        }

        [SetUp]
        protected void SetUp()
        {
        }

        public void testAppend()
        {
            string str = "xyzabc";
            StringTheory theory = new StringTheory("xyz");
            theory.Append("abc");
            Assert.True(str.Equals(theory.ToString()), "simple string append failed:" + theory.ToString());
            str = "xyzabc";
            theory = new StringTheory("xyz");
            StringTheory val = new StringTheory("abc");
            theory.Append(val);
            Assert.True(str.Equals(theory.ToString()), "string theory append failed");
            str = "xyzFooFooFoo";
            theory = new StringTheory("xyz");
            string[] strArray = new string[] { "Foo", "Foo", "Foo" };
            theory.Append((object[])strArray);
            Assert.True(str.Equals(theory.ToString()), "string theory append(array) failed");
            str = "xyzFooFooFoo";
            theory = new StringTheory("xyz");
            ArrayList list = new ArrayList();
            list.Add("Foo");
            list.Add("Foo");
            list.Add("Foo");
            theory.Append((ICollection)list);
            Assert.True(str.Equals(theory.ToString()), "string theory append(ArrayList) failed" + theory.ToString());
        }

        public void testAsBase()
        {
            StringTheory theory = new StringTheory("11111111");
            Assert.True(theory.AsBase(2) == 0xff, "Base 2 strings don't match");
            Assert.True(theory.AsBase(8) == 0x249249, "Base 8 strings don't match");
            Assert.True(theory.AsBase(0x10) == 0x11111111, "Base 16 strings don't match");
            theory.PasteOver("111111");
            Assert.True(theory.AsBase(0x1f) == 0, "Base 31 strings should yield zero." + theory.AsBase(0x1f));
        }

        public void testAsBase36()
        {
            StringTheory theory = new StringTheory("ZZZZ");
            Assert.True(theory.AsBase36() == 0, "strings don't match");
        }

        public void testAsHex()
        {
            StringTheory theory = new StringTheory("FFFF");
            Assert.True(theory.AsHex() == 0xffff, "strings don't match");
        }

        public void testAsOct()
        {
            StringTheory theory = new StringTheory("7777");
            Assert.True(theory.AsOct() == 0xfff, "strings don't match");
        }

        public void testBeginsWith()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.BeginsWith("0123") && !theory.BeginsWith("xxx"), "string starts differently?");
        }

        public void testClean()
        {
            StringTheory theory = new StringTheory("5555-5555-5555-4444");
            theory.Clean(Credit.MasterCard);
            Assert.True(theory.Rx.Matches(Credit.MasterCard), "Clean failed: " + theory);
        }

        public void testContains()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.Contains("1234") && !theory.Contains("xxx"), "string doesn't contain that?");
        }

        public void testContract()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("abcdefghijklmnopqrstuvwxyz");
            theory.Contract(6);
            Assert.True(str.Equals(theory.ToString()), "str doesn't match the comparison string!");
        }

        public void testCopy()
        {
            string str = new StringTheory("0123456789").Copy(4, 4);
            Assert.True(str.Length == 4, "Copied string length is wrong!");
        }

        public void testCopyAfter()
        {
            string str = "Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CopyAfter("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals(" Oats"), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyAfterLast()
        {
            string str = "Mares Eat Oats Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CopyAfterLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals(" Oats"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyBefore()
        {
            string str = "Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CopyBefore("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares "), "The copied string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyBefore("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyBeforeLast()
        {
            string str = "Mares Eat Oats Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CopyBeforeLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares Eat Oats Mares "), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyFrom()
        {
            string str = "Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CopyFrom("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Eat Oats"), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyFromLast()
        {
            string str = "Mares Eat Oats Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CopyFromLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Eat Oats"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyThru()
        {
            string str = "Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CopyThru("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares Eat"), "The copied string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyThru("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCopyThruLast()
        {
            string str = "Mares Eat Oats Mares Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CopyThruLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares Eat Oats Mares Eat"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCountPhrase()
        {
            StringTheory theory = new StringTheory("Mares eat oats and does eat oats and little lambs eat ivy.");
            Assert.True(theory.CountPhrase("eat") == 3, "Reported number of spaces was wrong.");
        }

        public void testCreateEmpty()
        {
            StringTheory theory = new StringTheory();
            Assert.True(theory != null, "Failed to create instance!");
        }

        public void testCreateWithData()
        {
            StringTheory theory = new StringTheory("foo");
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(new StringTheory("foo"));
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(new object());
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(new char[20]);
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(1);
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(1L);
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(1.0);
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
            theory = new StringTheory(1.8888888888888888);
            Assert.True(theory != null, "Failed to create instance!");
            this.AssertFalse("New ST thinks it was touched?", theory.Touched());
        }

        public void testCustomTemplatePopulationWithBean()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Phone: 202-456-1414Age: 57";
            StringTheory theory = new StringTheory("Name: $FirstName $LastNameAddress: $Address1 $Address2, $City, $State $ZipPhone: $PhoneAge: $Age");
            theory.Populate(this.CreatePersonBean(), "$*");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        public void testCustomTemplatePopulationWithHashtable()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Phone: 202-456-1414Age: 57";
            StringTheory theory = new StringTheory("Name: $FirstName $LastNameAddress: $Address1 $Address2, $City, $State $ZipPhone: $PhoneAge: $Age");
            Hashtable values = new Hashtable();
            values["FirstName"] = "George";
            values["LastName"] = "Bush";
            values["Address1"] = "1600 Pennsylvania Ave.";
            values["Address2"] = "(c/o Laura)";
            values["City"] = "Washington";
            values["State"] = "DC";
            values["Zip"] = "20500";
            values["Phone"] = "202-456-1414";
            values["Age"] = 0x39;
            theory.Populate(values, "$*");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        public void testCustomTemplatePopulationWithResultSetEntry()
        {
            OleDbConnection connected = this.GetConnected();
            this.PopulatingRecordCreate(connected);
            string str = "Name: George BushPhone: 202-456-1414";
            StringTheory theory = new StringTheory("Name: $FIRST_NAME $LAST_NAMEPhone: $PHONE");
            OleDbDataReader reader = this.PopulatingRecordRetrieve(connected);
            if (!reader.IsClosed && reader.HasRows)
            {
                reader.Read();
                theory.Populate((IDataReader)reader, "$*");
            }
            reader.Close();
            this.PopulatingRecordCleanup(connected);
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string." + theory);
            connected.Close();
        }

        public void testCustomTemplatePopulationWithSpecificBean()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Product Number: 202-456-1Qty: 57";
            StringTheory theory = new StringTheory("Name: $Built.Text.UnitTests.PersonBean.FirstName $Built.Text.UnitTests.PersonBean.LastNameAddress: $Built.Text.UnitTests.PersonBean.Address1 $Built.Text.UnitTests.PersonBean.Address2, $Built.Text.UnitTests.PersonBean.City, $Built.Text.UnitTests.PersonBean.State $Built.Text.UnitTests.PersonBean.ZipProduct Number: $Built.Text.UnitTests.OrderBean.ProductNumberQty: $Built.Text.UnitTests.OrderBean.Quantity");
            theory.PopulateExact(this.CreatePersonBean(), "$*");
            theory.PopulateExact(this.CreateOrderBean(), "$*");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        public void testCut()
        {
            StringTheory theory = new StringTheory("0123456789");
            string str = theory.Cut(4, 4);
            Assert.True(str.Length == 4, "Copied string length is wrong!");
            Assert.True(theory.Length == 6, "Original StringTheory length is wrong!");
            theory.Renew("");
            Assert.True(theory.Cut(0, 1).Equals(""), "bogus Cut didn't return empty.");
            theory.Renew("");
            Assert.True(theory.Cut(0, -1).Equals(""), "bogus Cut (2) didn't return empty.");
        }

        public void testCutAfter()
        {
            string str = "Mares Eat";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CutAfter("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals(" Oats"), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CutAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutAfterLast()
        {
            string str = "Mares Eat Oats Mares Eat";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CutAfterLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals(" Oats"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutAndPaste()
        {
            StringTheory theory = new StringTheory("0123456789");
            StringTheory theory2 = new StringTheory();
            string val = theory.Cut(4, 4);
            Assert.True(val.Length == 4, "Copied string length is wrong!");
            Assert.True(theory.Length == 6, "Original StringTheory length is wrong!");
            theory2.Paste(0, val);
            Assert.True(theory2.Length == 4, "Target StringTheory length is wrong!");
        }

        public void testCutBefore()
        {
            string str = "Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CutBefore("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares "), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CutBefore("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutBeforeLast()
        {
            string str = "Eat Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CutBeforeLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong:");
            Assert.True(str2.Equals("Mares Eat Oats Mares "), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutFrom()
        {
            string str = "Mares ";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CutFrom("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Eat Oats"), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CutAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutFromLast()
        {
            string str = "Mares Eat Oats Mares ";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CutFromLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Eat Oats"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutThru()
        {
            string str = " Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats");
            string str2 = theory.CutThru("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares Eat"), "The Cut string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CutThru("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testCutThruLast()
        {
            string str = " Oats";
            StringTheory theory = new StringTheory("Mares Eat Oats Mares Eat Oats");
            string str2 = theory.CutThruLast("Eat");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals("Mares Eat Oats Mares Eat"), "The emitted string is wrong.");
            theory.PasteOver("Mares Eat Oats");
            string str3 = theory.CopyAfter("XYZ");
            Assert.True("Mares Eat Oats".Equals(theory.ToString()), "[bogus] The resulting string is wrong.");
            Assert.True(str3.Length < 1, "[bogus] The Cut string is wrong.");
        }

        public void testDataIntegrity()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.ToString().Equals("0123456789"), "Data in StringTheory is wrong");
        }

        public void testEllipsize()
        {
            string str = "MaresEatOatsAnd";
            string str2 = "MaresEatOats...";
            StringTheory theory = new StringTheory("MaresEatOatsAnd");
            theory.Ellipsize(15);
            Assert.True(str.Equals(theory.ToString()), "str (1) doesn't match the comparison string!");
            theory = new StringTheory("MaresEatOatsAndDoesEatOats");
            theory.Ellipsize(15);
            Assert.True(str2.Equals(theory.ToString()), "str (2) doesn't match the ref2 string!");
        }

        public void testEndsWith()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.EndsWith("6789") && !theory.EndsWith("xxx"), "string ends differently?");
        }

        public void testEquals()
        {
            string str = "bozo";
            string str2 = "foo";
            StringTheory theory = new StringTheory("bozo");
            StringTheory theory2 = new StringTheory("bozo");
            StringTheory theory3 = new StringTheory("boxzxo");
            Assert.True(theory.Equals(str) && !theory.Equals(str2), "Failed while comparing with a string.");
            Assert.True(theory.Equals(theory2) && !theory.Equals(theory3), "Failed while comparing with another ST.");
        }

        public void testErase()
        {
            StringTheory theory = new StringTheory("0123456789");
            theory.Erase();
            Assert.True(theory.Length == 0, "StringTheory contents weren't erased! (1)");
            Assert.True(theory.IsEmpty(), "StringTheory contents weren't erased! (2)");
        }

        public void testEscape()
        {
            string str = "a\\\"b\\\\cd\\\\e\\\"f";
            StringTheory theory = new StringTheory("a\"b\\cd\\e\"f");
            theory.Escape();
            Assert.True(str.Equals(theory.ToString()), "str doesn't match the comparison string!");
        }

        public void testExpand()
        {
            string str = "abcdef    ";
            StringTheory theory = new StringTheory("abcdef");
            theory.Expand(10);
            Assert.True(str.Equals(theory.ToString()), "str doesn't match the comparison string!");
            str = "abcdefyyyy";
            theory = new StringTheory("abcdef");
            theory.Expand(10, 'y');
            Assert.True(str.Equals(theory.ToString()), "custom char didn't take");
        }

        public void testFill()
        {
            string str = "abcdefAAAAAAmnop";
            StringTheory theory = new StringTheory("abcdefghijklmnop");
            theory.Fill(6, 6, 'A');
            Assert.True(str.Equals(theory.ToString()), "comparison after Fill failed");
        }

        public void testFilterRx()
        {
            StringTheory theory = new StringTheory("5555-5555-5555-4444");
            theory.FilterRx(ST.ANY_NON_DIGIT + ST.ANY_COUNT);
            Assert.True(theory.Rx.Matches(ST.STARTS_WITH + ST.ANY_DIGIT + ST.REPEAT(0x10) + ST.END), "scrubRx failed: " + theory);
            theory = new StringTheory("5555,5555,5555,4444");
            theory.FilterRx(ST.ANY_NON_DIGIT + ST.ANY_COUNT);
            Assert.True(theory.Rx.Matches(ST.STARTS_WITH + ST.ANY_DIGIT + ST.REPEAT(0x10) + ST.END), "scrubRx failed: " + theory);
        }

        public void testFind()
        {
            StringTheory theory = new StringTheory("Mares eat oats");
            Assert.True(theory.Find("eat") == 6, "Couldn't find the test string");
            theory = new StringTheory("Mares eat oats Mares eat oats");
            Assert.True(theory.Find("eat") == 6, "1) Couldn't find the test string");
            Assert.True(theory.Find("eat", 7) == 0x15, "2) Couldn't find the test string");
            Assert.True(theory.Find("eat", 0) == 6, "3) Couldn't find the test string");
        }

        public void testFindNext()
        {
            StringTheory theory = new StringTheory("Mares eat oats Mares eat oats");
            Assert.True(theory.FindNext("eat") == 6, "1) Couldn't find the test string");
            Assert.True(theory.FindNext("eat") == 0x15, "2) Couldn't find the test string");
            theory.Reset();
            Assert.True(theory.FindNext("eat") == 6, "3) Couldn't find the test string");
        }

        public void testFindRx()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.Rx.Find(@"^\d+$") == 0, "Location of pattern is wrong");
            theory = new StringTheory("AbC123!!!");
            Assert.True(theory.Rx.Find("^[A].*") == 0, "Second location of pattern is wrong!");
        }

        public void testFirst()
        {
            StringTheory theory = new StringTheory("xyzabc");
            Assert.True(theory.First() == 'x', "first char doesn't match comparison");
        }

        public void testFix()
        {
            StringTheory theory = new StringTheory("5555-5555-5555-4444");
            theory.Fix(Credit.MasterCard);
            Assert.True(theory.Rx.Matches(Credit.MC_WITH_DASHES), "fix failed: " + theory);
        }

        public void testFixAMEX()
        {
            StringTheory theory = new StringTheory("341111111111111");
            Assert.True(theory.Fix(Credit.AMEX), "Couldn't fix test data to make it AMEX.");
            Assert.True(theory.Rx.Matches(Credit.AMEX), "AMEX fix didn't work correctly:" + theory);
        }

        public void testFlip()
        {
            string str = "987654321";
            StringTheory theory = new StringTheory("123456789");
            theory.Flip();
            Assert.True(str.Equals(theory.ToString()), "reverse results don't match the comparison string!" + theory);
        }

        public void testFormat()
        {
            StringTheory theory = new StringTheory("341111111111111");
            if (!theory.Equals("3411-111111-11111"))
            {
                theory.Format("####-######-#####");
            }
            Assert.True(theory.Equals("3411-111111-11111"), "AMEX Format didn't work:" + theory);
            StringTheory theory2 = new StringTheory("18003334456");
            if (!theory2.Equals("1-800-333-4456"))
            {
                theory2.Format("#-###-###-####");
            }
            Assert.True(theory2.Equals("1-800-333-4456"), "Phone Format didn't work:" + theory2);
            theory2.Renew("18003334456");
            theory2.Format("");
            Assert.True(theory2.Equals("18003334456"), "Format failed when given a blank Format. It *should* pass thru." + theory2);
        }

        public void testGet()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory[4] == '4', "Data in StringTheory is wrong");
        }

        public void testIsDifferent()
        {
            string str = "Mares eat oats.";
            StringTheory theory = new StringTheory("XMares eat oatsX.");
            Assert.True(theory.Differs(str), "Strings don't contain same content");
        }

        public void testIsLower()
        {
            StringTheory theory = new StringTheory("MARES eat oats");
            Assert.True( theory.IsLower(6, 8), "The specified range isn't lower case.");
            Assert.True(new StringTheory("mares eat oats").IsLower(), "The entire string isn't lower case.");
            this.AssertFalse("The entire string is upper case, but this is reporting it as lower case!", new StringTheory("MARES EAT OATS").IsLower());
            theory = new StringTheory("MARES EaT OATS");
            Assert.True(theory.IsLower(7), "The spec'd char isn't lower case.");
            theory = new StringTheory("mares e-t oats");
            this.AssertFalse("The spec'd non-alpha char was reported as lower case?", theory.IsLower(7));
        }

        public void testIsSame()
        {
            string str = "Mares eat oats.";
            StringTheory theory = new StringTheory("Mares eat oats.");
            Assert.True(theory.SameAs(str), "Strings don't contain same content");
        }

        public void testIsUpper()
        {
            StringTheory theory = new StringTheory("mares EAT OATS");
            Assert.True(theory.IsUpper(6, 8), "The specified range isn't upper case.");
            Assert.True(new StringTheory("MARES EAT OATS").IsUpper(), "The entire string isn't upper case.");
            this.AssertFalse("The entire string is lower case, but this is reporting it as upper case!", new StringTheory("mares eat oats").IsUpper());
            theory = new StringTheory("mares eAt oats");
            Assert.True(theory.IsUpper(7), "The spec'd char isn't upper case.");
            theory = new StringTheory("mares e-t oats");
            this.AssertFalse("The spec'd non-alpha char was reported as upper case?", theory.IsUpper(7));
        }

        public void testJoin()
        {
            string str = "foo.foo.foo.foo.foo";
            StringTheory theory = new StringTheory();
            string[] strArray = new string[] { "foo", "foo", "foo", "foo", "foo" };
            theory.Join(strArray, '.');
            Assert.True(str.Equals(theory.ToString()), "1) Joined string doesn't match comparison");
            theory = new StringTheory();
            ArrayList list = new ArrayList();
            list.Add("foo");
            list.Add("foo");
            list.Add("foo");
            list.Add("foo");
            list.Add("foo");
            theory.Join(list, '.');
            Assert.True(str.Equals(theory.ToString()), "2) Joined string doesn't match comparison");
        }

        public void testLast()
        {
            StringTheory theory = new StringTheory("xyzabc");
            Assert.True(theory.Last() == 'c', "last char doesn't match comparison");
        }

        public void testLeftJustify()
        {
            string str = "abcdef \t  \t   ";
            StringTheory theory = new StringTheory("  abcdef \t  \t ");
            theory.LeftJustify();
            Assert.True(str.Equals(theory.ToString()), "leftJustify str doesn't match the comparison string!");
            theory.PasteOver("      ");
            theory.LeftJustify();
            Assert.True(theory.Length == 6, "leftJustify str length isnt' right");
        }

        public void testLineCount()
        {
            StringTheory theory = new StringTheory("Line 1\r\nLine 2\r\nLine 3\r\n");
            StringTheory theory2 = new StringTheory("Line 1\nLine 2\nLine 3\n");
            StringTheory theory3 = new StringTheory("Line 1\rLine 2\rLine 3\r");
            StringTheory theory4 = new StringTheory("Line 1\r\nLine 2\r\nLine 3");
            StringTheory theory5 = new StringTheory("Line 1\nLine 2\nLine 3");
            StringTheory theory6 = new StringTheory("Line 1\rLine 2\rLine 3");
            Assert.True((theory2.Lines().Length == 3) && (theory5.Lines().Length == 3), "Wrong line count in {unix}: ");
            Assert.True((theory3.Lines().Length == 3) && (theory6.Lines().Length == 3), "Wrong line count in {mac}: ");
            Assert.True((theory.Lines().Length == 3) && (theory4.Lines().Length == 3), "Wrong line count in {pc}: " + theory.Lines().Length);
        }

        public void testLines()
        {
            string[] strArray = new StringTheory("Mares eat oats\nand does eat oats\nand little lambs eat ivy.\n").Lines();
            Assert.True(strArray.Length == 3, "Wrong line count! - " + strArray.Length);
        }

        public void testLoadFile()
        {
            StringTheory theory = new StringTheory();
            Assert.True(theory.LoadFile(@"C:\Built\Products\StringTheory\1.4\C#\_development\StringTheory\Built\UnitTests\justify.txt"), "Couldn't load file?");
            Assert.True(theory.Length > 0, "There should be some data in the file!");
            Assert.True(theory.Lines().Length > 1, "There should be many lines in the file!");
        }

        public void testLower()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("AbCDeF");
            theory.Lower();
            Assert.True(str.Equals(theory.ToString()), "lowered str doesn't match the comparison string!");
            str = "AbCdEFG";
            theory = new StringTheory("ABCDEFG");
            theory.Lower(1);
            theory.Lower(3);
            Assert.True(str.Equals(theory.ToString()), "lowered(idx) str doesn't match the comparison string!");
            str = "ABcdeFG";
            theory = new StringTheory("ABCDEFG");
            theory.Lower(2, 3);
            Assert.True(str.Equals(theory.ToString()), "lowered(idx, len) str doesn't match the comparison string!");
        }

        public void testMatches()
        {
            StringTheory theory = new StringTheory("0123456789");
            Assert.True(theory.Rx.Matches(@"^\d+$"), "Pattern doesn't match!");
            theory = new StringTheory("AbC123!!!");
            Assert.True(theory.Rx.Matches("^[A].*"), "Second pattern doesn't match!");
        }

        public void testPad()
        {
            string str = "123456    ";
            StringTheory theory = new StringTheory("123456");
            theory.Pad(4);
            Assert.True(str.Equals(theory.ToString()), "pad's result doesn't match the comparison string!");
            str = "123456";
            theory = new StringTheory("123456");
            theory.Pad(-1);
            Assert.True(str.Equals(theory.ToString()), "pad failed using a bogus quantity");
            str = "123456xxxx";
            theory = new StringTheory("123456");
            theory.Pad(4, 'x');
            Assert.True(str.Equals(theory.ToString()), "pad failed while padding with a custom character");
        }

        public void testPadEnds()
        {
            string str = "    123456    ";
            StringTheory theory = new StringTheory("123456");
            theory.PadEnds(4);
            Assert.True(str.Equals(theory.ToString()), "padEnds' result doesn't match the comparison string!");
            str = "123456";
            theory = new StringTheory("123456");
            theory.PadEnds(-1);
            Assert.True(str.Equals(theory.ToString()), "padEnds failed using a bogus quantity");
            str = "xxx123456xxx";
            theory = new StringTheory("123456");
            theory.PadEnds(3, 'x');
            Assert.True(str.Equals(theory.ToString()), "padEnds failed while padding with a custom character");
        }

        public void testPadLeft()
        {
            string str = "    123456";
            StringTheory theory = new StringTheory("123456");
            theory.PadLeft(4);
            Assert.True(str.Equals(theory.ToString()), "padLeft's result doesn't match the comparison string!");
            str = "123456";
            theory = new StringTheory("123456");
            theory.PadLeft(-1);
            Assert.True(str.Equals(theory.ToString()), "padLeft failed using a bogus quantity");
            str = "xxxx123456";
            theory = new StringTheory("123456");
            theory.PadLeft(4, 'x');
            Assert.True(str.Equals(theory.ToString()), "padLeft failed while padding with a custom character");
        }

        public void testPadRight()
        {
            string str = "123456    ";
            StringTheory theory = new StringTheory("123456");
            theory.PadRight(4);
            Assert.True(str.Equals(theory.ToString()), "padRight's result doesn't match the comparison string!");
            str = "123456";
            theory = new StringTheory("123456");
            theory.PadRight(-1);
            Assert.True(str.Equals(theory.ToString()), "padRight failed using a bogus quantity");
            str = "123456xxxx";
            theory = new StringTheory("123456");
            theory.PadRight(4, 'x');
            Assert.True(str.Equals(theory.ToString()), "padRight failed while padding with a custom character");
        }

        public void testpasteOver()
        {
            string str = "MaresEatOats";
            StringTheory theory = new StringTheory("MaresChowDownOnOats");
            theory.PasteOver(5, 10, "Eat");
            Assert.True(str.Equals(theory.ToString()), "overwritten (1) str doesn't match the comparison string!");
            str = "MaresEatOats";
            theory = new StringTheory("lorem ipsum");
            theory.PasteOver("MaresEatOats");
            Assert.True(str.Equals(theory.ToString()), "overwritten (2) str doesn't match the comparison string!");
        }

        public void testPattern_CC_AMEX()
        {
            StringTheory theory = new StringTheory();
            theory.Renew("3411-111111-11111");
            Assert.True(theory.Rx.Matches(Credit.AMEX), "AMEX pattern 1 failed?");
            theory.Renew("341111111111111");
            Assert.True(theory.Rx.Matches(Credit.AMEX), "AMEX pattern 2 failed");
            theory.Renew("3711 111111 11111");
            Assert.True(theory.Rx.Matches(Credit.AMEX), "AMEX pattern 3 failed");
            theory.Renew("123555-123xss");
            this.AssertFalse("Non-AMEX pattern 1 failed to be caught", theory.Rx.Matches(Credit.AMEX));
            theory.Renew("4311-111111-11111");
            this.AssertFalse("Non-AMEX pattern 2 failed to be caught", theory.Rx.Matches(Credit.AMEX));
        }

        public void testPattern_CC_DISC()
        {
            StringTheory theory = new StringTheory();
            theory.Renew("6011-1111-1111-1111");
            Assert.True(theory.Rx.Matches(Credit.DISC), "DISC pattern 1 failed?");
            theory.Renew("6011111111111111");
            Assert.True(theory.Rx.Matches(Credit.DISC), "DISC pattern 2 failed");
            theory.Renew("6011 1111 1111 1111");
            Assert.True(theory.Rx.Matches(Credit.DISC), "DISC pattern 3 failed");
            theory.Renew("123555-123xss");
            this.AssertFalse("Non-DISC pattern 1 failed to be caught", theory.Rx.Matches(Credit.DISC));
            theory.Renew("6111-1111-1111-1111");
            this.AssertFalse("Non-DISC pattern 2 failed to be caught", theory.Rx.Matches(Credit.DISC));
        }

        public void testPattern_CC_MC()
        {
            StringTheory theory = new StringTheory();
            theory.Renew("5111-1111-1111-1111");
            Assert.True(theory.Rx.Matches(Credit.MC), "MC pattern 1 failed?");
            theory.Renew("5511111111111111");
            Assert.True(theory.Rx.Matches(Credit.MC), "MC pattern 2 failed");
            theory.Renew("5311 1111 1111 1111");
            Assert.True(theory.Rx.Matches(Credit.MC), "MC pattern 3 failed");
            theory.Renew("123555-123xss");
            this.AssertFalse("Non-MC pattern 1 failed to be caught", theory.Rx.Matches(Credit.MC));
            theory.Renew("4111-1111-1111-1111");
            this.AssertFalse("Non-MC pattern 2 failed to be caught", theory.Rx.Matches(Credit.MC));
        }

        public void testPattern_CC_VISA()
        {
            StringTheory theory = new StringTheory();
            theory.Renew("4111-1111-1111-1111");
            Assert.True(theory.Rx.Matches(Credit.VISA), "VISA pattern 1 failed?" + Credit.VISA);
            theory.Renew("4111111111111111");
            Assert.True(theory.Rx.Matches(Credit.VISA), "VISA pattern 2 failed");
            theory.Renew("4111 1111 1111 1111");
            Assert.True(theory.Rx.Matches(Credit.VISA), "VISA pattern 3 failed");
            theory.Renew("123555-123xss");
            this.AssertFalse("Non-VISA pattern 1 failed to be caught", theory.Rx.Matches(Credit.VISA));
            theory.Renew("5111-1111-1111-1111");
            this.AssertFalse("Non-VISA pattern 2 failed to be caught", theory.Rx.Matches(Credit.VISA));
        }

        public void testPattern_US_SSN()
        {
            StringTheory theory = new StringTheory();
            theory.Renew("555-55-5555");
            Assert.True(theory.Rx.Matches(ID.SSN), "Perfect SSN pattern failed?");
            theory.Renew("555555555");
            Assert.True(theory.Rx.Matches(ID.SSN), "The imperfect SSN pattern failed");
            theory.Renew("123555-123xss");
            this.AssertFalse("Non-SSN pattern failed to be caught", theory.Rx.Matches(ID.SSN));
        }

        public void testPrepend()
        {
            string str = "abcxyz";
            StringTheory theory = new StringTheory("xyz");
            theory.Prepend("abc");
            Assert.True(str.Equals(theory.ToString()), "Simple string prepend failed");
            str = "abcxyz";
            theory = new StringTheory("xyz");
            StringTheory theory2 = new StringTheory("abc");
            theory.Prepend(theory2);
            Assert.True(str.Equals(theory.ToString()), "StringTheory prepend failed");
            str = "FooFooFooxyz";
            theory = new StringTheory("xyz");
            string[] strArray = new string[] { "Foo", "Foo", "Foo" };
            theory.Prepend((object[])strArray);
            Assert.True(str.Equals(theory.ToString()), "StringTheory prepend(array) failed");
            str = "FooFooFooxyz";
            theory = new StringTheory("xyz");
            ArrayList list = new ArrayList();
            list.Add("Foo");
            list.Add("Foo");
            list.Add("Foo");
            theory.Prepend((ICollection)list);
            Assert.True(str.Equals(theory.ToString()), "StringTheory prepend(ArrayList) failed" + theory);
        }

        public void testProper()
        {
            string str = "Thomas";
            StringTheory theory = new StringTheory("THOMAS");
            theory.Proper();
            Assert.True(str.Equals(theory.ToString()), "proper str doesn't match the comparison string!");
        }

        public void testProperWords()
        {
            string str = "Mares Eat Oats";
            StringTheory theory = new StringTheory("mares eat oats");
            theory.ProperWords();
            Assert.True(str.Equals(theory.ToString()), "properWords failed?");
        }

        public void testPush()
        {
            StringTheory theory = new StringTheory();
            Assert.True(theory.Push('m') == 1, "push is misreporting length!");
            Assert.True(theory.Push('a') == 2, "push is misreporting length!");
            Assert.True(theory.Push('r') == 3, "push is misreporting length!");
            Assert.True(theory.Push('e') == 4, "push is misreporting length!");
            Assert.True(theory.Push('s') == 5, "push is misreporting length!");
            Assert.True(theory.Equals("mares"), "PUSH results didn't match expectations: " + theory);
        }

        public void testQuote()
        {
            string str = "\"abcdef\"";
            StringTheory theory = new StringTheory("abcdef");
            theory.Quote();
            Assert.True(str.Equals(theory.ToString()), "str 1 doesn't match the comparison string!");
            str = "***abcdef***";
            theory = new StringTheory("abcdef");
            theory.Quote("***");
            Assert.True(str.Equals(theory.ToString()), "str 2 doesn't match the comparison string!");
            str = "|abcdef|";
            theory = new StringTheory("abcdef");
            theory.Quote('|');
            Assert.True(str.Equals(theory.ToString()), "str 3 doesn't match the comparison string!");
        }

        public void testRepeat()
        {
            string str = "Mares eat oats.Mares eat oats.Mares eat oats.Mares eat oats.Mares eat oats.Mares eat oats.";
            StringTheory theory = new StringTheory("Mares eat oats.");
            theory.Repeat(5);
            Assert.True(str.Equals(theory.ToString()), "Repeat 5 times doesn't match.");
            theory = new StringTheory("Mares eat oats.");
            theory.Repeat(0);
            Assert.True("Mares eat oats.".Equals(theory.ToString()), "Repeat 0 times doesn't match.");
            theory.Repeat(-1);
            Assert.True("Mares eat oats.".Equals(theory.ToString()), "Repeat -1 times doesn't match.");
            theory.Repeat(3);
            Assert.True("Mares eat oats.Mares eat oats.Mares eat oats.Mares eat oats.".Equals(theory.ToString()), "Repeat 3 times doesn't match.");
        }

        public void testReplace()
        {
            StringTheory theory = new StringTheory("Lorem ipsum. Ve misdebus. U misdebus. I misdebus.");
            StringTheory theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            int num = theory2.Replace("$", " ");
            Assert.True(num == 7, "Reported number of tilted slashes was wrong.");
            Assert.True(theory2.Equals(theory), "Chars didn't translate properly.");
            theory = new StringTheory("Lorem ipsum. Ve misdebus. U misdebus. I misdebus.");
            theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            num = theory2.Replace('$', ' ');
            Assert.True(num == 7, "Reported number of tilted slashes was wrong.");
            Assert.True(theory2.Equals(theory), "Chars didn't translate properly.");
            theory = new StringTheory("Lorem ipsum. Ve misdebus. U misdebus. I misdebus.");
            theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            num = theory2.Replace('$', " ");
            Assert.True(num == 7, "Reported number of tilted slashes was wrong.");
            Assert.True(theory2.Equals(theory), "Chars didn't translate properly.");
            theory = new StringTheory("Lorem ipsum. Ve misdebus. U misdebus. I misdebus.");
            theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            num = theory2.Replace("$", ' ');
            Assert.True(num == 7, "Reported number of tilted slashes was wrong.");
            Assert.True(theory2.Equals(theory), "Chars didn't translate properly.");
            theory = new StringTheory("Lorem_$_ipsum._$_Ve_$_misdebus._$_U_$_misdebus._$_I_$_misdebus.");
            theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            num = theory2.Replace("$", "_$_");
            Assert.True(theory2.Equals(theory), "(x)Chars didn't translate properly.");
        }

        public void testReplaceRx()
        {
            StringTheory theory = new StringTheory("aabfooaabfooabfoob");
            theory.Rx.Replace("a*b", "-");
            Assert.True(theory.Equals("-foo-foo-foo-"), "rx_replace() didn't emit the expected result.");
        }

        public void testRevFind()
        {
            StringTheory theory = new StringTheory("Mares eat oats");
            Assert.True(theory.RevFind("eat") == 6, "Couldn't find the test string");
            theory = new StringTheory("Mares eat oats Mares eat oats");
            Assert.True(theory.RevFind("eat") == 0x15, "1) Couldn't find the test string");
            Assert.True(theory.RevFind("eat", 20) == 6, "2) Couldn't find the test string");
            Assert.True(theory.RevFind("eat", theory.End()) == 0x15, "3) Couldn't find the test string");
        }

        public void testRevFindNext()
        {
            StringTheory theory = new StringTheory("Mares eat oats Mares eat oats");
            Assert.True(theory.RevFindNext("eat") == 0x15, "1) Couldn't find the test string");
            Assert.True(theory.RevFindNext("eat") == 6, "2) Couldn't find the test string");
            theory.Reset();
            Assert.True(theory.RevFindNext("eat") == 0x15, "3) Couldn't find the test string");
        }

        public void testRightJustify()
        {
            string str = " \t  \t   abcdef";
            StringTheory theory = new StringTheory("  abcdef \t  \t ");
            theory.RightJustify();
            Assert.True(str.Equals(theory.ToString()), "rightJustify str doesn't match the comparison string!");
            theory.PasteOver("      ");
            theory.RightJustify();
            Assert.True(theory.Length == 6, "rightJustify str length isnt' right");
        }

        public void testRxFindNext()
        {
            StringTheory theory = new StringTheory("Mares eat oats Mares eat oats");
            Assert.True(theory.Rx.FindNext("ea.?") == 6, "1) Couldn't find the test string");
            Assert.True(theory.Rx.FindNext("ea.?") == 0x15, "2) Couldn't find the test string");
            theory.Reset();
            Assert.True(theory.Rx.FindNext("ea.?") == 6, "3) Couldn't find the test string");
        }

        public void testScrub()
        {
            StringTheory theory = new StringTheory("%555-33-0000*");
            theory.Filter("-.$%*");
            Assert.True(theory.Equals("555330000"), "Scrub(1) failed: " + theory);
            theory.Renew("%555-33-0000*");
            theory.Filter("-");
            Assert.True(theory.Equals("%555330000*"), "Scrub(2) failed: " + theory);
        }

        public void testSerialization()
        {
            string str = "Mares eat oats Mares eat oats";
            StringTheory graph = new StringTheory("Mares eat oats Mares eat oats");
            graph.RevFindNext("eat");
            Assert.True(graph.RevFindNext("eat") == 6, "");
            string path = "st_test.per";
            bool condition = true;
            Stream serializationStream = null;
            try
            {
                serializationStream = File.Open(path, FileMode.Create, FileAccess.ReadWrite);
                new BinaryFormatter().Serialize(serializationStream, graph);
                serializationStream.Close();
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.StackTrace);
                condition = false;
            }
            Assert.True(condition, "Something went wrong while persisting the string to a file.");
            graph = null;
            condition = true;
            try
            {
                serializationStream = File.Open(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter2 = new BinaryFormatter();
                graph = (StringTheory)formatter2.Deserialize(serializationStream);
            }
            catch (IOException exception2)
            {
                Console.WriteLine(exception2.StackTrace);
                condition = false;
            }
            Assert.True(condition, "Something went wrong while retrieving the string from a file.");
            Assert.True(str.Equals(graph.ToString()), "Strings don't match now.");
            Assert.True(graph.FindNext("eat") == 0x15, "string state is incorrect!");
        }

        public void testSet()
        {
            StringTheory theory = new StringTheory("0123456789");
            theory[4] = '*';
            Assert.True(theory[4] == '*', "Data in StringTheory is wrong");
        }

        public void testShift()
        {
            StringTheory theory = new StringTheory("mares");
            Assert.True(theory.Shift() == 'm', "shift failed (m)");
            Assert.True(theory.Shift() == 'a', "shift failed (a)");
            Assert.True(theory.Shift() == 'r', "shift failed (r)");
            Assert.True(theory.Shift() == 'e', "shift failed (e)");
            Assert.True(theory.Shift() == 's', "shift failed (s)");
        }

        public void testSingleQuote()
        {
            string str = "'abcdef'";
            StringTheory theory = new StringTheory("abcdef");
            theory.SingleQuote();
            Assert.True(str.Equals(theory.ToString()), "str doesn't match the comparison string!");
        }

        public void testSingleSpace()
        {
            string str = " abc def ";
            StringTheory theory = new StringTheory("  abc   def \t  \t   ");
            theory.SingleSpace();
            Assert.True(str.Equals(theory.ToString()), "singleSpace str doesn't match the comparison string!");
        }

        public void testSpacesToTabs()
        {
            string str = "\t 123456\t   ";
            StringTheory theory = new StringTheory("      123456        ");
            theory.SpacesToTabs(5);
            Assert.True(str.Equals(theory.ToString()), "'tabsToSpaces' result doesn't match the comparison string!:" + theory);
            str = "123456";
            theory = new StringTheory("123456");
            theory.SpacesToTabs(-1);
            Assert.True(str.Equals(theory.ToString()), "tabsToSpaces failed using a bogus quantity");
        }

        public void testSplit()
        {
            StringTheory theory = new StringTheory("mares*eat*oats");
            ArrayList list = theory.Split('*');
            Assert.AreEqual(3, list.Count, "1) Wrong number of strings returned");
            Assert.AreEqual("mares", list[0].ToString(), "1) Returned list item[0] doesn't match.");
            Assert.AreEqual("eat", list[1].ToString(), "1) Returned list item[1] doesn't match.");
            Assert.AreEqual("oats", list[2].ToString(), "1) Returned list item[2] doesn't match.");
            theory.PasteOver("maresFOOBAReatFOOBARoats");
            list = theory.Split("FOOBAR");
            Assert.AreEqual(3, list.Count, "2) Wrong number of strings returned");
            Assert.AreEqual("mares", list[0].ToString(), "2) Returned list item[0] doesn't match.");
            Assert.AreEqual("eat", list[1].ToString(), "2) Returned list item[1] doesn't match.");
            Assert.AreEqual("oats", list[2].ToString(), "2) Returned list item[2] doesn't match.");
        }

        public void testSplitToStrings()
        {
            StringTheory theory = new StringTheory("mares*eat*oats");
            string[] strArray = theory.SplitToStrings('*');
            Assert.AreEqual(3, strArray.Length, "1) Wrong number of strings returned");
            Assert.AreEqual("mares", strArray[0], "1) Returned list item[0] doesn't match.");
            Assert.AreEqual("eat", strArray[1], "1) Returned list item[1] doesn't match.");
            Assert.AreEqual("oats", strArray[2], "1) Returned list item[2] doesn't match.");
            theory.PasteOver("maresFOOBAReatFOOBARoats");
            strArray = theory.SplitToStrings("FOOBAR");
            Assert.AreEqual(3, strArray.Length, "2) Wrong number of strings returned");
            Assert.AreEqual("mares", strArray[0], "2) Returned list item[0] doesn't match.");
            Assert.AreEqual("eat", strArray[1], "2) Returned list item[1] doesn't match.");
            Assert.AreEqual("oats", strArray[2], "2) Returned list item[2] doesn't match.");
        }

        public void testStrip()
        {
            string str = "123456";
            string str2 = "";
            StringTheory theory = new StringTheory("1 2   3\t 4\t\t\t\t\t\t5 6              ");
            theory.Strip();
            Assert.True(str.Equals(theory.ToString()), "strip test 1 results don't match the comparison string!");
            theory = new StringTheory("\t    \t\t  \t\t\t \t              ");
            theory.Strip();
            Assert.True(str2.Equals(theory.ToString()), "strip test 2 results don't match the comparison string!");
        }

        public void testStripRange()
        {
            string str = "abcd ef";
            StringTheory theory = new StringTheory("ab   \tcd ef");
            theory.StripRange(0, 7);
            Assert.True(str.Equals(theory.ToString()), "trimmed str doesn't match the comparison string!");
        }

        public void testStripSpaces()
        {
            StringTheory theory = new StringTheory(" Hi! There!");
            StringTheory theory2 = new StringTheory("Hi!There!");
            Assert.True(theory.StripSpaces() == 2, "Reported number of spaces was wrong.");
            Assert.True(theory.Equals(theory2), "Test string doesn't match the reference.");
        }

        public void testStripTabs()
        {
            StringTheory theory = new StringTheory("\tHi!\tThere!");
            StringTheory theory2 = new StringTheory("Hi!There!");
            Assert.True(theory.StripTabs() == 2, "Reported number of tabs was wrong.");
            Assert.True(theory.Equals(theory2), "Test string doesn't match the reference.");
        }

        public void testTabsToSpaces()
        {
            string str = "     123456     ";
            StringTheory theory = new StringTheory("\t123456\t");
            theory.TabsToSpaces(5);
            Assert.True(str.Equals(theory.ToString()), "tabsToSpaces' result doesn't match the comparison string!");
            str = "123456";
            theory = new StringTheory("123456");
            theory.TabsToSpaces(-1);
            Assert.True(str.Equals(theory.ToString()), "tabsToSpaces failed using a bogus quantity");
        }

        public void testTemplatePopulationWithBean()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Ph#: 202-456-1414Years old: 57";
            StringTheory theory = new StringTheory("Name: FirstName LastNameAddress: Address1 Address2, City, State ZipPh#: PhoneYears old: Age");
            theory.Populate(this.CreatePersonBean());
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string." + theory);
        }

        public void testTemplatePopulationWithHashtable()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Phone: 202-456-1414Age: 57";
            StringTheory theory = new StringTheory("Name: %FirstName% %LastName%Address: %Address1% %Address2%, %City%, %State% %Zip%Phone: %Phone%Age: %Age%");
            Hashtable values = new Hashtable();
            values["FirstName"] = "George";
            values["LastName"] = "Bush";
            values["Address1"] = "1600 Pennsylvania Ave.";
            values["Address2"] = "(c/o Laura)";
            values["City"] = "Washington";
            values["State"] = "DC";
            values["Zip"] = "20500";
            values["Phone"] = "202-456-1414";
            values["Age"] = 0x39;
            theory.Populate(values, "%*%");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        public void testTemplatePopulationWithListOfSpecificBeans()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Product Number: 202-456-1Qty: 57";
            StringTheory theory = new StringTheory("Name: %Built.Text.UnitTests.PersonBean.FirstName% %Built.Text.UnitTests.PersonBean.LastName%Address: %Built.Text.UnitTests.PersonBean.Address1% %Built.Text.UnitTests.PersonBean.Address2%, %Built.Text.UnitTests.PersonBean.City%, %Built.Text.UnitTests.PersonBean.State% %Built.Text.UnitTests.PersonBean.Zip%Product Number: %Built.Text.UnitTests.OrderBean.ProductNumber%Qty: %Built.Text.UnitTests.OrderBean.Quantity%");
            object[] list = new object[] { this.CreatePersonBean(), this.CreateOrderBean() };
            theory.PopulateExact(list, "%*%");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string." + theory);
        }

        public void testTemplatePopulationWithResultSetEntry()
        {
            OleDbConnection connected = this.GetConnected();
            this.PopulatingRecordCreate(connected);
            string str = "Name: George BushPhone: 202-456-1414";
            StringTheory theory = new StringTheory("Name: %FIRST_NAME% %LAST_NAME%Phone: %PHONE%");
            OleDbDataReader anObject = this.PopulatingRecordRetrieve(connected);
            Assert.NotNull(anObject, "The result set is coming back null.");
            if (!anObject.IsClosed && anObject.HasRows)
            {
                anObject.Read();
                theory.Populate((IDataReader)anObject, "%*%");
            }
            anObject.Close();
            this.PopulatingRecordCleanup(connected);
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string. " + theory);
            anObject.Close();
            connected.Close();
        }

        public void testTemplatePopulationWithSpecificBean()
        {
            string str = "Name: George BushAddress: 1600 Pennsylvania Ave. (c/o Laura), Washington, DC 20500Product Number: 202-456-1Qty: 57";
            StringTheory theory = new StringTheory("Name: %Built.Text.UnitTests.PersonBean.FirstName% %Built.Text.UnitTests.PersonBean.LastName%Address: %Built.Text.UnitTests.PersonBean.Address1% %Built.Text.UnitTests.PersonBean.Address2%, %Built.Text.UnitTests.PersonBean.City%, %Built.Text.UnitTests.PersonBean.State% %Built.Text.UnitTests.PersonBean.Zip%Product Number: %Built.Text.UnitTests.OrderBean.ProductNumber%Qty: %Built.Text.UnitTests.OrderBean.Quantity%");
            theory.PopulateExact(this.CreatePersonBean(), "%*%");
            theory.PopulateExact(this.CreateOrderBean(), "%*%");
            Assert.True(str.Equals(theory.ToString()), "Populated template doesn't match reference string.");
        }

        public void testTiltBack()
        {
            StringTheory theory = new StringTheory("http://www.google.com/folder/subfolder/file.html");
            StringTheory theory2 = new StringTheory(@"http:\\www.google.com\folder\subfolder\file.html");
            Assert.True(theory.TiltBack() == 5, "Reported number of tilted slashes was wrong.");
            Assert.True(theory.Equals(theory2), "Slashes aren't all tilted properly.");
        }

        public void testTiltForward()
        {
            StringTheory theory = new StringTheory(@"http:\\www.google.com\folder\subfolder\file.html");
            StringTheory theory2 = new StringTheory("http://www.google.com/folder/subfolder/file.html");
            Assert.True(theory.TiltForward() == 5, "Reported number of tilted slashes was wrong.");
            Assert.True(theory.Equals(theory2), "Slashes aren't all tilted properly.");
        }

        public void testToDouble()
        {
            StringTheory theory = new StringTheory("966700004");
            double num = theory.ToDouble();
            Assert.True(966700004.0 == num, "strings don't match");
            theory.Renew("");
            Assert.True(0.0 == theory.ToDouble(), "value should be zero");
        }

        public void testToFloat()
        {
            StringTheory theory = new StringTheory("4367.002");
            float num = theory.ToFloat();
            float num2 = 4367.002f;
            Assert.True(num2 == num, "strings don't match");
            theory.Renew("");
            Assert.True(0f == theory.ToFloat(), "value should be zero");
        }

        public void testToInt()
        {
            StringTheory theory = new StringTheory("056701");
            int actual = theory.ToInt();
            Assert.AreEqual(0xdd7d, actual, "strings don't match");
            theory.Renew("");
            Assert.True(0 == theory.ToInt(), "value should be zero");
        }

        public void testToLong()
        {
            StringTheory theory = new StringTheory("966700004");
            long num = theory.ToLong();
            Assert.True(0x399eabe4L == num, "strings don't match");
            theory.Renew("");
            Assert.True(0L == theory.ToLong(), "value should be zero");
        }

        public void testTouched()
        {
            StringTheory theory = new StringTheory("mares eat oats");
            theory.Append(theory.Cut(5, 4));
            Assert.True(theory.Touched(), "str doesn't think it was touched.");
            theory = new StringTheory("mares eat oats");
            theory.Upper(7);
            Assert.True(theory.Touched(), "str doesn't think it was touched. 2");
        }

        public void testTransWithACharacter()
        {
            StringTheory theory = new StringTheory("Lorem ipsum. Ve misdebus. U misdebus. I misdebus.");
            StringTheory theory2 = new StringTheory("Lorem$ipsum.$Ve$misdebus.$U$misdebus.$I$misdebus.");
            int num = theory2.Replace('$', ' ');
            Assert.True(num == 7, "Reported number of tilted slashes was wrong.");
            Assert.True(theory2.Equals(theory), "Chars didn't translate properly.");
        }

        public void testTrim()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("    \t abcdef  \t  ");
            theory.Trim();
            Assert.True(str.Equals(theory.ToString()), "trimmed str doesn't match the comparison string!");
        }

        public void testTrimLeft()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("    \t abcdef");
            theory.TrimLeft();
            Assert.True(str.Equals(theory.ToString()), "trimmed str doesn't match the comparison string!");
        }

        public void testTrimLineEnds()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("abcdef\n");
            theory.TrimLineEnds();
            Assert.True(str.Equals(theory.ToString()), "failed on unix eol");
            StringTheory theory2 = new StringTheory("abcdef\r\n");
            theory2.TrimLineEnds();
            Assert.True(str.Equals(theory2.ToString()), "failed on pc eol");
            StringTheory theory3 = new StringTheory("abcdef\r");
            theory3.TrimLineEnds();
            Assert.True(str.Equals(theory3.ToString()), "failed on mac eol");
        }

        public void testTrimRight()
        {
            string str = "abcdef";
            StringTheory theory = new StringTheory("abcdef  \t  ");
            theory.TrimRight();
            Assert.True(str.Equals(theory.ToString()), "trimmed str doesn't match the comparison string!");
        }

        public void testUnicode()
        {
            string str = "여보세요";
            StringTheory theory = new StringTheory("여보세요 세계");
            string str2 = theory.CutAfter("요");
            Assert.True(str.Equals(theory.ToString()), "The resulting string is wrong.");
            Assert.True(str2.Equals(" 세계"), "The Cut string is wrong.");
        }

        public void testUnshift()
        {
            StringTheory theory = new StringTheory();
            Assert.True(theory.Unshift('s') == 1, "Unshift is misreporting length!");
            Assert.True(theory.Unshift('e') == 2, "Unshift is misreporting length!");
            Assert.True(theory.Unshift('r') == 3, "Unshift is misreporting length!");
            Assert.True(theory.Unshift('a') == 4, "Unshift is misreporting length!");
            Assert.True(theory.Unshift('m') == 5, "Unshift is misreporting length!");
            Assert.True(theory.Equals("mares"), "UNSHIFT results didn't match expectations: [" + theory + "]");
        }

        public void testUpper()
        {
            string str = "ABCDEF";
            StringTheory theory = new StringTheory("AbCDeF");
            theory.Upper();
            Assert.True(str.Equals(theory.ToString()), "uppered str doesn't match the comparison string!");
            str = "AbCdEFG";
            theory = new StringTheory("abcdefg");
            theory.Upper(0);
            theory.Upper(2);
            theory.Upper(4);
            theory.Upper(5);
            theory.Upper(6);
            Assert.True(str.Equals(theory.ToString()), "uppered(idx) str doesn't match the comparison string!");
            str = "abCDEfg";
            theory = new StringTheory("abcdefg");
            theory.Upper(2, 3);
            Assert.True(str.Equals(theory.ToString()), "uppered(idx, len) str doesn't match the comparison string!");
        }

        public void testWords()
        {
            string[] strArray = new StringTheory("Mares eat oats and does eat oats and little lambs eat ivy.").Words();
            Assert.True(strArray.Length == 12, "Wrong word count!");
        }
    }




}
