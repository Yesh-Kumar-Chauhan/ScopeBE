using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Contact> GetAllContacts()
        {
            return _context.Contacts.AsQueryable();
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(long id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteContactAsync(long id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Contact> GetContactsBySiteId(long siteId)
        {
            return _context.Contacts.Where(c => c.SiteID == siteId);
        }
    }
}
