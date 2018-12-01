using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerTests
    {
        CustomerSQLDB db;
        private string dataSource = "Data Source=DESKTOP-AFHCP3M\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [SetUp]
        public void SetUpTests()
        {
            db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestRetrieveExistingCustomer()
        {
            // retrieves from Data Store
            Customer c = new Customer(1, dataSource);
            Assert.AreEqual(c.Name, "Molunguri, A");
            Assert.AreEqual(c.Address, "1108 Johanna Bay Drive");
            Assert.AreEqual(c.City, "Birmingham");
            Assert.AreEqual(c.State, "AL");
            Assert.AreEqual(c.ZipCode, "35216-6909");
            Console.WriteLine(c.ToString());

        }
        [Test]
        public void TestCreateCustomer()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Will";
            c.Address = "123 Will Way";
            c.City = "WillVille";
            c.State = "OR";
            c.ZipCode = "123456";
            c.Save();
            Customer c2 = new Customer(c.ID, dataSource);
            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.Name,c2.Name);
            Assert.AreEqual(c.Address,c2.Address);
            Assert.AreEqual(c.City, c2.City);
            Assert.AreEqual(c.ZipCode, c2.ZipCode);
            Console.WriteLine(c.ToString());

        }
        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(1, dataSource);
            c.Name = "Will";
            c.Address = "123 Will Way";
            c.City = "WillVille";
            c.State = "OR";
            c.ZipCode = "123456";
            
            c.Save();

            c = new Customer(1, dataSource);
            Assert.AreEqual(c.Name, "Will");
            Assert.AreEqual(c.Address, "123 Will Way");
            Assert.AreEqual(c.City,"WillVille");
            Assert.AreEqual(c.State,"OR");
            Assert.AreEqual(c.ZipCode, "123456");
        }
        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(2, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));

        }

        ////    [Test]
        ////    public void TestStaticDelete()
        ////    {
        ////        Event.Delete(2, dataSource);
        ////        Assert.Throws<Exception>(() => new Event(2, dataSource));
        ////    }

        ////[Test]
        ////public void TestStaticGetList()
        ////{
        ////    List<Event> events = Event.GetList(dataSource);
        ////    Assert.AreEqual(2, events.Count);
        ////    Assert.AreEqual(1, events[0].ID);
        ////    Assert.AreEqual("First Event", events[0].Title);
        ////}

        ////    // *** I added this
        [Test]
        public void TestGetList()
        {
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
            Assert.AreEqual("Muhinyi, Mauda", customers[1].Name);
        }

        ////    // *** I added this
        [Test]
        public void TestGetTable()
        {
            DataTable customers = Customer.GetTable(dataSource);
            Assert.AreEqual(customers.Rows.Count, 696);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Will";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "this is a test";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer(dataSource);
            //Assert.Throws<ArgumentException>(() => c.Name = "");
            Assert.Throws<ArgumentException>(() => c.State = "azxcvbnmsdfghj");
            Assert.Throws<ArgumentException>(() => c.ZipCode = "qwertyuiopasdfghjklzxcvbnmqqwertyuioplkjhgfdsazxcvbnmlpo");
        }

        //// *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1, dataSource);
            Customer c2 = new Customer(1, dataSource);

            c1.Name = "Updated this first";
            c1.Save();

            c2.Name = "Updated this second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
    
}
