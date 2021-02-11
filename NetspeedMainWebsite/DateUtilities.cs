using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite
{
    static class DateUtilities
    {       
        public static string ConvertToWebServiceDate(DateTime date)
        { 
            var birthDate=date.ToString("yyyy-MM-dd" ,CultureInfo.InvariantCulture);                    
            return birthDate;
        }
    }
}