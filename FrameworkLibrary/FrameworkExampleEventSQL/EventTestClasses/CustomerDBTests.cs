using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ToolsCSharp;
using EventPropsClasses;
using EventDBClasses;
using DBCommand = System.Data.SqlClient.SqlCommand;
using System.Data;

namespace EventTestClasses
{
    [TestFixture]
    class CustomerDBTests
    {
        CustomerProps props;
        CustomerProps props2;
        List<CustomerProps> propsList;
        //ProductSQLDB dB;
        CustomerSQLDB dB;
        private string dataSource = "Data Source=DESKTOP-AFHCP3M\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [SetUp]
        public void SetUp()
        {
            dB = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            dB.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestRetrieve()
        {
            props = (CustomerProps)dB.Retrieve(1);
            Assert.AreEqual("Molunguri, A", props.name);
            Assert.AreEqual("Birmingham", props.city);
        }
        [Test]
        public void TestRetreiveAll()
        {
            //assert count
            props = (CustomerProps)dB.Retrieve(1);
            List<CustomerProps> list = (List<CustomerProps>)dB.RetrieveAll(props.GetType());
            Assert.AreEqual(list.Count, 696);

        }
        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.name = "Will";
            p.address = "123 Will Dr";
            p.city = "WillTown";
            p.state = "OR";
            p.zip = "111111111111111";
            props = (CustomerProps)dB.Create(p);
            props2 = (CustomerProps)dB.Retrieve(props.ID);
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.name, props2.name);
            Assert.AreEqual(props.address, props2.address);
            Assert.AreEqual(props.city, props2.city);
            Assert.AreEqual(props.state, props2.state);
            Assert.AreEqual(props.zip, props2.zip);
            
            

        }
        [Test]
        public void TestDelete()
        {
            CustomerProps p = new CustomerProps();
            p.name = "Will";
            p.address = "123 WillTown";
            p.city = "WillTown";
            p.state = "OR";
            p.zip = "111111111111111";
            props = (CustomerProps)dB.Create(p);
            List<CustomerProps> list = (List<CustomerProps>)dB.RetrieveAll(props.GetType());
            int count1 = list.Count();
            dB.Delete(p);
            list = (List<CustomerProps>)dB.RetrieveAll(props.GetType());
            int count2 = list.Count();
            Assert.AreNotEqual(count1, count2);
        }
        [Test]
        public void TestUpdate()
        {
            CustomerProps p = new CustomerProps();
            p.name = "name";
            p.address = "address";
            p.zip = "1234";
            p.city = "city";
            p.state = "OR";
           
            dB.Create(p);
            p.name = "slkghfvlsj";
            p.address = "afasdad";
            p.zip="7896";
            p.state = "CA";
            p.city = "blahblahblah";
            dB.Update(p);
            CustomerProps props2 = new CustomerProps();
            props2 = (CustomerProps)dB.Retrieve(p.ID);
            Assert.AreNotEqual(props2.name, "name");
            Assert.AreNotEqual(props2.address, "address");
            Assert.AreNotEqual(props2.city, "city");
            Assert.AreNotEqual(props2.zip, "1234");
            Assert.AreNotEqual(props2.state, "OR");

           

        }
    }
}
