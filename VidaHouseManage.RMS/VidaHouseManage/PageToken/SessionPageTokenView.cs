using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VidaHouseManage.PageToken
{
    public class SessionPageTokenView : PageTokenViewBase
    {
        

        /// <summary>
        /// Generates the page token.
        /// </summary>
        /// <returns></returns>
        public override string GeneratePageToken()
        {
            if (HttpContext.Current.Session[SessionMyToken] != null)
            {
                return HttpContext.Current.Session[SessionMyToken].ToString();
            }
            else
            {
                var token = GenerateHashToken();
                HttpContext.Current.Session[SessionMyToken] = token;
                return token;
            }
        }

        /// <summary>
        /// Gets the get last page token from Form
        /// </summary>
        public override string GetLastPageToken
        {
            get
            {
                return HttpContext.Current.Request.Params[HiddenTokenName];
            }
        }

        /// <summary>
        /// Gets a value indicating whether [tokens match].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [tokens match]; otherwise, <c>false</c>.
        /// </value>
        public override bool TokensMatch
        {
            get
            {
                string formToken = GetLastPageToken;
                if (formToken != null)
                {
                    if (formToken.Equals(GeneratePageToken()))
                    {
                        //Refresh token
                        HttpContext.Current.Session[SessionMyToken] = GenerateHashToken();
                        return true;
                    }
                }
                return false;
            }
        }
        #region Private Help Method
        /// <summary>
        /// Generates the hash token.
        /// </summary>
        /// <returns></returns>
        private string GenerateHashToken()
        {
            return Utility.Encrypt(
                HttpContext.Current.Session.SessionID + DateTime.Now.Ticks.ToString());
        }
        #endregion

    }

}