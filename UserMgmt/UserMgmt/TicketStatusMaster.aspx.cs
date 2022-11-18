﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Usermngt.BL;
using Usermngt.Entities;

namespace UserMgmt
{
    public partial class TicketStatusMaster : System.Web.UI.Page
    {
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
                string rtype = Session["rtype"].ToString();
                Session["rtype"] = rtype;

                if (rtype != "0")
                {
                    string id = Session["ID"].ToString();
                    Session["ID"] = id;
                    txtid.Value = id;
                    List<Helpdesk> Ticket = new List<Helpdesk>();
                    Ticket = BL_HelpDesk.GetList(Session["UserID"].ToString());
                    var list = from s in Ticket
                               where s.ticketstatus_code == Session["Code"].ToString()
                               select s;
                    if (list.ToList().Count > 0)
                    {
                        txtTicketStatusCode.Attributes.Add("disabled", "disabled");
                        txtTicketStatusName.Attributes.Add("disabled", "disabled");
                    }
                    if (rtype == "1")
                    {
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                       txtTicketStatusCode.Text = Session["Code"].ToString();
                        txtTicketStatusName.Text = Session["Name"].ToString();
                        txtid.Value = Session["ID"].ToString();
                        txtTicketStatusCode.Enabled = false;
                        txtTicketStatusName.Enabled = false;
                       txtTicketStatusCode.BackColor = System.Drawing.Color.LightGray;
                        txtTicketStatusName.BackColor = System.Drawing.Color.LightGray;
                    }
                    if (rtype == "2")
                    {
                        txtTicketStatusCode.Text = Session["Code"].ToString();
                        txtTicketStatusName.Text = Session["Name"].ToString();
                        txtid.Value = Session["ID"].ToString();

                    }
                }
                else
                {
                    //int n = Convert.ToInt32(BL_org_Master.GetMaxID(""));
                    //txtid.Value = (n + 1).ToString();
                    //Session["ID"] = txtid.Value;
                    btnSave.Text = "Submit";
                    btnCancel.Text = "Cancel";
                }
            }
        }

        protected void ShowRecord_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect("TicketStatusLIst.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            TicketStatus ticket = new TicketStatus();
            string s = txtTicketStatusCode.Text.ToString();
            s = Regex.Replace(s, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
            ticket.ticketstatus_name = txtTicketStatusName.Text;
            ticket.ticketstatus_code= s;
           ticket.user_id = Session["UserID"].ToString();
            if (Session["rtype"].ToString() != "0")
            {
                ticket.ticketstatus_master_id = Convert.ToInt32(Session["ID"].ToString());
                if (BL_TicketStatus.Update(ticket))
                {
                    string message = "Record is Updated.";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", sb.ToString());
                    Session["UserID"] = Session["UserID"].ToString();
                    Response.Redirect("TicketStatusLIst.aspx");
                }
                else
                {

                }
            }
            else
            {
                if (BL_TicketStatus.Insert(ticket))
                {
                    string message = "Record is Inserted.";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", sb.ToString());
                    Session["UserID"] = Session["UserID"].ToString();
                    Response.Redirect("TicketStatusLIst.aspx");
                }
                else
                {

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect("TicketStatusLIst.aspx");
        }
        static string ticket = "";
        [WebMethod]
        public static string chkDuplicateAccessTypeName(Object name)
        {
            int value = 0;
            if (ticket != name.ToString())
            {
                string s = name.ToString();
                s = Regex.Replace(s, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
                value = BL_HelpDesk.GetExistsData("ticketstatus_master", "ticketstatus_name", s);
            }
            return value.ToString();
        }

        protected void Priority_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect("PriorityList.aspx");
        }

        protected void Helpdesk_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect("HelpDeskList.aspx");
        }

        protected void Ticket_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect("TicketStatusLIst.aspx");
        }
        protected void Ticketcategory_Click(object sender, EventArgs e)
        {
            Session["UserID"] = Session["UserID"].ToString();
            Response.Redirect(" TicketcategoryMasterList.aspx");
        }
    }
}