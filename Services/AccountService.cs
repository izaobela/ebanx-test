using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteEbanx.Models;
using System.IO;
using Newtonsoft.Json;

namespace TesteEbanx.Services
{
    public class AccountService : IAccountService
    {
        public Account PostEvent(Event eventModel)
        {
            eventModel.Type = eventModel.Type.ToLower();
            Account destination = GetAccountById(eventModel.Destination);
            Account origin = new Account();
            Accounts result = new Accounts();

            if (eventModel.Origin != null) 
            {
                origin = GetAccountById(eventModel.Origin);
            }

            if (destination != null && eventModel.Type != "withdraw")
            {
                switch (eventModel.Type)
                {
                    case "deposit":
                        destination.Balance += eventModel.Amount;
                        result = CreateUpdate(destination);
                        break;
                    case "transfer":
                        if (origin == null)
                        {
                            return null;
                        }
                        destination.Balance += eventModel.Amount;
                        origin.Balance -= eventModel.Amount;
                        result = CreateUpdate(origin);
                        result = CreateUpdate(destination);
                        break;
                }
                
            }

            else if(eventModel.Type != "withdraw")
            {
                destination = new Account()
                {
                        Id = Convert.ToInt64(eventModel.Destination),
                        Balance = eventModel.Amount
                };
                result = CreateUpdate(destination);

                if (eventModel.Type == "transfer" && origin != null)
                {
                    destination.Balance = eventModel.Amount;
                    origin.Balance -= eventModel.Amount;
                    result = CreateUpdate(origin);
                    result = CreateUpdate(destination);

                }

            }



            if(origin != null && eventModel.Type == "withdraw")
            {
                origin.Balance -= eventModel.Amount;
                result = CreateUpdate(origin);
            }
            else if(origin == null && eventModel.Type == "withdraw")
            {
                return null;
            }

            if (eventModel.Type == "deposit")
            {
                return destination;
            }
            return origin;
        }

        public Accounts GetAccounts()
        {
            string getJson = File.ReadAllText("registros");
            return JsonConvert.DeserializeObject<Accounts>(getJson);
        }

        public Account GetAccountById(string id)
        {
            Accounts accounts = GetAccounts();
            if (accounts != null)
            {
                foreach (Account account in accounts.ListAccounts)
                {
                    if(account.Id == Convert.ToInt64(id))
                    {
                        return account;
                    }
                }
            }

            return null;
        }

        public Accounts CreateUpdate(Account account)
        {
            Accounts accounts = GetAccounts();
            if (accounts != null)
            {
                Account accountId = GetAccountById(account.Id.ToString());
                if (accountId == null)
                {
                    accounts.ListAccounts.Add(account);
                }
                else
                {
                    foreach(var listAccount in accounts.ListAccounts)
                    {
                        if(listAccount.Id == account.Id)
                        {
                            listAccount.Balance = account.Balance;
                        }
                    }
                }
            }
            else
            {
                List<Account> list = new List<Account>() { account };
                accounts = new Accounts()
                {
                    ListAccounts = list
                };
            }

            string postJson = System.Text.Json.JsonSerializer.Serialize(accounts);
            File.WriteAllText("registros", postJson);
            return accounts;
        }

        public void Reset()
        {
            File.WriteAllText("registros", "");
        }

        public int GetBalance(int id)
        {
            Account account = GetAccountById(id.ToString());
            if(account == null)
            {
                return -1;
            }
            return account.Balance;
        }
    }
}
