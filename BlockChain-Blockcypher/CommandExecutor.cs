using System;
using System.Collections.Generic;
using System.Numerics;
using BlockChainBlockcypher.ConsoleWorkers;
using BlockChainBlockcypher.Models;
using BlockChainBlockcypher.Storage;

namespace BlockChainBlockcypher
{
    public class CommandExecutor
    {
        private readonly ParameterParser _parameterParser = new ParameterParser();
        private readonly AccountInfoStorage _accountStorage = new AccountInfoStorage(null);
        private readonly BlockcypherClient _blockcypherClient = new BlockcypherClient();
        private readonly NethereumManager _nethereumManager = new NethereumManager();


        public void ShowInsctruction()
        {
            var instruction =
@"COMMAND: balance
EXAMPLE: b 0
EXPLANATION: b - command, 0 - index of the account in the list

COMMAND: create
EXAMPLE: c password
EXPLANATION: c - command, password - use password for creation private key
EXAMPLE: c 
EXPLANATION: create account with random password

COMMAND: send
EXAMPLE: s 2 3 100 
EXPLANATION: s - command, 2 - index of account who will send, 3 - index user who will received, 100 - amount

COMMAND: faucet
EXAMPLE: f 4 20 
EXPLANATION: f - command, 4 - index of acoount who will received, 20 - amount

COMMAND: init
EXAMPLE: i path  
EXPLANATION: set path to json file where we are gonna store accounts information
";

            MessageHandler.SendMessage(instruction);
        }


        public void Execute(string param)
        {
            var parameters = _parameterParser.ParseParams(param);

            var command = parameters[0];
            switch (command.ToLower())
            {
                case "balance":
                case "b":
                    BalanceCommand(parameters);
                    break;

                case "create":
                case "c":
                    CreateCommand(parameters);
                    break;

                case "send":
                case "s":
                    SendCommand(parameters);
                    //https://www.blockcypher.com/dev/ethereum/#push-raw-transaction-endpoint
                    break;

                case "faucet":
                case "f":
                    FaucetCommand(parameters);
                    break;

                case "init":
                case "i":
                    InitCommand(parameters);
                    break;


                default:
                    MessageHandler.SendMessage($"Do not have implementation for command: {command}");
                    break;
            }

        }


        private void InitCommand(List<string> parameters)
        {
            if (parameters.Count != 2)
            {
                MessageHandler.SendMessage($"You must pass 2 parameters to execute command");
                return;
            }

            _accountStorage.SetPathToJson(parameters[1]);
        }

        private void BalanceCommand(List<string> parameters)
        {
            if (parameters.Count != 2)
            {
                MessageHandler.SendMessage($"You must pass 2 parameters to execute command");
                return;
            }

            var accountIndex = _parameterParser.ParseIndex(parameters[1]);

            if (accountIndex == null)
                return;


            AccountInfo account = _accountStorage.GetAccountInfo(accountIndex.Value);
            if (account == null)
            {
                MessageHandler.SendMessage($"Account by index {accountIndex.Value} is not exist");
                return;
            }

            var accountBalance = _blockcypherClient.GetBalanceAsync(account.Address).GetAwaiter().GetResult();
            MessageHandler.SendMessage($"Account balance: {accountBalance.Balance}, unconfirmed balance: {accountBalance.UnconfirmedBalance}");
        }

