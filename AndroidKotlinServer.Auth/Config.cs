﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace AndroidKotlinServer.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };


        public static IEnumerable<ApiResource> apiResources => new ApiResource[]
        {
            new ApiResource("resource_product_api"){ Scopes={"api_product_fullpermission" },
                ApiSecrets=new[] {
                    new Secret("apisecret".Sha256())
                }
                },
            new ApiResource("resource_photo_api"){Scopes={"api_photo_fullpermission"}},
            new ApiResource("resource_auth_api"){Scopes={IdentityServerConstants.LocalApi.ScopeName},
            ApiSecrets = new[]{
            new Secret("photosecret".Sha256())
            } }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api_product_fullpermission","Full access for ProductApi"),
                new ApiScope("api_photo_fullpermission", "Full access for PhotoApi"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "AndroidClient_ClientCredential",
                    ClientName = "AndroidClient Client Credential",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { IdentityServerConstants.LocalApi.ScopeName}
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "AndroidClient_ROP",
                    ClientName = "AndroidClient ROP",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,//Bir refresh tokendır.
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api_product_fullpermission",
                        "api_photo_fullpermission",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AccessTokenLifetime=1*60*60,//10 dakikalık lifetime 10*60
                    RefreshTokenUsage=TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenExpiration=TokenExpiration.Absolute //Kesin bir tarih ver ömrü var. İki aylık bir süre içersinde geçerli
                    //refresh token jwt değildir. kullanıcının tokenının süresi dolduğunda kullanıcıya token vermek için kullanılır.
                    //refresh token sayesinde api tarafından her hangi bir bilgi almayız.
                    //Access token expire olduğunda tekrar login ekranına dönmek yerine refresh token Identity Serverdan yeni bir access token alır.

                },
            };
    }
}