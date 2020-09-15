using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;

        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }

        public async Task<bool> AddContactAsync(int userId, UserIdentity userIdentity, CancellationToken cancellationToken)
        {
            if ((await _contactContext.ContactBooks.CountDocumentsAsync(c => c.UserId == userId)) == 0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook { UserId = userId });
            }
            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact
            {
                UserId = userIdentity.UserId,
                Name = userIdentity.Name,
                Avatar = userIdentity.Avatar,
                Company = userIdentity.Company,
                Title = userIdentity.Title,
            });
            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }

        public async Task<List<Models.Contact>> GetContactAsync(int userId, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userId))
                .FirstOrDefault(cancellationToken);

            if (contactBook != null)
            {
                return contactBook.Contacts;
            }
            return new List<Models.Contact>();
        }

        public async Task<bool> TagContactAsync(int userId, int contactId, List<string> tags, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.Eq(c => c.UserId, userId),
                Builders<ContactBook>.Filter.Eq("Contacts.UserId", contactId)
            );
            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Tags", tags);

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }

        public async Task<bool> UpdateContactInfoAsync(UserIdentity userIdentity, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userIdentity.UserId, null, cancellationToken))
                .SingleOrDefault(cancellationToken);
            if (contactBook == null)
            {
                return true;
            }

            var contactIds = contactBook.Contacts.Select(c => c.UserId);
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, c => c.UserId == userIdentity.UserId)
            );

            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", userIdentity.Name)
                .Set("Contacts.$.Avatar", userIdentity.Avatar)
                .Set("Contacts.$.Company", userIdentity.Company)
                .Set("Contacts.$.Phone", userIdentity.Phone)
                .Set("Contacts.$.Title", userIdentity.Title);

            var updateReuslt = _contactContext.ContactBooks.UpdateMany(filter, update);

            return updateReuslt.MatchedCount == updateReuslt.ModifiedCount;
        }
    }
}
