using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.MongoNoRMService
{
    /// <summary>
    /// Representation of the database, called NORM ( No-SQL Object relational mapping. ).
    /// The helper is composed by a MongoClient initialising the connection by services.
    /// ( wrong : Each transaction has to be in a given Unit of work, safing transactions. )
    /// </summary>
    /// <typeparam name="T">Type on the entity containing in the collections.</typeparam>
    public class MongoHelper<T> where T:class
    {
        private static object _lockObject = new object();
        public string TableName { get; set; }
        public string[] Indexes { get; set; }
        private MongoClient client;
        private MongoCollection<T> _collection;

        public MongoCollection<T> Collection
        {
            get
            {
                if (null == client)
                {
                    lock (_lockObject)
                    {
                        if (null == _collection)
                        {
                            Configure();
                        }
                    }
                }
                return _collection;
            }
            set { _collection = value; }
        }

        /// <summary>
        /// Helper Contructor. 
        /// </summary>
        /// <param name="tablename">The contructor takes a collection name in order to be connected to the right collection.</param>
        /// <param name="indexes">If needed, indexes can be given and insured.</param>
        public MongoHelper(string tablename, params string[] indexes)
        {
            if (indexes.Count() > 0)
                Indexes = indexes;

            TableName = tablename;

            Configure();
        }

        public void Configure()
        {
            //Getting the connection string in order to connect to the right database
            // ( Wrong : the connection string need to be setted in the Ninject bootstrapper, allowing each service to
            // be instanciated with connection string parameter given by the presentation layer only.
            client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
            var server = client.GetServer();
            var databaseSettings = server.CreateDatabaseSettings("Driveat");
            var db = server.GetDatabase(databaseSettings);
            Collection = db.GetCollection<T>(TableName.ToLower());

            if (Indexes != null)
                Collection.EnsureIndex(Indexes);
        }
    }
}
