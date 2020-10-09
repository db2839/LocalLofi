using System;
using System.Collections.Generic;
using LofiWebPlayer.Biz.Manager;

namespace LofiWebPlayer.Biz
{
    public class DatabaseDomain
    {
        private readonly DatabaseManager _databaseManager = new DatabaseManager();
        public DatabaseDomain()
        {

        }

        public void SetupDatabaseIfDoesNotExist()
        {
            this._databaseManager.CreateDatabaseIfDoesNotExist();
            this._databaseManager.InsertVideosIntoDatabse();
        }

        public void GetVideos()
        {
            try
            {
                var vids = this._databaseManager.ReadData();
                Console.WriteLine(vids.Count);
            }
            catch(Exception ex)
            {

                throw ex;
            }
        }

        //public IEnumerable<Manager.Services.YoutubeService.ResourceModel.Video> GrabVideos()
        //{
        //}
    }
}
