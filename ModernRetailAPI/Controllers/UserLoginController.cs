#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: LMS Info Details.Row: 2
#endregion===================================End of Revision History==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModernRetailAPI.Models;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Device;
using System.Device.Location;
using System.Data.Spatial;
using System.Xml;

namespace ModernRetailAPI.Controllers
{
    public class UserLoginController : ApiController
    {
       [HttpPost]
        public HttpResponseMessage Login(MDRUserLoginModel model)
        {
            MDRClassLoginOutput omodel = new MDRClassLoginOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    string token = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];
                    string profileImg = System.Configuration.ConfigurationManager.AppSettings["ProfileImageURL"];
                    Encryption epasswrd = new Encryption();
                    string Encryptpass = epasswrd.Encrypt(model.login_password);
                    string sessionId = "";


                    sessionId = HttpContext.Current.Session.SessionID;
                    string location_name = "Login from ";

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();

                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();

                    sqlcmd = new SqlCommand("PRC_MDRAPIUSERLOGIN", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@LOGIN_ID", model.login_id);
                    sqlcmd.Parameters.AddWithValue("@LOGIN_PASSWORD", Encryptpass);
                    sqlcmd.Parameters.AddWithValue("@APP_VERSION", model.app_version);
                    sqlcmd.Parameters.AddWithValue("@DEVICE_TOKEN", model.device_token);
                    sqlcmd.Parameters.AddWithValue("@Weburl", profileImg);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt.Rows.Count > 0)
                    {

                        if (Convert.ToString(dt.Rows[0]["success"]) == "200")
                        {
                            omodel.status = "200";
                            omodel.message = "User successfully logged in.";
                            omodel.user_name = Convert.ToString(dt.Rows[0]["user_name"]);
                            omodel.user_id = Convert.ToString(dt.Rows[0]["user_id"]);
                            omodel.contact_number = Convert.ToString(dt.Rows[0]["contact_number"]);
                            omodel.email = Convert.ToString(dt.Rows[0]["email"]);
                            omodel.city = Convert.ToString(dt.Rows[0]["city"]);
                            omodel.state = Convert.ToString(dt.Rows[0]["state"]);
                            omodel.country = Convert.ToString(dt.Rows[0]["country"]);
                            omodel.pincode = Convert.ToString(dt.Rows[0]["pincode"]);
                            omodel.address = Convert.ToString(dt.Rows[0]["address"]);
                            omodel.profile_pic_url = Convert.ToString(dt.Rows[0]["profile_pic_url"]);
                        }
                        else if (Convert.ToString(dt.Rows[0]["success"]) == "202")
                        {
                            omodel.status = "202";
                            omodel.message = "Invalid user credential.";
                        }
                        else if (Convert.ToString(dt.Rows[0]["success"]) == "206")
                        {
                            omodel.status = "206";
                            omodel.message = Convert.ToString(dt.Rows[0]["Dynamic_message"]);// "New version is available now. Please update it from the Play Store. Unless you can't login into the app. ";//Html.fromHtml('http://www.google.com')"
                        }
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "Invalid user credential.";
                    }

                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        string location = string.Empty;

        public string RetrieveFormatedAddress(string lat, string lng)
        {
            string address = "";
            string locationName = "";

            string url = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key=AIzaSyCbYMZjnt8T6yivYfIa4_R9oy-L3SIYyrQ", lat, lng);

            try 
            {
                XElement xml = XElement.Load(url);
                if (xml.Element("status").Value == "OK")
                {
                    locationName = string.Format("{0}",
                    xml.Element("result").Element("formatted_address").Value);
                }
            }
            catch
            {
                locationName = "";
            }
            return locationName;
        }       

        public static string RetrieveFormatedAddressNew(string lat, string lng)
        {
             string baseUri =  "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
             string location = string.Empty;

            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where
                                  elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where
                                   elm.Name == "formatted_address"
                               select elm).FirstOrDefault();
                    requestUri = res.Value;
                    location = requestUri;
                }
            }

            return location;
        }

        static string baseUri =
  "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=true";


        [HttpPost]
        public HttpResponseMessage SubmitHomeLocation(UserHomeLocation model)
        {
            MDRdaywiseOutput omodel = new MDRdaywiseOutput();
            List<MDRdaywiseList> oview = new List<MDRdaywiseList>();
            List<MDRdaywiseList> oview1 = new List<MDRdaywiseList>();
            ShopList odata = new ShopList();
            List<ShopList> shoplst = new List<ShopList>();
            MDRdaywiseList odelails = new MDRdaywiseList();
            if (!ModelState.IsValid)
            {
                omodel.status = "213";
                omodel.message = "Some input parameters are missing.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            }
            else
            {
                String token = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];

                string sessionId = "";


                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

                String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("Proc_FTS_UserHomeaddress", sqlcon);
                sqlcmd.Parameters.AddWithValue("@user_id", model.user_id);
                sqlcmd.Parameters.AddWithValue("@latitude", model.latitude);
                sqlcmd.Parameters.AddWithValue("@longitude", model.longitude);
                sqlcmd.Parameters.AddWithValue("@address", model.address);
                sqlcmd.Parameters.AddWithValue("@city", model.city);
                sqlcmd.Parameters.AddWithValue("@state", model.state);
                sqlcmd.Parameters.AddWithValue("@pincode", model.pincode);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Home address submitted successfully.";
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "No data found";
                }

                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }


        [HttpPost]
        public HttpResponseMessage ChangePass(ChangePassword model)
        {
            ChangePassOutput omodel = new ChangePassOutput();
           
            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    String token = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];
                    Encryption epasswrd = new Encryption();
                    string OldEncryptpass = epasswrd.Encrypt(model.old_pwd);
                    string NewEncryptpass = epasswrd.Encrypt(model.new_pwd);
                    string sessionId = "";

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_APIUserPasswordChange", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@user_id", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@Old_password", OldEncryptpass);
                    sqlcmd.Parameters.AddWithValue("@New_password", NewEncryptpass);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt!=null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["MSG"].ToString()=="Successfully changed password.")
                        {
                            omodel.status = "200";
                            omodel.message = dt.Rows[0]["MSG"].ToString();
                        }
                        else
                        {
                            omodel.status = "202";
                            omodel.message = dt.Rows[0]["MSG"].ToString();
                        }
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "Invalid user credential.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }


        [HttpPost]
        public HttpResponseMessage GetUser(GetUserInput model)
        {
            GetUserOutput omodel = new GetUserOutput();
            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    String token = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];
                   
                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_GetUserLoginByPhone", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@Phone", model.Phone);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                            omodel.status = "200";
                            omodel.message = dt.Rows[0]["MSG"].ToString();
                            omodel.user_id = dt.Rows[0]["user_id"].ToString();
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "Invalid user credential.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage GetUserList(GetUserListInput model)
        {
            GetUserListOutPut omodel = new GetUserListOutPut();
            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    String token = System.Configuration.ConfigurationManager.AppSettings["AuthToken"];

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_ActiveUserList", sqlcon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully get User list.";
                        omodel.UserList = APIHelperMethods.ToModelList<GetUserList>(dt);
                    }
                    else
                    {
                        omodel.status = "202";
                        omodel.message = "No data found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "209";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }
    }
}