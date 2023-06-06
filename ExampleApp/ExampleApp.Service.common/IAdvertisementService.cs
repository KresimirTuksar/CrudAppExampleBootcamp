using ExampleApp.Common;
using ExampleApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleApp.Service.common
{
    public interface IAdvertisementService
    {
        Task<PagingModel<AdModel>> GetAllAdsAsync(PagingModel<AdModel> paging, SortingModel sorting);
        void GetAllAdsCategoriesAsync();
        Task<AdModel> GetAdByIdAsync(Guid id);
        Task<bool> CreateAdAsync(AdModel request);
        Task<bool> EditAdAsync(AdModel request, Guid id);
        Task<bool> DeleteAdAsync(Guid id);
    }
}
