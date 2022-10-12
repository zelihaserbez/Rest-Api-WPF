using System;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using RestSharp.Authenticators.OAuth2;
using Microsoft.AspNetCore.Mvc;
using UPSTask.TaskObjects;

namespace UPSTask
{

    [ApiController]
    [Route("users")]
    public class WebAPI : ControllerBase
    {

        private RestClient client;
        private const string apiToken = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public WebAPI()
        {
            client = new RestClient("https://gorest.co.in/public-api/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUser(Filters pFilter)
        {
            try
            {
                int pagenum = 0;
                var request = new RestRequest("users");
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(apiToken, "Bearer");

                if (pFilter.id > 0)
                    request.AddParameter("id", pFilter.id);
                if (!string.IsNullOrEmpty(pFilter.name))
                    request.AddParameter("name", pFilter.name);

                var response = await client.ExecuteGetAsync<TaskObjects.RestResponse>(request);

                if (response.IsSuccessful)
                    pagenum = JsonSerializer.Deserialize<TaskObjects.RestResponse>(response.Content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }).Meta.Pagination.Pages;

                if (pFilter.pageNumber > 0 && pagenum > pFilter.pageNumber)
                    request.AddParameter("page", pFilter.pageNumber);
                else
                    request.AddParameter("page", pagenum);

                response = await client.ExecuteGetAsync<TaskObjects.RestResponse>(request);
                if (!response.IsSuccessful)
                {
                    return StatusCode(500, response.Data);
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User pUser)
        {
            var request = new RestRequest("users")
                        .AddJsonBody(pUser);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(apiToken, "Bearer");
            var response = await client.ExecutePostAsync<TaskObjects.UserCreateResponse>(request);

            if (!response.IsSuccessful)
            {
                return StatusCode(500, response.Data);
            }           

            return StatusCode(201, response.Data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User pUser)
        {
            try
            {
                var request = new RestRequest($"users/{pUser.Id}").AddJsonBody(pUser);
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(apiToken, "Bearer");
                var response = await client.ExecutePutAsync<UserUpdateResponse>(request);
                if (!response.IsSuccessful)
                {
                    return StatusCode(500, response.Data);
                }
               

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var request = new RestRequest($"users/{id}", Method.Delete);
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(apiToken, "Bearer");
                var response = await client.ExecuteAsync<PageResponse>(request);
                if (!response.IsSuccessful)
                {
                    return StatusCode(500, response.Data);
                }
                else
                    response.Data = new PageResponse { Success = true };
                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
