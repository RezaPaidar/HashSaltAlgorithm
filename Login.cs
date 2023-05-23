public partial class Login : System.Web.UI.Page
{
    private readonly IUser _user = new ClsUser();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (clsSession.CookieExist("yourCookieName", "UserId"))
        {
            var user = _user.Get_User_Details(clsSession.GetFromCookie("yourCookieName", "UserId"));
            if (user != null)
            {
                Response.Redirect("Index");
            }
            else
            {
                clsSession.Current.username = null;
                clsSession.RemoveCookie("yourCookieName", "UserId");
            }
        }
        else if (clsSession.Current.username != null)
        {
            var user = _user.Get_User_Details(clsSession.Current.username);
            if (user != null)
            {
                Response.Redirect("Index");
            }
            else
            {
                clsSession.Current.username = null;
                clsSession.RemoveCookie("yourCookieName", "UserId");
            }
        }
    }

    protected void btnLogIn_ServerClick(object sender, EventArgs e)
    {
        var username = HttpUtility.HtmlEncode(txtPhoneNumber.Value.Trim());
        if (!Xss.Check_Mobile_Number(username))
        {
            const string title = "خطا";
            const string textError = "<p>phone number is incorrect.</p>";
            /*I use send js alert using sweetAlert library*/
            ClientScript.RegisterStartupScript(GetType(), "userIncorrect",
                "warning('" + title + "','" + textError + "');", true);
            return;
        }

        var password = HttpUtility.HtmlEncode(txtPassword.Value);
        if (Xss.Check_Mobile_Number(username))
        {
            var exist = _user.Check_User_Password_Exist(username, password);
            if (exist > 0)
            {
                if (chbRemeber.Checked)
                {
                    clsSession.StoreInCookie("yourCookieName", "", "UserId", exist.ToString(), DateTime.Now.AddDays(30));
                    clsSession.Current.username = username.ToString();
                }
                else
                {
                    clsSession.Current.username = username.ToString();
                }
                Check_Authentication.Visible = true;
                const string script = "setTimeout(function(){ window.location.href='Index'; }, 1100);";
                ScriptManager.RegisterStartupScript(this, GetType(), "hwa", script, true);
            }
            else
            {
                const string title = "error";
                const string textError = "username or password is incorrect";
                ClientScript.RegisterStartupScript(GetType(), "warning", "warning('" + title + "','" + textError + "');", true);

            }
        }
    }
}
