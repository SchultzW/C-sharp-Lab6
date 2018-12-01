using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;

using DBDataReader = System.Data.SqlClient.SqlDataReader;//this is so we can swap out the sqldatareader with an oracle or anythign else
using System.Data.SqlClient;

namespace EventPropsClasses
{
    public class CustomerProps : IBaseProps
    {
        #region instance variables from fields of DB
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;//lowest possible 32bit int, is a negative num

        /// <summary>
        /// 
        /// </summary>
        public string name = "";

        /// <summary>
        /// 
        /// </summary>
        public string address = "";

        /// <summary>
        /// 
        /// </summary>
        public string city = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;

        public string state = "";
        public string zip = "";
        #endregion
        public CustomerProps()
        {
        }

        public object Clone()
        {
            CustomerProps c = new CustomerProps();
            c.ID = this.ID;
            c.name = this.name;
            c.address = this.address;
            c.city = this.city;
            c.state = this.state;
            c.ConcurrencyID = this.ConcurrencyID;
            c.zip = this.zip;
            return c;
        }

        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);//ask xmlserializer to change obeject to xml conver code to XML
            return writer.GetStringBuilder().ToString();//serilize as string
        }

        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            CustomerProps c = (CustomerProps)serializer.Deserialize(reader);
            this.ID = c.ID;
            this.name = c.name;
            this.address = c.address;
            this.city = c.city;
            this.state = c.state;
            this.zip = c.zip;
            this.ConcurrencyID = c.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["CustomerID"];
            this.name = ((string)dr["Name"]).Trim();
            this.address = ((string)dr["Address"]).Trim();
            this.city = ((string)dr["City"]).Trim();
            this.state = (string)dr["State"];
            this.zip = ((string)dr["ZipCode"]).Trim();
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

    }
}
