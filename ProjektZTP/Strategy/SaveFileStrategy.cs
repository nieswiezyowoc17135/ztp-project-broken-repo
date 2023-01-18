using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjektZTP.Strategy
{



    class Context
    {

        private IFileStorageStrategy _strategy;

        public Context()
        { }

        public Context(IFileStorageStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetStrategy(IFileStorageStrategy strategy)
        {
            this._strategy = strategy;
        }


        public void ExecuteStrategy(MemoryStream stream, string name)
        {
            this._strategy.SaveFile(stream, name);
        }
    }










    public interface IFileStorageStrategy
    {
        void SaveFile(MemoryStream stream, string name);
    }

    public class LocalFileStorage : IFileStorageStrategy
    {
        public void SaveFile(MemoryStream stream, string name)
        {
            string appPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string path = $"{appPath}\\file" + name + ".pdf";
            using (FileStream filepdf = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(filepdf);
            }
        }
    }

    public class CloudStorage : IFileStorageStrategy
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;
        private readonly CloudBlobContainer _container;

        public CloudStorage(string connectionString, string containerName)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExistsAsync();
        }

        public void SaveFile(MemoryStream stream, string name)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(name+".pdf");
            stream.Position = 0;
            blockBlob.UploadFromStreamAsync(stream);
        }

    }
}
