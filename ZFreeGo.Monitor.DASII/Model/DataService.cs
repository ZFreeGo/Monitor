using System;

namespace ZFreeGo.Monitor.DASII.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("欢迎使用SOJO DAS-II");
            callback(item, null);
        }
    }
}