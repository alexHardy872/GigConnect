using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;

namespace GigConnect.Services
{
    public class YouTubeAPI
    {

        YouTubeService client;

        public YouTubeAPI()
        {
            client = new YouTubeService(new BaseClientService.Initializer() { ApiKey = Private.Keys.youtubeAPIkey, ApplicationName = this.GetType().ToString() });
        }
        public void SearchForVideosByTitle(string searchValue, int maxResultCount)
        {
            SearchResource.ListRequest searchListRequest = client.Search.List("snippet");
            searchListRequest.Q = searchValue;
            searchListRequest.MaxResults = maxResultCount;

            SearchListResponse searchListResponse = searchListRequest.Execute();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            foreach (SearchResult searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }

        public void GetVideoDetails(string videoId)
        {
            VideosResource.ListRequest request = client.Videos.List("statistics, snippet");
            request.Id = videoId;

            VideoListResponse response = request.Execute();

            ulong? views = response.Items[0].Statistics.ViewCount;

            Console.WriteLine($"Video Name: {response.Items[0].Snippet.Title} Views: {views}");
        }
    }
}