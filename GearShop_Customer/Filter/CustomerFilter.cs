using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace SWP391_Group3_FinalProject.Filter
{
    public class CustomerFilter : IActionFilter
    {

        IAccountRepository acc_repository;
        IOrderRepository ord_repository;

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            acc_repository = new AccountRepository();
            ord_repository = new OrderRepository();
            try
            {
                //Get action (session) and role (cookie) (1. manager, 0. customer)
                var serializedAction = context.HttpContext.Session.GetString("action");
                int? action = null;
                if (!string.IsNullOrEmpty(serializedAction))
                {
                    action = JsonConvert.DeserializeObject<int>(serializedAction);
                    // Use the deserialized integer 'action' here
                }


                if (int.TryParse(context.HttpContext.Request.Cookies["role"], out int cookieValue))
                {

                    if (cookieValue == 0)
                    {
                        var username = context.HttpContext.Request.Cookies["customer"];
                        context.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(acc_repository.GetCustomerByUsername(username)));
                        context.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(ord_repository.GetCartsByUsername(username).Count()));
                    }
                }
                else if (action == 1) // If manager != null means that is admin/staff therefore redirect to dashboard
                {
                    context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
