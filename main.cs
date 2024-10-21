using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;

namespace CranchyLib.Networking
{
    public static class Networking
    {
        public enum E_StatusCode : int
        {
            // 1xx ---> Informational Response (the request was received, continuing process)
            // 2xx ---> Successful (the request was successfully received, understood, and accepted)
            // 3xx ---> Redirection (further action needs to be taken in order to complete the request)
            // 4xx ---> Client Error (the request contains bad syntax or cannot be fulfilled)
            // 5xx ---> Server Error (the server failed to fulfil an apparently valid request)
            // Learn More: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes



            // >>> CUSTOM <<<
            UNDEFINED_ERROR = -1,
            NONE = 0,



            // >>> 100 <<<
            // The server has received the request headers and the client should proceed to send the request body.
            CONTINUE = 100,

            // The requester has asked the server to switch protocols and the server has agreed to do so.
            SWITCHING_PROTOCOLS = 101,

            // (WebDAV; RFC 2518)
            // This code indicates that the server has received and is processing the request, but no response is available yet.
            PROCESSING = 102,

            // Used to return some response headers before final HTTP message.
            EARLY_HINTS = 103,

            // The response provided by a cache is stale (the content's age exceeds a maximum age set by a Cache-Control header or heuristically chosen lifetime).
            RESPONSE_IS_STALE = 110,

            // The cache was unable to validate the response, due to an inability to reach the origin server.
            REVALIDATION_FAILED = 111,

            // The cache is intentionally disconnected from the rest of the network.
            DISCONNECTED_OPERATION = 112,

            // The cache heuristically chose a freshness lifetime greater than 24 hours and the response's age is greater than 24 hours.
            HEURISTIC_EXPIRATION = 113,

            // Arbitrary, non-specific warning. The warning text may be logged or presented to the user.
            MISCELLANEOUS_WARNING = 199,



            // >>> 200 <<<
            // Standard response for successful HTTP requests.
            OK = 200,

            // The request has been fulfilled, resulting in the creation of a new resource.
            CREATED = 201,

            // The request has been accepted for processing, but the processing has not been completed.
            ACCEPTED = 202,

            // (since HTTP/1.1)
            // The server is a transforming proxy (e.g. a Web accelerator) that received a 200 OK from its origin, but is returning a modified version of the origin's response.
            NON_AUTHORITATIVE_INFORMATION = 203,

            // The server successfully processed the request, and is not returning any content.
            NO_CONTENT = 204,

            // The server successfully processed the request, asks that the requester reset its document view, and is not returning any content.
            RESET_CONTENT = 205,

            // (RFC 7233)
            // The server is delivering only part of the resource (byte serving) due to a range header sent by the client. The range header is used by HTTP clients to enable resuming of interrupted downloads, or split a download into multiple simultaneous streams.
            PARTIAL_CONTENT = 206,

            // (WebDAV; RFC 4918)
            // The message body that follows is by default an XML message and can contain a number of separate response codes, depending on how many sub-requests were made.
            MULTI_STATUS = 207,

            // (WebDAV; RFC 5842)
            // The members of a DAV binding have already been enumerated in a preceding part of the (multistatus) response, and are not being included again.
            ALREADY_REPORTED = 208,

            // Added by a proxy if it applies any transformation to the representation, such as changing the content encoding, media type or the like.
            TRANSFORMATION_APPLIED = 214,

            // Same as 199 (MISCELLANEOUS_WARNING), but indicating a persistent warning.
            MISCELLANEOUS_PERSISTENT_WARNING = 299,


            // (RFC 3229)
            // The server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
            IM_USED = 226,



            // >>> 300 <<<
            // Indicates multiple options for the resource from which the client may choose.
            MULTIPLE_CHOISES = 300,

            // This and all future requests should be directed to the given URI.
            //
            //       URI -> Uniform Resource Identifier
            //          userinfo       host      port
            //          ┌──┴───┐ ┌──────┴──────┐ ┌┴┐
            //  https://john.doe@www.example.com:123/forum/questions/?tag=networking&order=newest#top
            //  └─┬─┘   └───────────┬──────────────┘└───────┬───────┘ └───────────┬─────────────┘ └┬┘
            //  scheme          authority                  path                 query           fragment
            MOVED_PERMANENTLY = 301,

            // (Previously "Moved temporarily")
            // Tells the client to look at (browse to) another URL.
            FOUND = 302,

