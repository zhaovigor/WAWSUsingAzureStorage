using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;

namespace WAWSUsingAzureStorage
{
    public partial class Default : System.Web.UI.Page
    {
        private CloudBlobContainer blobContainer;

        protected void Page_Load(object sender, EventArgs e)
        {
            blobContainer = InitContainer();
            ListBlobsInContainer();
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            if (FileUploadCtrl.HasFile)
            {
                this.SaveImage(
                  Guid.NewGuid().ToString(),
                  FileUploadCtrl.FileName,
                  FileUploadCtrl.PostedFile.ContentType,
                  FileUploadCtrl.FileBytes
                );
            }
            ListBlobsInContainer();
        }

        private void SaveImage(string id, string fileName, string contentType, byte[] data)
        {
            // Create a blob in container and upload image bytes to it
            var blob = blobContainer.GetBlobReference(fileName);

            blob.Properties.ContentType = contentType;

            // Create some metadata for this image
            var metadata = new NameValueCollection();
            metadata["Id"] = id;
            metadata["Filename"] = fileName;
            
            // Add and commit metadata to blob
            blob.Metadata.Add(metadata);
            blob.UploadByteArray(data);
        }

        private CloudBlobContainer InitContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy

            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(ConfigurationManager.AppSettings["storageAccount"], ConfigurationManager.AppSettings["storageKey"]);
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, false);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["containerName"]);

            try
            {
                container.CreateIfNotExist();
            }
            catch (StorageClientException sce)
            { 
                if (sce.ErrorCode.Equals(StorageErrorCode.ContainerAlreadyExists))
                {
                    //nothing, already there;
                } else {
                    throw sce;
                }
            }

            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            return container;
        }

        private void ListBlobsInContainer()
        {
            BlobsLiteral.Text = "";
             //List blobs and directories in this container hierarchically (which is the default listing).
            foreach (var blobItem in blobContainer.ListBlobs())
            {
                BlobsLiteral.Text += blobItem.Uri;
                BlobsLiteral.Text += "</br>";
            }
            
        }
    }
}