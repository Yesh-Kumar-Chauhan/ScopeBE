using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IContactRepository
    {
        IQueryable<Contact> GetAllContacts();
        Task<List<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(long id);
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(long id);

        IQueryable<Contact> GetContactsBySiteId(long siteId);

    }
}
