
using ExampleApp.Model;
using ExampleApp.Repository;
using ExampleApp.Service.common;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace ExampleApp.Service
{
    public class AdvertisementService : IAdvertisementService
    {
        public AdvertisementRepository Repository { get; set; } = new AdvertisementRepository();
        public List<AdModel> GetAllAds()
        {
            List<AdModel> result = Repository.GetAllAds();
            return result;
        }
        public void GetAllAdsCategories() { }
        public AdModel GetAdById(Guid id)
        {
            AdModel result = Repository.GetAdById(id);
            return result;
        }
        public bool CreateAd(AdModel request)
        {
            //generate guid
            request.Id = Guid.NewGuid();

            bool response = Repository.CreateAd(request);
            return true;
        }
        public void EditAd() { }
        public void DeleteAd() { }

    }
}
