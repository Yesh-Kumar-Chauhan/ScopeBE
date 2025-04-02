using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IContactService
    {
        Task<GenericResponse<IEnumerable<ContactDto>>> GetContactsWithPaginationAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<ContactDto>>> GetAllContactsAsync(string? search);
        Task<GenericResponse<ContactDto>> GetContactByIdAsync(long id);
        Task<GenericResponse<ContactDto>> CreateContactAsync(ContactDto contactDto);
        Task<GenericResponse<ContactDto>> UpdateContactAsync(long id, ContactDto contactDto);
        Task<GenericResponse<bool>> DeleteContactAsync(long id);

        Task<GenericResponse<IEnumerable<ContactDto>>> GetAllContactsBySiteIdAsync(long siteId, string? search);
        Task<GenericResponse<IEnumerable<ContactDto>>> GetContactsBySiteIdWithPaginationAsync(long siteId, string? search, int page, int pageSize);

    }
}
