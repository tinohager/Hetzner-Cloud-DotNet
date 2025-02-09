﻿using HetznerDotNet.Objects.Image.GetOne;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerDotNet.Api
{
    public class Image : Objects.Image.Image
    {
        public static async Task<List<Image>> Get()
        {
            List<Image> listImage = new List<Image>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Objects.Image.Get.Response response = JsonConvert.DeserializeObject<Objects.Image.Get.Response>(await ApiCore.SendGetRequest($"/images?page={page}&per_page=25")) ?? new HetznerDotNet.Objects.Image.Get.Response();

                // Run
                foreach (Image row in response.Images)
                {
                    listImage.Add(row);
                }

                // Finish?
                if (response.Meta.Pagination.NextPage == null)
                {
                    // Yes, finish
                    return listImage;
                }
            }
        }

        public static async Task<Image> Get(long id)
        {
            // Get list
            Response response = JsonConvert.DeserializeObject<Response>(await ApiCore.SendGetRequest($"/images/{id}")) ?? new Response();

            // Return
            return response.Image;
        }
    }
}
