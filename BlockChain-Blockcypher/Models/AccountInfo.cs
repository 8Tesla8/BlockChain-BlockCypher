using System;
namespace BlockChainBlockcypher.Models
{
    public class AccountInfo
    {
        public string Address { get; set; }
        public string PrivateKey { get; set; }
        public long TransactionCount { get; set; }
    }
}
