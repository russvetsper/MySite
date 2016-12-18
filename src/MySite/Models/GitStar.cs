using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace MySite.Models
{
    public class GitStar
    {
        public string Name { get; set; }
        public string Stargazers_count { get; set;
        }
        public static List<GitStar> GetRepos()
        {
            
            var client = new RestClient("https://api.github.com/search/repositories?page=1&q=user:russvetsper&sort=stars:>0&order=desc");
           
            var request = new RestRequest("", Method.GET);
            
            request.AddParameter("Access_token", EnvironmentVariables.AuthToken);
            request.AddHeader("User-Agent", "russvetsper");
            
            request.AddHeader("Accept", "application/vnd.github.v3.text-match+json");
           
            client.Authenticator = new HttpBasicAuthenticator("/Itmes.json", EnvironmentVariables.AuthToken);
        

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var repoList = JsonConvert.DeserializeObject<List<GitStar>>(jsonResponse["items"].ToString());
            return repoList;
        }
        
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
