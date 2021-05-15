using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteEbanx.Models
{
    public class Accounts
    {
        
        public List<Account> ListAccounts { get; set; }
    }
    public class Account
    {
        public long Id { get; set; }
        public int Balance { get; set; }
    }


}
