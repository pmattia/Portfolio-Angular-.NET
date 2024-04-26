using System;
using System.Collections.Generic;
using System.Text;

namespace Gnappo.Portfolio.Infrastructure.Storage
{
    public class AzureStorageSettings
    {
        public string BlobServiceConnectionString { get; set; }
        public BlobContainerNames BlobContainerNames { get; set; }
        public string FileShareFolderName { get; set; }
        public string FileShareUrlPattern { get; set; }
        public string ContainerSasToken { get; set; }
    }
    public class BlobContainerNames
    {
        public string BlobAvatarContainerName { get; set; }
        public string BlobContentsContainerName { get; set; }
        public string BlobArticlesContainerName { get; set; }
        public string BlobBlogContainerName { get; set; }
    }
}
