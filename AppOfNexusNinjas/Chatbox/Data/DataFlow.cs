using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AppOfNexusNinjas.Chatbox.Data
{
    public class DataStore
    {
        private readonly string _foundationJsonPath; // Path to FoundationInfo.json
        private readonly string _utsJsonPath; // Path to UTSInfo.json

        public DataStore(string foundationJsonPath, string utsJsonPath)
        {
            _foundationJsonPath = foundationJsonPath;
            _utsJsonPath = utsJsonPath;
        }

        // Method to retrieve data from the JSON files
        public (List<UTSInfo>, List<UTSInfo>) GetAllData()
        {
            var foundationInfos = new List<UTSInfo>();
            var utsInfos = new List<UTSInfo>();

            try
            {
                // Read FoundationInfo.json
                var foundationData = File.ReadAllText(_foundationJsonPath);
                foundationInfos = JsonConvert.DeserializeObject<List<UTSInfo>>(foundationData) ?? new List<UTSInfo>();

                // Read UTSInfo.json
                var utsData = File.ReadAllText(_utsJsonPath);
                utsInfos = JsonConvert.DeserializeObject<List<UTSInfo>>(utsData) ?? new List<UTSInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading JSON files: " + ex.Message);
            }

            return (foundationInfos, utsInfos);
        }
    }

    public class UTSInfo
    {
        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}

