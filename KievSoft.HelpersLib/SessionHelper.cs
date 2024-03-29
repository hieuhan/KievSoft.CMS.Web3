﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KievSoft.HelpersLib
{
    public class SessionHelper
    {
        #region Enumerators

        /// <summary>
        /// Available session scopes
        /// </summary>
        public enum Scope
        {
            Global,
            Page,
            PageAndQuery
            /* put any other scopes you may find useful here */
        }

        #endregion

        #region Class members

        #region Key existence

        /// <summary>
        /// Indicates if the key associated to given scope and category exists
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        public static bool Exists(Scope scope, string category, string key)
        {
            return SessionKey(scope, category, key) != null;
        }

        /// <summary>
        /// Indicates if the key associated to given category exists (global scope)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        public static bool Exists(string category, string key)
        {
            return Exists(Scope.Global, category, key);
        }

        /// <summary>
        /// Indicates if the key associated to given scope exists
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        public static bool Exists(Scope scope, string key)
        {
            return Exists(scope, string.Empty);
        }

        /// <summary>
        /// Indicates if the key exists (global scope)
        /// </summary>
        /// <param name="key"></param>
        public static bool Exists(string key)
        {
            return Exists(string.Empty, key);
        }

        #endregion

        #region Values storing

        /// <summary>
        /// Stores a value to session, using a scope a category and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Store(Scope scope, string category, string key, object value)
        {
            StoreFormattedKey(FormatKey(scope, category, key), value);
        }

        /// <summary>
        /// Stores a value to session, using a category and a key (global scope)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Store(string category, string key, object value)
        {
            Store(Scope.Global, category, key, value);
        }

        /// <summary>
        /// Stores a value to session, using a scope and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        public static void Store(Scope scope, string key, object value)
        {
            Store(scope, string.Empty, key, value);
        }

        /// <summary>
        /// Stores a value to session, using a key (global scope)
        /// </summary>
        /// <param name="key"></param>
        public static void Store(string key, object value)
        {
            Store(string.Empty, key, value);
        }

        #endregion

        #region Values retrieving

        #region Values retrieving (null if not found)

        /// <summary>
        /// Stores a value to session, using a scope a category and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static object Retrieve(Scope scope, string category, string key)
        {
            return SessionKey(scope, category, key);
        }

        /// <summary>
        /// Stores a value to session, using a category and a key (global scope)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static object Retrieve(string category, string key)
        {
            return Retrieve(Scope.Global, category, key);
        }

        /// <summary>
        /// Stores a value to session, using a scope and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        public static object Retrieve(Scope scope, string key)
        {
            return Retrieve(scope, string.Empty, key);
        }

        /// <summary>
        /// Stores a value to session, using a key (global scope)
        /// </summary>
        /// <param name="key"></param>
        public static object Retrieve(string key)
        {
            return Retrieve(string.Empty, key);
        }

        #endregion

        #region Values retrieving (with default value)

        /// <summary>
        /// Stores a value to session, using a scope a category and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static object RetrieveWithDefault(Scope scope, string category, string key, object defaultValue)
        {
            object value = SessionKey(scope, category, key);

            return value == null ? defaultValue : value;
        }

        /// <summary>
        /// Stores a value to session, using a category and a key (global scope)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static object RetrieveWithDefault(string category, string key, object defaultValue)
        {
            return RetrieveWithDefault(Scope.Global, category, key, defaultValue);
        }

        /// <summary>
        /// Stores a value to session, using a scope and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        public static object RetrieveWithDefault(Scope scope, string key, object defaultValue)
        {
            return RetrieveWithDefault(scope, string.Empty, key, defaultValue);
        }

        /// <summary>
        /// Stores a value to session, using a key (global scope)
        /// </summary>
        /// <param name="key"></param>
        public static object RetrieveWithDefault(string key, object defaultValue)
        {
            return RetrieveWithDefault(string.Empty, key, defaultValue);
        }

        #endregion

        #endregion

        #region Values clearing

        /// <summary>
        /// Clears all session values
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Clears all  session values of given scope
        /// </summary>
        /// <param name="scope"></param>
        /// <returns>Number of affected values</returns>
        public static int ClearScope(Scope scope)
        {
            return ClearStartsWith(string.Format("{0}.", GetScopeHash(scope)));
        }

        /// <summary>
        /// Clears all session values of given scope's category
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        public static int ClearCategory(Scope scope, string category)
        {
            return ClearStartsWith(string.Format("{0}.{1}.", GetScopeHash(scope), category));
        }

        /// <summary>
        /// Clears all session values of given category (global scope)
        /// </summary>
        /// <param name="category"></param>
        public static int ClearCategory(string category)
        {
            return ClearCategory(Scope.Global, category);
        }

        /// <summary>
        /// Clears a session value, using a scope a category and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Clear(Scope scope, string category, string key)
        {
            Store(scope, category, key, null);
        }

        /// <summary>
        /// Clears a session value, using a category and a key (global scope)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Clear(string category, string key)
        {
            Clear(Scope.Global, category, key);
        }

        /// <summary>
        /// Clears a session value, using a scope and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        public static void Clear(Scope scope, string key)
        {
            Clear(scope, string.Empty, key);
        }

        /// <summary>
        /// Clears a session value, using a key (global scope)
        /// </summary>
        /// <param name="key"></param>
        public static void Clear(string key)
        {
            Clear(string.Empty, key);
        }

        #endregion

        #endregion

        #region Utilities

        #region Session's key format

        /// <summary>
        /// Format a key, using a scope a category and a key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string FormatKey(Scope scope, string category, string key)
        {
            // clean up any points
            string scopeHash = GetScopeHash(scope);
            category = category.Replace(".", "");
            key = key.Replace(".", "");

            return string.Format("{0}.{1}.{2}", scopeHash, category, key);
        }

        /// <summary>
        /// Format a key, using a category and a key (global scope)
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string FormatKey(string category, string key)
        {
            return FormatKey(Scope.Global, category);
        }

        /// <summary>
        /// Format a key, using a scope and a key
        /// </summary>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string FormatKey(Scope scope, string key)
        {
            return FormatKey(scope, string.Empty);
        }

        /// <summary>
        /// Format a key, using a key (global scope)
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string FormatKey(string key)
        {
            return FormatKey(string.Empty);
        }

        #endregion

        #region Cryptography

        /// <summary>
        /// Creates a MD5 based hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetHash(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Get the hash of a scope
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        private static string GetScopeHash(Scope scope)
        {
            // Get scope name
            string scopeName = Enum.GetName(scope.GetType(), scope);

            switch (scope)
            {
                case Scope.Page:
                    scopeName = HttpContext.Current.Request.Url.AbsoluteUri;
                    if (HttpContext.Current.Request.Url.Query != string.Empty)
                    {
                        scopeName = scopeName.Replace(HttpContext.Current.Request.Url.Query, "");
                    }
                    break;
                case Scope.PageAndQuery:
                    scopeName = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.AbsoluteUri);
                    break;
            }

            return GetHash(scopeName);
        }

        /// <summary>
        /// Shortcut to formatted session value
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="category"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object SessionKey(Scope scope, string category, string key)
        {
            return HttpContext.Current.Session[FormatKey(scope, category, key)];
        }

        /// <summary>
        /// Rawly store a value in a formatted key
        /// </summary>
        /// <param name="formattedKey"></param>
        /// <param name="value"></param>
        private static void StoreFormattedKey(string formattedKey, object value)
        {
            HttpContext.Current.Session[formattedKey] = value;
        }

        /// <summary>
        /// Rawly clear a formatted key value
        /// </summary>
        /// <param name="formattedKey"></param>
        private static void ClearFormattedKey(string formattedKey)
        {
            HttpContext.Current.Session.Remove(formattedKey);
        }

        /// <summary>
        /// Clears all formatted keys starting with given value
        /// </summary>
        /// <param name="startOfFormattedKey"></param>
        private static int ClearStartsWith(string startOfFormattedKey)
        {
            List<string> formattedKeysToClear = new List<string>();

            // Gather formatted keys to clear (to prevent collection modification during parsing)
            foreach (string key in HttpContext.Current.Session)
            {
                if (key.StartsWith(startOfFormattedKey))
                {
                    // Add key
                    formattedKeysToClear.Add(key);
                }
            }

            foreach (string formattedKey in formattedKeysToClear)
            {
                ClearFormattedKey(formattedKey);
            }

            return formattedKeysToClear.Count;
        }

        #endregion
    }
}