            // (since HTTP/1.1)
            // The response to the request can be found under another URI using the GET method. When received in response to a POST (or PUT/DELETE), the client should presume that the server has received the data and should issue a new GET request to the given URI.
            SEE_OTHER = 303,

            // (RFC 7232)
            // Indicates that the resource has not been modified since the version specified by the request headers If-Modified-Since or If-None-Match.
            NOT_MODIFIED = 304,

            // (since HTTP/1.1)
            // The requested resource is available only through a proxy, the address for which is provided in the response.
            USE_PROXY = 305,

            // No longer used. Originally meant "Subsequent requests should use the specified proxy."
            SWITCH_PROXY = 306,

            // (since HTTP/1.1)
            // In this case, the request should be repeated with another URI; however, future requests should still use the original URI.
            TEMPORARY_REDIRECT = 307,

            // (RFC 7538)
            // This and all future requests should be directed to the given URI.
            PERMANENT_REDIRECT = 308,




            // >>> 400 <<<
            // The server cannot or will not process the request due to an apparent client error (e.g., malformed request syntax, size too large, invalid request message framing, or deceptive request routing).
            BAD_REQUEST = 400,

            // (RFC 7235)
            // Similar to 403 Forbidden, but specifically for use when authentication is required and has failed or has not yet been provided.
            UNAUTHORIZED = 401,

            // Reserved for future use.
            PAYMENT_REQUIRED = 402,

            // The request contained valid data and was understood by the server, but the server is refusing action
            FORBIDDEN = 403,

            // The requested resource could not be found but may be available in the future.
            NOT_FOUND = 404,

            // A request method is not supported for the requested resource; for example, a GET request on a form that requires data to be presented via POST, or a PUT request on a read-only resource.
            METHOD_NOT_ALLOWED = 405,

            // The requested resource is capable of generating only content not acceptable according to the Accept headers sent in the request.
            NOT_ACCEPTABLE = 406,

            // (RFC 7235)
            // The client must first authenticate itself with the proxy.
            PROXY_AUTHENTICATION_REQUIRED = 407,

            // The server timed out waiting for the request. 
            REQUEST_TIMEOUT = 408,

            // Indicates that the request could not be processed because of conflict in the current state of the resource, such as an edit conflict between multiple simultaneous updates.
            CONFLICT = 409,

            // Indicates that the resource requested is no longer available and will not be available again.
            GONE = 410,

            // The request did not specify the length of its content, which is required by the requested resource.
            LENGTH_REQUIRED = 411,

            // (RFC 7232)
            // The server does not meet one of the preconditions that the requester put on the request header fields.
            PRECONDITION_FAILED = 412,

            // (RFC 7231)
            // The request is larger than the server is willing or able to process.
            PAYLOAD_TOO_LARGE = 413,

            // (RFC 7231)
            // The URI provided was too long for the server to process.
            URI_TOO_LONG = 414,

            // (RFC 7231)
            // The request entity has a media type which the server or resource does not support.
            UNSUPPORTED_MEDIA_TYPE = 415,

            // (RFC 7233)
            // The client has asked for a portion of the file (byte serving), but the server cannot supply that portion.
            RANGE_NOT_SATISFIABLE = 416,

            // The server cannot meet the requirements of the Expect request-header field.
            EXPECTATION_FAILED = 417,

            // (RFC 2324, RFC 7168)
            // This code was defined in 1998 as one of the traditional IETF April Fools' jokes.
            IM_A_TEAPOT = 418,

            // (Laravel Framework)
            // Used by the Laravel Framework when a CSRF Token is missing or expired.
            PAGE_EXPIRED = 419,

            // (Twitter)
            // Returned by version 1 of the Twitter Search and Trends API when the client is being rate limited.
            ENCHANCE_YOUR_CALM = 420,

            // (RFC 7540)
            // The request was directed at a server that is not able to produce a response.
            MISDIRECTED_REQUEST = 421,

            // (WebDAV; RFC 4918)
            // The request was well-formed but was unable to be followed due to semantic errors.
            UNPROCESSABLE_ENTITY = 422,

            // (WebDAV; RFC 4918)
            // The resource that is being accessed is locked.
            LOCKED = 423,

            // (WebDAV; RFC 4918)
            // The request failed because it depended on another request and that request failed.
            FAILED_DEPENDECY = 424,

            // (RFC 8470)
            // Indicates that the server is unwilling to risk processing a request that might be replayed.
            TOO_EARLY = 425,

