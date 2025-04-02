using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository, IMapper mapper, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<ContactDto>>> GetContactsWithPaginationAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _contactRepository.GetAllContacts();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.CONTACT.Contains(search) || c.CHILD.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var contacts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                return new GenericResponse<IEnumerable<ContactDto>>(true, "Contacts retrieved successfully.", contactDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving contacts with pagination.");
                return new GenericResponse<IEnumerable<ContactDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<ContactDto>>> GetAllContactsAsync(string? search)
        {
            try
            {
                var query = _contactRepository.GetAllContacts();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.CHILD.Contains(search) || c.CONTACT.Contains(search));
                }

                var contacts = await query.ToListAsync();
                var contactDtos = _mapper.Map<List<ContactDto>>(contacts);

                return new GenericResponse<List<ContactDto>>(true, "All contacts retrieved successfully.", contactDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all contacts.");
                return new GenericResponse<List<ContactDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<ContactDto>> GetContactByIdAsync(long id)
        {
            try
            {
                var contact = await _contactRepository.GetContactByIdAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found.", id);
                    return new GenericResponse<ContactDto>(false, "Contact not found.", null);
                }

                var contactDto = _mapper.Map<ContactDto>(contact);
                return new GenericResponse<ContactDto>(true, "Contact retrieved successfully.", contactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving contact with ID {ContactId}.", id);
                return new GenericResponse<ContactDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<ContactDto>> CreateContactAsync(ContactDto contactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                var createdContact = await _contactRepository.AddContactAsync(contact);
                var createdContactDto = _mapper.Map<ContactDto>(createdContact);

                return new GenericResponse<ContactDto>(true, "Contact created successfully.", createdContactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new contact.");
                return new GenericResponse<ContactDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<ContactDto>> UpdateContactAsync(long id, ContactDto contactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                contact.ContactID = id;

                var updatedContact = await _contactRepository.UpdateContactAsync(contact);
                if (updatedContact == null)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found or update failed.", id);
                    return new GenericResponse<ContactDto>(false, "Contact not found or update failed.", null);
                }

                var updatedContactDto = _mapper.Map<ContactDto>(updatedContact);
                return new GenericResponse<ContactDto>(true, "Contact updated successfully.", updatedContactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating contact with ID {ContactId}.", id);
                return new GenericResponse<ContactDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteContactAsync(long id)
        {
            try
            {
                var deleted = await _contactRepository.DeleteContactAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Contact not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Contact deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting contact with ID {ContactId}.", id);
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<GenericResponse<IEnumerable<ContactDto>>> GetAllContactsBySiteIdAsync(long siteId, string? search)
        {
            try
            {
                var query = _contactRepository.GetContactsBySiteId(siteId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.CHILD.Contains(search) || c.CONTACT.Contains(search));
                }

                var contacts = await query.ToListAsync();
                var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                return new GenericResponse<IEnumerable<ContactDto>>(true, "Contacts retrieved successfully.", contactDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving contacts by Site ID.");
                return new GenericResponse<IEnumerable<ContactDto>>(false, "An error occurred while retrieving contacts.", null);
            }
        }

        public async Task<GenericResponse<IEnumerable<ContactDto>>> GetContactsBySiteIdWithPaginationAsync(long siteId, string? search, int page, int pageSize)
        {
            try
            {
                var query = _contactRepository.GetContactsBySiteId(siteId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.CHILD.Contains(search) || c.CONTACT.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var contacts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                return new GenericResponse<IEnumerable<ContactDto>>(true, "Contacts retrieved successfully.", contactDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving contacts by Site ID with pagination.");
                return new GenericResponse<IEnumerable<ContactDto>>(false, "An error occurred while retrieving contacts.", null);
            }
        }
    }
}
