using ExampleApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp.Repository.Common
{
    public interface IAdvertisementRepository
    {
        List<AdModel> GetAllAds();
        void GetAllAdsCategories();
        AdModel GetAdById(Guid id);
        bool CreateAd(AdModel request);
        void EditAd();
        void DeleteAd();

    }
}
