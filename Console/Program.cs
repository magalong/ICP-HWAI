using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YC.Models;

namespace YC
{
    class Program
    {
        static void setDBFilePath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string relative = @"..\..\App_Data\";
            string relative = @"..\..\..\Web\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        static void Main(string[] args)
        {
            setDBFilePath();
            Repository.StationRepository stationDB = new Repository.StationRepository();

            var stations=stationDB.FindAllStationsWithGroups();
            ShowStation(stations);


            Console.ReadKey();
        }

        public static void ShowStation(List<Station> stations)
        {

            Console.WriteLine(string.Format("共收到{0}筆監測站的資料", stations.Count));
            stations.ForEach(x =>
            {
                Console.WriteLine(string.Format("站點名稱：{0},地址:{1},{2}個組別在觀察中", x.ObservatoryName, x.LocationAddress,x.Groups.Count));


            });


        }


        private static void createRecord()
        {
            Service.ImportService import = new Service.ImportService();
            var records = import.CreateRecordsFromJson();


        }
    }
}
