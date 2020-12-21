using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Models
{
    public class RoundModel
    {
        public int Round { get; set; }
        public int ReadyCount { get; set; }
        public List<int> Dices { get; set; }

        public RoundModel()
        {
            Round = 1;
            Dices = new List<int>();
        }

        public RoundModel(int round)
        {
            Round = round + 1;
            Dices = new List<int>();
        }
    }
}
