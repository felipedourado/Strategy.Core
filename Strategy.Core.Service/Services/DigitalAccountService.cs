using MongoDB.Driver;
using Strategy.Core.Domain.Entities;
using Strategy.Core.Domain.Enum;
using Strategy.Core.Domain.Interfaces.Repositories;
using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Models;

namespace Strategy.Core.Services
{
    public class DigitalAccountService : IProducts
    {
        private readonly IMongoGenericRepository<DigitalAccountEntity> _mongoRepository;

        public AccountType AccountType => AccountType.Digital;

        public DigitalAccountService(IMongoGenericRepository<DigitalAccountEntity> mongoRepository)
        {
            _mongoRepository= mongoRepository;
        }

        public async Task Save(AccountBase request)
        {
            //rule for this product
            await _mongoRepository.CreateAsync(new DigitalAccountEntity
            {
                Account = request.Account, Agency = request.Agency, Product = "Zyon", Name = "Orion" 
            });
        }

        public async Task<IEnumerable<DigitalAccountEntity>> Get(DateTime data)
        {
            var filterBuilder = Builders<DigitalAccountEntity>.Filter;
            FilterDefinition<DigitalAccountEntity> filter;

            filter = filterBuilder.Gt("createDate", data.ToUniversalTime());

            //if (!string.IsNullOrEmpty(status))
            //{
            //    filter = filterBuilder.Gt("createDate", data.ToUniversalTime()) &
            //             filterBuilder.Where(filter => (filter.Status == status));
            //}

            var customers = await _mongoRepository.FilterByAsync(filter);

            return customers;
        }

        public async Task<object> Update(DigitalAccountRequest request)
        {
            var updateDef = Builders<DigitalAccountEntity>.Update.Set(item => item.Product, request.Product)
                                                                  .Set(item => item.Name, request.Name);

            var result = await _mongoRepository.UpdateOneBuilderAsync(item => item.Id == request.Id, updateDef);

            return result;
        }
    }
}