﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchmakingService.Models
{
    public class ActivityUser
    {
        public long UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}