using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.Filter;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using DataAccess.IRepository;
using DataAccess.Repository;
using BusinessObject;
using DataAccess.DataAccess;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        #region Setting
        IAccountRepository acc_repository;
        IOrderRepository ord_repository;
        private readonly IHttpContextAccessor _contx;
        public AccountController(IHttpContextAccessor contx)
        {
            acc_repository = new AccountRepository();
            ord_repository = new OrderRepository();
            _contx = contx;
        }
        #endregion

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult MyAccount()
        {
            try
            {
                // Retrieve the serialized list from the session
                var Cus = _contx.HttpContext.Session.GetString("Session");

                // Deserialize the JSON string into a list
                var acc = JsonConvert.DeserializeObject<CustomerObject>(Cus);
                CustomerObject cs = acc_repository.GetCustomerByUsername(acc.Username);
                ViewBag.Account = cs;
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult MyAddress()
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(customer);

                ViewBag.ListAddress = acc_repository.GetDeliveryAddress(cus.Username);
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ViewOrder()
        {
            try
            {
                var Cus = _contx.HttpContext.Session.GetString("Session");
                var uid = JsonConvert.DeserializeObject<CustomerObject>(Cus);
                if (uid != null)
                {
                    List<OrderObject> list = ord_repository.GetOrderByUsername(uid.Username).ToList();
                    ViewBag.ListOrder = list;
                }
                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult UpdateProfile(CustomerObject n)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                // Deserialize the JSON string into a list
                var cus = JsonConvert.DeserializeObject<CustomerObject>(customer);
                n.Username = cus.Username;
                var tmp = acc_repository.GetAllCustomer().ToList();

                foreach (var item in tmp)
                {
                    if (n.Email == item.Email && item.Username != n.Username)
                    {
                        return Json("Existed");
                    }
                }
                acc_repository.Update(n);
                return RedirectToAction("MyAccount", "Account");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult ChangePassword(string newPass)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                // Deserialize the JSON string into a list
                var cus = JsonConvert.DeserializeObject<CustomerObject>(customer);


                acc_repository.ChangePassword(cus, newPass);
                return RedirectToAction("ChangePassword", "Account");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult CheckPassword(string pw)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                // Deserialize the JSON string into a list
                var cus = JsonConvert.DeserializeObject<CustomerObject>(customer);

                CustomerObject cs = acc_repository.GetCustomer(cus.Username, pw);
                if (cs != null)
                {
                    return Content("Sucess");
                }
                else
                {
                    return Content("Fail");
                }
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult OrderDetail(string id)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(customer);

                List<OrderDetailObject> od = ord_repository.GetOrderDetail(id).ToList();
                var a = ord_repository.GetOrderAddress(id);
                var orderThis = ord_repository.GetOrderByUsername(cus.Username).FirstOrDefault(o => o.OrderId == id);
                var rs = new
                {
                    orderDetails = od,
                    addresses = a,
                    orderDick = orderThis
                };
                return Json(rs);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult UpdateAddress(DeliveryAddressObject a)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(customer);
                a.Username = cus.Username;
                acc_repository.UpdateAddress(a);

                DeliveryAddressObject tmp = cus.addresses.FirstOrDefault(z => z.ID == a.ID);
                int index = cus.addresses.IndexOf(tmp);
                cus.addresses[index] = a;
                _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
                return RedirectToAction(nameof(MyAddress));
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }
        [HttpPost]
        public IActionResult AddAddress(DeliveryAddressObject a)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(customer);
                if (a.PhoneNum != null && a.AddressInformation != null && a.Fullname != null)
                {
                    int kq = acc_repository.AddAddress(a, cus.Username);
                    if (kq != null)
                    {
                        a.ID = kq;
                    }
                    cus.addresses.Add(a);
                }
                else
                {
                    ViewBag.Message = "Invalid";
                }

                _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
                return RedirectToAction(nameof(MyAddress));
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult DeleteAddress(int id)
        {
            try
            {
                var customer = _contx.HttpContext.Session.GetString("Session");

                CustomerObject cus = JsonConvert.DeserializeObject<CustomerObject>(customer);

                acc_repository.DeleteAddress(id);
                var address = cus.addresses.FirstOrDefault(a => a.ID == id);
                cus.addresses.Remove(address);
                _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
                return Content("Success");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult CheckEmail(string email)
        {
            try
            {
                bool result = acc_repository.CheckEmail(email);

                return result == true ? Content("true") : Content("false");
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpPost]
        public IActionResult SendOTP(string email)
        {
            try
            {


                string fromEmail = "clenynguyen@gmail.com";
                string password = "pyaotxulqjcgttwl";

                string reciever = email;

                Random random = new Random();

                string otp = random.Next(100000, 999999).ToString();

                string htmlContent = @"
<!DOCTYPE html>
<html>

<head>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <title>OTP SENDING</title>
    <link href='css/styledone.css' rel='stylesheet'>
    <style>
        body {

            font-family: Arial, sans-serif;
            text-align: center;
            padding: 20px;
        }

        .container {
            max-width: 900px;
            margin: 0 auto;
            background-color: rgba(255, 247, 247, 0.1);
            height: 650px;
        }

        .circle {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            background-color: #008000;

            margin: 20px auto;
        }

        .circle::before {
            content: '\2714';
            font-size: 110px;
            color: #fff;
        }

        h1 {
            color: #008000;
            margin-top: 20px;
        }

        .divider {
            margin-top: 20px;
            border-top: 2px solid #333;
        }

        .thank-you {
            font-weight: bold;
            margin-top: 20px;
        }

        .button {
            display: inline-block;
            background-color: #008000;
            color: #fff;
            padding: 10px 20px;
            border-radius: 5px;
            text-decoration: none;
            margin-top: 20px;
        }

        .hotline {
            font-size: 24px;
            color: red;
        }

        p {
            font-size: 17px;
            color: #333;
            margin-left: 20px;
        }

        .container_1 {
            max-width: 620px;
            margin: 0 auto;
            background-color: rgba(255, 247, 247, 0.1);
            height: 100%;
            border: 2px solid gray;

            border-radius: 10px;
        }

        .container_1 p {
            text-align: left;

        }

        .circle_1 {

            border-radius: 50%;

            margin: 20px auto;
        }


        img {
            width: 25%;
        }

        .divider_1 {

            margin-top: 20px;
            border-top: 2px solid gray;
            width: 570px;
            margin-left: 24px;
        }

        .hotline_1 {
            font-size: 50px;
            color: Black;
        }

        .footer {
            max-width: 40%;
            margin-left: 29%;
            text-align: center;
        }

        .footer p {
            color: grey;
            text-align: center;
            font-size: 15px;
        }
    </style>
</head>

<body style=""text-align: center;"">
    <div class='container_1'>
        <div class='circle_1'>
             <img src=""cid:imageId"" alt=""Circle Image"">
        </div>

        <p style='color: black; font-size: 33px; text-align: center; margin-top: 10px;'>Verify your recovery email</p>


        <div class='divider_1'></div>

        <p class='thank-you'></p>
        <p>GEARSHOP has received a request to use <span style='font-weight: bold;'>" + reciever + @"</span>to
            recovery your GEARSHOP's account.
        </p>

        <p> Use this code to finish setting up this recovery account:</p><br>

        <span class='hotline_1'>" + otp + @"</span>
        <br>
        <p style='text-align: center; margin-bottom:0;'>If you do not recognize this request, you can safely
            ignore this email.<br>
        <p style='text-align: center; margin: 0;'><strong>Please do not provide this OTP code to
                others</strong></p>
        </p>

    </div>
    <div class='footer'>
        <p>We are sending this email to let you know
            about important changes to Google Accounts and
            your service.</p>
        <p>2023 Google LLC, 227 Thi Tran Phong Dien, Thanh Pho
            Can Tho, Viet Nam.</p>
    </div>
</body>
</html>";



                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.Subject = "The OTP to reset password";
                message.To.Add(new MailAddress(reciever));
                message.Body = htmlContent;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true,
                };

                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlContent, null, MediaTypeNames.Text.Html);
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Create the relative path to the image
                string relativeImagePath = "wwwroot/source_img/advertising_img/logoGearshop.png";

                //Combine the base directory and the relative path to get the full path
                string fullPath = Path.Combine(baseDirectory, relativeImagePath);

                //Create the LinkedResource using the full path
                LinkedResource imageResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "imageId"
                };
                //Load the image and attach it as linked resource
                alternateView.LinkedResources.Add(imageResource);
                message.AlternateViews.Add(alternateView);

                // Send the email
                try
                {
                    smtpClient.Send(message);
                    return Content(otp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
                return Content(otp);
                //smtpClient.Send(message);
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }
        }
    }
}
