using System;
using System.Configuration;
using System.Web;
using Telerik.Web.UI;

namespace Telerik.QuickStart
{
    class QsfCdnConfigurator
    {
        private static string QsfCdnEnabledAppSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["qsfCdnEnabled"];
            }
        }

        public static bool QsfCdnIsEnabled
        {
            get 
            {
                bool cdnIsEnabled = false;

                bool.TryParse(QsfCdnEnabledAppSetting, out cdnIsEnabled);

                return cdnIsEnabled;
            }
        }

        public static bool HasCanAccessCdnCookie(HttpRequest request)
        {
            bool hasAccessCdnCookie = request.Cookies["canAccessCdn"] != null;

            return hasAccessCdnCookie;
        }

        public static bool GetCanAccessCdnFromCookie(HttpRequest request)
        {
            bool canAccessCdn = false;

            if (HasCanAccessCdnCookie(request))
                canAccessCdn = bool.Parse(request.Cookies["canAccessCdn"].Value);

            return canAccessCdn;
        }

        private static string UncompressedPath
        {
            get { return "ajax-qsf"; }
        }

        private static string CompressedPath
        {
            get { return "ajax-qsfz"; }
        }

        private static string ResolveResourceFullCdnPath(string resourceBaseCdnUrl, IHttpRequestInfo httpRequestInfo)
        {
            string compressionPartOfUrl = httpRequestInfo.SupportsGzip ? CompressedPath : UncompressedPath;

            return String.Format("{0}/{1}", resourceBaseCdnUrl, compressionPartOfUrl);
        }

        public static string ResolveHttpPrefix(string url)
        {
            string httpPrefix = "http://";
            string httpPrefixedUrl = url;

            if (url.IndexOf(httpPrefix) == -1)
                httpPrefixedUrl = String.Format("{0}{1}", httpPrefix, url);

            if (httpPrefixedUrl.EndsWith("/"))
                httpPrefixedUrl = httpPrefixedUrl.TrimEnd('/');

            return httpPrefixedUrl;
        }

        public static string ResolveScriptsFullCdnPath(IHttpRequestInfo requestInfo)
        {
            return ResolveResourceFullCdnPath(ScriptsCdnUrlAppSetting, requestInfo);
        }

        public static string ResolveSkinsFullCdnPath(IHttpRequestInfo requestInfo)
        {
            return ResolveResourceFullCdnPath(SkinsCdnUrlAppSetting, requestInfo);
        }

        /// <summary>
        /// Gets the Url of the Scripts from the AppSettings section of the web.config in the 'qsfCdnScriptsUrl' key.
        /// First check <see cref="ScriptsCdnUrlIsConfigured"/> to know whether the corresponding key is present or has a non-empty value.
        /// </summary>
        public static string ScriptsCdnUrlAppSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["qsfScriptsCdnUrl"];
            }
        }

        /// <summary>
        /// Gets the Url of the Skins from the AppSettings section of the web.config in the 'qsfCdnSkinsUrl' key.
        /// First check <see cref="SkinsCdnUrlIsConfigured"/> to know whether the corresponding key is present or has a non-empty value.
        /// </summary>
        public static string SkinsCdnUrlAppSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["qsfSkinsCdnUrl"];
            }
        }

        public static bool IsCdnDisabledByQueryParam(HttpRequest request)
        {
            if (request.Params["disableCdn"] != null && request.Params["disableCdn"] == "1")
                return true;

            return false;
        }
    }
}
