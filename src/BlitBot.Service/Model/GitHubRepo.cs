using System;
using System.Collections.Generic;
using System.Text;

namespace BlitBot.Service.Model
{
    public class GitHubRepo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public DateTime created_at { get; set; }
    }
}
