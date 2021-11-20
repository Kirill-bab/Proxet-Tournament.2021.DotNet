using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proxet.Tournament
{
    public class TeamGenerator
    {
        private readonly List<VehichleType> _vehichleTypeList = Enum.GetValues(typeof(VehichleType)).Cast<VehichleType>().ToList();

        public (string[] team1, string[] team2) GenerateTeams(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {filePath} was not found!");

            var playersList = new List<Player>();
            var team1 = new List<string>(9);
            var team2 = new List<string>(9);

            //write data from file to players list
            using (StreamReader sr = new StreamReader(filePath))
            {
                sr.SkipHeaders();
                string line;

                while ((line = sr.ReadLine()) is not null)
                {
                    playersList.Add(ParsePlayer(line));
                }
                sr.Close();
            }


            foreach (var vehichleType in _vehichleTypeList)
            {
                var players = playersList.Where(x => x.VehichleType == vehichleType)
                    .OrderByDescending(x => x.WaitingTime).Take(6).Select(x => x.Nickname).ToList();

                team1.AddRange(players.Take(3));
                team2.AddRange(players.Skip(3));
            }

            return (team1.ToArray(), team2.ToArray());
        }

        private Player ParsePlayer(string line)
        {
            var playerInfo = line.Split('\t', StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim());

            if (playerInfo.Count() != 3) return null;

            return new Player
            {
                Nickname = playerInfo.ElementAt(0),
                WaitingTime = int.Parse(playerInfo.ElementAt(1)),
                VehichleType = (VehichleType)int.Parse(playerInfo.ElementAt(2))
            };
        }
    }
}