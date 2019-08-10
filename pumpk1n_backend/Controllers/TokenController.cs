using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Models.TransferModels.Tokens;
using pumpk1n_backend.Responders;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("token")]
    public class TokenController : ControllerBase
    {
        public async Task<IActionResult> ProcessCoinGateHook()
        {
            var model = new CoinGateHookTransferModel();
            foreach (var key in Request.Form.Keys)
            {
                switch (key)
                {
                    case "order_id":
                        model.OrderId = long.Parse(Request.Form[key]);
                        break;
                    case "status":
                        model.Status = Request.Form[key];
                        break;
                    case "id":
                        model.Id = long.Parse(Request.Form[key]);
                        break;
                    case "price_amount":
                        model.PriceAmount = float.Parse(Request.Form[key]);
                        break;
                    case "price_currency":
                        model.PriceCurrency = Request.Form[key];
                        break;
                    case "receive_amount":
                        model.ReceiveAmount = float.Parse(Request.Form[key]);
                        break;
                    case "receive_currency":
                        model.ReceiveCurrency = Request.Form[key];
                        break;
                    case "pay_amount":
                        model.PayAmount = float.Parse(Request.Form[key]);
                        break;
                    case "pay_currency":
                        model.PayCurrency = Request.Form[key];
                        break;
                    case "created_at":
                        model.CreatedAt = DateTime.Parse(Request.Form[key]);
                        break;
                    case "token":
                        model.Token = Request.Form[key];
                        break;
                    default:
                        continue;
                }
            }

            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}