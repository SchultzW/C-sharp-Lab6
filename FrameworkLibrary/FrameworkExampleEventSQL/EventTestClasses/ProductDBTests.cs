using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ToolsCSharp;
using EventPropsClasses;
using EventDBClasses;
using System.Data;
using System.Data.Common;
using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductProps props;
        ProductProps props2;
        List<ProductProps> propsList;
        ProductSQLDB dB;
        private string dataSource = "Data Source=DESKTOP-AFHCP3M\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void SetUp()
        {
            dB = new ProductSQLDB(dataSource);

           
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            dB.RunNonQueryProcedure(command);


        }
        [Test]
        public void TestRetrieve()
        {
            props = (ProductProps)dB.Retrieve(1);
            Assert.AreEqual("A4CS", props.code);
            Assert.AreEqual(56.50, props.price);
        }
        [Test]
        public void TestRetreiveAll()
        {
            //assert count
            props = (ProductProps)dB.Retrieve(1);
            List<ProductProps> list = (List<ProductProps>)dB.RetrieveAll(props.GetType());
            Assert.AreEqual(list.Count, 16);

        }
        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.price = 12.99M;
            p.quantity = 10;
            p.description = "A Product";
            p.code = "ZZZZ";
            props = (ProductProps)dB.Create(p);
            props2 = (ProductProps)dB.Retrieve(p.ID);
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.price, props2.price);
            Assert.AreEqual(props.code, props2.code);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.quantity, props2.quantity);
            dB.Delete(props);

        }
        [Test]
        public void TestDelete()
        {
            ProductProps p = new ProductProps();
            p.price = 12.99M;
            p.quantity = 10;
            p.description = "A Product";
            p.code = "ZZZZ";
            props = (ProductProps)dB.Create(p);
            List<ProductProps> list = (List<ProductProps>)dB.RetrieveAll(props.GetType());
            int count1 = list.Count();
            dB.Delete(p);
            list = (List<ProductProps>)dB.RetrieveAll(props.GetType());
            int count2 = list.Count();
            Assert.AreNotEqual(count1, count2);
        }
        [Test]
        public void TestUpdate()
        {
            ProductProps p = new ProductProps();
            p.price = 12.99M;
            p.quantity = 10;
            p.description = "A Product";
            p.code = "ZZZZ";
            p.price = 100M;
            
            dB.Create(p);
            p.price = 1.99M;
            p.quantity = 1;
            p.description = "Updated Product";
            p.code = "QQQQ";          
            dB.Update(p);
            ProductProps props2 = new ProductProps();
            props2= (ProductProps)dB.Retrieve(p.ID);
            Assert.AreNotEqual(props2.code,"ZZZZ");
            Assert.AreNotEqual(props2.quantity, 10);
            Assert.AreNotEqual(props2.description, "A Product");
            Assert.AreNotEqual(props2.quantity, 10);
            Assert.AreNotEqual(props2.price, 100m);
            
        }

    }
}
