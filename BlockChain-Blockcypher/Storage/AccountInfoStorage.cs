using System;
using System.Collections.Generic;
using System.IO;
using BlockChainBlockcypher.Models;

namespace BlockChainBlockcypher.Storage
{

    public class AccountInfoStorage
    {
        private readonly FileWorker _fileWorker = new FileWorker();
        private readonly JsonParser _jsonParser = new JsonParser();

        private List<AccountInfo> _accountInfoList = new List<AccountInfo>();

        private string _path;


        public AccountInfoStorage(string path)
        {
            SetPathToJson(path);
        }


        public void SetPathToJson(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                _path = path;

                var obj = _fileWorker.Read(path);

                var listFromFile = _jsonParser.Deserialize<List<AccountInfo>>(obj);

                if(listFromFile != null && listFromFile.Count > 0)
                    _accountInfoList = listFromFile;
            }
        }

        public void SaveList(string path = null)
        {
            string filePath = null;

            if (!string.IsNullOrWhiteSpace(path))
                filePath = path;
            else if (!string.IsNullOrWhiteSpace(_path))
                filePath = _path;

            if (string.IsNullOrWhiteSpace(filePath))
                return;
          

            var data = _jsonParser.Serialize<List<AccountInfo>>(_accountInfoList);
  
            _fileWorker.Write(_path, data);
        }


        public AccountInfo GetAccountInfo(int index) 
        {
            var inBounds = (index >= 0) && (index < _accountInfoList.Count);

            if (!inBounds) 
                return null;
            
            return _accountInfoList[index];
        }

        public void AddAccount(AccountInfo account)
        {
            _accountInfoList.Add(account);
        }

        public void RemoveAccount(AccountInfo account)
        {
            _accountInfoList.Remove(account);
        }

    }
}
