using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string OwnerUserId { get; set; }
        [NotMapped]
        public string UserId { get; set; }

        public GameModel() { }

        public GameModel(string gamecode, int maxUserCount, bool isOpen = false)
        {
            maxUserCount = maxUserCount > 6 ? 6 : maxUserCount;
            GameCode = gamecode;
            UserCount = 0;
            MaxUserCount = maxUserCount < 2 ? 2 : maxUserCount;
            IsOpen = isOpen;
            CreationTime = DateTime.Now;
        }

        public void Validate()
        {
            MaxUserCount = Math.Clamp(MaxUserCount, 2, 6);
            CreationTime = DateTime.Now;
            UserCount = 0;
        }
    }
}
