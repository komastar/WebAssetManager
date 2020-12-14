namespace AssetWebManager.Models
{
    public class VersionModel
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int HotFix { get; set; }
        public int Build { get; set; }

        public VersionModel(string version)
        {
            string[] splitString = version.Split('.');
            //  Major.Minor.HotFix
            Major = int.Parse(splitString[0]);
            Minor = int.Parse(splitString[1]);
            HotFix = int.Parse(splitString[2]);
            if (4 == splitString.Length)
            {
                //  Major.Minor.HotFix.Build
                Build = int.Parse(splitString[3]);
            }
        }

        public static bool operator >=(VersionModel a, VersionModel b)
        {
            if (a.Major < b.Major)
            {
                return false;
            }
            else if (a.Minor < b.Minor)
            {
                return false;
            }

            return true;
        }

        public static bool operator <=(VersionModel a, VersionModel b)
        {
            if (a.Major > b.Major)
            {
                return true;
            }
            else if (a.Minor > b.Minor)
            {
                return true;
            }

            return false;
        }
    }
}
