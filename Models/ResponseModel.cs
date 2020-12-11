﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetWebManager.Models
{
    public class ResponseModel
    {
        public bool ProcessResult { get; set; }
        public string Data { get; set; }
        public ResponseModel(bool procResult, object data)
        {
            ProcessResult = procResult;
            if (null != data)
            {
                Data = JObject.FromObject(data).ToString();
            }
            else
            {
                Data = null;
            }
        }
        public ResponseModel(bool procResult) : this(procResult, null) { }
        public ResponseModel(object data) : this(true, data) { }
        public ResponseModel() { }
    }
}
