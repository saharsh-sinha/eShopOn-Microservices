using eShopOn.Ordering.API.Client.Settings;
using eShopOn.Ordering.Behaviors;
using eShopOn.Ordering.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopOn.Ordering.API.Client.Behaviors
{
    internal class OrderClient : IOrderProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public OrderClient(HttpClient httpClient, IOptions<Config> settings)
        {
            _httpClient = httpClient;

            _remoteServiceBaseUrl = $"{settings.Value.OrderingBaseUrl}/api/";
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var putRequest = new StringContent(orderId.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_remoteServiceBaseUrl}/cancel", putRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<CardType>> GetCardTypesAsync() =>
            await Get<IEnumerable<CardType>>($"{_remoteServiceBaseUrl}/v1/card-types");

        public async Task<Order> GetOrderAsync(int orderId) =>
            await Get<Order>($"{_remoteServiceBaseUrl}/v1/order");

        public async Task<IEnumerable<OrderSummary>> GetOrdersAsync() =>
            await Get<IEnumerable<OrderSummary>>($"{_remoteServiceBaseUrl}/v1/orders");

        public async Task<bool> ShipOrderAsync(int orderNumber)
        {
            var putRequest = new StringContent(orderNumber.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_remoteServiceBaseUrl}/ship", putRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
        }

        private async Task<TResponse> Get<TResponse>(string uri)
        {
            var responseString = await _httpClient.GetStringAsync(uri);
            var response = JsonConvert.DeserializeObject<TResponse>(responseString);
            return response;
        }
    }
}