        private void SendCommand(List<string> parameters)
        {
            if (parameters.Count != 4)
            {
                MessageHandler.SendMessage($"You must pass 4 parameters to execute command");
                return;
            }

            var fromAccountIndex = _parameterParser.ParseIndex(parameters[1]);
            var toAccountIndex = _parameterParser.ParseIndex(parameters[2]);
            var amount = _parameterParser.ParseAmount(parameters[3]);

            if (fromAccountIndex == null || toAccountIndex == null || amount == null)
                return;

            AccountInfo fromAccount = _accountStorage.GetAccountInfo(fromAccountIndex.Value);
            AccountInfo toAccount = _accountStorage.GetAccountInfo(toAccountIndex.Value);


            if (fromAccount == null)
                MessageHandler.SendMessage($"Account by index {fromAccountIndex.Value} is not exist");
            if (toAccount == null)
                MessageHandler.SendMessage($"Account by index {toAccountIndex.Value} is not exist");

            if (fromAccount == null || toAccount == null)
                return;


            var hex = _nethereumManager.SignTransaction(fromAccount, toAccount, new BigInteger(amount.Value), fromAccount.TransactionCount);
            MessageHandler.SendMessage($"Transaction hex = {hex}");

            var verificationResult = _nethereumManager.VerifyTransaction(hex);

            var transactionRezult = _blockcypherClient.SendRawTransactionAsync(hex).GetAwaiter().GetResult();


            if (!string.IsNullOrWhiteSpace(transactionRezult.Error))
            {
                MessageHandler.SendMessage($"Error = {transactionRezult.Error}");
            }
            else if(transactionRezult.TransactionInfo != null)
            {
                MessageHandler.SendMessage($"Transaction hash = {transactionRezult.TransactionInfo.Hash}");

                //show ballance
                var balanceFromAccount = _blockcypherClient.GetBalanceAsync(toAccount.Address).GetAwaiter().GetResult();
                MessageHandler.SendMessage($"Balance account who send: {balanceFromAccount.Balance}, unconfirmed balance: {balanceFromAccount.UnconfirmedBalance}");

                var balanceToAccount = _blockcypherClient.GetBalanceAsync(fromAccount.Address).GetAwaiter().GetResult();
                MessageHandler.SendMessage($"Balance account who received: {balanceToAccount.Balance}, unconfirmed balance: {balanceToAccount.UnconfirmedBalance}");


                //save list to json file, because we changed transaction count
                fromAccount.TransactionCount++;
                _accountStorage.SaveList();
            }
        }

        private void FaucetCommand(List<string> parameters)
        {
            if (parameters.Count != 3)
            {
                MessageHandler.SendMessage($"You must pass 3 parameters to execute command");
                return;
            }


            var accountIndex = _parameterParser.ParseIndex(parameters[1]);
            var amount = _parameterParser.ParseAmount(parameters[2]);

            if (accountIndex == null || amount == null)
                return;

            AccountInfo account = _accountStorage.GetAccountInfo(accountIndex.Value);


            if (account == null)
            {
                MessageHandler.SendMessage($"Account by index {accountIndex} is not exist");
                return;
            }

            var result = _blockcypherClient.FaucetsAsync(account.Address, amount.Value).GetAwaiter().GetResult();


            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                MessageHandler.SendMessage($"Error = {result.Error}");
            }
            else if(!string.IsNullOrWhiteSpace(result.TxRef))
            {
                MessageHandler.SendMessage($"Transaction reference = {result.TxRef}");

                var balanceAccount = _blockcypherClient.GetBalanceAsync(account.Address).GetAwaiter().GetResult();
                MessageHandler.SendMessage($"Balance account who send: {balanceAccount.Balance}, unconfirmed balance: {balanceAccount.UnconfirmedBalance}");
            }
        }

        private void CreateCommand(List<string> parameters)
        {
            if (parameters.Count != 1 || parameters.Count != 2)
            {
                MessageHandler.SendMessage($"You must pass 1 or 2 parameters to execute command");
                return;
            }


            AccountInfo account;
            if (parameters.Count == 2 && !string.IsNullOrWhiteSpace(parameters[1])) 
                account = _nethereumManager.CreateAccount(parameters[1]);
            else
                account = _nethereumManager.CreateAccountWithRandomPassowrd();


            _accountStorage.AddAccount(account);
            _accountStorage.SaveList();

            MessageHandler.SendMessage($"Account information, adress = {account.Address}, private key = {account.PrivateKey}");
        }

    }
}