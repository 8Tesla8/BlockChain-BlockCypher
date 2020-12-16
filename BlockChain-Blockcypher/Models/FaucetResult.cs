using System;
using Newtonsoft.Json;

namespace BlockChainBlockcypher.Models
{
    public class FaucetRezult
    {
        [JsonProperty("tx_ref")]
        public string TxRef { get; set; }


        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
