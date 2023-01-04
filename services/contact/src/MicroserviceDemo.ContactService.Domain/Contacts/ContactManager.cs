using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace MicroserviceDemo.ContactService.Contacts
{
    public class ContactManager : DomainService
    {
        private IRepository<Contact, Guid> ContactRepository { get; }

        public ContactManager(IRepository<Contact, Guid> contactRepository)
        {
            ContactRepository = contactRepository;
        }

        public Task<IQueryable<Contact>> GetQueryableAsync() => ContactRepository.GetQueryableAsync();

        public Task<IQueryable<Contact>> WithDetailsAsync() => ContactRepository.WithDetailsAsync();

        public Task<IQueryable<Contact>> WithDetailsAsync(params Expression<Func<Contact, object>>[] propertySelectors) => ContactRepository.WithDetailsAsync(propertySelectors);

        public Contact Create(string name, string surName, string company)
        {
            return new Contact(GuidGenerator.Create(), name, surName, company);
        }

        [UnitOfWork]
        public async Task<Contact> InsertAsync(Contact contact, bool autoSave = false)
        {
            return await ContactRepository.InsertAsync(contact, autoSave);
        }

        [UnitOfWork]
        public async Task<Contact> UpdateAsync(Contact contact, bool autoSave = false)
        {
            return await ContactRepository.UpdateAsync(contact, autoSave);
        }

        [UnitOfWork]
        public async Task DeleteAsync(Guid id, bool autoSave = false)
        {
            var contact = await GetAsync(id, false);

            await ContactRepository.DeleteAsync(contact, autoSave);
        }

        public async Task<Contact> GetAsync(Guid id, bool includeDetails = true)
        {
            return await ContactRepository.GetAsync(id, includeDetails);
        }

        public async Task<Contact> GetAsync(Guid id, params Expression<Func<Contact, object>>[] propertySelectors)
        {
            var contact = await AsyncExecuter.SingleOrDefaultAsync(
                await ContactRepository.WithDetailsAsync(propertySelectors),
                c => c.Id == id
            );

            if (contact == null)
            {
                throw new EntityNotFoundException(typeof(Contact));
            }

            return contact;
        }

        public async Task<Contact> FindAsync(Guid id, bool includeDetails = true)
        {
            return await ContactRepository.FindAsync(id, includeDetails);
        }

        public async Task<Contact> FindAsync(Expression<Func<Contact, bool>> predicate, bool includeDetails = true)
        {
            return await ContactRepository.FindAsync(predicate, includeDetails);
        }
    }
}