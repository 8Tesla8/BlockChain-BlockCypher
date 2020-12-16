using System;
using System.Collections.Generic;

namespace BlockChainBlockcypher.ConsoleWorkers
{

    public class ParameterParser
    {

        public List<string> ParseParams(string param)
        {
            if (string.IsNullOrEmpty(param))
                return new List<string>();


            var splitedParams = param.Split(' ');

            var passedParameteres = new List<string>();

            foreach (var item in splitedParams)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    passedParameteres.Add(item);
                }
            }

            return passedParameteres;
        }

        public int? ParseIndex(string param)
        {
            if (!int.TryParse(param, out int index))
            {
                MessageHandler.SendMessage($"Can not parse value:{param}");
                return null;
            }

            return index;
        }

        public long? ParseAmount(string param)
        {
            if (!long.TryParse(param, out long amount))
            {
                MessageHandler.SendMessage($"Can not parse amount:{param}");
                return null;
            }

            return amount;
        }
    }
}

