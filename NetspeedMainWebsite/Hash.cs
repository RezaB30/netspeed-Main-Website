using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NetspeedMainWebsite
{
    public class Hash
    {
        public string HashCalculate(string value)
        {
            var hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}