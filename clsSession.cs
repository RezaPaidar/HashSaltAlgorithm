using System;
using System.Collections.Generic;
using System.Web;

    public class clsSession
    {
        private clsSession()
        {
            username = null;
        }

        public string username { get; set; }

        // Gets the current session.
        public static clsSession Current
        {
            get
            {
                var session =
                (clsSession)HttpContext.Current.Session["__MySession__"];
                if (session != null) return session;
                session = new clsSession();
                HttpContext.Current.Session["__MySession__"] = session;
                return session;
            }
        }

        public static void StoreInCookie(
            string cookieName,
            string cookieDomain,
            string keyName,
            string value,
            DateTime? expirationDate,
            bool httpOnly = false)
        {
            // NOTE: we have to look first in the response, and then in the request.
            // This is required when we update multiple keys inside the cookie.
            var cookie = (HttpContext.Current.Response.Cookies[cookieName]
                                 ?? HttpContext.Current.Request.Cookies[cookieName]) ?? new HttpCookie(cookieName);
            if (!string.IsNullOrEmpty(keyName))
                cookie.Values.Set(keyName, value);
            else cookie.Value = value;
            if (expirationDate.HasValue)
                cookie.Expires = expirationDate.Value;
            if (!string.IsNullOrEmpty(cookieDomain))
                cookie.Domain = cookieDomain;
            if (httpOnly)
                cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        
        //Stores multiple values in a Cookie using a key-value dictionary, creating the cookie (and/or the key) if it does not exists yet. 
        public static void StoreInCookie(
            string cookieName,
            string cookieDomain,
            Dictionary<string, string> keyValueDictionary,
            DateTime? expirationDate,
            bool httpOnly = false)
        {
            // NOTE: we have to look first in the response, and then in the request.
            // This is required when we update multiple keys inside the cookie.
            var cookie = (HttpContext.Current.Response.Cookies[cookieName]
                                 ?? HttpContext.Current.Request.Cookies[cookieName]) ?? new HttpCookie(cookieName);
            if (keyValueDictionary == null || keyValueDictionary.Count == 0)
                cookie.Value = null;
            else
                foreach (var kvp in keyValueDictionary)
                    cookie.Values.Set(kvp.Key, kvp.Value);
            if (expirationDate.HasValue)
                cookie.Expires = expirationDate.Value;
            if (!string.IsNullOrEmpty(cookieDomain))
                cookie.Domain = cookieDomain;
            if (httpOnly)
                cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        /// <summary>
        /// Retrieves a single value from Request.Cookies
        /// </summary>
        public static string GetFromCookie(string cookieName, string keyName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null) return null;
            string val = (!string.IsNullOrEmpty(keyName)) ? cookie[keyName] : cookie.Value;
            return !string.IsNullOrEmpty(val) ? Uri.UnescapeDataString(val) : null;
        }
        /// <summary>
        /// Removes a single value from a cookie or the whole cookie (if keyName is null)
        /// </summary>
        public static void RemoveCookie(string cookieName, string keyName, string domain = null)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                if (HttpContext.Current.Request.Cookies[cookieName] == null) return;
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                cookie.Expires = DateTime.UtcNow.AddYears(-1);
                if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(cookieName);
            }
            else
            {
                var cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie == null) return;
                cookie.Values.Remove(keyName);
                if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// Checks if a cookie / key exists in the current HttpContext.
        /// </summary>
        public static bool CookieExist(string cookieName, string keyName)
        {
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            return 
                (string.IsNullOrEmpty(keyName))
                ? cookies[cookieName] != null
                : cookies[cookieName] != null && cookies[cookieName][keyName] != null;
        }
    }


