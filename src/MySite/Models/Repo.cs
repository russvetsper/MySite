using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySite.Models
{
    public class Repo
    {
        public string Name { get; set; }
        public string Stargazers_count { get; set; }


        public static List<Repo> GetRepos()
        {

            var client = new RestClient("https://api.github.com/search/repositories?page=1&q=user:russvetsper&sort=stars:>=1&order=desc");
            var request = new RestRequest(/*"Accounts/" + EnvironmentVariables.AccountSid + "/Projects.json",*/ Method.GET);

           
            request.AddHeader("User-Agent", "russvetsper");
           
            request.AddHeader("Accept", "application/vnd.github.v3.+json");


            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();


            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            
            var repoList = JsonConvert.DeserializeObject<List<Repo>>(jsonResponse["items"].ToString());
            return repoList;
        }
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;

        }

    }
}