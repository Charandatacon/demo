using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using Microsoft.Reporting.WebForms;
using Usermngt.Entities;
using Usermngt.BL;
using EnvDTE80;

namespace UserMgmt
{
    public partial class LabInchargeLogin : System.Web.UI.Page
    {
        List<Server_Configs> server = new List<Server_Configs>();
        string messgae = "";
        public class LabInchargeList
        {
            public string form_no { get; set; }
            public int total { get; set; }
            public int status_code { get; set; }
            public int untested { get; set; }
            public int tested { get; set; }
            public int retest { get; set; }
            public int verified { get; set; }
            public string status_string { get; set; }
            public string fir_no { get; set; }
            public string email { get; set; }
            public string mobileno { get; set; }

            public string GetStatusString(int x)
            {
                if (x == 1) return "All samples tested";
                if (x == 2) return "All samples verified";
                if (x == 3) return "All samples not tested";
                return null;
            }

            public LabInchargeList(string FormNo,string FIRNO, int Total, int Status_code, int Untested, int Tested, int Retest, int Verified,string email,string mobileno)
            {
                this.form_no = FormNo;
                this.fir_no = FIRNO;
                this.total = Total;
                this.status_code = Status_code;
                this.untested = Untested;
                this.tested = Tested;
                this.retest = Retest;
                this.verified = Verified;
                this.status_string = GetStatusString(Status_code);
                this.email = email;
                this.mobileno = mobileno;
                
            }

            public LabInchargeList()
            {

            }

        }

        public string ConnectionString => ConfigurationManager.ConnectionStrings["CASEMGMT"].ConnectionString;

        public NpgsqlConnection GetSqlConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public NpgsqlCommand GetSqlCommand(string commandText, CommandType commandType = CommandType.Text)
        {
            var sqlCommand = new NpgsqlCommand(commandText, GetSqlConnection())
            {
                CommandType = commandType
            };
            return sqlCommand;
        }

