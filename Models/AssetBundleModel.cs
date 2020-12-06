using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AssetWebManager.Models
{
    public class AssetBundleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        [DisplayName("Code")]
        public string BundleVersionCode { get; set; }
        [DisplayName("Version")]
        public string BundleVersion { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class AssetBundleUploadModel : AssetBundleModel
    {
        public List<IFormFile> Files { get; set; }
        public List<SelectListItem> ProjectList { get; } = new List<SelectListItem>
        { new SelectListItem {Value = "defensquare", Text = "DEFENSQUARE" } };
        public List<SelectListItem> BranchList { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "dev", Text = "DEV" },
            new SelectListItem { Value = "alpha", Text = "ALPHA" },
            new SelectListItem { Value = "beta", Text = "BETA" },
            new SelectListItem { Value = "release", Text = "RELEASE" }
        };
    }
}
