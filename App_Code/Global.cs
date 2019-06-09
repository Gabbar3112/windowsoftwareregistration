using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for Global
/// </summary>
public class Global
{
    public static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AagakhanConnectionString"].ConnectionString);

    public static string MachineName = HttpContext.Current.Server.MachineName;// System.Environment.MachineName.ToString();
    
    public static void SMSsend(string no, string message)
    {
        try
        {
            string URL = ConfigurationManager.AppSettings["SMSUrl"].ToString();
            URL += "user=" + ConfigurationManager.AppSettings["SMSUser"].ToString();
            URL += "&key=" + ConfigurationManager.AppSettings["SMSKey"].ToString() + "&mobile=" + no + "&message=" + message;
            URL += "&senderid=" + ConfigurationManager.AppSettings["SMSSender"].ToString() + "&accusage=1";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }
        catch (Exception ex)
        {
        }
    }

    public static void ErrorInsert(string Error, string Form, string Event)
    {
        try
        {
            DateTime ErrorDate = DateTime.Now;
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            SqlCommand cmd = new SqlCommand("ErrorMasterInsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Error", SqlDbType.VarChar).Value = Error;
            cmd.Parameters.Add("@FormName", SqlDbType.VarChar).Value = Form;
            cmd.Parameters.Add("@FormEvent", SqlDbType.VarChar).Value = Event;
            cmd.Parameters.Add("@ErrorDateTime", SqlDbType.DateTime).Value = ErrorDate;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (conn.State.Equals(ConnectionState.Open))
                conn.Close();
        }
    }

    public static void LogInsert(string Form, string Event, string TableName, string TransactionDetail, Int64 RecordID = 0)
    {
        try
        {
            DateTime InsertDate = DateTime.Now;
            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();
            SqlCommand cmd = new SqlCommand("LogMasterInsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.BigInt).Value = HttpContext.Current.Session["UserID"];
            cmd.Parameters.Add("@EventName", SqlDbType.VarChar).Value = Event;
            cmd.Parameters.Add("@FormName", SqlDbType.VarChar).Value = Form;
            cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = TableName;
            cmd.Parameters.Add("@MachineName", SqlDbType.VarChar).Value = MachineName;
            cmd.Parameters.Add("@TransactionDetail", SqlDbType.VarChar).Value = TransactionDetail;
            cmd.Parameters.Add("@UpdateTime", SqlDbType.DateTime).Value = InsertDate;
            cmd.Parameters.Add("@YearID", SqlDbType.BigInt).Value = HttpContext.Current.Session["YearID"];
            cmd.Parameters.Add("@RecordID", SqlDbType.BigInt).Value = RecordID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (conn.State.Equals(ConnectionState.Open))
                conn.Close();
        }
    }

    public static class NumberToGujarati
    {
        public static string changeNumericToWords(double numb)
        {
            String num = numb.ToString();
            return changeToWords(num, true);
        }
        //public String changeCurrencyToWords(String numb)
        //{
        //    return changeToWords(numb, true);
        //}
        //public String changeNumericToWords(String numb)
        //{
        //    return changeToWords(numb, false);
        //}
        //public String changeCurrencyToWords(double numb)
        //{
        //    return changeToWords(numb.ToString(), true);
        //}
        private static string changeToWords(String numb, bool isCurrency)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            //String endStr = (isCurrency) ? ("Only") : ("");
            String endStr = (isCurrency) ? ("પુરા") : ("");
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? ("અને") : ("પોઈન્ટ");// just to separate whole numbers from points/cents
                        //endStr = (isCurrency) ? ("Cents " + endStr) : ("");
                        endStr = (isCurrency) ? ("પૈસા " + endStr) : ("");
                        pointStr = translateCents(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { ;}
            return val;
        }
        private static string translateWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                double tm = (Convert.ToDouble(number));
                int newnm = 0;
                double rm = 0;
                string strfst;
                string tmpnumber = number.ToString();
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");

                    int numDigits = tmpnumber.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            rm = tm / 100;
                            newnm = (int)Math.Floor(rm);
                            pos = (numDigits % 3) + 1;
                            place = " સો ";
                            //strfst = tmpnumber.Substring(0, pos);
                            //if (strfst == "2" || strfst == "02")
                            // place = " bso ";
                            //else
                            //if (newnm == 2)
                            //{
                            // place = " bso ";
                            //}
                            //else
                            //{
                            // if (strfst != "0")
                            // place = " so ";
                            //}

                            //if (newnm == 2)
                            //{
                            // number = tmpnumber.Substring(pos, tmpnumber.Length - 1);
                            //}
                            break;
                        case 4://thousands' range
                        case 5:
                            pos = (numDigits % 4) + 1;
                            strfst = tmpnumber.Substring(0, pos);
                            if (strfst != "0" && strfst != "00")
                                place = "  	હજાર ";
                            //number = tmpnumber.Substring(pos, tmpnumber.Length - 1);
                            break;
                        case 6:
                        case 7://millions' range
                            pos = (numDigits % 6) + 1;
                            place = "  	લાખ ";
                            break;
                        case 8:
                        case 9:
                            pos = (numDigits % 8) + 1;
                            place = " કરોડ઼ ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)

                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));

