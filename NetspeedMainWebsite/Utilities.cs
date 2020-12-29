using NetspeedMainWebsite.MainSiteServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite
{
    static class Utilities
    {
        public static string ClientUtilities(string value)
        {
            MainSiteServiceClient client = new MainSiteServiceClient();
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var genericHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetProvinces(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = genericHash,
                Username = username,
            });
            return response.ToString();
        }
    }
}