
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace App.Web1.Code
{
    public class RandomUserRepository
    {
        private static RandomUserRepository _repository;
        private static readonly object syncRoot = new object();
        private const string RANDOMUSER_ME_URL = @"http://api.randomuser.me/?results=10";

        private ConcurrentBag<User> users = new ConcurrentBag<User>();
        public List<ZipcodeEntry> Zipcodes { get; private set; }

        private RandomUserRepository()
        {
            var path = GetPersistentStoragePath();
            if (File.Exists(path))
            {
                LoadFromDisk();
            }
            
            if(users.Count < 1000)
            {
                LoadFromWebsite().ContinueWith(t =>
                {
                    // persist to disk
                    PersistToDisk(path);
                });

            }

            var zipPath = GetZipcodeStoragePath();

            using (var tr = new StringReader(File.ReadAllText(zipPath)))
            {
                using (var csv = new CsvHelper.CsvReader(tr))
                {
                    csv.Configuration.AutoMap<ZipcodeEntry>();
                    var zipcodes = csv.GetRecords<ZipcodeEntry>().ToList();

                    Zipcodes = (from z in zipcodes
                                where z.type == "STANDARD"
                                && z.latitude != 0.0m
                                && z.longitude != 0.0m
                                select z).ToList();
                }
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return users.ToList();
        }

        private string GetPersistentStoragePath()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/users.json");
            return path;

        }

        private string GetZipcodeStoragePath()
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/zip_code_database.csv");
            return path;

        }

        private void PersistToDisk(string path)
        {
            // create a result array to persist
            var resultArray = (from u in users.ToList()
                               select new Result()
                               {
                                   User = u
                               }).ToArray();
            // create our serialization structure
            var result = new RandomUserResults()
            {
                Results = resultArray
            };

            // serialize the results
            var serializedText = result.Serialize();

            // persist it to storage
            File.WriteAllText(path, serializedText);

        }

        private void LoadFromDisk()
        {
            // get our path to restore from
            var path = GetPersistentStoragePath();

            // get our serialized text from storage
            var serializedText = File.ReadAllText(path);

            // create our deserialization structure
            var result = RandomUserResults.FromJson(serializedText);

            // add our users to the collection
            AddUserResultsToCollection(result);


        }

        private void AddUserResultsToCollection(RandomUserResults results)
        {
            foreach (var userResult in results.Results)
                users.Add(userResult.User);
        }

        private async Task LoadFromWebsite()
        {
            
            var results = new RandomUserResults()
            {
                Results = new Result[0]
            };

            HttpClient client = new HttpClient();

            while (true)
            {
                if (results.Results.Length > 1000)
                    break;

                string webResult = await ProcessURLAsync(RANDOMUSER_ME_URL, client);

                if (string.IsNullOrWhiteSpace(webResult))
                    break;

                var result = RandomUserResults.FromJson(webResult);

                var list = results.Results.ToList();
                list.AddRange(result.Results);
                results.Results = list.ToArray();
            }

            // add our users to the collection
            AddUserResultsToCollection(results);

        }

        async Task<string> ProcessURLAsync(string url, HttpClient client)
        {
            var result = await client.GetStringAsync(url);

            return result;
        }

        public static RandomUserRepository GetInstance()
        {
            lock (syncRoot)
            {
                if (_repository == null)
                {
                    _repository = new RandomUserRepository();
                }

                return _repository;
            }
        }

        public void CheckUserLocation(RandomSale sale)
        {
            var user = sale.User;
            var location = user.Location;

            ZipcodeEntry entry = null;

            entry = (from z in Zipcodes
                     where z.zip == location.Zip
                     select z).FirstOrDefault();

            if(entry == null)
            {
                entry = (from z in Zipcodes
                         where z.state.ToLower() == location.State.ToLower()
                         && z.primary_city.ToLower() == location.City.ToLower()
                         select z).FirstOrDefault();
            }

            if (entry == null)
            {
                entry = (from z in Zipcodes
                         where z.state.ToLower() == location.State.ToLower()
                         select z).Shuffle().FirstOrDefault();
            }

            if (entry == null)
            {
                entry = (from z in Zipcodes
                         where Convert.ToInt32(z.zip.Substring(0,1)) > 0
                         select z).Shuffle().FirstOrDefault();
            }

            if(entry != null)
            {
                location.City = entry.primary_city;
                location.State = entry.state;
                location.Zip = entry.zip;

                sale.Latitude = entry.latitude;
                sale.Longitude = entry.longitude;
                
            }
        }
    }
}