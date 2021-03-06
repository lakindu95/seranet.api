﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Seranet.Api.Core
{
    public class SeranetAuthAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated) {
                string path = actionContext.Request.RequestUri.LocalPath.Substring(actionContext.Request.RequestUri.LocalPath.LastIndexOf("/") + 1);
                string authorizedUsersString = ConfigurationManager.AppSettings[path.ToLower()];

                if (authorizedUsersString == null)
                {
                    // if there is no auth configuration for the url, we assume it is an open service
                    return;
                } 
                else
                {
                    var userArray = authorizedUsersString.Split(',');
                    foreach (var user in userArray)
	                {
                        if (HttpContext.Current.User.Identity.Name.Trim().Equals(user.Trim(), StringComparison.CurrentCultureIgnoreCase)) 
                        {
                            return;
                        }
	                }

                }
            }

            //if user is not found to be authorized, return unauthorized
            HandleUnauthorizedRequest(actionContext);
        }

    }
}
