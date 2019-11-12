---
languages:
- csharp
products:
- azure
- azure-storage
page_type: sample
---

# .NET Photo Gallery Web Application Sample with Azure Blob Storage

This sample application creates a web photo gallery that allows you to host and view images through a .NET web frontend. The code sample also includes functionality for deleting images. At the end, you have the option of deploying the application to Azure.

![Azure Blob Storage Photo Gallery Web Application Sample .NET](./images/photo-gallery.png)

## Technologies used
- ASP.NET MVC 5
- Azure Storage emulator
- Azure Web Apps
- Azure Storage

Azure Blob Storage Photo Gallery Web Application using ASP.NET MVC 5. The sample uses asynchronous programming model to demonstrate how to call the Storage Service using the Storage .NET client library's asynchronous APIs.

## Running this sample
1. Before you can run this sample, you must have the following prerequisites:
	- The Azure Storage Emulator, which you can download [here](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409). You can also read more about [Using the Azure Storage Emulator for development](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).
	- Visual Studio 2017 or Visual Studio 2019.

2. Open the Azure Storage Emulator. Once the emulator is running it will be able to process the images from the application.

3. Clone this repository using Git :
	```cmd
	git clone https://github.com/tzhanl/storage-blobs-dotnet-webapp.git
	```	

4. Switch to the appropriate folder. Navigate to your directory where the project file (.csproj) resides and open it with Visual Studio.

5. In Visual Studio Build menu, select **Build Solution** (or Press F6).

6. You can now run and debug the application locally by pressing **F5** in Visual Studio.

## Deploy this sample to Azure

1. To make the sample work in the cloud, you must replace the connection string with the values of an active Azure Storage Account. If you don't have an account, refer to the [Create a Storage Account](https://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/) article.

2. Click Storage Account in the Azure Portal to open it. Select **Settings** > **Access keys** > **Key1/key**, copy the associated **Connection string** to the clipboard, then paste it into a text editor for later use.

3. In the **Web.config** file, located in the project root, find the **StorageConnectionString** app setting and replace the placeholder values with the values obtained for your account.
	```
	<add key="StorageConnectionString" value="[Enter Your Storage Connection string]" />
	```
4. In Visual Studio Solution Explorer, right-click on the project name and select **Publish...**

5. Using the Publish Website dialog, select **Microsoft Azure Web Apps**

6. In the next dialog, either select an existing web app, or follow the prompts to create a new web application. Note: If you choose to create a web application, the Web App Name chosen must be globally unique.

7. Once you have selected the web app, click **Publish**

8. After a short time, Visual Studio will complete the deployment and open a browser with your deployed application.

For additional ways to deploy this web application to Azure, please refer to the [Deploy a web app in Azure App Service](https://azure.microsoft.com/en-us/documentation/articles/web-sites-deploy/) article which includes information on using Azure Resource Manager (ARM) Templates, Git, MsBuild, PowerShell, Web Deploy, and many more.

## This Sample shows how to do some basic operations of Storage Blobs.
- Create a container.
- Upload images to storage blob.
- List block blobs.
- Delete blobs.

## Folders Introduction
You will find the following folders: WebApp-Storage-DotNet-v3, which references the [Microsoft.Azure.Storage.Blob](https://www.nuget.org/packages/Microsoft.Azure.Storage.Blob/) SDK and WebApp-Storage-DotNet-v12, which uses the [Azure.Storage.Blobs](https://www.nuget.org/packages/Azure.Storage.Blobs/) version of the SDK.

## About the code
The code included in this sample is meant to be a quick start sample for learning about Azure Web Apps and Azure Storage. It is not intended to be a set of best practices on how to build scalable enterprise grade web applications.

## More information
- [What is Azure Blob Storage](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-overview)
- [Getting Started with Blobs](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/)
- [Blob Service Concepts](http://msdn.microsoft.com/en-us/library/dd179376.aspx)
- [Blob Service REST API](http://msdn.microsoft.com/en-us/library/dd135733.aspx)
- [Blob Service C# API](http://go.microsoft.com/fwlink/?LinkID=398944)
- [Delegating Access with Shared Access Signatures](http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-1/)
