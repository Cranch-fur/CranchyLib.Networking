# CranchyLib.Networking
Beginner, user friendly dynamic link library for C# programming language (.NET Framework).

**Development Framework:** .NET Framework 4.8.1
> :grey_question: Library can be back ported down to .NET Framework 4.6.2, older versions wasn't tested.
> 
**License:** General Public License
> :white_check_mark: No restrictions for sharing, editing & updating the library!

____

## CranchyLib.Networking.Demo
```c#
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CranchyLib.Networking.Demo
{
    internal class Program
    {
        private static void SimpleGetRequest()
        {
            Console.Clear();
            Console.WriteLine("Simple GET Request");

            string requestUrl = "https://api.myip.com";
            Console.WriteLine($"\nGET: {requestUrl}");

            var requestResponse = Networking.Get(requestUrl);
            Console.WriteLine($"\nSTATUS CODE: {requestResponse.statusCode} [{(int)requestResponse.statusCode}]");
            if (requestResponse.statusCode == Networking.E_StatusCode.OK)
            {
                Console.WriteLine($"Content-Type: {requestResponse.headers["Content-Type"]}");
                Console.WriteLine($"Content:\n{requestResponse.content}");
            }
            else
            {
                Console.WriteLine("Failed to perform the request!");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
        private static void CustomHeadersGetRequest()
        {
            Console.Clear();
            Console.WriteLine("Custom Headers GET Request");

            string requestUrl = "https://api.myip.com";
            List<string> headers = new List<string>()
            {
                "User-Agent: CranchyLib Networking Demo",
                "Custom-Header: Example of an custom header"
            };
            Console.WriteLine($"\nGET: {requestUrl}");
            
            Console.WriteLine("headers:");
            foreach(string header in headers)
            {
                Console.WriteLine(header);
            }

            var requestResponse = Networking.Get(requestUrl, headers);
            Console.WriteLine($"\nSTATUS CODE: {requestResponse.statusCode} [{(int)requestResponse.statusCode}]");
            if (requestResponse.statusCode == Networking.E_StatusCode.OK)
            {
                Console.WriteLine($"Content-Type: {requestResponse.headers["Content-Type"]}");
                Console.WriteLine($"Content:\n{requestResponse.content}");
            }
            else
            {
                Console.WriteLine("Failed to perform the request!");
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }


        private static void SimpleDownloadRequest()
        {
            Console.Clear();
            Console.WriteLine("Simple DOWNLOAD Request");

            string requestUrl = "https://i.imgur.com/h5ykMyf.jpeg";
            Console.WriteLine($"\nDOWNLOAD: {requestUrl}");

            var requestResponse = Networking.Download(requestUrl);
            Console.WriteLine($"\nSTATUS CODE: {requestResponse.statusCode} [{(int)requestResponse.statusCode}]");

            if (requestResponse.statusCode == Networking.E_StatusCode.OK && File.Exists(requestResponse.content))
            {
                Console.Write(requestResponse.content);
                Process.Start(requestResponse.content);
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
        private static void CustomDestinationDownloadRequest()
        {
            Console.Clear();
            Console.WriteLine("Custom Destination DOWNLOAD Request");

            string requestUrl = "https://i.imgur.com/h5ykMyf.jpeg";
            Console.WriteLine("\nSpecify destination folder path:");
            string destinationPath = Console.ReadLine();
            Console.WriteLine($"\nDOWNLOAD: {requestUrl}");

            var requestResponse = Networking.Download(requestUrl, destinationPath);
            Console.WriteLine($"\nSTATUS CODE: {requestResponse.statusCode} [{(int)requestResponse.statusCode}]");

            if (requestResponse.statusCode == Networking.E_StatusCode.OK && File.Exists(requestResponse.content))
            {
                Console.Write(requestResponse.content);
                Process.Start(requestResponse.content);
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }




        private static void Main(string[] args)
        {
            Console.WriteLine("CranchyLib.Networking.Demo");
            Console.WriteLine("\nPress ENTER to start...");
            Console.ReadLine();

            SimpleGetRequest();
            CustomHeadersGetRequest();

            SimpleDownloadRequest();
            CustomDestinationDownloadRequest();
        }
    }
}
```
