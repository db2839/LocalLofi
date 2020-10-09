using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using LofiWebPlayer.Biz.Enums;
using LofiWebPlayer.Biz.Manager.Configuration;

namespace LofiWebPlayer.Biz.Manager
{
    internal class DatabaseManager
    {
    //{
    //    private static SqliteConnection sqlLiteConnection;

    //    internal static SqliteConnection SqlLiteConnection
    //    {
    //        get
    //        {
    //            if(sqlLiteConnection==null)
    //            {
    //                sqlLiteConnection = new SqliteConnection(SqlConfiguraqtion.connectionStringBuilder.ConnectionString);
    //            }
    //            return sqlLiteConnection;
    //        }
    //    }

        private readonly ServiceLocatorBase _serviceLocator;

        private YoutubeVideoManager _youtubeVideoManager;

        internal YoutubeVideoManager YoutubeVideoManager
        {
            get { return this._youtubeVideoManager ?? (this._youtubeVideoManager = this._serviceLocator.CreateYoutubeVideoManager()); }
        }


        internal DatabaseManager() : this(new ServiceLocator())
        {

        }

        internal DatabaseManager(ServiceLocatorBase serviceLocator)
        {
            this._serviceLocator = serviceLocator;
        }



        internal void CreateDatabaseIfDoesNotExist()
        {
            //using(SqlLiteConnection)
            //{
            //    SqlLiteConnection.Open();

            //    SqliteCommand createTable = new SqliteCommand(SqlConfiguraqtion.CreationSqlCommand,SqlLiteConnection);
            //    var result = createTable.ExecuteReader();
            //    SqlLiteConnection.Close();
            //}
            var sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);


        }

        internal void InsertVideosIntoDatabse(string query = VideoQuerys.LofiSummer)
        {
            var videoList = this.GVid(query).Result;

            using(var sqlite_conn = CreateConnection())
            {
                foreach(var video in videoList)
                {
                    var sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = $"INSERT INTO VideoTable (Title, Url) VALUES('{video.Ttile}','{video.Url}')";
                    var result = sqlite_cmd.ExecuteNonQuery();
                    Console.WriteLine(result.ToString());
                }
                sqlite_conn.Close();
            }
        }

        public async Task<IEnumerable<Services.YoutubeService.ResourceModel.Video>> GVid(string query)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBRJgMCvgXoRV8lGYUHlr-3hGATl34fEkU",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            SearchListResponse searchListResponse = null;

            try
            {
                searchListRequest.Q = query; // Replace with your search term.
                searchListRequest.MaxResults = 15;
                searchListRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.Moderate;
                searchListRequest.VideoDuration = SearchResource.ListRequest.VideoDurationEnum.Long__;
                searchListRequest.VideoDefinition = SearchResource.ListRequest.VideoDefinitionEnum.High;
                searchListRequest.Type = "video";
                searchListResponse = await searchListRequest.ExecuteAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

            return this.MapToVideos(searchListResponse);
        }

         private IEnumerable<Services.YoutubeService.ResourceModel.Video> MapToVideos(SearchListResponse result)
        {
            foreach(var searchResult in result.Items)
            {
                if(!this.VerifySearchResultIsValid(searchResult))
                {
                    continue;
                }

                yield return new Services.YoutubeService.ResourceModel.Video()
                {
                    Ttile = searchResult.Snippet.Title.Substring(0,10),
                    Url = $"https://www.youtube.com/embed/{searchResult.Id.VideoId}"
                };
            }
        }

        private bool VerifySearchResultIsValid(SearchResult searchResult) => searchResult.Id.Kind.Equals("youtube#video") && searchResult.Snippet.Title != null && searchResult.Id.VideoId != null;

        private async Task<IEnumerable<Services.YoutubeService.ResourceModel.Video>> GetVideos(string query)
        {
            var videos = await this.YoutubeVideoManager.GetVideos(query).ConfigureAwait(false);

            if(!videos.Any())
            {
                throw new Exception();
            }

            return videos;
        }

          public static SQLiteConnection CreateConnection()
          {
 
             SQLiteConnection sqlite_conn;
             // Create a new database connection:
             sqlite_conn = new SQLiteConnection("Data Source=Videos.db;Version=3;New=True;Compress=True;");
             // Open the connection:
             try
             {
                sqlite_conn.Open();
             }
             catch (Exception ex)
             {
                throw ex;
             }
             return sqlite_conn;
          }
        public static void CreateTable(SQLiteConnection conn)
        {
 
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = SqlConfiguraqtion.CreationSqlCommand;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<Services.YoutubeService.ResourceModel.Video> ReadData()
        {
            List<Services.YoutubeService.ResourceModel.Video> videos = new List<Services.YoutubeService.ResourceModel.Video>();
            try
            {
                using(var sqlite_conn = CreateConnection())
                {
                    //SQLiteDataReader sqlite_datareader;
                    //SQLiteCommand sqlite_cmd;
                    //sqlite_cmd = sqlite_conn.CreateCommand();
                    //sqlite_cmd.CommandText = "SELECT * FROM VideoTable";
 
                    //sqlite_datareader = sqlite_cmd.ExecuteReader();
                    //while (sqlite_datareader.Read())
                    //{
                    //    string myreader = sqlite_datareader.GetString(0);
                    //    Console.WriteLine(myreader);
                    //}

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM VideoTable", sqlite_conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Services.YoutubeService.ResourceModel.Video video = new Services.YoutubeService.ResourceModel.Video();
                                video.Ttile = reader["Title"].ToString();
                                video.Url = reader["Url"].ToString();
                                videos.Add(video);
                            }
                        }
                    }
                    sqlite_conn.Close();
                }
            }
            catch(SQLiteException e)
            {
                throw e;
            }
            return videos;
        }
    }
}
