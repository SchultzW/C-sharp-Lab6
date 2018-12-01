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
    public class ProductTests
    {
        ProductSQLDB db;
        private string dataSource = "Data Source=DESKTOP-AFHCP3M\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [SetUp]
        public void SetUpTests()
        {
            db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestRetrieveExistingProduct()
        {
            // retrieves from Data Store
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Code, "A4CS");
            Assert.AreEqual(p.Price, 56.50m);
            Assert.AreEqual(p.Quantity, 4637);
            Console.WriteLine(p.ToString());

        }
        [Test]
        public void TestCreateProduct()
        {
            Product p = new Product(dataSource);
            p.Code = "XXXX";
            p.Description = "This is a test product";
            p.Quantity = 10;
            p.Price = 10.99m;
            p.Save();
            Product p2 = new Product(p.ID, dataSource);
            Assert.AreEqual(p.ID, p2.ID);
            Assert.AreEqual(p.Code, p2.Code);
            Assert.AreEqual(p.Price, p2.Price);
            Assert.AreEqual(p.Quantity, p2.Quantity);
            Console.WriteLine(p.ToString());

        }
        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1, dataSource);
            p.Code = "XXXX";
            p.Description = "Edited Product";
            p.Price = 9999.99M;
            p.Quantity = 1;
            p.Save();

            p = new Product(1, dataSource);
            Assert.AreEqual(p.Code, "XXXX");
            Assert.AreEqual(p.Description, "Edited Product");
            Assert.AreEqual(p.Price, 9999.99M);
            Assert.AreEqual(p.Quantity, 1);
        }
        [Test]
        public void TestDelete()
        {
            Product p = new Product(2, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, dataSource));
           
        }

        //    [Test]
        //    public void TestStaticDelete()
        //    {
        //        Event.Delete(2, dataSource);
        //        Assert.Throws<Exception>(() => new Event(2, dataSource));
        //    }

        //[Test]
        //public void TestStaticGetList()
        //{
        //    List<Event> events = Event.GetList(dataSource);
        //    Assert.AreEqual(2, events.Count);
        //    Assert.AreEqual(1, events[0].ID);
        //    Assert.AreEqual("First Event", events[0].Title);
        //}

        //    // *** I added this
        [Test]
        public void TestGetList()
        {
            Product p = new Product(dataSource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ID);
            Assert.AreEqual("A4CS", products[0].Code);
        }

        //    // *** I added this
        [Test]
        public void TestGetTable()
        {
            DataTable products = Product.GetTable(dataSource);
            Assert.AreEqual(products.Rows.Count, 16);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p= new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.Code = "ABCD";
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "this is a test";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.Price = -1);
            Assert.Throws<ArgumentException>(()=>p.Code = "azxcvbnmsdfghj");
            Assert.Throws<ArgumentException>(() => p.Description = "qwertyuiopasdfghjklzxcvbnmqqwertyuioplkjhgfdsazxcvbnmlpo");
        }

        // *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1, dataSource);
            Product p2 = new Product(1, dataSource);

            p1.Description = "Updated this first";
            p1.Save();

            p2.Description = "Updated this second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}

