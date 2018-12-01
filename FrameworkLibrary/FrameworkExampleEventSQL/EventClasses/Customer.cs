using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;
using EventDBClasses;

// *** I had to change this
using CustomerDB = EventDBClasses.CustomerSQLDB;

// *** I added this
using System.Data;

namespace EventClasses
{
    public class Customer : BaseBusiness
    {

        public int ID
        {
            get
            {
                return ((CustomerProps)mProps).ID;
            }
        }
        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).name;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).name))
                {
                    if (value.Length>1)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("A name must be entered");
                    }
                }
            }

        }
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).address;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).address))
                {
                    if (value.Length < 100 || value.Length>=0)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("An address between 0 and 100 characters must be entered.");
                    }
                }
            }

        }
        public string City
        {
            get
            {
                return ((CustomerProps)mProps).city;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).city))
                {
                    if (value.Length<25&&value.Length>=0)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).city = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("A City between 0 and 25 characters must be entered");
                    }
                }
            }


        }
        public string State
        {
            get
            {
                return ((CustomerProps)mProps).state;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).state))
                {
                    if (value.Length==2)
                    {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).state = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Enter two characters for the State");
                    }
                }
            }

        }
        public string ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).zip;
            }

            set
            {
                if (!(value == ((CustomerProps)mProps).zip))
                {
                    if (value.Length < 15)
                    {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).zip = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("A Zip code with less than 15 characters must be entered");
                    }
                }
            }

        }

        #region constructors

        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Customer() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Customer(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Customer(CustomerProps props)
            : base(props)
        {
        }

        public Customer(CustomerProps props, string cnString)
            : base(props, cnString)
        {
        }
        #endregion
        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop, this.mConnectionString);
                customers.Add(c);
            }

            return customers;
        }

        protected override void SetDefaultProperties()
        {
            
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new CustomerDB();
                mdbWriteable = new CustomerDB();
            }

            else
            {
                mdbReadable = new CustomerDB(this.mConnectionString);
                mdbWriteable = new CustomerDB(this.mConnectionString);
            }
        }
        public static DataTable GetTable(string cnString)
        {
            CustomerDB db = new CustomerDB(cnString);
            return db.RetrieveTable();
        }
    }
}
