﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Helpa.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
    }

    public class HelperResponseModel
    {
        public string Message { get; set; }
        public int helperid { get; set; }
        public string Code { get; set; }
    }

    public class LoginResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public int id { get; set; }
        public List<string> roles { get; set; }
    }

    public class LoginErrorResponseModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}