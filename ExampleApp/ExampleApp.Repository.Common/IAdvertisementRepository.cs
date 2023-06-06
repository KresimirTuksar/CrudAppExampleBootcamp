using ExampleApp.Common;
using ExampleApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleApp.Repository.Common
{
    public interface IAdvertisementRepository
    {
        Task<PagingModel<AdModel>> GetAllAdsAsync(PagingModel<AdModel> paging);
        void GetAllAdsCategoriesAsync();
        Task<AdModel> GetAdByIdAsync(Guid id);
        Task<bool> CreateAdAsync(AdModel request);
        Task<bool> EditAdAsync(AdModel request);
        Task<bool> DeleteAdAsync(Guid id);
    }
}
