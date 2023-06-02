using ExampleApp.Model;
using ExampleApp.WebApi.Responses.Advertisement;
using System.Collections.Generic;

namespace ExampleApp.WebApi.Helpers
{
    public static class HelpersGeneral
    {
        public static List<AdResponseModel> MapToResult(List<AdModel> models)
        {
            List<AdResponseModel> response = new List<AdResponseModel>();
            foreach (var item in models)
            {
                response.Add(
                    new AdResponseModel()
                    {
                        Id = item.Id.ToString(),
                        UserId = item.UserId.ToString(),
                        Content = item.Content,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                    });
            }
            return response;
        }


    }
}