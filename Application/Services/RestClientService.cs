using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RestClientService
    {
        private readonly RestClient _client;
        private readonly TimeSpan Timeout = TimeSpan.FromMinutes(5); // Set timeout to 5 minutes

        public RestClientService(IConfiguration configuration)
        {
            var baseUrl = configuration["ExternalApi:BaseUrl"];
            _client = new RestClient(baseUrl);
        }

        private byte[] CompressData(object data)
        {
            using var ms = new MemoryStream();
            using (var gzip = new GZipStream(ms, CompressionMode.Compress))
            using (var sw = new StreamWriter(gzip))
            {
                var json = JsonConvert.SerializeObject(data);
                sw.Write(json);
            }
            return ms.ToArray();
        }

        public async Task<RestResponse<T>> PostAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post)
            {
                Timeout = Timeout 
            };
            request.AddHeader("Content-Type", "application/json"); 
            request.AddJsonBody(body);
            var response = await _client.ExecuteAsync<T>(request);
            return response;
        }
        //public async Task<RestResponse<T>> PostStringifyAsync<T>(string endpoint, object body)
        //{
        //    var request = new RestRequest(endpoint, Method.Post)
        //    {
        //        Timeout = Timeout 
        //    };
        //    // Stringify the payload
        //    var jsonPayload = JsonConvert.SerializeObject(body);
        //    request.AddHeader("Content-Type", "application/json"); // Add the correct content type
        //    request.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);

        //    var response = await _client.ExecuteAsync<T>(request);

        //    return response;
        //}

        public async Task<RestResponse<T>> PostStringifyAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post)
            {
                Timeout = Timeout
            };

            var jsonPayload = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            // Stringify the payload
            request.AddHeader("Content-Type", "application/json"); // Add the correct content type
            request.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);

            var response = await _client.ExecuteAsync<T>(request);

            return response;
        }

        public async Task<RestResponse<T>> PutAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Put)
            {
                Timeout = Timeout
            };
            request.AddJsonBody(body);
            return await _client.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> GetAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get)
            {
                Timeout = Timeout
            };
            return await _client.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse> DeleteAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete)
            {
                Timeout = Timeout
            };
            return await _client.ExecuteAsync(request);
        }
    }
}
