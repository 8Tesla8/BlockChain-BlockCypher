using BlockChainBlockcypher.Models;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using System.Text;

namespace BlockChainBlockcypher
{
    public class NethereumManager
    {

        public AccountInfo CreateAccountWithRandomPassowrd()
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();

            var account = new Account(privateKey);

            return new AccountInfo()
            {
                Address = account.Address,
                PrivateKey = account.PrivateKey,
            };
        }

        public AccountInfo CreateAccount(string password)
        {
            var hex = Encoding.Default.GetBytes(password).ToHex();

            var account = new Account(hex);

            return new AccountInfo()
            {
                Address = account.Address,
                PrivateKey = account.PrivateKey,
            };
        }



        /// <summary>
        /// sign transaction
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <param name="transactionCount"></param>
        /// <returns>
        /// return transaction hex
        /// </returns>
        public string SignTransaction(AccountInfo from, AccountInfo to, BigInteger amount, BigInteger transactionCount)
        {
            //gas price
            //42 TRADER < ASAP
            //36 FAST < 2m
            //30 STANDARD < 5m

            return Web3.OfflineTransactionSigner.SignTransaction(from.PrivateKey, to.Address, amount, transactionCount);
            //return Web3.OfflineTransactionSigner.SignTransaction(from.PrivateKey, to.Address, amount, transactionCount, 50, 25000);
        }

        public bool VerifyTransaction(string transactionHex)
        {
            return Web3.OfflineTransactionSigner.VerifyTransaction(transactionHex);
        }
    }
}
