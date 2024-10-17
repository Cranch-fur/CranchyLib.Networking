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
    class Program
    {
        static void GetRequestDemo()
        {
            Console.WriteLine();
            Console.WriteLine();


            string url = "https://api.myip.com";
            List<string> headers = new List<string>()
            {
                "User-Agent: CranchyLib Networking Demo",
                "Custom-Header: Example of the custom header"
            };


            Console.WriteLine($"GET: {url}");
            var webRequest = Networking.Get(url, headers);

            if (webRequest.statusCode == Networking.E_StatusCode.OK)
            {
                Console.WriteLine(webRequest.content);
            }

            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }


        static void DownloadRequestDemo()
        {
            Console.WriteLine();
            Console.WriteLine();


            string url = "https://i.imgur.com/h5ykMyf.jpeg";
            List<string> headers = new List<string>()
            {
                "User-Agent: CranchyLib Networking Demo",
                "Custom-Header: Example of the custom header"
            };


            Console.WriteLine($"DOWNLOAD: {url}");
            var webRequest = Networking.Download(url, headers);

            if (webRequest.statusCode == Networking.E_StatusCode.OK && File.Exists(webRequest.content))
            {
                Console.Write(webRequest.content);
                Process.Start(webRequest.content);
            }

            Console.WriteLine("\nPress ENTER to exit...");
            Console.ReadLine();
        }




        static void Main(string[] args)
        {
            Console.WriteLine("CranchyLib.Networking.Demo");

            GetRequestDemo();
            DownloadRequestDemo();
        }
    }
}
```
