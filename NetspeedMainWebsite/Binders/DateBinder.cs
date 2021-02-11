using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Binders
{
    public class DateBinder : IModelBinder
    {
        Logger binderLogger = LogManager.GetLogger("binders");


        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
           

            string key = bindingContext.ModelName;
            var result = bindingContext.ValueProvider.GetValue(key);

            //applicationLogger.Error($"{ } - date for application");
            binderLogger.Error($"key: {key}- result: {result}  date for application dates BINDER");

            if (result == null)
            {
                return null;
            }

            if (bindingContext.ModelType == typeof(DateTime?) && string.IsNullOrWhiteSpace(result.AttemptedValue))
            {
                binderLogger.Error($"key2: {key}- result2: {result}  date for application dates BINDER");
                
                return null;
            }

            DateTime value;

            if (DateTime.TryParse(result.AttemptedValue, CultureInfo.CreateSpecificCulture("tr-TR"), System.Globalization.DateTimeStyles.AllowWhiteSpaces, out value))
            {
                binderLogger.Error($"key3: {key}- result3: {result} -return:{value.Date} date for application dates BINDER");

                return value.Date;
            }

            bindingContext.ModelState.AddModelError(key, string.Format("Tarihi Gün.Ay.Yıl Formatında Giriniz.", bindingContext.ModelMetadata.DisplayName));

            binderLogger.Error($"key4: {key}- result4: {result} -return:{value.Date} date for application dates BINDER");

            return null;
        }
    }
}