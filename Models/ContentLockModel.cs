namespace AssetWebManager.Models
{
    public class ContentLockModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string Version { get; set; }
        public string Region { get; set; }
        public bool Lock { get; set; } = false;
    }
}
