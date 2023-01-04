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
    public class ContactInfoManager : DomainService
    {
        private IRepository<ContactInfo, Guid> ContactInfoRepository { get; }

        public ContactInfoManager(IRepository<ContactInfo, Guid> contactInfoRepository)
        {
            ContactInfoRepository = contactInfoRepository;
        }

        public Task<IQueryable<ContactInfo>> GetQueryableAsync() => ContactInfoRepository.GetQueryableAsync();

        public Task<IQueryable<ContactInfo>> WithDetailsAsync() => ContactInfoRepository.WithDetailsAsync();

        public Task<IQueryable<ContactInfo>> WithDetailsAsync(params Expression<Func<ContactInfo, object>>[] propertySelectors) => ContactInfoRepository.WithDetailsAsync(propertySelectors);

        public ContactInfo Create(Guid contactId, ContactInfoType type, string value)
        {
            return new ContactInfo(GuidGenerator.Create(), contactId, type, value);
        }

        public ContactInfo Create(ContactInfoType type, string value)
        {
            return new ContactInfo(GuidGenerator.Create(), type, value);
        }

        [UnitOfWork]
        public async Task<ContactInfo> InsertAsync(ContactInfo contactInfo, bool autoSave = false)
        {
            return await ContactInfoRepository.InsertAsync(contactInfo, autoSave);
        }

        [UnitOfWork]
        public async Task<ContactInfo> UpdateAsync(ContactInfo contactInfo, bool autoSave = false)
        {
            return await ContactInfoRepository.UpdateAsync(contactInfo, autoSave);
        }

        [UnitOfWork]
        public async Task DeleteAsync(Guid id, bool autoSave = false)
        {
            var contactInfo = await GetAsync(id, false);

            await ContactInfoRepository.DeleteAsync(contactInfo, autoSave);
        }

        public async Task<ContactInfo> GetAsync(Guid id, bool includeDetails = true)
        {
            return await ContactInfoRepository.GetAsync(id, includeDetails);
        }

        public async Task<ContactInfo> GetAsync(Guid id, params Expression<Func<ContactInfo, object>>[] propertySelectors)
        {
            var contactInfo = await AsyncExecuter.SingleOrDefaultAsync(
                await ContactInfoRepository.WithDetailsAsync(propertySelectors),
                c => c.Id == id
            );

            if (contactInfo == null)
            {
                throw new EntityNotFoundException(typeof(ContactInfo));
            }

            return contactInfo;
        }

        public async Task<ContactInfo> FindAsync(Guid id, bool includeDetails = true)
        {
            return await ContactInfoRepository.FindAsync(id, includeDetails);
        }

        public async Task<ContactInfo> FindAsync(Expression<Func<ContactInfo, bool>> predicate, bool includeDetails = true)
        {
            return await ContactInfoRepository.FindAsync(predicate, includeDetails);
        }
    }
}