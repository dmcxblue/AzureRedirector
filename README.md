# AzureRedirector
A C# project that builds a web application which redirects all HTTPS traffic.

## AzureWebsites

I noticed that building an Azure Redirector requires multiple steps. I found out that some projects utilized an outdated .NET package version 3.1, added some small updates and now this one uses version 9.0.

### Build
Building the C# DLL is as simple as loading it into your Visual Studio, changing the IP located in the `ReverseProxy.cs` file, and then compiling.

### Uploading and Building the App

Login to your Azure Account using the  `AZ CLI` tool and use the following command in the ROOT folder of the project

```powershell
az webapp up --sku F1 --name <NAME> --location <LOCATION>
```

![AzureRedirector](https://github.com/user-attachments/assets/019494a8-a1d6-493c-8338-2bacb60f21c1)

You will receive a URL that can be placed in the URL for the Cobalt Strike Listener
