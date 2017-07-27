using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YC.Models;

namespace YC.Service
{
    public class ImportService
    {
        public List<Station> FindStations(string xmlPath)
        {
            List<Station> stations = new List<Station>();
            var xml = XElement.Load(xmlPath);
            XNamespace gml = @"http://www.opengis.net/gml/3.2";
            XNamespace twed = @"http://twed.wra.gov.tw/twedml/opendata";
            var stationsNode = xml.Descendants(twed + "RiverStageObservatoryProfile").ToList();
            stationsNode
                .Where(x => !x.IsEmpty).ToList()
                .ForEach(stationNode =>
                {
                    var BasinIdentifier = stationNode.Element(twed + "BasinIdentifier").Value.Trim();
                    var ObservatoryName = stationNode.Element(twed + "ObservatoryName").Value.Trim();
                    var LocationAddress = stationNode.Element(twed + "LocationAddress").Value.Trim();

                    var LocationByTWD67pos = stationNode.Element(twed + "LocationByTWD67").Descendants(gml + "pos").FirstOrDefault().Value.Trim();
                    var LocationByTWD97pos = stationNode.Element(twed + "LocationByTWD97").Descendants(gml + "pos").FirstOrDefault().Value.Trim();
                    Station stationData = new Station();
                    stationData.ID = BasinIdentifier;
                    stationData.LocationAddress = LocationAddress;
                    stationData.LocationByTWD67 = LocationByTWD67pos;
                    stationData.ObservatoryName = ObservatoryName;
                    stationData.CreateTime = DateTime.Now;
                    stations.Add(stationData);

                });



            return stations;

        }
        public List<Record> CreateRecordsFromJson()
        {
            var URL = @"http://data.wra.gov.tw/Service/OpenData.aspx?id=2D09DB8B-6A1B-485E-88B5-923A462F475C&format=json";
            List<Record> result = new List<Record>();

            var stationDb = new YC.Repository.StationRepository();
            var recordDb = new YC.Repository.RecordRepository();
            var jsonString = "";
            using (var webClient = new System.Net.WebClient())
            {
                jsonString = webClient.DownloadString(URL);
            }

            var stations = stationDb.FindAllStations()
                .ToDictionary(x => x.ID, x => x);
            var json = JsonConvert.DeserializeObject<JObject>(jsonString);
            var jsonDatas = json.Property("RealtimeWaterLevel_OPENDATA").Values().ToList();


            jsonDatas.ForEach(item =>
            {
                var recordObj = item as JObject;
                var StationIdentifier = recordObj.Property("StationIdentifier").Value.ToString().Trim();
                if (!stations.ContainsKey(StationIdentifier))
                {
                    return;
                }
                var station = stations[StationIdentifier];
                var RecordTime = recordObj.Property("RecordTime").Value.ToString().Trim();
                var WaterLevel = recordObj.Property("WaterLevel").Value.ToString().Trim();
                var recordTime = DateTime.Parse(RecordTime);
                var waterLevel = double.Parse(WaterLevel);

                Record newRecord = new Record();
                newRecord.CreateTime = DateTime.Now;
                newRecord.StationID = station.ID;
                newRecord.RecordTime = recordTime;
                newRecord.WaterLevel = waterLevel;
                newRecord.Station = station;
                //判斷是否已經存在此筆紀錄，不存在時存檔Record並更新Station
                var isExist = recordDb.IsExist(newRecord);
                if (!isExist)
                {
                    station.LastRecordTime = newRecord.RecordTime;
                    station.LastRecordWaterLevel = newRecord.WaterLevel;
                    stationDb.UpdateLastRecord(station);
                    result.Add(newRecord);
                    
                }
            });
            recordDb.Create(result);
            return result;
        }

    }
}
