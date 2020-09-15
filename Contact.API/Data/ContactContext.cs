using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;
using Microsoft.Extensions.Options;
using MongoDB;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class ContactContext
    {
        private IMongoDatabase _database;
        // private IMongoCollection<ContactBook> _collection;
        private AppSettings _appsettings;

        public ContactContext(IOptions<AppSettings> appsettings)
        {
            _appsettings = appsettings.Value;
            var client = new MongoClient(_appsettings.MongoConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_appsettings.MongoContactDatabaseName);
            }
        }

        private void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _database.ListCollections().ToList();
            var collectionNames = new List<string>();
            collectionList.ForEach(b => collectionNames.Add(b["name"].AsString));
            if (!collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }
        /// <summary>
        /// 用户通讯录
        /// </summary>
        /// <value></value>
        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreateCollection("ContactBooks");
                return _database.GetCollection<ContactBook>("ContactBooks");
            }
        }

        /// <summary>
        /// 好友申请请求记录
        /// </summary>
        /// <value></value>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get
            {
                CheckAndCreateCollection("ContactApplyRequests");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequests");
            }
        }
    }
}
