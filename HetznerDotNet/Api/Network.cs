﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerDotNet.Api
{
    public class Network : Objects.Network.Network
    {
        /// <summary>
        /// Get all network
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Network>> Get()
        {
            List<Network> list = new List<Network>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Objects.Network.Get.Response response = JsonConvert.DeserializeObject<Objects.Network.Get.Response>(await ApiCore.SendGetRequest($"/networks?page={page}&per_page=25")) ?? new HetznerDotNet.Objects.Network.Get.Response();

                // Run
                foreach (Network row in response.Networks)
                {
                    list.Add(row);
                }

                // Finish?
                if (response.Meta.Pagination.NextPage == null)
                {
                    // Yes, finish
                    return list;
                }
            }
        }

        /// <summary>
        /// Get netwotk with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Network> Get(long id)
        {
            // Get list
            Objects.Network.GetOne.Response response = JsonConvert.DeserializeObject<Objects.Network.GetOne.Response>(await ApiCore.SendGetRequest($"/networks/{id}")) ?? new Objects.Network.GetOne.Response();

            // Return
            return response.Network;
        }

        /// <summary>
        /// Create a new network
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ipRange"></param>
        /// <returns></returns>
        public static async Task<Network> Create(string name, string ipRange)
        {
            // Preparing raw
            string raw = $"{{\"ip_range\":\"{ipRange}\",\"name\":\"{name}\"}}";

            // Send post
            string jsonResponse = await ApiCore.SendPostRequest("/networks", raw);

            // Return
            JObject result = JObject.Parse(jsonResponse);
            return JsonConvert.DeserializeObject<Network>($"{result["network"]}") ?? new Network();
        }

        /// <summary>
        /// Update a network
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static async Task<Network> Update(Network network)
        {
            // Preparing raw
            string raw = $"{{\"name\":\"{network.Name}\"}}";

            // Send post
            string jsonResponse = await ApiCore.SendPutRequest($"/networks/{network.Id}", raw);

            // Return
            JObject result = JObject.Parse(jsonResponse);
            return JsonConvert.DeserializeObject<Network>($"{result["network"]}") ?? new Network();
        }

        /// <summary>
        /// Delete network ='(
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static async Task Delete(Network network)
        {
            // Send post
            await ApiCore.SendDeleteRequest($"/networks/{network.Id}");
        }

        /// <summary>
        /// Create new subnetwork
        /// </summary>
        /// <param name="network"></param>
        /// <param name="ipRange"></param>
        /// <param name="networkZone"></param>
        /// <returns></returns>
        public static async Task SubnetCreate(Network network, string ipRange, string networkZone)
        {
            // Preparing raw
            string raw = $"{{\"network_zone\": \"{networkZone}\",\"ip_range\": \"{ipRange}\",\"type\": \"cloud\"}}";

            // Send post
            await ApiCore.SendPostRequest($"/networks/{network.Id}/actions/add_subnet", raw);
        }

        /// <summary>
        /// Delete a subnetwork
        /// </summary>
        /// <param name="network"></param>
        /// <param name="ipRange"></param>
        /// <returns></returns>
        public static async Task SubnetDelete(Network network, string ipRange)
        {
            // Preparing raw
            string raw = $"{{\"ip_range\":\"{ipRange}\"}}";

            // Send post
            await ApiCore.SendPostRequest($"/networks/{network.Id}/actions/delete_subnet", raw);
        }
    }
}
