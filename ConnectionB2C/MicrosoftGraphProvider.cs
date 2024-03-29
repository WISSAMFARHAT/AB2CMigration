﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionB2C
{
    public class MicrosoftGraphProvider
    {
        private string ClientId { get; set; }// = CoreConfig.Configuration["GraphB2C:ClientId"]; 
        private string TenantId { get; set; }// CoreConfig.Configuration["GraphB2C:TenantId"];
        private string ClientSecret { get; set; }// CoreConfig.Configuration["GraphB2C:CLientSecret"];
        private static IConfidentialClientApplication _clientApplication { get; set; }


        private static string[] _scopes = new string[] { "https://graph.microsoft.com/.default" };

        private static GraphServiceClient MicrosoftGraph;

        public MicrosoftGraphProvider() { }
        public static async Task<MicrosoftGraphProvider> Build(string clientId, string tenantId, string clientSecret)
        {
            MicrosoftGraphProvider microsoft = new()
            {
                ClientId = clientId,
                TenantId = tenantId,
                ClientSecret = clientSecret,

            };
            _clientApplication = ConfidentialClientApplicationBuilder
              .Create(clientId)
              .WithTenantId(tenantId)
              .WithClientSecret(clientSecret)
              .Build();

            var authResult = await _clientApplication.AcquireTokenForClient(_scopes).ExecuteAsync();


            MicrosoftGraph = new GraphServiceClient(new DelegateAuthenticationProvider(
                (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.CompletedTask;
                    }));

            return microsoft;

        }


        public async Task<List<UserModel>> GetAllUsers()
        {

            IGraphServiceUsersCollectionPage allUsers = await MicrosoftGraph.Users
                .Request()
                .Top(999)
                .Select("id,displayname,createddatetime,mail,othermails,givenname,surname,authentication,authenticationinfo,outlook,onpremiseslastsyncdatetime")
                .GetAsync();

            List<UserModel> users = new();

            foreach (User? user in allUsers)
            {
                UserModel newUser = new()
                {
                    ID = user.Id,
                    Email = user.OtherMails.First(),
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    DisplayName=user.DisplayName,
                    CreatedOn=user.CreatedDateTime,
                    LastSignedIn=user.OnPremisesLastSyncDateTime
                };

                users.Add(newUser);
            }

            return users;

        }

        //public async Task<UserModel> FetchUser(string id)
        //{
        //    var user = await MicrosoftGraph.Users[id]
        //        .Request()
        //        .Select("authentication")
        //        .GetAsync();

        //    return new();
        //}
    }
}
