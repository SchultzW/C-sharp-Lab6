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
    public class ProductProps : IBaseProps
    {

        #region instance variables from fields of DB
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;//lowest possible 32bit int, is a negative num

        /// <summary>
        /// 
        /// </summary>
        public int quantity = 0;

     

        /// <summary>
        /// 
        /// </summary>
        public string code = "";

        /// <summary>
        /// 
        /// </summary>
        public string description = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;

        public decimal price = 0.0M;
        #endregion
        public ProductProps()
        {
        }

        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.ID = this.ID;
            p.quantity = this.quantity;
            p.price = this.price;
            p.code = this.code;
            p.description = this.description;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
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
            ProductProps p = (ProductProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.quantity = p.quantity;
            this.price = p.price;
            this.code = p.code;
            this.description = p.description;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["ProductID"];
            this.quantity = (Int32)dr["OnHandQuantity"];
            this.code = ((string)dr["ProductCode"]).Trim();
            this.description = (string)dr["Description"];
            this.price = (decimal)dr["UnitPrice"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

    }
}
