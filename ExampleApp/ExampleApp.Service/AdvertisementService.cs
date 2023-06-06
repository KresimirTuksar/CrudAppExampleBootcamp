﻿
using ExampleApp.Model;
using ExampleApp.Repository;
using ExampleApp.Service.common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleApp.Service
{
    public class AdvertisementService : IAdvertisementService
    {
        public AdvertisementRepository Repository { get; set; } = new AdvertisementRepository();
        public async Task<List<AdModel>> GetAllAdsAsync()
        {
            List<AdModel> result = await Repository.GetAllAdsAsync();
            return result;
        }
        public async Task<List<AdCategoryModel>> GetAllAdsCategoriesAsync() {
            List<AdCategoryModel> result = await Repository.GetAllAdsCategoriesAsync();
            return result;
        }
        public async Task<AdModel> GetAdByIdAsync(Guid id)
        {
            AdModel result = await Repository.GetAdByIdAsync(id);
            return result;
        }
        public async Task<bool> CreateAdAsync(AdModel request)
        {
            //generate guid
            request.Id = Guid.NewGuid();

            bool response = await Repository.CreateAdAsync(request);
            return response;
        }
        public async Task<bool> EditAdAsync(AdModel request, Guid id)
        {
            request.Id = id;
            request.UpdatedAt = DateTime.Now;
            bool response = await Repository.EditAdAsync(request);

            return response;
        }
        public async Task<bool> DeleteAdAsync(Guid id)
        {
            bool response = await Repository.DeleteAdAsync(id);
            return response;
        }

    }
}
