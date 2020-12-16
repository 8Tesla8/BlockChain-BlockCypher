using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlockChainBlockcypher.Models
{
    public class TranscationRezult
    {
        [JsonProperty("tx")]
        public TransactionInfoRezult TransactionInfo { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }


    public class TransactionInfoRezult
    {
        [JsonProperty("tx")]
        public TransactionInfoRezult TransactionInfo { get; set; }


        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("block_index")]
        public long BlockIndex { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("addresses")]
        public IList<string> Addresses { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("fees")]
        public long Fees { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("gas_used")]
        public long GasUsed { get; set; }

        [JsonProperty("gas_price")]
        public long GasPrice { get; set; }

        [JsonProperty("relayed_by")]
        public string RelayedBy { get; set; } //IpAdress

        [JsonProperty("received")]
        public DateTime Received { get; set; }

        [JsonProperty("ver")]
        public int Ver { get; set; }

        [JsonProperty("double_spend")]
        public bool DoubleSpend { get; set; }

        [JsonProperty("vin_sz")]
        public int VinSz { get; set; }

        [JsonProperty("vout_sz")]
        public int VoutSz { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("inputs")]
        public IList<Input> Inputs { get; set; }

        [JsonProperty("outputs")]
        public IList<Output> Outputs { get; set; }
    }


    public class Input
    {
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("addresses")]
        public IList<string> Addresses { get; set; }
    }

    public class Output
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("addresses")]
        public IList<string> Addresses { get; set; }
    }
}
