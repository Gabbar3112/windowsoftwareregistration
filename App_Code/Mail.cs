using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Mail
/// </summary>
public static class Mail
{
    //
    // TODO: Add constructor logic here
    //
    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RadhaKrishnaConnectionString"].ConnectionString;
    //try
    //{
    //    conn.Open();


    //}
    //catch (Exception ex)
    //{

    //}
    //finally
    //{
    //      if (conn.State.Equals(ConnectionState.Open))            
    //          conn.Close();
    //}



    //try
    //{

    //}
    //catch (Exception ex)
    //{

    //}
    //finally
    //{

    //}
    #region  "Send email text body"
    public static void SendMail(string senderName, string frmAddress, string toAddress, string subject, string cc1, string cc2, string bcc1, string bcc2, string messageText)
    {
        String smtpHost, port1;

        smtpHost = ConfigurationManager.AppSettings["smtphost"].ToString();
        port1 = ConfigurationManager.AppSettings["port"].ToString();

        SmtpClient mailClient = new SmtpClient(smtpHost, Convert.ToInt16(port1));
        mailClient.EnableSsl = true;
        mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        mailClient.UseDefaultCredentials = false;

        NetworkCredential cred = new NetworkCredential();
        cred.UserName = ConfigurationManager.AppSettings["username"].ToString();
        cred.Password = ConfigurationManager.AppSettings["password"].ToString();
        mailClient.Credentials = cred;

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        try
        {
            MailAddress fromAddress = new MailAddress(frmAddress, senderName);
            message.From = fromAddress;

            message.To.Add(toAddress);
            message.Subject = subject;

            message.IsBodyHtml = true;
            message.Body = messageText;

            mailClient.Send(message);
            //Response.Write("<script  language='javascript' type='text/javascript'>window.alert('Email Successfully Sent.');</script>");
        }
        catch
        {
            //Response.Write("<script  language='javascript' type='text/javascript'>window.alert('Send Email Failed '" + ex.Message + ");</script>");
        }
    }
    #endregion

    #region  "Send email html body"
    public static string SendHTMLMail(string host, string senderName, string frmAddress, string toAddress, int port, string subject, string cc1, string cc2, string bcc1, string bcc2, string messageText)
    {

        SmtpClient smtpClient = new SmtpClient();
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        try
        {
            String smtpHost, port1;

            smtpHost = ConfigurationManager.AppSettings["smtphost"].ToString();
            port1 = ConfigurationManager.AppSettings["port"].ToString();

            SmtpClient mailClient = new SmtpClient(smtpHost, Convert.ToInt16(port1));
            mailClient.EnableSsl = true;
            mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;

            NetworkCredential cred = new NetworkCredential();
            cred.UserName = ConfigurationManager.AppSettings["username"].ToString();
            cred.Password = ConfigurationManager.AppSettings["password"].ToString();
            mailClient.Credentials = cred;

            MailAddress fromAddress = new MailAddress(frmAddress, senderName);
            message.From = fromAddress;

            message.To.Add(toAddress);
            message.Subject = subject;

            message.IsBodyHtml = true;
            message.Body = messageText;

            /*  Attachment attach = new Attachment(messageText);
         Attach the file 
           message.Attachments.Add(attach);*/
            mailClient.Send(message);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {

        }
        return "suc";
    }
    #endregion

    #region GetLastUploadDate
    public static string GetLastUploadDate()
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RadhaKrishnaConnectionString"].ConnectionString);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select convert(char(12) ,lastupload, 0) from LastUploadDate", conn);
            object r = cmd.ExecuteScalar();
            if (r == DBNull.Value || r == null)
                return "";
            else
                return "Last Update: " + r.ToString();
        }
        catch
        {
        }
        finally
        {
            if (conn.State.Equals(ConnectionState.Open))
                conn.Close();
        }
        return "";
    }
    #endregion

    #region Convert Date From DDMMYYYY to MMDDYYYY
    public static string ConvertDateFromDDMMtoMMDD(string tx)
    {
        if (tx == " - -" || string.IsNullOrEmpty(tx))
            return "";
        else
        {
            try
            {
                string year = tx.Substring(6, 4);
                string month = tx.Substring(3, 2);
                string day = tx.Substring(0, 2);
                return month + "/" + day + "/" + year;
            }
            catch
            {
                return "";
            }
        }
    }
    #endregion
}

