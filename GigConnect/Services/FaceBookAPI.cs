﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GigConnect.Models;
using GigConnect.Models.ViewModels;

namespace GigConnect.Services
{
    public static class FacebookAPI
    {


        public static async Task<List<string>> GetPermaUrlFromPost(string bandId)
        {
            List<string> permaLinks = new List<string>();
            try
            {
                string requestUri = string.Format("https://graph.facebook.com/{0}/posts?fields=permalink_url&access_token={1}", bandId, Private.Keys.facebookAccessToken);
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                dynamic stuff = await Task.Run(() => JObject.Parse(content));
                var data = stuff["data"];
                
                foreach (var post in data)
                {
                    string perm = post.permalink_url;
                    permaLinks.Add(perm);
                }
                return permaLinks;


            }
            catch(Exception e)
            {
                return permaLinks;
            }
       
        }

        public static async Task<string> GetProfilePicture(string bandId)
        {

            try
            {
                string requestUri = string.Format("https://graph.facebook.com/{0}?fields=picture.width(400).height(400)&access_token={1}", bandId, Private.Keys.facebookAccessToken);
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                dynamic stuff = await Task.Run(() => JObject.Parse(content));
                var data = stuff["picture"];
                string imageUrl = data.data.url;

                return imageUrl;
            }
            catch(Exception e)
            {
                return "Error: No Token or facebook id";
            }
           
        }











    }
}