using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace IdentityServer
{
    public class InMemoryManager
    {


        public List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "vasileioskk@gmail.com",
                    Username = "vasileioskk@gmail.com",
                    Password="password",
                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.Name , "Vasileios K")
                    }

                }

            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name="read",
                    DisplayName="Read User Data"

                }


            };

        }

        public IEnumerable<Client> GetClients()
        {

            return new[]
            {

                new Client
                {

                    ClientId="mvcclient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())

                    },
                    ClientName="Mvc client",
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {

                        Constants.StandardScopes.OpenId,
                        "read"

                    },
                    Enabled= true

                },
                 new Client
                {

                    ClientId="MvcImplicitClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())

                    },
                    ClientName="Mvc client",
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string>
                    {

                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        "read"

                    },
                    RedirectUris = new List<string>
                        {
                          "http://localhost:55082/"
                        },
                    PostLogoutRedirectUris =new List<string>
                        {
                          "http://localhost:55082/"
                        },
                        Enabled= true

                },
                new Client
                {

                    ClientId="MvcAuthorizationCodeClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())

                    },
                    ClientName="Mvc client",
                    Flow = Flows.Hybrid,
                    AllowedScopes = new List<string>
                    {

                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "read"

                    },
                    RedirectUris = new List<string>
                        {
                          "http://localhost:55082/",
                          "http://localhost:55082",
                          "http://localhost:55082/Home/AuthorizationCallback"
                        },
                    PostLogoutRedirectUris =new List<string>
                        {
                           "http://localhost:55082",
                           "http://localhost:55082/"
                        },
                        Enabled= true

                },
                new Client
                {

                    ClientId="MvcResourceOwnerClient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())

                    },
                    ClientName="Mvc ResourceOwner client",
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>
                    {

                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "read"

                    },
                    Enabled= true

                },


            };



        }


    }
}