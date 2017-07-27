using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YC.Repository
{
    public class StationRepository
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\WaterDB.mdf;Integrated Security=True";



        public void Create(List<Models.Station> stations)
        {
            var connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();

            foreach (var station in stations)
            {
                var command = new System.Data.SqlClient.SqlCommand("", connection);
                command.CommandText = string.Format(@"
INSERT        INTO    Station(ID, LocationAddress, ObservatoryName, LocationByTWD67, CreateTime)
VALUES          (N'{0}',N'{1}',N'{2}',N'{3}',N'{4}')
", station.ID, station.LocationAddress, station.ObservatoryName.Replace("'", "''"), station.LocationByTWD67, station.CreateTime.ToString("yyyy/MM/dd"));

                command.ExecuteNonQuery();
            }



            connection.Close();
        }
        public void UpdateLastRecord(Models.Station station)
        {
            var connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();


            var command = new System.Data.SqlClient.SqlCommand("", connection);
            command.CommandText = string.Format(@"
UPDATE [dbo].[Station]
   SET 
       [LastRecordTime] ='{0}'
      ,[LastRecordWaterLevel] ={1}
 WHERE [ID] = N'{2}'
", station.LastRecordTime.ToString("yyyy/MM/dd"), station.LastRecordWaterLevel, station.ID);

            command.ExecuteNonQuery();
            connection.Close();
        }
        public void Update(Models.Station station)
        {
            var connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();


            var command = new System.Data.SqlClient.SqlCommand("", connection);
            command.CommandText = string.Format(@"
UPDATE [dbo].[Station]
   SET 
       [LocationAddress]=N'{0}'
      ,[ObservatoryName]=N'{1}'
      ,[LocationByTWD67]=N'{2}'
 WHERE [ID] = N'{3}'
", station.LocationAddress, station.ObservatoryName, station.LocationByTWD67, station.ID);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Models.Station> FindAllStations()
        {
            var result = new List<Models.Station>();
            var connection = new System.Data.SqlClient.SqlConnection(_connectionString);
            connection.Open();
            var command = new System.Data.SqlClient.SqlCommand("", connection);
            command.CommandText = @"
Select * from Station";
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Models.Station item = new Models.Station();
                item.ID = reader["ID"].ToString();
                item.LocationAddress = reader["LocationAddress"].ToString();
                item.ObservatoryName = reader["ObservatoryName"].ToString();
                item.LocationByTWD67 = reader["LocationByTWD67"].ToString();
                item.CreateTime = DateTime.Parse(reader["CreateTime"].ToString());

                if (!string.IsNullOrEmpty(reader["LastRecordTime"].ToString()))
                {
                    item.LastRecordTime = DateTime.Parse(reader["LastRecordTime"].ToString());
                }
                if (!string.IsNullOrEmpty(reader["LastRecordWaterLevel"].ToString()))
                {
                    item.LastRecordWaterLevel = double.Parse(reader["LastRecordWaterLevel"].ToString());
                }

                result.Add(item);
            }
            connection.Close();


            return result;
        }

        public List<Models.Station> FindAllStationsWithGroups()
        {
            var result = new List<Models.Station>();
            var connection = new System.Data.SqlClient.SqlConnection(_connectionString);
            connection.Open();
            var command = new System.Data.SqlClient.SqlCommand("", connection);
            command.CommandText = @"
Select s.ID as StationID,s.LocationAddress,s.ObservatoryName,s.LocationByTWD67,s.CreateTime,s.LastRecordTime,s.LastRecordWaterLevel,
	   g.Id as GroupID,g.Name as GroupName
from Station s
left join StationGroup sg on s.ID=sg.StationID
left join [Group]  g on g.Id= sg.GroupID
order by s.ID
";
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Models.Station item = null;
                var dbID = reader["StationID"].ToString();
                if (result.Any(x => x.ID == dbID))
                {
                    item = result.SingleOrDefault(x => x.ID == dbID);
                }
                else
                {
                    item = new Models.Station();
                    result.Add(item);
                }
                item.ID = reader["StationID"].ToString();
                item.LocationAddress = reader["LocationAddress"].ToString();
                item.ObservatoryName = reader["ObservatoryName"].ToString();
                item.LocationByTWD67 = reader["LocationByTWD67"].ToString();
                item.CreateTime = DateTime.Parse(reader["CreateTime"].ToString());
                if (!string.IsNullOrEmpty(reader["LastRecordTime"].ToString()))
                {
                    item.LastRecordTime = DateTime.Parse(reader["LastRecordTime"].ToString());
                }
                if (!string.IsNullOrEmpty(reader["LastRecordWaterLevel"].ToString()))
                {
                    item.LastRecordWaterLevel = double.Parse(reader["LastRecordWaterLevel"].ToString());
                }

                var groupID = reader["GroupID"].ToString();
                var groupName = reader["GroupName"].ToString();

                if (!string.IsNullOrEmpty(groupID))
                {
                    item.Groups.Add(new Models.Group() { Id = int.Parse(groupID), Name = groupName });
                }







            }
            connection.Close();


            return result;
        }

        public Models.Station FindByID(string id)
        {
            Models.Station result = null;
            var connection = new System.Data.SqlClient.SqlConnection(_connectionString);
            connection.Open();
            var command = new System.Data.SqlClient.SqlCommand("", connection);
            command.CommandText = string.Format(@"
Select * from Station
Where ID='{0}'",
id);
            var reader = command.ExecuteReader();
            var list = new List<YC.Models.Station>();
            while (reader.Read())
            {
                Models.Station item = new Models.Station();
                item.ID = reader["ID"].ToString();
                item.LocationAddress = reader["LocationAddress"].ToString();
                item.ObservatoryName = reader["ObservatoryName"].ToString();
                item.LocationByTWD67 = reader["LocationByTWD67"].ToString();
                item.CreateTime = DateTime.Parse(reader["CreateTime"].ToString());

                if (!string.IsNullOrEmpty(reader["LastRecordTime"].ToString()))
                {
                    item.LastRecordTime = DateTime.Parse(reader["LastRecordTime"].ToString());
                }
                if (!string.IsNullOrEmpty(reader["LastRecordWaterLevel"].ToString()))
                {
                    item.LastRecordWaterLevel = double.Parse(reader["LastRecordWaterLevel"].ToString());
                }

                list.Add(item);
            }
            connection.Close();
            if (list.Count == 1)
                result = list.Single();

            return result;
        }



    }
}