            // The client should switch to a different protocol such as TLS/1.3, given in the Upgrade header field.
            UPGRADE_REQUIRED = 426,

            // (RFC 6585)
            // The origin server requires the request to be conditional.
            PRECONDITION_REQUIRED = 428,

            // (RFC 6585)
            // The user has sent too many requests in a given amount of time.
            TOO_MANY_REQUESTS = 429,

            // (RFC 6585)
            // The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large.
            REQUEST_HEADER_FIELDS_TOO_LARGE = 431,

            // The client's session has expired and must log in again.
            LOGIN_TIMEOUT = 440,

            // Used internally to instruct the server to return no information to the client and close the connection immediately.
            NO_RESPONSE = 444,

            // The server cannot honour the request because the user has not provided the required information.
            RETRY_WITH = 449,

            // (Microsoft)
            // The Microsoft extension code indicated when Windows Parental Controls are turned on and are blocking access to the requested webpage.
            BLOCKED_BY_WINDOWS_PARENTAL_CONTROL = 450,

            // (RFC 7725)
            // A server operator has received a legal demand to deny access to a resource or to a set of resources that includes the requested resource.
            UNAVAILABLE_FOR_LEGAL_REASONS = 451,

            // Client closed the connection with the load balancer before the idle timeout period elapsed.
            ELB460 = 460,

            // The load balancer received an X-Forwarded-For request header with more than 30 IP addresses.
            ELB463 = 463,

            // An error around authentication returned by a server registered with a load balancer.
            ELBUNAUTHORIZED = 561,

            // Client sent too large request or too long header line.
            REQUEST_HEADER_TOO_LARGE = 494,

            // An expansion of the 400 Bad Request response code, used when the client has provided an invalid client certificate.
            SSL_CERTIFICATE_ERROR = 495,

            // An expansion of the 400 Bad Request response code, used when a client certificate is required but not provided.
            SSL_CERTIFICATE_REQUIRED = 496,

            // An expansion of the 400 Bad Request response code, used when the client has made a HTTP request to a port listening for HTTPS requests.
            HTTP_REQUEST_SENT_TO_HTTPS_PORT = 497,

            // Used when the client has closed the request before the server could send a response.
            CLIENT_CLOSED_REQUEST = 499,



            // >>> 500 <<<
            // A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.
            INTERNAL_SERVER_ERROR = 500,

            // The server either does not recognize the request method, or it lacks the ability to fulfil the request.
            NOT_IMPLEMENTED = 501,

            // The server was acting as a gateway or proxy and received an invalid response from the upstream server.
            BAD_GATEWAY = 502,

            // The server cannot handle the request.
            SERVICE_UNAVAILABLE = 503,

            // The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.
            GATEWAY_TIMEOUT = 504,

            // The server does not support the HTTP protocol version used in the request.
            HTTP_VERSION_NOT_SUPPORTED = 505,

            // (RFC 2295)
            // Transparent content negotiation for the request results in a circular reference.
            //
            //  Circular Reference Example
            //   def posn(k: int) -> int:
            //       if k < 0:
            //           return plus1(k)
            //       return k
            VARIANT_ALSO_NEGOTIATES = 506,

            // (WebDAV; RFC 4918)
            // The server is unable to store the representation needed to complete the request.
            INSUFFICIENT_STORAGE = 507,

            // (WebDAV; RFC 5842)
            // The server detected an infinite loop while processing the request.
            LOOP_DETECTED = 508,

            // (Apache Web Server/cPanel)
            // The server has exceeded the bandwidth specified by the server administrator.
            BANDWIDTH_LIMIT_EXCEEDED = 509,

            // (RFC 2774)
            // Further extensions to the request are required for the server to fulfil it.
            NOT_EXTENDED = 510,

            // (RFC 6585)
            // The client needs to authenticate to gain network access.
            NETWORK_AUTHENTICATION_REQUIRED = 511,

            // The origin server returned an empty, unknown, or unexpected response to Cloudflare.
            WEB_SERVER_RETURNED_AN_UNKNOWN_ERROR = 520,

            // The origin server refused connections from Cloudflare.
            WEB_SERVER_IS_DOWN = 521,

            // Cloudflare timed out contacting the origin server.
            CONNECTION_TIMED_OUT = 522,

            // Cloudflare could not reach the origin server.
            ORIGIN_IS_UNREACHABLE = 523,

