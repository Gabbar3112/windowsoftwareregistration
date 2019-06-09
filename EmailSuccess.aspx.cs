using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmailSuccess : System.Web.UI.Page
{
    int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        string msg = string.Empty;
        var ProcessorID = GetProcessorId();
        var ProcessorInfo = GetProcessorInformation();
        var HDDSrNO = GetHDDSerialNo();
        var BordMaker = GetBoardMaker();
        var BordProductID = GetBoardProductId();
        var BiosCaption = GetBIOScaption();
        var PhysicalMemory = GetPhysicalMemory();
        var CPUSpeed = GetCpuSpeedInGHz();
        var CPUMaker = GetCPUManufacturer();
        var IPAddress= GetIPAddress();
        var MacAddress=GetMACAddress();
        var AccName = GetAccountName();
        var OSInfo = GetOSInformation();
        var Computername = GetComputerName();
        var CurrntLanguage = GetCurrentLanguage();
        var NoOfSlot = GetNoRamSlots();
        var RouterIP = NetworkGateway();
        var RouterMac = GetMacAddress(RouterIP);


        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UserDetails"].ConnectionString);
        conn.Open();
        try
        {
            var RegNo = Request.QueryString["RegNo"];
            SqlCommand cmd1 = new SqlCommand("GetUserInformation", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add("@RegistrationID", SqlDbType.VarChar).Value = RegNo;
            string strRes = cmd1.ExecuteScalar().ToString();

            string[] arrRes = strRes.Split(',');
            HttpContext.Current.Session["UserType"] = arrRes[0].ToString();
            HttpContext.Current.Session["MobileNo"] = arrRes[1].ToString();
            HttpContext.Current.Session["UserName"] = arrRes[2].ToString();
            HttpContext.Current.Session["Password"] = arrRes[3].ToString();
            HttpContext.Current.Session["Email"] = arrRes[4].ToString();
            HttpContext.Current.Session["ClientCode"] = arrRes[5].ToString();
            conn.Close();

            conn.Open();
            SqlCommand cmd = new SqlCommand("InsertSystemInformation", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RegistrationID",SqlDbType.VarChar).Value = RegNo;
            cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = HttpContext.Current.Session["UserType"];
            cmd.Parameters.Add("@ProccessorID",SqlDbType.VarChar).Value = ProcessorID;
            cmd.Parameters.Add("@ProcessorInfo",SqlDbType.VarChar).Value = ProcessorInfo;
            cmd.Parameters.Add("@HDDSerialNo",SqlDbType.VarChar).Value = HDDSrNO;

            cmd.Parameters.Add("@BoardMaker",SqlDbType.VarChar).Value = BordMaker;
            cmd.Parameters.Add("@BoardProductID",SqlDbType.VarChar).Value = BordProductID;
            cmd.Parameters.Add("@BiosCaption",SqlDbType.VarChar).Value = BiosCaption;
            cmd.Parameters.Add("@PhysicalMemory",SqlDbType.VarChar).Value = PhysicalMemory;

            cmd.Parameters.Add("@CPUSpeed",SqlDbType.VarChar).Value = CPUSpeed;
            cmd.Parameters.Add("@CPUMaker",SqlDbType.VarChar).Value = CPUMaker;
            cmd.Parameters.Add("@IPAddress",SqlDbType.VarChar).Value = IPAddress;
            cmd.Parameters.Add("@MACAddress",SqlDbType.VarChar).Value = MacAddress;

            cmd.Parameters.Add("@AccountName",SqlDbType.VarChar).Value = AccName;
            cmd.Parameters.Add("@OsInformation",SqlDbType.VarChar).Value = OSInfo;
            cmd.Parameters.Add("@ComputerName",SqlDbType.VarChar).Value = Computername;
            cmd.Parameters.Add("@CurrentLanguage",SqlDbType.VarChar).Value = CurrntLanguage;

            cmd.Parameters.Add("@NoOfRamSlot",SqlDbType.VarChar).Value = NoOfSlot;
            cmd.Parameters.Add("@RouterMACAddress", SqlDbType.VarChar).Value = RouterMac;
            cmd.Parameters.Add("@RouterIPAddress", SqlDbType.VarChar).Value = RouterIP;
            
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            conn.Close();

            //OTP();
           
        }        
        catch (Exception ex)
        {
         //   Global.ErrorInsert(ex.Message, formname, "UserManage");
            msg = "Error" + ex.Message;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
     
    }

    public void OTP()
    {
        #region Send SMS
        var PhnNo = HttpContext.Current.Session["MobileNo"];
        if (PhnNo != "")
        {
            Random r = new Random();
            string OTP = r.Next(1000, 9999).ToString();
            HttpContext.Current.Session["OTP"] = OTP;



            string strMsg = "Welcome to Jaimini Software. Your One Time Password is:" + OTP;

            string URL = "http://SMS17.JAIMINISOFTWARE.COM/submitsms.jsp?";
            URL += "user=JAMINI";
            URL += "&key=6229b0e195XX" + "&mobile=" + PhnNo + "&message= " + strMsg;
            URL += "&senderid=TARANG" + "&accusage=1";
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }
        #endregion
    }


    public static string NetworkGateway()
    {
        string ip = null;

        foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (f.OperationalStatus == OperationalStatus.Up)
            {
                foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                {

                    ip = d.Address.ToString();
                }
            }
        }

        return ip;
    }


    //Get MAC address
    public static string GetMacAddress(string ipAddress)
    {
        string macAddress = string.Empty;
        System.Diagnostics.Process Process = new System.Diagnostics.Process();
        Process.StartInfo.FileName = "arp";
        Process.StartInfo.Arguments = "-a " + ipAddress;
        Process.StartInfo.UseShellExecute = false;
        Process.StartInfo.RedirectStandardOutput = true;
        Process.StartInfo.CreateNoWindow = true;
        Process.Start();
        string strOutput = Process.StandardOutput.ReadToEnd();
        string[] substrings = strOutput.Split('-');
        if (substrings.Length >= 8)
        {
            macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                     + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                     + "-" + substrings[7] + "-"
                     + substrings[8].Substring(0, 2);
            return macAddress;
        }

        else
        {
            return "OWN Machine";
        }
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

    public static String GetHDDSerialNo()
    {
        ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
        ManagementObjectCollection mcol = mangnmt.GetInstances();
        string result = "";
        foreach (ManagementObject strt in mcol)
        {
            result += Convert.ToString(strt["VolumeSerialNumber"]);
        }
        return result;
    }

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

    public static string GetIPAddress()
    {
        string hostName = String.Empty;
        hostName = Dns.GetHostName(); // Retrive the Name of HOST  
        Console.WriteLine(hostName);
        // Get the IP  
        string myIP = String.Empty;
        myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
        return myIP;
    }

    public static string GetBoardMaker()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Manufacturer").ToString();
            }

            catch { }

        }

        return "Board Maker: Unknown";

    }

    public static string GetBoardProductId()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Product").ToString();

            }

            catch { }

        }

        return "Product: Unknown";

    }

    public static string GetBIOSserNo()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("SerialNumber").ToString();

            }

            catch { }

        }

        return "BIOS Serial Number: Unknown";

    }

    public static string GetBIOScaption()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Caption").ToString();

            }
            catch { }
        }
        return "BIOS Caption: Unknown";
    }

    public static string GetAccountName()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {

                return wmi.GetPropertyValue("Name").ToString();
            }
            catch { }
        }
        return "User Account Name: Unknown";

    }

    public static string GetPhysicalMemory()
    {
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
        ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
        ManagementObjectCollection oCollection = oSearcher.Get();

        long MemSize = 0;
        long mCap = 0;

        // In case more than one Memory sticks are installed
        foreach (ManagementObject obj in oCollection)
        {
            mCap = Convert.ToInt64(obj["Capacity"]);
            MemSize += mCap;
        }
        MemSize = (MemSize / 1024) / 1024;
        return MemSize.ToString() + "MB";
    }

    public static string GetNoRamSlots()
    {

        int MemSlots = 0;
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
        ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
        ManagementObjectCollection oCollection2 = oSearcher2.Get();
        foreach (ManagementObject obj in oCollection2)
        {
            MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

        }
        return MemSlots.ToString();
    }

    public static string GetCPUManufacturer()
    {
        string cpuMan = String.Empty;
        //create an instance of the Managemnet class with the
        //Win32_Processor class
        ManagementClass mgmt = new ManagementClass("Win32_Processor");
        //create a ManagementObjectCollection to loop through
        ManagementObjectCollection objCol = mgmt.GetInstances();
        //start our loop for all processors found
        foreach (ManagementObject obj in objCol)
        {
            if (cpuMan == String.Empty)
            {
                // only return manufacturer from first CPU
                cpuMan = obj.Properties["Manufacturer"].Value.ToString();
            }
        }
        return cpuMan;
    } 

    public static string GetDefaultIPGateway()
    {
        //create out management class object using the
        //Win32_NetworkAdapterConfiguration class to get the attributes
        //of the network adapter
        ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //create our ManagementObjectCollection to get the attributes with
        ManagementObjectCollection objCol = mgmt.GetInstances();
        string gateway = String.Empty;
        //loop through all the objects we find
        foreach (ManagementObject obj in objCol)
        {
            if (gateway == String.Empty)  // only return MAC Address from first card
            {
                //grab the value from the first network adapter we find
                //you can change the string to an array and get all
                //network adapters found as well
                //check to see if the adapter's IPEnabled
                //equals true
                if ((bool)obj["IPEnabled"] == true)
                {
                    gateway = obj["DefaultIPGateway"].ToString();
                }
            }
            //dispose of our object
            obj.Dispose();
        }
        //replace the ":" with an empty space, this could also
        //be removed if you wish
        gateway = gateway.Replace(":", "");
        //return the mac address
        return gateway;
    }

    public static double? GetCpuSpeedInGHz()
    {
        double? GHz = null;
        using (ManagementClass mc = new ManagementClass("Win32_Processor"))
        {
            foreach (ManagementObject mo in mc.GetInstances())
            {
                GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                break;
            }
        }
        return GHz;
    }

    public static string GetCurrentLanguage()
    {

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("CurrentLanguage").ToString();

            }

            catch { }

        }

        return "BIOS Maker: Unknown";

    }

    public static string GetOSInformation()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
            }
            catch { }
        }
        return "BIOS Maker: Unknown";
    }

    public static String GetProcessorInformation()
    {
        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            string name = (string)mo["Name"];
            name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

            info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
            //mo.Properties["Name"].Value.ToString();
            //break;
        }
        return info;
    }

    public static String GetComputerName()
    {
        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
        ManagementObjectCollection moc = mc.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject mo in moc)
        {
            info = (string)mo["Name"];
            //mo.Properties["Name"].Value.ToString();
            //break;
        }
        return info;
    }

    protected void btnGenerateOTP_Click(object sender, EventArgs e)
    {

        btnGenerateOTP.Visible = false;
        VerifyCode.Visible = true;
        txtVerifyCode.Visible = true;
        btnVerify.Visible = true;
        OTP();
    }

    protected void btnVerify_Click(object sender, EventArgs e)
   {
       
        var VC = txtVerifyCode.Text;
        var sessotp = HttpContext.Current.Session["OTP"];
        if (sessotp.ToString() == VC.ToString())
        {
            //string pageurl = "PhoneVerification.aspx";
            //Response.Write("<script> window.location.href = '" + pageurl );
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "JS", "javascript:hello(this.value); ", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Ming", "alert('Wrong OTP')", true);
            
        }
        
    }
}