                        //check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                        if (beginsZero) word = " " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { ;}
            return word.Trim();
        }
        private static string tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "દસ";//"Ten"
                    break;
                case 11:
                    name = "અગિયાર";//"Eleven"
                    break;
                case 12:
                    name = "બાર";//"Twelve"
                    break;
                case 13:
                    name = "તેર";//"Thirteen"
                    break;
                case 14:
                    name = "ચૌદ";//"Fourteen"
                    break;
                case 15:
                    name = "પંદર";//"Fifteen"
                    break;
                case 16:
                    name = "સોળ";//"Sixteen"
                    break;
                case 17:
                    name = "સત્તર";//"Seventeen"
                    break;
                case 18:
                    name = "અઢાર";//"Eighteen"
                    break;
                case 19:
                    name = "ઓગણીસ";//"Nineteen"
                    break;
                case 20:
                    name = "વીસ";
                    break;
                case 21:
                    name = "એકવીસ";
                    break;
                case 22:
                    name = "બાવીસ";
                    break;
                case 23:
                    name = "ત્રેવીસ";
                    break;
                case 24:
                    name = "ચોવીસ";
                    break;
                case 25:
                    name = "પચ્ચીસ";
                    break;
                case 26:
                    name = "છવીસ";
                    break;
                case 27:
                    name = "સત્તાવીસ";
                    break;
                case 28:
                    name = "અઠ્ઠાવીસ";
                    break;
                case 29:
                    name = "ઓગણત્રીસ";
                    break;
                case 30:
                    name = "ત્રીસ";
                    break;
                case 31:
                    name = "એકત્રીસ";
                    break;
                case 32:
                    name = "બત્રીસ";
                    break;
                case 33:
                    name = "તેત્રીસ";
                    break;
                case 34:
                    name = "ચોત્રીસ";
                    break;
                case 35:
                    name = "પાંત્રીસ";
                    break;
                case 36:
                    name = "છત્રીસ";
                    break;
                case 37:
                    name = "સડત્રીસ";
                    break;
                case 38:
                    name = "અડત્રીસ";
                    break;
                case 39:
                    name = "ઓગણચાલીસ";
                    break;
                case 40:
                    name = "ચાલીસ";
                    break;
                case 41:
                    name = "એકતાલીસ";
                    break;
                case 42:
                    name = "બેતાલીસ";
                    break;
                case 43:
                    name = "તેતાલીસ";
                    break;
                case 44:
                    name = "ચુંમાલીસ";
                    break;
                case 45:
                    name = "પિસ્તાલીસ";
                    break;
                case 46:
                    name = "છેતાલીસ";
                    break;
                case 47:
                    name = "સુડતાલીસ";
                    break;
                case 48:
                    name = "અડતાલીસ";
                    break;
                case 49:
                    name = "ઓગણપચાસ";
                    break;
                case 50:
                    name = "પચાસ";
                    break;
                case 51:
                    name = "એકાવન";
                    break;
                case 52:
                    name = "બાવન";
                    break;
                case 53:
                    name = "તેપન";
                    break;
                case 54:
                    name = "ચોપન";
                    break;
                case 55:
                    name = "પંચાવન";
                    break;
                case 56:
                    name = "છપ્પન";
                    break;
                case 57:
                    name = "સત્તાવન";
                    break;
                case 58:
                    name = "અઠ્ઠાવન";
                    break;
                case 59:
                    name = "ઓગણસાઇઠ";
                    break;
                case 60:
                    name = "સાઇઠ";
                    break;
                case 61:
                    name = "એકસઠ";
                    break;
                case 62:
                    name = "બાસઠ";
                    break;
                case 63:
                    name = "તેસઠ";
                    break;
                case 64:
                    name = "ચોસઠ";
                    break;
                case 65:
                    name = "પાંસઠ";
                    break;
                case 66:
                    name = "છાસઠ";
                    break;
                case 67:
                    name = "સડસઠ";
                    break;
                case 68:
                    name = "અડસઠ";
                    break;
                case 69:
                    name = "અગણોસિત્તેર";
                    break;
                case 70:
                    name = "સિત્તેર";
                    break;
                case 71:
                    name = "ઇકોત્તર";
                    break;
                case 72:
                    name = "બોતેર";
                    break;
                case 73:
                    name = "તોતેર";
                    break;
                case 74:
                    name = "ચુમોતેર";
                    break;
                case 75:
                    name = "પંચોતેર";
                    break;
                case 76:
                    name = "છોતેર";
                    break;
                case 77:
                    name = "સિત્યોતર";
                    break;
                case 78:
                    name = "ઇઠયોતર";
                    break;
                case 79:
                    name = "અગણયાએંસી";
                    break;
                case 80:
                    name = "એંસી";
                    break;
                case 81:
                    name = "એકયાસી";
                    break;
                case 82:
                    name = "બ્યાસી";
                    break;
                case 83:
                    name = "ત્યાસી";
                    break;
                case 84:
                    name = "ચોર્યાસી";
                    break;
                case 85:
                    name = "પંચાસી";
                    break;
                case 86:
                    name = "છયાસી";
                    break;
                case 87:
                    name = "સિત્યાસી";
                    break;
                case 88:
                    name = "ઇઠયાસી";
                    break;
                case 89:
                    name = "નેવ્યાસી";
                    break;
                case 90:
                    name = "નેવું";
                    break;
                case 91:
                    name = "એકાણું";
                    break;
                case 92:
                    name = "બાણું";
                    break;
                case 93:
                    name = "તાણું";
                    break;
                case 94:
                    name = "ચોરાણું";
                    break;
                case 95:
                    name = "પંચાણું";
                    break;
                case 96:
                    name = "છન્નું";
                    break;
                case 97:
                    name = "સત્તાણું";
                    break;
                case 98:
                    name = "અઠ્ઠાણું";
                    break;
                case 99:
                    name = "નવ્વાણું";
                    break;

                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static string ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = "એક";//"One"
                    break;
                case 2:
                    name = "બે";//"Two"
                    break;
                case 3:
                    name = "ત્રણ";//"Three"
                    break;
                case 4:
                    name = "ચાર";//"Four"
                    break;
                case 5:
                    name = "પાંચ";//"Five"
                    break;
                case 6:
                    name = "છ";//"Six"
                    break;
                case 7:
                    name = "સાત";//"Seven"
                    break;
                case 8:
                    name = "આઠ";//"Eight"
                    break;
                case 9:
                    name = "નવ";//"Nine"
                    break;
            }
            return name;
        }
        private static string translateCents(String cents)
        {
            String cts = "", digit = "", engOne = "";
            for (int i = 0; i < cents.Length; i++)
            {
                digit = cents[i].ToString();
                if (digit.Equals("0"))
                {
                    //engOne = "";
                }
                else
                {
                    engOne = ones(digit);
                }
                cts += " " + engOne;
            }
            return cts;
        }

        public static void CompressImage(System.Drawing.Image img, string fileName, long quality)
        {
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            img.Save(fileName, GetCodecInfo("image/jpeg"), parameters);
        }

        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            foreach (ImageCodecInfo encoder in ImageCodecInfo.GetImageEncoders())
                if (encoder.MimeType == mimeType)
                    return encoder;
            throw new ArgumentOutOfRangeException(
                string.Format("'{0}' not supported", mimeType));
        }


    }
}