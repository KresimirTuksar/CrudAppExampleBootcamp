using ExampleApp.Model;
using System;
using System.Collections.Generic;

namespace ExampleApp.Service.common
{
    public interface IAdvertisementService
    {
        List<AdModel> GetAllAds();
        void GetAllAdsCategories();
        AdModel GetAdById(Guid id);
        bool CreateAd(AdModel request);
        void EditAd();
        void DeleteAd();
    }
}
