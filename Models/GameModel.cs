﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Models
{
    public class GameModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string GameCode { get; set; }
        public int UserCount { get; set; }
        public int MaxUserCount { get; set; }
        public bool IsOpen { get; set; }
        public DateTime CreationTime { get; set; }

        public GameModel()
        {
        }

        public GameModel(string gamecode, int maxUserCount, bool isOpen = false)
        {
            GameCode = gamecode;
            UserCount = 0;
            MaxUserCount = maxUserCount;
            IsOpen = isOpen;
            CreationTime = DateTime.Now;
        }
    }
}