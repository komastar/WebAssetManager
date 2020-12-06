using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string GameCode { get; set; }
        public int UserCount { get; set; }
        public int MaxUserCount { get; set; }
        public bool IsOpen { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
