﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace PRN211_Group3_FinalProject.Filter
{
    public class LoginFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //Get action (1. manager, 0. customer)
                var serializedAction = context.HttpContext.Session.GetString("SessionAdmin");
                int? action = null;
                if (!string.IsNullOrEmpty(serializedAction))
                {
                    action = JsonConvert.DeserializeObject<int>(serializedAction);
                    // Use the deserialized integer 'action' here
                }

                if (context.HttpContext.Request.Cookies["username"] == null)
                {
                    if (action == null)
                    {
                        context.Result = new RedirectToActionResult("Index", "Login", null);
                    }
                    //context.Result = new RedirectToActionResult("Index", "Login", null);

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