        string fullname = "";
        NpgsqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LoginPage");
                }
                conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["CASEMGMT"].ToString());
                try
                {
                    //string query = $@"SELECT a.*,c.fir_no,g.emailid ,g.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date b on a.form_no=b.form_no  inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on a.form_no = f.form_no  inner join   exciseautomation.tab_district_login g on f.dist_id =g.dist_id  WHERE b.form_no != '' order by b.date_of_creation desc";
                    string query = $@"select distinct(b.date_of_creation), a.*,c.fir_no,g.emailid ,g.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date b on a.form_no=b.form_no  inner join exciseautomation.tab_police_info c on b.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no =f.form_no  inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no    inner join   exciseautomation.tab_district_login g on f.dist_id =g.dist_id and h.depart_id=g.department_id    WHERE b.form_no != '' order by b.date_of_creation desc";
                    LoadGrid(query);
                }

                catch (Exception ex)
                {
                    Response.Write("Contact Admin : " + ex.Message.ToString());
                }
            }
        }

        public string get_query()
        {
            string query = $@"";
            string fn = txtForm.Text.ToString();
            string fir = txtFIR.Text.ToString();
            int type = int.Parse(ddStatus.SelectedValue.ToString());
            if (fn == "" && fir=="")
            {
                if (type != 0)
                {
                    // query = $@"SELECT a.*,c.fir_no FROM exciseautomation.tab_form_no_status a inner join exciseautomation.tab_police_info c on a.form_no =c.form_no WHERE quantity_status_overall = {type}";
                    query = $@"SELECT distinct(n.date_of_creation),a.*,c.fir_no,b.emailid ,b.mobileno  FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no   inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id WHERE quantity_status_overall = {type}";
                }
                else
                {
                    query = $@"SELECT distinct(n.date_of_creation),a.*,c.fir_no,b.emailid ,b.mobileno  FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no   inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id";
                }
            }
            else if(fn!="")
            {
                if (type != 0)
                {
                    //query = $@"SELECT a.*,c.fir_no FROM exciseautomation.tab_form_no_status a inner join exciseautomation.tab_police_info c on a.form_no =c.form_no WHERE a.form_no = '{fn}' AND quantity_status_overall = {type}";
                    query = $@"SELECT distinct(n.date_of_creation),a.*,c.fir_no,b.emailid ,b.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no   inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id WHERE a.form_no = '{fn}' AND quantity_status_overall = {type}";
                }
                else
                {
                    // query = $@"SELECT a.*,c.fir_no FROM exciseautomation.tab_form_no_status a inner join exciseautomation.tab_police_info c on a.form_no =c.form_no WHERE a.form_no = '{fn}'";
                    query = $@"SELECT distinct(n.date_of_creation),a.*,c.fir_no,b.emailid ,b.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no   inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id WHERE a.form_no = '{fn}'";
                }
            }
            else
            if (type != 0)
            {
                //query = $@"SELECT a.*,b.fir_no FROM exciseautomation.tab_form_no_status a inner join exciseautomation.tab_police_info b on a.form_no = b.form_no where fir_no='{fir}'AND quantity_status_overall = {type}";
                query = $@"SELECT distinct(n.date_of_creation),a.*,b.fir_no,b.emailid ,b.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no = c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no    inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id where fir_no='{fir}'AND quantity_status_overall = {type}";
            }
            else
            {
                query = $@"SELECT distinct(n.date_of_creation),a.*,c.fir_no,b.emailid ,b.mobileno FROM exciseautomation.tab_form_no_status a inner join  exciseautomation.tab_form_no_date n on a.form_no=n.form_no inner join exciseautomation.tab_police_info c on a.form_no =c.form_no inner join exciseautomation.tab_form_no_dist_id f on c.form_no = f.form_no inner join exciseautomation.tab_form_no_depart h on f.form_no=h.form_no   inner join   exciseautomation.tab_district_login b on f.dist_id =b.dist_id and h.depart_id=b.department_id WHERE c.fir_no = '{fir}'";
            }
            
            return query;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string query = get_query();
            LoadGrid(query);
        }

        protected void grdLabInchargeLogin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            if (e.CommandName == "View")
            {
                var inputParams = e.CommandArgument.ToString().Split(',');
                var form_no = inputParams.Length > 0 ? inputParams[0] : default(string);
                Response.Redirect($"~/LabInchargeView.aspx?form_no={form_no}");
            }
            if (e.CommandName == "SMS")
            {
                string smss="N";
                //avani
                //String username = "BIHAREDISTRICT-excise";
                //String password = "Excise@123";
                //String senderid = "BRGOVT";
                ////String mobileNos = "9712915870";
                //String mobileNos = "9714797592,9712915870";
                //String message = "This is test message";
                //String secureKey = "584fdb0f-6287-490f-a071-6102880a9327";
                //String templateid = "1307161012046394150";
                //string result = btnSMSCol_Click(username, password, senderid, mobileNos, message, secureKey, templateid);
                //int a = Convert.ToInt32(e.CommandArgument);
                //Session["index"] = a;
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RemoveAt = gvr.RowIndex;
                string mobile = "7892437909,7259951221,6380156300";/* (grdLabInchargeLogin.Rows[RemoveAt].FindControl("lblmobile") as Label).Text;*/
                //  Session["mobileno"] = (gvr.Cells[grdLabInchargeLogin.Rows.Count - 1].FindControl("mobileno") as Label).Text;
                Session["mobileno"] = mobile;
                //working code 
                //var request = (HttpWebRequest)WebRequest.Create("http://sms.hspsms.com/sendSMS");
                //var postData = "&username=yogibaluragi@datacontech.com";
                //postData += "&message=Dear User,"
                //         + " MUKESH KUMAR (IDB1016) Exit Allowed at CALL CENTER BIHAR on 11/04/2022 02:40:08 PM"
                //         + "DATACON TECHNOLOGIES";
                //postData += "&sendername=DATACO";
                //postData += "&smstype=TRANS";
                string[] mulp = mobile.Split(',');
                //foreach (string mul in mulp)
                //{
                //    postData += "&numbers="+mul+"";
                //}
                ////7892437909
                //postData += "&apikey=13a25db6-1fc1-4b5a-acb9-1e5b2cf1bb90";

                //var data = Encoding.ASCII.GetBytes(postData);

                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = data.Length;

                //using (var stream = request.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}

                //var response = (HttpWebResponse)request.GetResponse();

                //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //foreach (GridViewRow dr1 in grdLabInchargeLogin.Rows)

                //{
                foreach (string mul in mulp)
                {
                    messgae += "Your FIR No. 481/21 dated 16-09-21 of"
                        +"Thana Muffasil Gaya SHO for sample test"
                        +"has deen done and report set to your"
                        +"registered email id."
                        +"Thanks,"
                        +"Excise Chemical Lab, Patna";
                    //sendBulkSMS("BIHAREDISTRICT-excise", "Excise@123", "BRGOVT", "9473029410", messgae, "584fdb0f-6287-490f-a071-6102880a9327", "1307161012079589890");
                    sendBulkSMS("BIHAREDISTRICT-excise", "Excise@123", "BRGOVT",mul, messgae, "584fdb0f - 6287 - 490f - a071 - 6102880a9327", "1307161012079589890");
    
                    smss = "Y";
                }

               

                LinkButton btn = grdLabInchargeLogin.Rows[RemoveAt].FindControl("btnSMSCol") as LinkButton;
                    btn.ForeColor = System.Drawing.Color.Green;
                    //}
                
            }
            if (e.CommandName == "Email")
            {

                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RemoveAt = gvr.RowIndex;
                Session["index"] = RemoveAt;
                // string a = (gvr.Cells[grdLabInchargeLogin.Rows.Cells.Count -1].FindControl("lblemail") as Label).Text.ToString();
                string a = (grdLabInchargeLogin.Rows[RemoveAt].FindControl("lblemail") as Label).Text;

                Session["formno"] = (grdLabInchargeLogin.Rows[RemoveAt].FindControl("lblForm") as Label).Text;
                Session["email"] = a;
                //Bhavin
                SendEmail();


            }
            if(e.CommandName == "Download")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RemoveAt = gvr.RowIndex;
                Session["index"] = RemoveAt;
                Session["formno"] = (grdLabInchargeLogin.Rows[RemoveAt].FindControl("lblForm") as Label).Text;
                download();
            }
        }


     

        public void download()
        {
           
               
                server = new List<Server_Configs>();
                server = BL_SeverConfig.GetServerList("");
                //IReportServerCredentials irsc = new CustomReportCredentials("Admin", "Admin123", "NAVEEN"); // e.g.: ("demo-001", "123456789", "ifc")
                IReportServerCredentials irsc = new CustomReportCredentials(server[0].server_user, server[0].server_password, server[0].server_domain);

                Session["ReportProject1"] = server[0].projectname;

                string ReportPath = "/" + Session["ReportProject1"].ToString() + "/Lab_incharge_letter";
                ReportParameter[] Param = new Microsoft.Reporting.WebForms.ReportParameter[1];
                Param[0] = new ReportParameter("Parameter1", Session["formno"].ToString());
                //Param[1] = new ReportParameter("Parameter2", "2021-2022");
                //Session["IndentNO"] = 7;
                string fileName = "Lab_incharge_letter_" + Session["formno"].ToString();
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                var viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Remote;
                //  IReportServerCredentials irsc = new CustomReportCredentials(server[0].server_user, server[0].server_password, server[0].server_domain);
                viewer.ServerReport.ReportServerCredentials = irsc;
                viewer.ServerReport.ReportServerUrl = new Uri(server[0].server_url);
                viewer.ServerReport.ReportPath = "" + ReportPath;
                viewer.ServerReport.SetParameters(Param.ToList());
                byte[] bytes = viewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension,
                    out streamIds, out warnings);
                File.WriteAllBytes(Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf"), bytes);
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName + ".pdf"));
                if (File.Exists(Server.MapPath("~/All_Approved_Docs/" + Path.GetFileName(fileName + ".pdf"))))
                    Response.WriteFile(Server.MapPath("~/All_Approved_Docs/" + Path.GetFileName(fileName + ".pdf")));
                else
                    Response.WriteFile(Server.MapPath("~/All_Approved_Docs/" + Path.GetFileName(fileName + ".pdf")));
            if (Session["index"] != null)
            {

                int a = Convert.ToInt32(Session["index"]);
                LinkButton btn = grdLabInchargeLogin.Rows[a].FindControl("btnDownloadCol") as LinkButton;
                btn.ForeColor = System.Drawing.Color.Green;
            }

            Response.End();
           
        }

        //Bhavin
        public void SendEmail()
        {

            // ServicePointManager.ServerCertificateValidationCallback =
            //delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};

            // using (var message = new MailMessage())
            // {
            //     string toEmail = Session["email_Id"].ToString();
            //     //message.To.Add(new MailAddress("bhavin@rifesoftware.com"));
            //     message.To.Add(toEmail);
            //     message.Subject = "This is IEMS EMAIL";
            //     message.Body = "This EMAIL from IEMS LAB";
            //     message.IsBodyHtml = true;

            //     try
            //     {
            //         using (var smtp = new SmtpClient())
            //         {
            //             smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            //             smtp.Send(message);

            //         }

            //     }
            //     catch (Exception ex)
            //     {
            //         // Logger.Log("11). added CC email >>" + ex.Message);
            //         throw new Exception(ex.Message);
            //     }
            // }

            server = new List<Server_Configs>();
            server = BL_SeverConfig.GetServerList("");
            //IReportServerCredentials irsc = new CustomReportCredentials("Admin", "Admin123", "NAVEEN"); // e.g.: ("demo-001", "123456789", "ifc")
            IReportServerCredentials irsc = new CustomReportCredentials(server[0].server_user, server[0].server_password, server[0].server_domain);
         
            Session["ReportProject1"] = server[0].projectname;

            string ReportPath = "/" + Session["ReportProject1"].ToString() + "/Lab_incharge_letter";
            ReportParameter[] Param = new Microsoft.Reporting.WebForms.ReportParameter[1];
            Param[0] = new ReportParameter("Parameter1", Session["formno"].ToString());
            //Param[1] = new ReportParameter("Parameter2", "2021-2022");
            //Session["IndentNO"] = 7;
            string fileName = "Lab_incharge_letter_" + Session["formno"].ToString();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Remote;
          //  IReportServerCredentials irsc = new CustomReportCredentials(server[0].server_user, server[0].server_password, server[0].server_domain);
            viewer.ServerReport.ReportServerCredentials = irsc;
            viewer.ServerReport.ReportServerUrl = new Uri(server[0].server_url);
            viewer.ServerReport.ReportPath = "" + ReportPath;
            viewer.ServerReport.SetParameters(Param.ToList());
            byte[] bytes = viewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);
            File.WriteAllBytes(Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf"), bytes);
            //  txtpdf.Text = Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf");
            //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "GetRequestData();", true);

            //string FilePath = Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf");
            //WebClient User = new WebClient();
            //Byte[] FileBuffer = User.DownloadData(FilePath);
            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //}



            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("controlroom@prohibitionbihar.in");
            string email = "parthasarathiks67@gmail.com,rakeshm@datacontech.com,charan@datacontech.com,marithangam31196@gmail.com,mukeshkumar@datacontech.com";/* Session["email"].ToString();*/
            string[] mulp = email.Split(',');
            foreach(string mul in mulp)
            {

               msg.To.Add(new MailAddress(mul));
               // msg.To.Add("charan@datacontech.com");
            }
            
           // msg.To.Add("mukeshkumar@datacontech.com");
            msg.Subject = "This is IEMS EMAIL";
                string FileName = Path.GetFileName(fileName);
            string File1 = (Server.MapPath("~/All_Approved_Docs/"+FileName+".pdf"));
            msg.Attachments.Add(new Attachment(File1));
         
            msg.Body = "This email from IEMS CHEMICAL LAB";
            SmtpClient smt = new SmtpClient();
            smt.Host = "smtpout.asia.secureserver.net";
            // smt.Host = "smtp.gmail.com";
            System.Net.NetworkCredential ntwd = new NetworkCredential();
            ntwd.UserName = "controlroom@prohibitionbihar.in"; //Your Email ID  
            ntwd.Password = "IEMS@123"; // Your Password  
            smt.UseDefaultCredentials = true;
            smt.Credentials = ntwd;
            smt.Port = 587;
            smt.EnableSsl = true;
            try
            {
                smt.Send(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Email sent.');", true);
                //foreach (GridViewRow dr1 in grdLabInchargeLogin.Rows)
                //{
                //    LinkButton btn = dr1.FindControl("btnEmailCol") as LinkButton;
                //    btn.ForeColor = System.Drawing.Color.Green;
                //}
                if (Session["index"] != null)
                {
                   
                    int a = Convert.ToInt32(Session["index"]);
                    LinkButton btn = grdLabInchargeLogin.Rows[a].FindControl("btnEmailCol") as LinkButton;
                    btn.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Email not sent.');", true);
                if (Session["index"] != null)
                {
                    int a = Convert.ToInt32(Session["index"]);
                    LinkButton btn = grdLabInchargeLogin.Rows[0].FindControl("btnEmailCol") as LinkButton;
                    btn.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        public List<LabInchargeList> GetList(string query)
        {
            List<LabInchargeList> items = new List<LabInchargeList>();
            using (var command = GetSqlCommand(query))
            {
                command.Connection.Open();
                NpgsqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string t_form_no = dr.GetString(dr.GetOrdinal("form_no"));
                        string t_fir_no = dr.GetString(dr.GetOrdinal("fir_no"));
                        int t_total = dr.GetInt32(dr.GetOrdinal("quantity_count"));
                        int t_status_code = dr.GetInt32(dr.GetOrdinal("quantity_status_overall"));
                        int t_untested = dr.GetInt32(dr.GetOrdinal("quantity_untested"));
                        int t_tested = dr.GetInt32(dr.GetOrdinal("quantity_tested"));
                        int t_retest = dr.GetInt32(dr.GetOrdinal("quantity_retest"));
                        int t_verified = dr.GetInt32(dr.GetOrdinal("quantity_verified"));
                        string t_email = dr.GetString(dr.GetOrdinal("emailid"));
                        string t_mobileno = dr.GetString(dr.GetOrdinal("mobileno"));
                        items.Add(new LabInchargeList(t_form_no,t_fir_no, t_total, t_status_code, t_untested, t_tested, t_retest, t_verified,t_email,t_mobileno));
                    }
                }
                command.Connection.Close();
            }
            return items;
        }

        protected void grdLabInchargeLogin_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLabInchargeLogin.PageIndex = e.NewPageIndex;
            string query = get_query();
            LoadGrid(query);
        }

        private void LoadGrid(string query)
        {
            grdLabInchargeLogin.DataSource = GetList(query);
            grdLabInchargeLogin.DataBind();
        }

        //avani
        public String btnSMSCol_Click(String username, String password, String senderid, String mobileNos, String message, String secureKey, String templateid)
        {
            try
            {
                Stream dataStream;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;

                request.ServicePoint.ConnectionLimit = 1;

                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";

                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
                String encryptedPassword = encryptedPasswod(password);
                String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
                Console.Write(NewsecureKey);
                Console.Write(encryptedPassword);
                String smsservicetype = "bulkmsg"; // for bulk msg
                String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(message.Trim()) +
                "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +
                "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
                "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());

                Console.Write(query);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);

                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = byteArray.Length;

                dataStream = request.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse response = request.GetResponse();

                String Status = ((HttpWebResponse)response).StatusDescription;


                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);


                String responseFromServer = reader.ReadToEnd();

                reader.Close();

                dataStream.Close();

                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result = System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {

                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();

        }


        protected String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("controlroom@prohibitionbihar.in");
            msg.To.Add("charan@datacontech.com");
            msg.Subject = "Issues";
            //if (idupDocument.HasFile)
            //{
            //    string FileName = Path.GetFileName(idupDocument.PostedFile.FileName);
            //    msg.Attachments.Add(new Attachment(idupDocument.PostedFile.InputStream, FileName));
            //}
            msg.Body = "Chemical Lab";
            SmtpClient smt = new SmtpClient();
            smt.Host = "smtpout.asia.secureserver.net";
            // smt.Host = "smtp.gmail.com";
            System.Net.NetworkCredential ntwd = new NetworkCredential();
            ntwd.UserName = "controlroom@prohibitionbihar.in"; //Your Email ID  
            ntwd.Password = "IEMS@123"; // Your Password  
            smt.UseDefaultCredentials = true;
            smt.Credentials = ntwd;
            smt.Port = 587;
            smt.EnableSsl = true;
            try
            {
                smt.Send(msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Email sent.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Email not sent.');", true);
            }
        }


        protected void btnSMS_Click(object sender, EventArgs e)
        {

            var request = (HttpWebRequest)WebRequest.Create("http://sms.hspsms.com/sendSMS");
            var postData = "&username=yogibaluragi@datacontech.com";
            postData += "&message=Dear User,"
                     + " NEHA KUMARI (IDB1072) Exit Allowed at CALL CENTER BIHAR on 11/04/2022 02:40:08 PM"
                     + "DATACON TECHNOLOGIES";
            postData += "&sendername=DATACO";
            postData += "&smstype=TRANS";
            postData += "&numbers=7892437909";

            postData += "&apikey=13a25db6-1fc1-4b5a-acb9-1e5b2cf1bb90";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public class CustomReportCredentials : IReportServerCredentials
        {
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get { return null; }
            }

            public ICredentials NetworkCredentials
            {
                get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
            }

            public bool GetFormsCredentials(out Cookie authCookie, out string user,
             out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;
                return false;
            }


        }


        public void ExportToPDF(string path, List<ReportParameter> reportParams)
        {
            
           string ReportPath = "/" + Session["ReportProject1"].ToString() + "/molasses_indent_mf1";
            ReportParameter[] Param = new Microsoft.Reporting.WebForms.ReportParameter[2];
            Param[0] = new ReportParameter("Parameter1", Session["SeizureNo"].ToString());
            Param[1] = new ReportParameter("Parameter2", "2021-2022");

            string fileName = "MF1_" +Param.ToList()[0];
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Remote;
            IReportServerCredentials irsc = new CustomReportCredentials(server[0].server_user, server[0].server_password, server[0].server_domain);
            viewer.ServerReport.ReportServerCredentials = irsc;
            viewer.ServerReport.ReportServerUrl = new Uri(server[0].server_url);
            viewer.ServerReport.ReportPath = "" + ReportPath;
            viewer.ServerReport.SetParameters(Param.ToList());
            byte[] bytes = viewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension,
                out streamIds, out warnings);
            File.WriteAllBytes(Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf"), bytes);
            //  txtpdf.Text = Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf");
            //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "GetRequestData();", true);

            string FilePath = Server.MapPath("~/All_Approved_Docs/" + fileName + ".pdf");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }



        }




        public String sendBulkSMS(String username, String password, String senderid, String mobileNos, String message, String secureKey, String templateid)
        {
            Stream dataStream;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;

            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";

            request.Method = "POST";

            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            Console.Write(NewsecureKey);
            Console.Write(encryptedPassword);

            String smsservicetype = "bulkmsg"; // for bulk msg

            //String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +

            // "&password=" + HttpUtility.UrlEncode(encryptedPassword) +

            // "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

            // "&content=" + HttpUtility.UrlEncode(message.Trim()) +

            // "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +

            // "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +

            //"&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim());



            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) +

            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +

            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

            "&content=" + HttpUtility.UrlEncode(message.Trim()) +

            "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +

            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +

           "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +

       "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());


            Console.Write(query);

            byte[] byteArray = Encoding.ASCII.GetBytes(query);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;

            dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse response = request.GetResponse();

            String Status = ((HttpWebResponse)response).StatusDescription;

            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            String responseFromServer = reader.ReadToEnd();

            reader.Close();

            dataStream.Close();

            response.Close();
            return responseFromServer;

        }

    }

}


class MyPolicy : ICertificatePolicy
{
    public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
    {
        return true;
    }
}




//end


