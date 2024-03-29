﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FormacaoRazor.Extensions.Alerts
{
    public class AlertDecoratorResult : IActionResult
    {
        public IActionResult Result { get; }
        public string Type { get; }
        public string Title { get; }
        public string Body { get; }

        public AlertDecoratorResult(IActionResult result, string type, string title, string body)
        {
            Result = result;
            Type = type;
            Title = title;
            Body = body;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null) { throw new NullReferenceException(); }

            ITempDataDictionaryFactory factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            ITempDataDictionary tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.title"] = Title;
            tempData["_alert.body"] = Body;

            await Result.ExecuteResultAsync(context).ConfigureAwait(true);
        }
    }
}