            // Cloudflare was able to complete a TCP connection to the origin server, but did not receive a timely HTTP response.
            A_TIMEOUT_OCCURED = 524,

            // Cloudflare could not negotiate a SSL/TLS handshake with the origin server.
            SSL_HANDSHAKE_FAILED = 525,

            // Cloudflare could not validate the SSL certificate on the origin web server. Also used by Cloud Foundry's gorouter.
            INVALID_SSL_CERTIFICATE = 526,

            // Error 527 indicates an interrupted connection between Cloudflare and the origin server's Railgun server.
            RAILGUN_ERROR = 527,

            // Used by Qualys in the SSLLabs server testing API to signal that the site can't process the request.
            SITE_IS_OVERLOADED = 529,

            // Error 530 is returned along with a 1xxx error.
            С530 = 530,

            // (Informal convention)
            // Used by some HTTP proxies to signal a network read timeout behind the proxy to a client in front of the proxy.
            NETWORK_READ_TIMEOUT_ERROR = 598,

            // An error used by some HTTP proxies to signal a network connect timeout behind the proxy to a client in front of the proxy.
            NETWORK_CONNECT_TIMEOUT_ERROR = 599
        }
        public class SE_ContentType
        {
            public static readonly string application_java_archive = "application/java-archive";
            public static readonly string application_EDI_X12 = "application/EDI-X12";
            public static readonly string application_EDIFACT = "application/EDIFACT";
            public static readonly string application_javascript = "application/javascript";
            public static readonly string application_octet_stream = "application/octet-stream";
            public static readonly string application_ogg = "application/ogg";
            public static readonly string application_pdf = "application/pdf";
            public static readonly string application_xhtml_xml = "application/xhtml+xml";
            public static readonly string application_x_shockwave_flash = "application/x-shockwave-flash";
            public static readonly string application_json = "application/json";
            public static readonly string application_ld_json = "application/ld+json";
            public static readonly string application_xml = "application/xml";
            public static readonly string application_zip = "application/zip";
            public static readonly string application_x_www_form_urlencoded = "application/x-www-form-urlencoded";
            public static readonly string audio_mpeg = "audio/mpeg";
            public static readonly string audio_x_ms_wma = "audio/x-ms-wma";
            public static readonly string audio_vnd_rn_realaudio = "audio/vnd.rn-realaudio";
            public static readonly string audio_x_wav = "audio/x-wav";
            public static readonly string image_gif = "image/gif";
            public static readonly string image_jpeg = "image/jpeg";
            public static readonly string image_png = "image/png";
            public static readonly string image_tiff = "image/tiff";
            public static readonly string image_vnd_microsoft_icon = "image/vnd.microsoft.icon";
            public static readonly string image_x_icon = "image/x-icon";
            public static readonly string image_vnd_djvu = "image/vnd.djvu";
            public static readonly string image_svg_xml = "image/svg+xml";
            public static readonly string multipart_mixed = "multipart/mixed";
            public static readonly string multipart_alternative = "multipart/alternative";
            public static readonly string multipart_related = "multipart/related(usingbyMHTML(HTMLmail).)";
            public static readonly string multipart_form_data = "multipart/form-data";
            public static readonly string text_css = "text/css";
            public static readonly string text_csv = "text/csv";
            public static readonly string text_html = "text/html";
            public static readonly string text_javascript = "text/javascript(obsolete)";
            public static readonly string text_plain = "text/plain";
            public static readonly string text_xml = "text/xml";
            public static readonly string video_mpeg = "video/mpeg";
            public static readonly string video_mp4 = "video/mp4";
            public static readonly string video_quicktime = "video/quicktime";
            public static readonly string video_x_ms_wmv = "video/x-ms-wmv";
            public static readonly string video_x_msvideo = "video/x-msvideo";
            public static readonly string video_x_flv = "video/x-flv";
            public static readonly string video_webm = "video/webm";
            public static readonly string application_vnd_android_package_archive = "application/vnd.android.package-archive";
            public static readonly string application_vnd_oasis_opendocument_text = "application/vnd.oasis.opendocument.text";
            public static readonly string application_vnd_oasis_opendocument_spreadsheet = "application/vnd.oasis.opendocument.spreadsheet";
            public static readonly string application_vnd_oasis_opendocument_presentation = "application/vnd.oasis.opendocument.presentation";
            public static readonly string application_vnd_oasis_opendocument_graphics = "application/vnd.oasis.opendocument.graphics";
            public static readonly string application_vnd_ms_excel = "application/vnd.ms-excel";
            public static readonly string application_vnd_openxmlformats_officedocument_spreadsheetml_sheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public static readonly string application_vnd_ms_powerpoint = "application/vnd.ms-powerpoint";
            public static readonly string application_vnd_openxmlformats_officedocument_presentationml_presentation = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            public static readonly string application_msword = "application/msword";
            public static readonly string application_vnd_openxmlformats_officedocument_wordprocessingml_document = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            public static readonly string application_vnd_mozilla_xul_xml = "application/vnd.mozilla.xul+xml";
        }
        public class SE_UserAgent
        {
            public static readonly string Self = System.AppDomain.CurrentDomain.FriendlyName;
                           
