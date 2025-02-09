﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerDotNet.Api
{
    public class ServerType : Objects.ServerType.ServerType
    {
        public static async Task<List<ServerType>> Get()
        {
            List<ServerType> listServerType = new List<ServerType>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Objects.ServerType.Get.Response response = JsonConvert.DeserializeObject<Objects.ServerType.Get.Response>(await ApiCore.SendGetRequest($"/server_types?page={page}&per_page=25")) ?? new HetznerDotNet.Objects.ServerType.Get.Response();

                // Run
                foreach (ServerType row in response.ServerTypes)
                {
                    listServerType.Add(row);
                }

                // Finish?
                if (response.Meta.Pagination.NextPage == null)
                {
                    // Yes, finish
                    return listServerType;
                }
            }
        }

        public static async Task<ServerType> Get(long id)
        {
            // Get list
            Objects.ServerType.GetOne.Response response = JsonConvert.DeserializeObject<Objects.ServerType.GetOne.Response>(await ApiCore.SendGetRequest($"/server_types/{id}")) ?? new HetznerDotNet.Objects.ServerType.GetOne.Response();

            // Return
            return response.ServerType;
        }
    }
}
