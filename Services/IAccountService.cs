using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteEbanx.Models;

namespace TesteEbanx.Services
{
    public interface IAccountService
    {
        Account PostEvent(Event eventModel);
        void Reset();
        public Account GetAccountById(string id);
        public int GetBalance(int id);
    }
}
