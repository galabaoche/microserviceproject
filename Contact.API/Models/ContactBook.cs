using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Models
{
    [BsonIgnoreExtraElements]
    public class ContactBook
    {
        public int UserId { get; set; }
        /// <summary>
        /// 联系人列表
        /// </summary>
        /// <value></value>
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
