using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPal.Api;
using Paypalcore8.Models;
using Project.Data;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ProjectContext _context;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;
        public PaymentController(ProjectContext context, IHttpContextAccessor context1, IConfiguration iconfiguration)
        {
            _context = context;
            httpContextAccessor = context1;
            _configuration = iconfiguration;
        }
        public IActionResult PlaceOrder(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        {
            try
            {
                var clientId = _configuration["PayPal:ClientId"];
                var clientSecret = _configuration["PayPal:ClientSecret"];
                var mode = _configuration["PayPal:Mode"];

                APIContext apiContext = PaypalConfiguration.GetAPIContext(clientId, clientSecret, mode);

                // Process payment
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    // Payment creation and redirection to PayPal
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Payment/PlaceOrder?";
                    // Generate a GUID for storing the paymentID received in session
                    var guidValue = Convert.ToString((new Random()).Next(100000));
                    guid = guidValue;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // Save the paymentID in the session
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section executes after receiving all parameters for the payment

                    // Execute payment
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    // If executed payment failed then show payment failure message to the user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFailed");
                    }
                    List<Items> Cart = JsonConvert.DeserializeObject<List<Items>>(HttpContext.Session.GetString("Cart"));

                    OrderTable order = new OrderTable
                    {
                        OrderTime = DateTime.Now,
                        OrderItems = new List<OrderItem>()
                    };

                    foreach (var cartItem in Cart)
                    {
                        // Create OrderItem for each cart item
                        OrderItem orderItem = new OrderItem
                        {
                            ProductId = cartItem.Product.PId,
                            ProductName = cartItem.Product.PName,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Product.Price,
                            img_name = cartItem.Product.img_name
                        };
                        order.OrderItems.Add(orderItem);
                    }

                    // Clear the cart session after checkout
                    HttpContext.Session.Remove("Cart");
                    // Save the order to the database
                    _context.OrderTable.Add(order);
                    _context.SaveChanges();
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                    List<OrderItem> orderItems = order.OrderItems;
                    return View("OrderPlaced", orderItems);
                }
            }
            catch (Exception ex)
            {
                return View("PaymentFailed");
            }
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            //create itemlist and add item objects to it  

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Detail",
                currency = "USD",
                price = "1.00",
                quantity = "1",
                sku = "asd"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            //var details = new Details()
            //{
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "1"
            //};
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "1.00", // Total must be equal to sum of tax, shipping and subtotal.  
                //details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> PlaceOrderCOD()
        {
            var cart = HttpContext.Session.GetString("Cart");

            if (cart != null)
            {
                List<Items> Cart = JsonConvert.DeserializeObject<List<Items>>(cart);

                OrderTable order = new OrderTable
                {
                    OrderTime = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                foreach (var cartItem in Cart)
                {
                    // Create OrderItem for each cart item
                    OrderItem orderItem = new OrderItem
                    {
                        ProductId = cartItem.Product.PId,
                        ProductName = cartItem.Product.PName,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Product.Price,
                        img_name = cartItem.Product.img_name
                    };

                    order.OrderItems.Add(orderItem);
                }


                // Save the order to the database asynchronously
                _context.OrderTable.Add(order);
                await _context.SaveChangesAsync();

                // Clear the cart session after checkout
                HttpContext.Session.Remove("Cart");

                // Pass a list of OrderItem to the view
                List<OrderItem> orderItems = order.OrderItems;
                return View("OrderPlaced", orderItems);
            }

            // Handle case when cart is empty
            return RedirectToAction("Index", "Home"); // Redirect to home page or any other page
        }
    }
}




