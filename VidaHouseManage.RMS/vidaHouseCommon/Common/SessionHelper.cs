﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VidaHouseManage.Common
{
    /// <summary>  
    /// Session辅助类  
    /// </summary>  
    public static class SessionHelper
    {
        /// <summary>  
        /// 设置Session，调动有效期为60分钟  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <param name="strValue">Session值</param>  
        public static void SetSession(string strSessionName, string strValue)
        {
            HttpContext.Current.Session.Remove(strSessionName);
            HttpContext.Current.Session.Add(strSessionName,strValue);
            HttpContext.Current.Session.Timeout = 60;
           
        }

        /// <summary>  
        /// 添加Session，调动有效期为60分钟  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <param name="strValues">Session值数组</param>  
        public static void Adds(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session.Remove(strSessionName);
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 60;
        }

        /// <summary>  
        /// 添加Session  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <param name="strValue">Session值</param>  
        /// <param name="iExpires">调动有效期（分钟）</param>  
        public static void SetSession(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session.Remove(strSessionName);
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>  
        /// 添加Session  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <param name="strValues">Session值数组</param>  
        /// <param name="iExpires">调动有效期（分钟）</param>  
        public static void Adds(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>  
        /// 读取某个Session对象值  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <returns>Session对象值</returns>  
        public static string GetSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }

        /// <summary>  
        /// 读取某个Session对象值数组  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        /// <returns>Session对象值数组</returns>  
        public static string[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[strSessionName];
            }
        }

        /// <summary>  
        /// 删除某个Session对象  
        /// </summary>  
        /// <param name="strSessionName">Session对象名称</param>  
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
}
