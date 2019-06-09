using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetCircle(string knownCategoryValues)
    {
        SqlCommand cmd = new SqlCommand("ListCircleByUserID");
        cmd.CommandType = CommandType.StoredProcedure;

        if (HttpContext.Current.Session["UserID"] != null)
        {
            cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserID"]).DbType = DbType.Int64;
        }
        else
        {
            cmd.Parameters.AddWithValue("@UserID", 0).DbType = DbType.Int64;
        }
        List<CascadingDropDownNameValue> CircleMaster = GetData(cmd);
        return CircleMaster.ToArray();
    }


    [WebMethod]
    public CascadingDropDownNameValue[] GetDivisionByCircle(string knownCategoryValues)
    {
        string CircleID = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)["CircleID"];
        SqlCommand cmd = new SqlCommand("ListDivisionByCircle");
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CircleID", CircleID).DbType = DbType.Int64;
        List<CascadingDropDownNameValue> ExpenseCategoryMaster = GetData(cmd);
        return ExpenseCategoryMaster.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetAllDistrict(string knownCategoryValues)
    {
        SqlCommand cmd = new SqlCommand("ListAllDistrict");
        cmd.CommandType = CommandType.StoredProcedure;

        List<CascadingDropDownNameValue> DistrictMaster = GetData(cmd);
        return DistrictMaster.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetTalukaByDistrict(string knownCategoryValues)
    {
        string DistrictID = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)["DistrictID"];
        SqlCommand cmd = new SqlCommand("ListTalukaByDistrict");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DistrictID", DistrictID).DbType = DbType.Int64;
        List<CascadingDropDownNameValue> TalukaMaster = GetData(cmd);
        return TalukaMaster.ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetVillageByTaluka(string knownCategoryValues)
    {
        string TalukaID = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)["TalukaID"];
        SqlCommand cmd = new SqlCommand("ListVillageByTaluka");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TalukaID", TalukaID).DbType = DbType.Int64;
        List<CascadingDropDownNameValue> VillageMaster = GetData(cmd);
        return VillageMaster.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetSubDivision(string knownCategoryValues)
    {
        SqlCommand cmd = new SqlCommand("RptGetSubDivforCanal");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserID"]).DbType = DbType.Int64;
        List<CascadingDropDownNameValue> SubDivision = GetData(cmd);
        return SubDivision.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public CascadingDropDownNameValue[] GetSectionBySubDivision(string knownCategoryValues)
    {
        string SubDivisionID = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)["SubDivisionID"];
        SqlCommand cmd = new SqlCommand("RptGetSectionforCanal");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SubDivisionID", SubDivisionID).DbType = DbType.Int64;
        cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserID"]).DbType = DbType.Int64;
        List<CascadingDropDownNameValue> Sections = GetData(cmd);
        return Sections.ToArray();
    }

    private List<CascadingDropDownNameValue> GetData(SqlCommand cmdIn)
    {
        string conString = ConfigurationManager.ConnectionStrings["AagakhanConnectionString"].ConnectionString;
        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        using (SqlConnection con = new SqlConnection(conString))
        {
            con.Open();
            cmdIn.Connection = con;
            using (SqlDataReader reader = cmdIn.ExecuteReader())
            {
                while (reader.Read())
                {
                    values.Add(new CascadingDropDownNameValue
                    {
                        name = reader[1].ToString(),
                        value = reader[0].ToString()
                    });
                }
                reader.Close();
                con.Close();
                return values;
            }
        }
    }
    
}
