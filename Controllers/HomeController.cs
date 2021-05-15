using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TesteEbanx.Models;
using TesteEbanx.Services;
using System.Net;

namespace TesteEbanx.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet()]
        public ActionResult Get()
        {
                return new StatusCodeResult(statusCode: 200);

        }

        // POST /<reset>
        [HttpPost("reset")]
        public ActionResult Reset()
        {
            _accountService.Reset();
            return Ok();
        }

        // GET /<balance>/5
        [HttpGet("balance")]
        public ActionResult<int> Balance(int account_id)
        {
            var result =_accountService.GetBalance(account_id);
            if (result == -1)
            {
                return new StatusCodeResult(statusCode: 404);
            }
            return result;
        }

        // POST /<event>/eventModel
        [HttpPost("event")]
        public ActionResult<String> Event( Event eventModel)
        {
            if (eventModel.Destination != null || eventModel.Destination != "string")
            {

                var result = _accountService.PostEvent(eventModel);
                if (result != null)
                {


                    return Created("",result);
                }
            }

            return NotFound();
         
        }
    }
}
