﻿//---------------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved. 
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,  
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES  
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
//---------------------------------------------------------------------------------- 
// The example companies, organizations, products, domain names, 
// e-mail addresses, logos, people, places, and events depicted 
// herein are fictitious.  No association with any real company, 
// organization, product, domain name, email address, logo, person, 
// places, or events is intended or should be inferred. 

namespace WebApp_Storage_DotNet.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web;
    using System.Threading.Tasks;
    using System.IO;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Azure;
    using System.Configuration;

    /// <summary> 
    /// Azure Blob Storage Photo Gallery - Demonstrates how to use the Blob Storage service.  
    /// Blob storage stores unstructured data such as text, binary data, documents or media files.  
    /// Blobs can be accessed from anywhere in the world via HTTP or HTTPS. 
    /// 
    /// Note: This sample uses the .NET 4.5 asynchronous programming model to demonstrate how to call the Storage Service using the  
    /// storage client libraries asynchronous API's. When used in real applications this approach enables you to improve the  
    /// responsiveness of your application. Calls to the storage service are prefixed by the await keyword.  
    ///  
    /// Documentation References:  
    /// - What is a Storage Account - http://azure.microsoft.com/en-us/documentation/articles/storage-whatis-account/ 
    /// - Getting Started with Blobs - http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/ 
    /// - Blob Service Concepts - http://msdn.microsoft.com/en-us/library/dd179376.aspx  
    /// - Blob Service REST API - http://msdn.microsoft.com/en-us/library/dd135733.aspx 
    /// - Blob Service C# API - http://go.microsoft.com/fwlink/?LinkID=398944 
    /// - Delegating Access with Shared Access Signatures - http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-1/ 
    /// </summary> 

    public class HomeController : Controller
    {   
         static string blobContainerName = "webappstoragedotnet-imagecontainer";
         static BlobContainerClient blobContainer;

        #region controller
        /// <summary> 
        /// ActionResult Index() 
        /// Documentation References:  
        /// - What is a Storage Account: http://azure.microsoft.com/en-us/documentation/articles/storage-whatis-account/ 
        /// - Create a Storage Account: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#create-an-azure-storage-account
        /// - Create a Storage Container: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#create-a-container
        /// - List all Blobs in a Storage Container: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#list-the-blobs-in-a-container
        /// </summary> 
        public ActionResult Index()
        {
            try
            {
                // Retrieve the connection string
                // How to create a storage connection string - https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();

                // Create a client to interact with blob service.
                BlobServiceClient client = new BlobServiceClient(connectionString);

                // Get the container named "webappstoragedotnet-imagecontainer" from this storage account.
                // You need create this container in this storage account first if you want interact with truly Azure service.
                blobContainer = client.GetBlobContainerClient(blobContainerName);

                // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
                // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
                // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
                // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
                blobContainer.SetAccessPolicy(PublicAccessType.Blob);

                // Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
                List<Uri> allBlobs = new List<Uri>();
                foreach (BlobItem blob in blobContainer.GetBlobs())
                {
                    if (blob.Properties.BlobType == BlobType.BlockBlob)
                    {
                        allBlobs.Add(blob.Properties.CopySource);
                    }
                }

                return View(allBlobs);
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }       
        }

        /// <summary> 
        /// Task<ActionResult> UploadAsync() 
        /// Documentation References:  
        /// - UploadAsync Method: https://azure.github.io/azure-sdk-for-net/api/Azure.Storage.Blobs/Azure.Storage.Blobs.BlobClient.html#methods
        /// </summary> 
        [HttpPost]
        public async Task<ActionResult> UploadAsync()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                int fileCount = files.Count;

                if (fileCount > 0)
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        BlobClient blob = blobContainer.GetBlobClient(GetRandomBlobName(files[i].FileName));
                        await blob.UploadAsync(files[i].InputStream);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }            
        }

        /// <summary> 
        /// Task<ActionResult> DeleteImage(string name) 
        /// Documentation References:  
        /// - Delete Blobs : https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs
        /// </summary> 
        [HttpPost]
        public async Task<ActionResult> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);
                string filename = Path.GetFileName(uri.LocalPath);

                var blob = blobContainer.GetBlobClient(filename);
                await blob.DeleteAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary> 
        /// Task<ActionResult> DeleteAll(string name) 
        /// Documentation References:  
        /// - Delete Blobs: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs
        /// </summary> 
        [HttpPost]
        public async Task<ActionResult> DeleteAll()
        {
            try
            {
                foreach (BlobItem blob in blobContainer.GetBlobs())
                {
                    if (blob.Properties.BlobType == BlobType.BlockBlob)
                    {
                        await blobContainer.DeleteBlobAsync(blob.Name);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
        #endregion

        #region helpers
        /// <summary> 
        /// string GetRandomBlobName(string filename): Generates a unique random file name to be uploaded  
        /// </summary> 
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
        #endregion
    }
}