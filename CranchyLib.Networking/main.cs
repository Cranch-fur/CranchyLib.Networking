using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CranchyLib.Networking
{
    public static class Networking
    {
        /// <summary>
        /// Default amount of time (in seconds) GET / POST request awaits for response.
        /// </summary>
        public const double DEFAULT_TIMEOUT = 10.0;
        /// <summary>
        /// Default amount of time (in seconds) DOWNLOAD request awaits for response.
        /// </summary>
        public const double DEFAULT_DOWNLOAD_TIMEOUT = 600.0;




        /// <summary>
        /// 1xx --> Informational Response (the request was received, continuing process)
        /// 2xx --> Successful (the request was successfully received, understood, and accepted)
        /// 3xx --> Redirection (further action needs to be taken in order to complete the request)
        /// 4xx --> Client Error (the request contains bad syntax or cannot be fulfilled)
        /// 5xx --> Server Error (the server failed to fulfil an apparently valid request)
        /// <see href="https://en.wikipedia.org/wiki/List_of_HTTP_status_codes">Learn More</see>
        /// </summary>
        public enum E_StatusCode : int
        {
            // >>> CUSTOM <<<
            /// <summary>
            /// Negative value indicating that something went wrong. Usually used in context of an unhandled exception.
            /// </summary>
            UNDEFINED_ERROR = -1,
            /// <summary>
            /// Default value indicating that there's nothing to return. Usually used in context of request timeout.
            /// </summary>
            NONE = 0,



            // >>> 100 <<<
            /// <summary>
            /// The server has received the request headers and the client should proceed to send the request body.
            /// </summary>
            CONTINUE = 100,

            /// <summary>
            /// The requester has asked the server to switch protocols and the server has agreed to do so.
            /// </summary>
            SWITCHING_PROTOCOLS = 101,

            /// <summary>
            /// (WebDAV; RFC 2518) This code indicates that the server has received and is processing the request, but no response is available yet.
            /// </summary>
            PROCESSING = 102,

            /// <summary>
            /// Used to return some response headers before final HTTP message.
            /// </summary>
            EARLY_HINTS = 103,

            /// <summary>
            /// The response provided by a cache is stale (the content's age exceeds a maximum age set by a Cache-Control header or heuristically chosen lifetime).
            /// </summary>
            RESPONSE_IS_STALE = 110,

            /// <summary>
            /// The cache was unable to validate the response, due to an inability to reach the origin server.
            /// </summary>
            REVALIDATION_FAILED = 111,

            /// <summary>
            /// The cache is intentionally disconnected from the rest of the network.
            /// </summary>
            DISCONNECTED_OPERATION = 112,

            /// <summary>
            /// The cache heuristically chose a freshness lifetime greater than 24 hours and the response's age is greater than 24 hours.
            /// </summary>
            HEURISTIC_EXPIRATION = 113,

            /// <summary>
            /// Arbitrary, non-specific warning. The warning text may be logged or presented to the user.
            /// </summary>
            MISCELLANEOUS_WARNING = 199,



            // >>> 200 <<<
            /// <summary>
            /// Standard response for successful HTTP requests.
            /// </summary>
            OK = 200,

            /// <summary>
            /// The request has been fulfilled, resulting in the creation of a new resource.
            /// </summary>
            CREATED = 201,

            /// <summary>
            /// The request has been accepted for processing, but the processing has not been completed.
            /// </summary>
            ACCEPTED = 202,

            /// <summary>
            /// (since HTTP/1.1) The server is a transforming proxy (e.g. a Web accelerator) that received a 200 OK from its origin, but is returning a modified version of the origin's response.
            /// </summary>
            NON_AUTHORITATIVE_INFORMATION = 203,

            /// <summary>
            /// The server successfully processed the request, and is not returning any content.
            /// </summary>
            NO_CONTENT = 204,

            /// <summary>
            /// The server successfully processed the request, asks that the requester reset its document view, and is not returning any content.
            /// </summary>
            RESET_CONTENT = 205,

            /// <summary>
            /// (RFC 7233) The server is delivering only part of the resource (byte serving) due to a range header sent by the client. The range header is used by HTTP clients to enable resuming of interrupted downloads, or split a download into multiple simultaneous streams.
            /// </summary>
            PARTIAL_CONTENT = 206,

            /// <summary>
            /// (WebDAV; RFC 4918) The message body that follows is by default an XML message and can contain a number of separate response codes, depending on how many sub-requests were made.
            /// </summary>
            MULTI_STATUS = 207,

            /// <summary>
            /// (WebDAV; RFC 5842) The members of a DAV binding have already been enumerated in a preceding part of the (multistatus) response, and are not being included again.
            /// </summary>
            ALREADY_REPORTED = 208,

            /// <summary>
            /// Added by a proxy if it applies any transformation to the representation, such as changing the content encoding, media type or the like.
            /// </summary>
            TRANSFORMATION_APPLIED = 214,

            /// <summary>
            /// Same as 199 (MISCELLANEOUS_WARNING), but indicating a persistent warning.
            /// </summary>
            MISCELLANEOUS_PERSISTENT_WARNING = 299,

            /// <summary>
            /// (RFC 3229) The server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
            /// </summary>
            IM_USED = 226,



            // >>> 300 <<<
            /// <summary>
            /// Indicates multiple options for the resource from which the client may choose.
            /// </summary>
            MULTIPLE_CHOISES = 300,

            //       URI -> Uniform Resource Identifier
            //          userinfo       host      port
            //          ┌──┴───┐ ┌──────┴──────┐ ┌┴┐
            //  https://john.doe@www.example.com:123/forum/questions/?tag=networking&order=newest#top
            //  └─┬─┘   └───────────┬──────────────┘└───────┬───────┘ └───────────┬─────────────┘ └┬┘
            //  scheme          authority                  path                 query           fragment
            /// <summary>
            /// This and all future requests should be directed to the given URI.
            /// </summary>
            MOVED_PERMANENTLY = 301,

            /// <summary>
            /// (Previously "Moved temporarily") Tells the client to look at (browse to) another URL.
            /// </summary>
            FOUND = 302,

            /// <summary>
            /// (since HTTP/1.1) The response to the request can be found under another URI using the GET method. When received in response to a POST (or PUT/DELETE), the client should presume that the server has received the data and should issue a new GET request to the given URI.
            /// </summary>
            SEE_OTHER = 303,

            /// <summary>
            /// (RFC 7232) Indicates that the resource has not been modified since the version specified by the request headers If-Modified-Since or If-None-Match.
            /// </summary>
            NOT_MODIFIED = 304,

            /// <summary>
            /// (since HTTP/1.1) The requested resource is available only through a proxy, the address for which is provided in the response.
            /// </summary>
            USE_PROXY = 305,

            /// <summary>
            /// No longer used. Originally meant "Subsequent requests should use the specified proxy."
            /// </summary>
            SWITCH_PROXY = 306,

            /// <summary>
            /// (since HTTP/1.1) In this case, the request should be repeated with another URI; however, future requests should still use the original URI.
            /// </summary>
            TEMPORARY_REDIRECT = 307,

            /// <summary>
            /// (RFC 7538) This and all future requests should be directed to the given URI.
            /// </summary>
            PERMANENT_REDIRECT = 308,



            // >>> 400 <<<
            /// <summary>
            /// The server cannot or will not process the request due to an apparent client error (e.g., malformed request syntax, size too large, invalid request message framing, or deceptive request routing).
            /// </summary>
            BAD_REQUEST = 400,

            /// <summary>
            /// (RFC 7235) Similar to 403 Forbidden, but specifically for use when authentication is required and has failed or has not yet been provided.
            /// </summary>
            UNAUTHORIZED = 401,

            /// <summary>
            /// Reserved for future use.
            /// </summary>
            PAYMENT_REQUIRED = 402,

            // 
            /// <summary>
            /// The request contained valid data and was understood by the server, but the server is refusing action.
            /// </summary>
            FORBIDDEN = 403,

            /// <summary>
            /// The requested resource could not be found but may be available in the future.
            /// </summary>
            NOT_FOUND = 404,

            /// <summary>
            /// A request method is not supported for the requested resource; for example, a GET request on a form that requires data to be presented via POST, or a PUT request on a read-only resource.
            /// </summary>
            METHOD_NOT_ALLOWED = 405,

            /// <summary>
            /// The requested resource is capable of generating only content not acceptable according to the Accept headers sent in the request.
            /// </summary>
            NOT_ACCEPTABLE = 406,

            /// <summary>
            /// (RFC 7235) The client must first authenticate itself with the proxy.
            /// </summary>
            PROXY_AUTHENTICATION_REQUIRED = 407,

            /// <summary>
            /// The server timed out waiting for the request. 
            /// </summary>
            REQUEST_TIMEOUT = 408,

            /// <summary>
            /// Indicates that the request could not be processed because of conflict in the current state of the resource, such as an edit conflict between multiple simultaneous updates.
            /// </summary>
            CONFLICT = 409,

            /// <summary>
            /// Indicates that the resource requested is no longer available and will not be available again.
            /// </summary>
            GONE = 410,

            /// <summary>
            /// The request did not specify the length of its content, which is required by the requested resource.
            /// </summary>
            LENGTH_REQUIRED = 411,

            /// <summary>
            /// (RFC 7232) The server does not meet one of the preconditions that the requester put on the request header fields.
            /// </summary>
            PRECONDITION_FAILED = 412,

            /// <summary>
            /// (RFC 7231) The request is larger than the server is willing or able to process.
            /// </summary>
            PAYLOAD_TOO_LARGE = 413,

            /// <summary>
            /// (RFC 7231) The URI provided was too long for the server to process.
            /// </summary>
            URI_TOO_LONG = 414,

            /// <summary>
            /// (RFC 7231) The request entity has a media type which the server or resource does not support.
            /// </summary>
            UNSUPPORTED_MEDIA_TYPE = 415,

            /// <summary>
            /// (RFC 7233) The client has asked for a portion of the file (byte serving), but the server cannot supply that portion.
            /// </summary>
            RANGE_NOT_SATISFIABLE = 416,

            /// <summary>
            /// The server cannot meet the requirements of the Expect request-header field.
            /// </summary>
            EXPECTATION_FAILED = 417,

            /// <summary>
            /// (RFC 2324, RFC 7168) This code was defined in 1998 as one of the traditional IETF April Fools' jokes.
            /// </summary>
            IM_A_TEAPOT = 418,

            /// <summary>
            /// (Laravel Framework)  Used by the Laravel Framework when a CSRF Token is missing or expired.
            /// </summary>
            PAGE_EXPIRED = 419,

            /// <summary>
            /// (Twitter) Returned by version 1 of the Twitter Search and Trends API when the client is being rate limited.
            /// </summary>
            ENCHANCE_YOUR_CALM = 420,

            /// <summary>
            /// (RFC 7540) The request was directed at a server that is not able to produce a response.
            /// </summary>
            MISDIRECTED_REQUEST = 421,

            /// <summary>
            /// (WebDAV; RFC 4918) The request was well-formed but was unable to be followed due to semantic errors.
            /// </summary>
            UNPROCESSABLE_ENTITY = 422,

            /// <summary>
            /// (WebDAV; RFC 4918) The resource that is being accessed is locked.
            /// </summary>
            LOCKED = 423,

            /// <summary>
            /// (WebDAV; RFC 4918) The request failed because it depended on another request and that request failed.
            /// </summary>
            FAILED_DEPENDECY = 424,

            /// <summary>
            /// (RFC 8470) Indicates that the server is unwilling to risk processing a request that might be replayed.
            /// </summary>
            TOO_EARLY = 425,

            /// <summary>
            /// The client should switch to a different protocol such as TLS/1.3, given in the Upgrade header field.
            /// </summary>
            UPGRADE_REQUIRED = 426,

            /// <summary>
            /// (RFC 6585) The origin server requires the request to be conditional.
            /// </summary>
            PRECONDITION_REQUIRED = 428,

            /// <summary>
            /// (RFC 6585) The user has sent too many requests in a given amount of time.
            /// </summary>
            TOO_MANY_REQUESTS = 429,

            /// <summary>
            /// (RFC 6585) The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large.
            /// </summary>
            REQUEST_HEADER_FIELDS_TOO_LARGE = 431,

            /// <summary>
            /// The client's session has expired and must log in again.
            /// </summary>
            LOGIN_TIMEOUT = 440,

            /// <summary>
            /// Used internally to instruct the server to return no information to the client and close the connection immediately.
            /// </summary>
            NO_RESPONSE = 444,

            /// <summary>
            /// The server cannot honour the request because the user has not provided the required information.
            /// </summary>
            RETRY_WITH = 449,

            /// <summary>
            /// (Microsoft) The Microsoft extension code indicated when Windows Parental Controls are turned on and are blocking access to the requested webpage.
            /// </summary>
            BLOCKED_BY_WINDOWS_PARENTAL_CONTROL = 450,

            /// <summary>
            /// (RFC 7725) A server operator has received a legal demand to deny access to a resource or to a set of resources that includes the requested resource.
            /// </summary>
            UNAVAILABLE_FOR_LEGAL_REASONS = 451,

            /// <summary>
            /// Client closed the connection with the load balancer before the idle timeout period elapsed.
            /// </summary>
            ELB460 = 460,

            /// <summary>
            /// The load balancer received an X-Forwarded-For request header with more than 30 IP addresses.
            /// </summary>
            ELB463 = 463,

            /// <summary>
            /// An error around authentication returned by a server registered with a load balancer.
            /// </summary>
            ELBUNAUTHORIZED = 561,

            /// <summary>
            /// Client sent too large request or too long header line.
            /// </summary>
            REQUEST_HEADER_TOO_LARGE = 494,

            /// <summary>
            /// An expansion of the 400 Bad Request response code, used when the client has provided an invalid client certificate.
            /// </summary>
            SSL_CERTIFICATE_ERROR = 495,

            /// <summary>
            /// An expansion of the 400 Bad Request response code, used when a client certificate is required but not provided.
            /// </summary>
            SSL_CERTIFICATE_REQUIRED = 496,

            /// <summary>
            /// An expansion of the 400 Bad Request response code, used when the client has made a HTTP request to a port listening for HTTPS requests.
            /// </summary>
            HTTP_REQUEST_SENT_TO_HTTPS_PORT = 497,

            /// <summary>
            /// Used when the client has closed the request before the server could send a response.
            /// </summary>
            CLIENT_CLOSED_REQUEST = 499,



            // >>> 500 <<<
            /// <summary>
            /// A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.
            /// </summary>
            INTERNAL_SERVER_ERROR = 500,

            /// <summary>
            /// The server either does not recognize the request method, or it lacks the ability to fulfil the request.
            /// </summary>
            NOT_IMPLEMENTED = 501,

            /// <summary>
            /// The server was acting as a gateway or proxy and received an invalid response from the upstream server.
            /// </summary>
            BAD_GATEWAY = 502,

            /// <summary>
            /// The server cannot handle the request.
            /// </summary>
            SERVICE_UNAVAILABLE = 503,

            /// <summary>
            /// The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.
            /// </summary>
            GATEWAY_TIMEOUT = 504,

            /// <summary>
            /// The server does not support the HTTP protocol version used in the request.
            /// </summary>
            HTTP_VERSION_NOT_SUPPORTED = 505,

            //  Circular Reference Example
            //   def posn(k: int) -> int:
            //       if k < 0:
            //           return plus1(k)
            //       return k
            /// <summary>
            /// (RFC 2295) Transparent content negotiation for the request results in a circular reference.
            /// </summary>
            VARIANT_ALSO_NEGOTIATES = 506,

            /// <summary>
            /// (WebDAV; RFC 4918) The server is unable to store the representation needed to complete the request.
            /// </summary>
            INSUFFICIENT_STORAGE = 507,

            /// <summary>
            /// (WebDAV; RFC 5842) The server detected an infinite loop while processing the request.
            /// </summary>
            LOOP_DETECTED = 508,

            /// <summary>
            /// (Apache Web Server/cPanel) The server has exceeded the bandwidth specified by the server administrator.
            /// </summary>
            BANDWIDTH_LIMIT_EXCEEDED = 509,

            /// <summary>
            /// (RFC 2774) Further extensions to the request are required for the server to fulfil it.
            /// </summary>
            NOT_EXTENDED = 510,

            /// <summary>
            /// (RFC 6585) The client needs to authenticate to gain network access.
            /// </summary>
            NETWORK_AUTHENTICATION_REQUIRED = 511,

            /// <summary>
            /// The origin server returned an empty, unknown, or unexpected response to Cloudflare.
            /// </summary>
            WEB_SERVER_RETURNED_AN_UNKNOWN_ERROR = 520,

            /// <summary>
            /// The origin server refused connections from Cloudflare.
            /// </summary>
            WEB_SERVER_IS_DOWN = 521,

            /// <summary>
            /// Cloudflare timed out contacting the origin server.
            /// </summary>
            CONNECTION_TIMED_OUT = 522,

            /// <summary>
            /// Cloudflare could not reach the origin server.
            /// </summary>
            ORIGIN_IS_UNREACHABLE = 523,

            /// <summary>
            /// Cloudflare was able to complete a TCP connection to the origin server, but did not receive a timely HTTP response.
            /// </summary>
            A_TIMEOUT_OCCURED = 524,

            /// <summary>
            /// Cloudflare could not negotiate a SSL/TLS handshake with the origin server.
            /// </summary>
            SSL_HANDSHAKE_FAILED = 525,

            /// <summary>
            /// Cloudflare could not validate the SSL certificate on the origin web server. Also used by Cloud Foundry's gorouter.
            /// </summary>
            INVALID_SSL_CERTIFICATE = 526,

            /// <summary>
            /// Error 527 indicates an interrupted connection between Cloudflare and the origin server's Railgun server.
            /// </summary>
            RAILGUN_ERROR = 527,

            /// <summary>
            /// Used by Qualys in the SSLLabs server testing API to signal that the site can't process the request.
            /// </summary>
            SITE_IS_OVERLOADED = 529,

            /// <summary>
            ///  Error 530 is returned along with a 1xxx error.
            /// </summary>
            С530 = 530,

            /// <summary>
            /// (Informal convention) Used by some HTTP proxies to signal a network read timeout behind the proxy to a client in front of the proxy.
            /// </summary>
            NETWORK_READ_TIMEOUT_ERROR = 598,

            /// <summary>
            /// An error used by some HTTP proxies to signal a network connect timeout behind the proxy to a client in front of the proxy.
            /// </summary>
            NETWORK_CONNECT_TIMEOUT_ERROR = 599
        }




        /// <summary>
        /// Default set of content-type headers.
        /// </summary>
        public class SE_ContentType
        {
            /// <summary>
            /// Java archive files, often used for distributing Java applications.
            /// </summary>
            public const string application_java_archive          = "application/java-archive";
            /// <summary>
            /// Format for electronic data interchange (EDI) used in business transactions.
            /// </summary>
            public const string application_EDI_X12               = "application/EDI-X12";
            /// <summary>
            /// Standard for EDI used in international trade.
            /// </summary>
            public const string application_EDIFACT               = "application/EDIFACT";
            /// <summary>
            /// JavaScript files used to execute scripts on web pages.
            /// </summary>
            public const string application_javascript            = "application/javascript";
            /// <summary>
            /// Generic binary data, often used for arbitrary file types.
            /// </summary>
            public const string application_octet_stream          = "application/octet-stream";
            /// <summary>
            /// Container format for multimedia, including audio and video.
            /// </summary>
            public const string application_ogg                   = "application/ogg";
            /// <summary>
            /// Portable Document Format, used for documents that preserve formatting.
            /// </summary>
            public const string application_pdf                   = "application/pdf";
            /// <summary>
            /// XHTML documents, an XML-based version of HTML.
            /// </summary>
            public const string application_xhtml_xml             = "application/xhtml+xml";
            /// <summary>
            /// Format for multimedia, especially web animations (obsolete).
            /// </summary>
            public const string application_x_shockwave_flash     = "application/x-shockwave-flash";
            /// <summary>
            /// Data format used for structured data exchange.
            /// </summary>
            public const string application_json                  = "application/json";
            /// <summary>
            /// JSON format used for linked data in web applications.
            /// </summary>
            public const string application_ld_json               = "application/ld+json";
            /// <summary>
            /// General XML data files.
            /// </summary>
            public const string application_xml                   = "application/xml";
            /// <summary>
            /// Compressed archive files containing one or more files.
            /// </summary>
            public const string application_zip                   = "application/zip";
            /// <summary>
            /// Format for sending form data in HTTP requests.
            /// </summary>
            public const string application_x_www_form_urlencoded = "application/x-www-form-urlencoded";



            /// <summary>
            /// MPEG audio files, commonly known as MP3.
            /// </summary>
            public const string audio_mpeg             = "audio/mpeg";
            /// <summary>
            /// Windows Media Audio format.
            /// </summary>
            public const string audio_x_ms_wma         = "audio/x-ms-wma";
            /// <summary>
            /// RealAudio format for streaming audio.
            /// </summary>
            public const string audio_vnd_rn_realaudio = "audio/vnd.rn-realaudio";
            /// <summary>
            /// Waveform Audio File Format, uncompressed audio.
            /// </summary>
            public const string audio_x_wav            = "audio/x-wav";



            /// <summary>
            /// Graphics Interchange Format, supporting animations.
            /// </summary>
            public const string image_gif                = "image/gif";
            /// <summary>
            /// Compressed image format for digital photos.
            /// </summary>
            public const string image_jpeg               = "image/jpeg";
            /// <summary>
            /// Lossless image format, widely used for web graphics.
            /// </summary>
            public const string image_png                = "image/png";
            /// <summary>
            /// High-quality image format, often used in publishing.
            /// </summary>
            public const string image_tiff               = "image/tiff";
            /// <summary>
            /// Format for Windows icons.
            /// </summary>
            public const string image_vnd_microsoft_icon = "image/vnd.microsoft.icon";
            /// <summary>
            /// Icon file format, used for web favicons.
            /// </summary>
            public const string image_x_icon             = "image/x-icon";
            /// <summary>
            /// Format for scanned documents with high compression.
            /// </summary>
            public const string image_vnd_djvu           = "image/vnd.djvu";
            /// <summary>
            /// Scalable Vector Graphics, an XML-based vector image format.
            /// </summary>
            public const string image_svg_xml            = "image/svg+xml";



            /// <summary>
            /// Used for multipart email content combining different media types.
            /// </summary>
            public const string multipart_mixed       = "multipart/mixed";
            /// <summary>
            /// Email format containing the same content in different forms (e.g., plain text and HTML).
            /// </summary>
            public const string multipart_alternative = "multipart/alternative";
            /// <summary>
            /// Email format for sending related data, such as inline images in HTML.
            /// </summary>
            public const string multipart_related     = "multipart/related(usingbyMHTML(HTMLmail).)";
            /// <summary>
            /// Used for forms that upload files via HTTP.
            /// </summary>
            public const string multipart_form_data   = "multipart/form-data";



            /// <summary>
            /// Cascading Style Sheets, used for styling web pages.
            /// </summary>
            public const string text_css        = "text/css";
            /// <summary>
            /// Comma-separated values, a format for tabular data.
            /// </summary>
            public const string text_csv        = "text/csv";
            /// <summary>
            /// Standard markup language for web pages.
            /// </summary>
            public const string text_html       = "text/html";
            /// <summary>
            /// Legacy MIME type for JavaScript scripts (obsolete).
            /// </summary>
            public const string text_javascript = "text/javascript(obsolete)";
            /// <summary>
            /// Plain text files without formatting.
            /// </summary>
            public const string text_plain      = "text/plain";
            /// <summary>
            /// XML text files.
            /// </summary>
            public const string text_xml        = "text/xml";



            /// <summary>
            /// MPEG video format, an early standard for video compression.
            /// </summary>
            public const string video_mpeg      = "video/mpeg";
            /// <summary>
            /// Modern, widely used format for video compression.
            /// </summary>
            public const string video_mp4       = "video/mp4";
            /// <summary>
            /// Format for QuickTime multimedia by Apple.
            /// </summary>
            public const string video_quicktime = "video/quicktime";
            /// <summary>
            /// Windows Media Video format.
            /// </summary>
            public const string video_x_ms_wmv  = "video/x-ms-wmv";
            /// <summary>
            /// AVI (Audio Video Interleave) format.
            /// </summary>
            public const string video_x_msvideo = "video/x-msvideo";
            /// <summary>
            /// Flash Video format (obsolete).
            /// </summary>
            public const string video_x_flv     = "video/x-flv";
            /// <summary>
            /// Open, web-optimized video format.
            /// </summary>
            public const string video_webm      = "video/webm";



            /// <summary>
            /// Android Package files for app distribution.
            /// </summary>
            public const string application_vnd_android_package_archive                                   = "application/vnd.android.package-archive";
            /// <summary>
            /// OpenDocument text format.
            /// </summary>
            public const string application_vnd_oasis_opendocument_text                                   = "application/vnd.oasis.opendocument.text";
            /// <summary>
            /// OpenDocument spreadsheet format.
            /// </summary>
            public const string application_vnd_oasis_opendocument_spreadsheet                            = "application/vnd.oasis.opendocument.spreadsheet";
            /// <summary>
            /// OpenDocument presentation format.
            /// </summary>
            public const string application_vnd_oasis_opendocument_presentation                           = "application/vnd.oasis.opendocument.presentation";
            /// <summary>
            /// OpenDocument graphics format.
            /// </summary>
            public const string application_vnd_oasis_opendocument_graphics                               = "application/vnd.oasis.opendocument.graphics";
            /// <summary>
            /// Microsoft Excel spreadsheet format.
            /// </summary>
            public const string application_vnd_ms_excel                                                  = "application/vnd.ms-excel";
            /// <summary>
            /// Excel format for Office Open XML.
            /// </summary>
            public const string application_vnd_openxmlformats_officedocument_spreadsheetml_sheet         = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            /// <summary>
            /// Microsoft PowerPoint presentation format.
            /// </summary>
            public const string application_vnd_ms_powerpoint                                             = "application/vnd.ms-powerpoint";
            /// <summary>
            /// PowerPoint format for Office Open XML.
            /// </summary>
            public const string application_vnd_openxmlformats_officedocument_presentationml_presentation = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            /// <summary>
            /// Microsoft Word document format.
            /// </summary>
            public const string application_msword                                                        = "application/msword";
            /// <summary>
            /// Word document format for Office Open XML.
            /// </summary>
            public const string application_vnd_openxmlformats_officedocument_wordprocessingml_document   = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            /// <summary>
            /// XML User Interface Language format used by Mozilla applications.
            /// </summary>
            public const string application_vnd_mozilla_xul_xml                                           = "application/vnd.mozilla.xul+xml";
        }
        /// <summary>
        /// Default set of user-agent headers.
        /// </summary>
        public class SE_UserAgent
        {
            /// <summary>
            /// Represents Google's web crawling bot, used for indexing and searching web pages.
            /// </summary>            
            public const string GoogleBot = "Googlebot/2.1 (+http://www.google.com/bot.html)";
            /// <summary>
            /// Represents Apple's tvOS, indicating an Apple TV device.
            /// </summary>
            public const string AppleTV = "AppleTV6,2/11.1";



            /// <summary>
            /// Opera browser running on a 64-bit Windows platform with the Presto rendering engine.
            /// </summary>
            public const string Opera_Windows = "Opera/9.80 (Windows NT 6.2; Win64; x64) Presto/2.12.388 Version/12.16";
            /// <summary>
            /// Opera browser running on a 32-bit Linux platform with the Presto rendering engine.
            /// </summary>
            public const string Opera_Linux = "Opera/10.00 (X11; Linux i686; U; en) Presto/2.2.0";
            /// <summary>
            /// Lightweight version of Opera designed for mobile devices, using the Presto engine with WebKit.
            /// </summary>
            public const string Opera_Mini = "Opera/10.61 (J2ME/MIDP; Opera Mini/5.1.21219/19.999; en-US; rv:1.9.3a5) WebKit/534.5 Presto/2.6.30";



            /// <summary>
            /// Mozilla Firefox browser on a 64-bit Windows platform, powered by the Gecko engine.
            /// </summary>
            public const string Mozilla_Windows = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0";
            /// <summary>
            /// Mozilla Firefox browser on a 32/64-bit Linux platform, powered by the Gecko engine.
            /// </summary>
            public const string Mozilla_Linux = "Mozilla/5.0 (X11; Linux x86_64; rv:70.0) Gecko/20100101 Firefox/70.0";
            /// <summary>
            /// Mozilla Firefox browser on a 64-bit Ubuntu Linux platform, powered by the Gecko engine.
            /// </summary>
            public const string Mozilla_Ubuntu = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:70.0) Gecko/20100101 Firefox/70.0";
            /// <summary>
            /// Mozilla Firefox browser on an Android device, incorporating WebKit and Chrome-like capabilities.
            /// </summary>
            public const string Mozilla_Android = "Mozilla/5.0 (Linux; Android 10; AKA-L29 Build/HONORAKA-L29; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/78.0.3904.108 Mobile Safari/537.36";
            /// <summary>
            /// An older version of Mozilla compatible with Internet Explorer 2.0 on Windows 3.1.
            /// </summary>
            public const string Mozilla_Legacy_Windows = "Mozilla/1.22 (compatible; MSIE 2.0; Windows 3.1)";
            /// <summary>
            /// Legacy User-Agent for the Konqueror browser on Linux, using the KHTML engine (like Gecko).
            /// </summary>
            public const string Mozilla_Legacy_Linux = "Mozilla/1.22 (compatible; Konqueror/4.3; Linux) KHTML/4.3.5 (like Gecko)";
        }




        /// <summary>
        /// Object containing the response from the request.
        /// </summary>
        public struct S_Response
        {
            /// <summary>
            /// Status code of the response.
            /// </summary>
            public E_StatusCode statusCode { get; }
            /// <summary>
            /// Headers of the response as a dictionary of key-value pairs.
            /// </summary>
            public Dictionary<string, string> headers { get; }
            /// <summary>
            /// Content of the response as a string.
            /// </summary>
            public string content { get; }

            public S_Response(E_StatusCode statusCode, Dictionary<string, string> headers, string content)
            {
                this.statusCode = statusCode;
                this.headers = headers;
                this.content = content;
            }
        }




        #region Get() function overload
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url)
        {
            return Get(url, new List<string>(), null, DEFAULT_TIMEOUT);
        }
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url, double timeout)
        {
            return Get(url, new List<string>(), null, timeout);
        }
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url, List<string> headers)
        {
            return Get(url, headers, null, DEFAULT_TIMEOUT);
        }
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url, List<string> headers, double timeout)
        {
            return Get(url, headers, null, timeout);
        }
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="data">Optional data to be sent with the request.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url, List<string> headers, string data)
        {
            return Get(url, headers, data, DEFAULT_TIMEOUT);
        }
        #endregion
        /// <summary>
        /// Sends an HTTP GET request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="data">Optional data to be sent with the request.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Get(string url, List<string> headers, string data, double timeout)
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

            httpRequest.ReadWriteTimeout = timeout.ToMilliseconds();

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            try
            {
                using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)httpResponse.StatusCode;
                        Dictionary<string, string> responseHeaders = httpResponse.Headers
                                                                     .AllKeys
                                                                     .ToDictionary(key => key, key => httpResponse.Headers[key]);
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
                        Dictionary<string, string> errorHeaders = errorResponse.Headers
                                                                  .AllKeys
                                                                  .ToDictionary(key => key, key => errorResponse.Headers[key]);
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




        #region Post() function overload
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url)
        {
            return Post(url, new List<string>(), null, DEFAULT_TIMEOUT);
        }
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url, double timeout)
        {
            return Post(url, new List<string>(), null, timeout);
        }
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url, List<string> headers)
        {
            return Post(url, headers, null, DEFAULT_TIMEOUT);
        }
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url, List<string> headers, double timeout)
        {
            return Post(url, headers, null, timeout);
        }
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="data">Optional data to be sent with the request.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url, List<string> headers, string data)
        {
            return Post(url, headers, data, DEFAULT_TIMEOUT);
        }
        #endregion
        /// <summary>
        /// Sends an HTTP POST request to the specified URL with optional headers, data, and timeout.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="data">Optional data to be sent with the request.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Post(string url, List<string> headers, string data, double timeout)
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

            httpRequest.ReadWriteTimeout = timeout.ToMilliseconds();

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            try
            {
                using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        E_StatusCode statusCode = (E_StatusCode)httpResponse.StatusCode;
                        Dictionary<string, string> responseHeaders = httpResponse.Headers
                                                                     .AllKeys
                                                                     .ToDictionary(key => key, key => httpResponse.Headers[key]);
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
                        Dictionary<string, string> errorHeaders = errorResponse.Headers
                                                                  .AllKeys
                                                                  .ToDictionary(key => key, key => errorResponse.Headers[key]);
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




        #region Download() function overload
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url)
        {
            return Download(url, new List<string>(), null, null, DEFAULT_DOWNLOAD_TIMEOUT);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="destinationPath">The file path to save the downloaded content. If not provided, the content will be stored in current user downloads folder.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, string destinationPath)
        {
            return Download(url, new List<string>(), null, destinationPath, DEFAULT_DOWNLOAD_TIMEOUT);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_DOWNLOAD_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, double timeout)
        {
            return Download(url, new List<string>(), null, null, timeout);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="destinationPath">The file path to save the downloaded content. If not provided, the content will be stored in current user downloads folder.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_DOWNLOAD_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, string destinationPath, double timeout)
        {
            return Download(url, new List<string>(), null, destinationPath, timeout);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, List<string> headers)
        {
            return Download(url, headers, null, null, DEFAULT_DOWNLOAD_TIMEOUT);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="destinationPath">The file path to save the downloaded content. If not provided, the content will be stored in current user downloads folder.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, List<string> headers, string destinationPath)
        {
            return Download(url, headers, null, destinationPath, DEFAULT_DOWNLOAD_TIMEOUT);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_DOWNLOAD_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, List<string> headers, double timeout)
        {
            return Download(url, headers, null, null, timeout);
        }
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="destinationPath">The file path to save the downloaded content. If not provided, the content will be stored in current user downloads folder.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_DOWNLOAD_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, List<string> headers, string destinationPath, double timeout)
        {
            return Download(url, headers, null, destinationPath, timeout);
        }
        #endregion
        /// <summary>
        /// Downloads content from the specified URL with optional headers, data, and timeout,
        /// and saves it to a specified destination path if provided.
        /// </summary>
        /// <param name="url">The URL to download the content from.</param>
        /// <param name="headers">A list of headers to include in the request.</param>
        /// <param name="data">Optional data to be sent with the request.</param>
        /// <param name="destinationPath">The file path to save the downloaded content. If not provided, the content will be stored in current user downloads folder.</param>
        /// <param name="timeout">The timeout duration for the request in seconds. Defaults to <see cref="DEFAULT_DOWNLOAD_TIMEOUT"/> seconds.</param>
        /// <returns>An <see cref="S_Response"/> object containing the response from the request.</returns>
        public static S_Response Download(string url, List<string> headers, string data, string destinationPath, double timeout)
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

            httpRequest.ReadWriteTimeout = timeout.ToMilliseconds();

            if (string.IsNullOrEmpty(data) == false)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
            }

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
                        Dictionary<string, string> responseHeaders = httpResponse.Headers
                                                                     .AllKeys
                                                                     .ToDictionary(key => key, key => httpResponse.Headers[key]);
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
                        Dictionary<string, string> errorHeaders = errorResponse.Headers
                                                                  .AllKeys
                                                                  .ToDictionary(key => key, key => errorResponse.Headers[key]);
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
}
