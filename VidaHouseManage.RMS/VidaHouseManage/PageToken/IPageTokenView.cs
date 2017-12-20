using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VidaHouseManage.PageToken
{
    
        public interface IPageTokenView
        {
            /// <summary>
            /// Generates the page token.
            /// </summary>
            string GeneratePageToken();

            /// <summary>
            /// Gets the get last page token from Form
            /// </summary>
            string GetLastPageToken { get; }

            /// <summary>
            /// Gets a value indicating whether [tokens match].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [tokens match]; otherwise, <c>false</c>.
            /// </value>
            bool TokensMatch { get; }
        }
    }