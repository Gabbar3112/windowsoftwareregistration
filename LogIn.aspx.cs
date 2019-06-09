using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogIn : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod]
    [ScriptMethod]
    public static string Login(string UN, string PWD, int UT)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UserDetails"].ConnectionString);
        string msg;
        string IsExist = "0";
        try      
        {   var Mac = GetMACAddress();            
            var proce = GetProcessorId();

            con.Open();
            SqlCommand cmd = new SqlCommand("GetLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = UT;
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UN;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = PWD;
            cmd.Parameters.Add("@MacAddress", SqlDbType.VarChar).Value = Mac;
            cmd.Parameters.Add("@ProcessorID", SqlDbType.VarChar).Value = proce;     
            msg = cmd.ExecuteScalar().ToString();
            string[] values = msg.Split(',');
            if (values[0] == "0")
                IsExist = values[0];
            else
            {
                IsExist = values[0];
                HttpContext.Current.Session["UserID"] = values[1];
                HttpContext.Current.Session["UserType"] = UT;
            }
                
            con.Close();

        }
        catch (Exception ex)
        {
            //   Global.ErrorInsert(ex.Message, formname, "UserManage");
            msg = "Error" + ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        return IsExist;
    }

    [WebMethod]
    public static string GetMACAddress()
    {
        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection moc = mc.GetInstances();
        string MACAddress = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            if (MACAddress == String.Empty)
            {
                if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
            }
            mo.Dispose();
        }

        MACAddress = MACAddress.Replace(":", "");
        return MACAddress;
    }

    public static String GetProcessorId()
    {

        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        String Id = String.Empty;
        foreach (ManagementObject mo in moc)
        {

            Id = mo.Properties["processorID"].Value.ToString();
            break;
        }
        return Id;

    }
}