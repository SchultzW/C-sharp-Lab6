using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventPropsClasses;

namespace EventTestClasses
{
    [TestFixture]
     public class CustomerPropsTest
    {
        CustomerProps c;
        [SetUp]
        public void setup()
        {
            c = new CustomerProps();
            c.ID = 1;
            c.name = "Will";
            c.address = "123 Will St";
            c.city = "Willton";
            c.state = "Or";
            c.zip = "0000000-000000";           
        }
        [Test]
        public void TestClone()
        {
            CustomerProps c2 = (CustomerProps)c.Clone();
            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.name, c2.name);
            Assert.AreEqual(c.address, c2.address);
            Assert.AreEqual(c.city, c2.city);
            Assert.AreEqual(c.state, c2.state);
            Assert.AreEqual(c.zip, c2.zip);
            Assert.AreEqual(c.ConcurrencyID, c2.ConcurrencyID);
            c2.name="Jim";
            Assert.AreNotEqual(c.name, c2.name);

        }
        [Test]
        public void TestGetState()
        {
            //takes obj and converts to string
            string cString = c.GetState();
            Console.WriteLine(cString);

        }
        [Test]
        public void TestSetState()
        {

            string cString = c.GetState();
            CustomerProps c2 = new CustomerProps();
            c2.SetState(cString);
            
            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.name, c2.name);
            Assert.AreEqual(c.address, c2.address);
            Assert.AreEqual(c.city, c2.city);
            Assert.AreEqual(c.state, c2.state);
            Assert.AreEqual(c.zip, c2.zip);
            Assert.AreEqual(c.ConcurrencyID, c2.ConcurrencyID);
        }
    }
}