            public static readonly string GoogleBot = "Googlebot/2.1 (+http://www.google.com/bot.html)";
            public static readonly string AppleTV = "AppleTV6,2/11.1";
                           
            public static readonly string Opera_Windows = "Opera/9.80 (Windows NT 6.2; Win64; x64) Presto/2.12.388 Version/12.16";
            public static readonly string Opera_Linux = "Opera/10.00 (X11; Linux i686; U; en) Presto/2.2.0";
            public static readonly string Opera_Mini = "Opera/10.61 (J2ME/MIDP; Opera Mini/5.1.21219/19.999; en-US; rv:1.9.3a5) WebKit/534.5 Presto/2.6.30";
                           
            public static readonly string Mozilla_Windows = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0";
            public static readonly string Mozilla_Linux = "Mozilla/5.0 (X11; Linux x86_64; rv:70.0) Gecko/20100101 Firefox/70.0";
            public static readonly string Mozilla_Ubuntu = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:70.0) Gecko/20100101 Firefox/70.0";
            public static readonly string Mozilla_Android = "Mozilla/5.0 (Linux; Android 10; AKA-L29 Build/HONORAKA-L29; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.108 Mobile Safari/537.36";
            public static readonly string Mozilla_Legacy_Windows = "Mozilla/1.22 (compatible; MSIE 2.0; Windows 3.1)";
            public static readonly string Mozilla_Legacy_Linux = "Mozilla/1.22 (compatible; Konqueror/4.3; Linux) KHTML/4.3.5 (like Gecko)";
        }
        public struct S_Response
        {
            public E_StatusCode statusCode { get; }
            public List<string> headers { get; }
            public string content { get; }

            public S_Response(E_StatusCode statusCode, List<string> headers, string content)
            {
                this.statusCode = statusCode;
                this.headers = headers;
                this.content = content;
            }
        }


        public static S_Response Get(string url, List<string> headers, string data = null, int timeout = 10)
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = "GET";

