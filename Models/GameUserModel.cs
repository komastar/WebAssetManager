using System.ComponentModel.DataAnnotations;

namespace AssetWebManager.Models
{
    public class GameUserModel
    {
        public int Id { get; set; }
        [Key]
        public string UserId { get; set; }
        public int GameRoomId { get; set; }
    }
}
