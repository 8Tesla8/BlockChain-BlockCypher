using System;
using Newtonsoft.Json;

namespace BlockChainBlockcypher.Models
{
    public class BalanceResult
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("total_received")]
        public string TotalReceived { get; set; }

        [JsonProperty("total_sent")]
        public string TotalSent { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }

        [JsonProperty("unconfirmed_balance")]
        public long UnconfirmedBalance { get; set; }

        [JsonProperty("final_balance")]
        public long FinalBalance { get; set; }

        [JsonProperty("n_tx")]
        public long NTX { get; set; }

        [JsonProperty("unconfirmed_n_tx")]
        public long UnconfirmedNTX { get; set; }

        [JsonProperty("final_n_tx")]
        public long FinalNTX { get; set; }


        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
