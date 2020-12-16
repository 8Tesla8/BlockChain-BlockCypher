using System;
using System.IO;
using System.Text;

namespace BlockChainBlockcypher.Storage
{
    public class FileWorker
    {

        public string Read(string path)
        {
            return File.ReadAllText(path);
        }

        public void Write(string path, string data)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(new UTF8Encoding(true).GetBytes(data), 0, data.Length);
            }
        }
    }
}
