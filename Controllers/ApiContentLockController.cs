﻿using AssetWebManager.Data;
using AssetWebManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AssetWebManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiContentLockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiContentLockController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region RESTAPI
        // GET: api/ApiContentLock/Check/{PROJECT}/{VERSION}/{CONTENT_NAME}
        [HttpGet("{project}/{version}/{content}")]
        public bool Check(string project, string version, string content)
        {
            bool contentLockCheck = false;
            var contentLock = FindContentLock(project, content);
            if (null != contentLock)
            {
                VersionModel versionFromData = new VersionModel(contentLock.Version);
                VersionModel versionFromClient = new VersionModel(version);
                contentLockCheck = (versionFromClient >= versionFromData) && !contentLock.Lock;
            }

            return contentLockCheck;
        }
        #endregion

        private ContentLockModel FindContentLock(string project, string content)
        {
            return _context.ContentLock.SingleOrDefault(c => c.Name == content && c.Project == project);
        }
    }
}
