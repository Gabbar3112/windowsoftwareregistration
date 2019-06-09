using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PhoneVerification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //lblUN.Text = HttpContext.Current.Session["UserName"].ToString();
        //lblPWD.Text = HttpContext.Current.Session["Password"].ToString();
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        #region Send Email to User including Verify Link
        string strStatus = "";

        var Email = HttpContext.Current.Session["Email"];
        var ClientCode = HttpContext.Current.Session["ClientCode"];
        

        XmlDocument XMLdoc = new XmlDocument();

        XMLdoc.Load(HttpContext.Current.Server.MapPath("Email/AuthorRegistration.xml"));
        XmlElement root = XMLdoc.DocumentElement;
        XmlNodeList nodes = root.SelectNodes("/email");

        string strSubject = "";
        string strBody = "";

        foreach (XmlNode node in nodes)
        {
            strSubject = node["subject"].InnerText;
            strBody = node["messgae"].InnerText;

        }

        strBody = strBody.Replace("##ClientCode##", ClientCode.ToString());       
        strBody = strBody.Replace("##VERIFYPAGEURL##", ConfigurationSettings.AppSettings["AdminSiteURL"].ToString());
        strStatus = Mail.SendHTMLMail(ConfigurationManager.AppSettings["smtphost"].ToString(), "Jaimini Software Pvt. Ltd.", ConfigurationManager.AppSettings["From"].ToString(), Email.ToString(), Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString()), strSubject, "", "", "", "", strBody);
        #endregion
    }
}