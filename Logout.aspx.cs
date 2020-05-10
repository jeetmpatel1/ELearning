using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            /*
             * (UC004) Logout User
             *  4a: User clicks "No" Button.
                    .1: Redirect user back to UserAccount.aspx.

                This use case is already handled in aspx page in default code by redirection.
            */
               
                // close the session variables
                Session.Abandon();

                //redirect User to the login page
                Response.Redirect("~/UserLogin.aspx");
        }
    }
}