            foreach (var header in headers)
            {
                string[] parts = header.Split(':');
                if (parts.Length == 2)
                {
                    switch (parts[0])
                    {
                        case "User-Agent":
                            httpRequest.UserAgent = parts[1];
                            break;

                        case "Content-Type":
                            httpRequest.ContentType = parts[1];
                            break;

                        default:
                            httpRequest.Headers.Add(parts[0], parts[1]);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            httpRequest.ReadWriteTimeout = timeout.ToSeconds();

            try
            {
                using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)httpResponse.StatusCode;
                        List<string> responseHeaders = httpResponse.Headers?.ToList();
                        string responseContent = reader.ReadToEnd();

                        return new S_Response(statusCode, responseHeaders, responseContent);
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                return new S_Response(E_StatusCode.NONE, null, null);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    HttpStatusCode httpStatusCode = errorResponse.StatusCode;
                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)(int)httpStatusCode;
                        List<string> errorHeaders = errorResponse.Headers?.ToList();
                        string errorContent = reader.ReadToEnd();

                        return new S_Response(statusCode, errorHeaders, errorContent);
                    }
                }
                else throw;
            }
            catch (Exception e)
            {
                return new S_Response(E_StatusCode.UNDEFINED_ERROR, null, e.Message);
            }

        }


        public static S_Response Post(string url, List<string> headers, string data = null, int timeout = 10)
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = "POST";

            foreach (var header in headers)
            {
                string[] parts = header.Split(':');
                if (parts.Length == 2)
                {
                    switch (parts[0])
                    {
                        case "User-Agent":
                            httpRequest.UserAgent = parts[1];
                            break;

                        case "Content-Type":
                            httpRequest.ContentType = parts[1];
                            break;

                        default:
                            httpRequest.Headers.Add(parts[0], parts[1]);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            httpRequest.ReadWriteTimeout = timeout.ToSeconds();

            try
            {
                using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)httpResponse.StatusCode;
                        List<string> responseHeaders = httpResponse.Headers?.ToList();
                        string responseContent = reader.ReadToEnd();

                        return new S_Response(statusCode, responseHeaders, responseContent);
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                return new S_Response(E_StatusCode.NONE, null, null);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    HttpStatusCode httpStatusCode = errorResponse.StatusCode;
                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)(int)httpStatusCode;
                        List<string> errorHeaders = errorResponse.Headers?.ToList();
                        string errorContent = reader.ReadToEnd();

                        return new S_Response(statusCode, errorHeaders, errorContent);
                    }
                }
                else throw;
            }
            catch (Exception e)
            {
                return new S_Response(E_StatusCode.UNDEFINED_ERROR, null, e.Message);
            }

        }


        public static S_Response Download(string url, List<string> headers, string data = null, string destinationPath = null, int timeout = 900)
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp(url);
            httpRequest.Method = "GET";

            foreach (var header in headers)
            {
                string[] parts = header.Split(':');
                if (parts.Length == 2)
                {
                    switch (parts[0])
                    {
                        case "User-Agent":
                            httpRequest.UserAgent = parts[1];
                            break;

                        case "Content-Type":
                            httpRequest.ContentType = parts[1];
                            break;

                        default:
                            httpRequest.Headers.Add(parts[0], parts[1]);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            httpRequest.ReadWriteTimeout = timeout.ToSeconds();

            try
            {
                using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    using (Stream responseStream = httpResponse.GetResponseStream())
                    {

                        bool destinationPathIsEmpty = string.IsNullOrEmpty(destinationPath);
                        string downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string timeStampString = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();


                        Uri uri = new Uri(url);
                        string uriFileName = Path.GetFileName(uri.LocalPath);

                        if (uriFileName != null)
                        {
                            if (destinationPathIsEmpty)
                            {
                                destinationPath = Path.Combine(downloadsFolder, uriFileName);
                            }
                            else
                            {
                                destinationPath = Path.Combine(destinationPath, uriFileName);
                            }
                        }
                        else
                        {
                            if (destinationPathIsEmpty)
                            {
                                destinationPath = Path.Combine(downloadsFolder, timeStampString);
                            }
                            else
                            {
                                destinationPath = Path.Combine(destinationPath, timeStampString);
                            }
                        }

                        if (File.Exists(destinationPath))
                        {
                            for (int index = 1; index <= 1000; index++)
                            {
                                if (index == 1000)
                                {
                                    destinationPath = destinationPath.Replace(uriFileName, $"[{timeStampString}] {uriFileName}");
                                    break;
                                }
                                else
                                {
                                    string suggestedDestinationPath = destinationPath.Replace(uriFileName, $"[{index}] {uriFileName}");
                                    if (!File.Exists(suggestedDestinationPath))
                                    {
                                        destinationPath = suggestedDestinationPath;
                                        break;
                                    }
                                }
                            }
                        }
                     
                        using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                        {
                            responseStream.CopyTo(fileStream);
                        }

                        E_StatusCode statusCode = (E_StatusCode)httpResponse.StatusCode;
                        List<string> responseHeaders = httpResponse.Headers?.ToList();
                        return new S_Response(statusCode, responseHeaders, destinationPath);
                    }
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                return new S_Response(E_StatusCode.NONE, null, null);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    HttpStatusCode httpStatusCode = errorResponse.StatusCode;
                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)(int)httpStatusCode;
                        List<string> errorHeaders = errorResponse.Headers?.ToList();
                        string errorContent = reader.ReadToEnd();

                        return new S_Response(statusCode, errorHeaders, errorContent);
                    }
                }
                else throw;
            }
            catch (Exception e)
            {
                return new S_Response(E_StatusCode.UNDEFINED_ERROR, null, e.Message);
            }

        }
    }




    internal static class NetworkingLogic
    {
        internal static List<string> ToList(this WebHeaderCollection headers)
        {
            List<string> headerList = new List<string>();

            foreach (string key in headers.Keys)
            {
                headerList.Add($"{key}: {headers[key]}");
            }

            return headerList;
        }

        internal static int ToSeconds(this int milliseconds)
        {
            return milliseconds * 1000;
        }
    }
}
