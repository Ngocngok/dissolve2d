using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RocketTeam.Sdk.Services.Ads
{
    public struct BlacklistData
    {
        public string url;
        public int impressions;

        public BlacklistData(string url, int impressions) {
            this.url = url;
            this.impressions = impressions;
        }
    }
}
