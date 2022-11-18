using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

namespace UserMgmt
{
    public partial class LabInchargeVerify : System.Web.UI.Page
    {
        public class parameterChecklist
        {
            public string txt { get; set; }
            public bool tick { get; set; }

            public parameterChecklist() { }

            public parameterChecklist(string Txt, bool Tick)
            {
                this.txt = Txt;
                this.tick = Tick;
            }

        }

        public class parameterTextlist
        {
            public string txt { get; set; }
            public string input { get; set; }

            public parameterTextlist() { }

            public parameterTextlist(string Txt, string Input)
            {
                this.txt = Txt;
                this.input = Input;
            }
        }
        public int assign_parameter_id;
        public string form_no, qid, mode;
        public int form_no_int, qid_int;
        public string quantity, batch_no, address, status;
        public int compactor_id, liq_id, liq_sub_type_id;
        public string liq_type, liq_sub_type_name, size_name, brand_name;
        public string date_of_creation, proof_type, color, smell, remarks;
        public float proof_strength;
        public int labdevice, temperature, parametertestresult, parametertest;
        public string indication, pyknometerempty, pyknometerdmwater, pyknometersample;
        public string parameter_ids, parameter_checked, parameter_text, testremarks;
        public string formanddate;
        public string hydroChecked = "", pyknoChecked = "";
        public string pyknotemperature = "", hydrotemperature = "";
        public string assigned_parameter_str;
        public Dictionary<int, string> dict;
        public List<string> assigned_para_list, checked_para_list, input_list;
        public List<parameterChecklist> parameterchecklist;
        public List<parameterTextlist> parametertextlist;
        public List<string> colors;
        public bool btnActive;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            form_no = "";
            qid = "";
            mode = "";
            form_no_int = -1;
            qid_int = -1;
            dict = new Dictionary<int, string>();
            assigned_para_list = new List<string>();
            checked_para_list = new List<string>();
            parameterchecklist = new List<parameterChecklist>();
            parametertextlist = new List<parameterTextlist>();
            input_list = new List<string>();
            colors = new List<string>();
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
                populate();
            }
        }

        public void disable()
        {
            proofDropdown.Enabled = false;
            txtProofstr.Enabled = false;
            colorDropdown.Enabled = false;
            txtSmell.Enabled = false;
            //txtRemarks.Enabled = false;
            txtRemarks.Enabled = false;
            labdeviceradiohydro.Enabled = false;
            labdeviceradiopykno.Enabled = false;
            txtIndication.Enabled = false;
            txtHydrotemp.Enabled = false;
            txtPyknometerempty.Enabled = false;
            txtPyknometerdmwater.Enabled = false;
            txtPyknometersample.Enabled = false;
            txtPyknotemperature.Enabled = false;
            parameterpasses.Enabled = false;
            parameterpassesbyvalue.Enabled = false;
            testresultpassed.Enabled = false;
            testresultnotpassed.Enabled = false;
            txtTestRemarks.Enabled = false;
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                string old_status = "";
                form_no = Request.QueryString["form_no"];
                form_no_int = int.Parse(form_no);
                qid = Request.QueryString["qid"];
                qid_int = int.Parse(qid);
                var query = $@"select status from exciseautomation.tab_quant_received where quant_received_id={qid_int} LIMIT 1";
                using (var command = GetSqlCommand(query))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            old_status = dr.GetString(dr.GetOrdinal("status"));
                        }
                    }
                    command.Connection.Close();
                }

                string query_update_status = $@"update exciseautomation.tab_quant_received set status='verified' where quant_received_id={qid_int}";
                using (var command = GetSqlCommand(query_update_status))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }

                int quantity_count = -1, quantity_status_overall = -1, quantity_untested = -1, quantity_tested = -1, quantity_retest = -1, quantity_verified = -1;
                int quantity_verified_final = -1;
                var query2 = $@"select * from exciseautomation.tab_form_no_status where form_no='{form_no}' LIMIT 1";
                using (var command = GetSqlCommand(query2))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            quantity_count = dr.GetInt32(dr.GetOrdinal("quantity_count"));
                            quantity_status_overall = dr.GetInt32(dr.GetOrdinal("quantity_status_overall"));
                            quantity_untested = dr.GetInt32(dr.GetOrdinal("quantity_untested"));
                            quantity_tested = dr.GetInt32(dr.GetOrdinal("quantity_tested"));
                            quantity_retest = dr.GetInt32(dr.GetOrdinal("quantity_retest"));
                            quantity_verified = dr.GetInt32(dr.GetOrdinal("quantity_verified"));
                        }
                    }
                    command.Connection.Close();
                }

                if (old_status == "tested")
                {
                    quantity_verified_final = quantity_verified + 1;
                    if (quantity_untested >= 1) quantity_status_overall = 3;
                    else if (quantity_retest >= 1) quantity_status_overall = 3;
                    else if ((quantity_count > quantity_verified_final) && (quantity_tested >= 1) && (quantity_retest == 0) && (quantity_untested == 0))
                    {
                        quantity_status_overall = 1;
                    }
                    else if ((quantity_count == quantity_verified_final) && (quantity_tested >= 1) && (quantity_retest == 0) && (quantity_untested == 0))
                    {
                        quantity_status_overall = 2;
                    }

                    string query_update2 = $@"update exciseautomation.tab_form_no_status set quantity_status_overall={quantity_status_overall},quantity_verified=quantity_verified+1 where form_no={form_no}";
                    using (var command = GetSqlCommand(query_update2))
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                    }
                }
                Response.Redirect($"~/LabInchargeView.aspx?form_no={form_no}");
            }
            catch (Exception ex)
            {
                Response.Write("Contact Admin : " + ex.Message.ToString());
                populate();
            }

        }

        public void populate()
        {
            
            form_no = Request.QueryString["form_no"];
            form_no_int = int.Parse(form_no);
            qid = Request.QueryString["qid"];
            qid_int = int.Parse(qid);
            mode = Request.QueryString["mode"];
            if (mode == "2")
            {
                btnActive = true;
            }
            else
            {
                btnActive = false;
            }
            try
            {
                var query = $@"SELECT
                            A.quant_received_id,
                            A.quantity,
                            A.batch_no,
                            A.address,
                            A.compactor_id,
                            A.status,
                            B.liq_type,
                            B.liq_id,
                            C.liq_sub_type_id,
                            C.liq_sub_type_name,
                            D.size_name,
                            E.brand_name,
                            F.form_no,
                            G.date_of_creation,
                            H.proof_strength,
                            H.proof_type,
                            H.color,
                            H.smell,
                            H.remarks,
                            I.parametertestresult,
                            I.labdevice,
                            I.indication,
                            I.temperature,
                            I.pyknometerempty, 
                            I.pyknometerdmwater,
                            I.pyknometersample,
                            I.parametertest,
                            I.parameter_ids,
                            I.parameter_checked,
                            I.parameter_text,
                            I.testremarks
                           FROM
                            exciseautomation.tab_quant_received A
                            LEFT JOIN exciseautomation.tab_liq_type B ON A.liq_type_id=B.liq_id
                            LEFT JOIN exciseautomation.tab_liq_subtype C ON A.sub_type_id=C.liq_sub_type_id
                            LEFT JOIN exciseautomation.tab_liq_size D ON A.size_id=D.size_id
                            LEFT JOIN exciseautomation.tab_liq_brand_name E ON A.brand_name_id=E.brand_id
                            LEFT JOIN exciseautomation.tab_quant_form_no F ON A.quant_received_id=F.quant_received_id
                            LEFT JOIN exciseautomation.tab_form_no_date G ON F.form_no = G.form_no
                            LEFT JOIN exciseautomation.tab_quant_id_report H ON A.quant_received_id = H.quant_received_id
                            LEFT JOIN exciseautomation.tab_quant_id_report_parameter I ON A.quant_received_id = I.quant_received_id
                           WHERE
                            A.quant_received_id={qid_int}";

                using (var command = GetSqlCommand(query))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            quantity = dr.IsDBNull(dr.GetOrdinal("quantity")) ? "" : dr.GetString(dr.GetOrdinal("quantity"));
                            batch_no = dr.IsDBNull(dr.GetOrdinal("batch_no")) ? "" : dr.GetString(dr.GetOrdinal("batch_no"));
                            address = dr.IsDBNull(dr.GetOrdinal("address")) ? "" : dr.GetString(dr.GetOrdinal("address"));
                            status = dr.IsDBNull(dr.GetOrdinal("status")) ? "" : dr.GetString(dr.GetOrdinal("status"));
                            compactor_id = dr.GetInt32(dr.GetOrdinal("compactor_id"));
                            liq_id = dr.GetInt32(dr.GetOrdinal("liq_id"));
                            liq_sub_type_id = dr.GetInt32(dr.GetOrdinal("liq_sub_type_id"));
                            liq_type = dr.IsDBNull(dr.GetOrdinal("liq_type")) ? "" : dr.GetString(dr.GetOrdinal("liq_type"));
                            liq_sub_type_name = dr.IsDBNull(dr.GetOrdinal("liq_sub_type_name")) ? "" : dr.GetString(dr.GetOrdinal("liq_sub_type_name"));
                            size_name = dr.IsDBNull(dr.GetOrdinal("size_name")) ? "" : dr.GetString(dr.GetOrdinal("size_name"));
                            brand_name = dr.IsDBNull(dr.GetOrdinal("brand_name")) ? "" : dr.GetString(dr.GetOrdinal("brand_name"));
                            proof_type = dr.IsDBNull(dr.GetOrdinal("proof_type")) ? "" : dr.GetString(dr.GetOrdinal("proof_type"));
                            color = dr.IsDBNull(dr.GetOrdinal("color")) ? "" : dr.GetString(dr.GetOrdinal("color"));
                            smell = dr.IsDBNull(dr.GetOrdinal("smell")) ? "" : dr.GetString(dr.GetOrdinal("smell"));
                            remarks = dr.IsDBNull(dr.GetOrdinal("remarks")) ? "" : dr.GetString(dr.GetOrdinal("remarks"));
                            proof_strength = dr.IsDBNull(dr.GetOrdinal("proof_strength")) ? (float)0.0 : dr.GetFloat(dr.GetOrdinal("proof_strength"));
                            DateTime creationDate = dr.GetDateTime(dr.GetOrdinal("date_of_creation"));
                            date_of_creation = creationDate.ToString();
                            formanddate = form_no + '/' + date_of_creation;
                            labdevice = dr.IsDBNull(dr.GetOrdinal("labdevice")) ? -1 : dr.GetInt32(dr.GetOrdinal("labdevice"));
                            temperature = dr.IsDBNull(dr.GetOrdinal("temperature")) ? -1 : dr.GetInt32(dr.GetOrdinal("temperature"));
                            parametertest = dr.IsDBNull(dr.GetOrdinal("parametertest")) ? -1 : dr.GetInt32(dr.GetOrdinal("parametertest"));
                            parametertestresult = dr.IsDBNull(dr.GetOrdinal("parametertestresult")) ? -1 : dr.GetInt32(dr.GetOrdinal("parametertestresult"));
                            indication = dr.IsDBNull(dr.GetOrdinal("indication")) ? "" : dr.GetString(dr.GetOrdinal("indication"));
                            pyknometerempty = dr.IsDBNull(dr.GetOrdinal("pyknometerempty")) ? "" : dr.GetString(dr.GetOrdinal("pyknometerempty"));
                            pyknometerdmwater = dr.IsDBNull(dr.GetOrdinal("pyknometerdmwater")) ? "" : dr.GetString(dr.GetOrdinal("pyknometerdmwater"));
                            pyknometersample = dr.IsDBNull(dr.GetOrdinal("pyknometersample")) ? "" : dr.GetString(dr.GetOrdinal("pyknometersample"));
                            parameter_ids = dr.IsDBNull(dr.GetOrdinal("parameter_ids")) ? "" : dr.GetString(dr.GetOrdinal("parameter_ids"));
                            parameter_checked = dr.IsDBNull(dr.GetOrdinal("parameter_checked")) ? "" : dr.GetString(dr.GetOrdinal("parameter_checked"));
                            parameter_text = dr.IsDBNull(dr.GetOrdinal("parameter_text")) ? "" : dr.GetString(dr.GetOrdinal("parameter_text"));
                            testremarks = dr.IsDBNull(dr.GetOrdinal("testremarks")) ? "" : dr.GetString(dr.GetOrdinal("testremarks"));
                        }
                    }
                    command.Connection.Close();
                }
                if (parameter_checked.Length != 0)
                {
                    checked_para_list = parameter_checked.Split(',').ToList<string>();
                }
                if (parameter_text.Length != 0)
                {
                    input_list = parameter_text.Split(',').ToList<string>();
                }
                //var query2 = $@"SELECT parameter_id, parameter_type FROM exciseautomation.tab_parameter order by parameter_type asc ";
                var query2 = $@"SELECT parameter_master_id, parameter_master_name FROM exciseautomation.parameter_master order by parameter_master_name asc ";
                using (var command = GetSqlCommand(query2))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int t1 = dr.GetInt32(dr.GetOrdinal("parameter_master_id"));
                            string t2 = dr.GetString(dr.GetOrdinal("parameter_master_name"));
                            dict.Add(t1, t2);
                        }
                    }
                    command.Connection.Close();
                }
                var Assignparameter = $@"SELECT * FROM exciseautomation.assign_parameter WHERE liquor_sub_type_id='{liq_sub_type_id}' AND type_of_liquor_id='{liq_id}'";
                using (var command = GetSqlCommand(Assignparameter))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                             assign_parameter_id = dr.GetInt32(dr.GetOrdinal("assign_parameter_id"));
                           
                        }
                    }
                    command.Connection.Close();
                }
                var query3 = $@"SELECT * FROM exciseautomation.assign_parameter_assigned_list WHERE assign_parameter_id='{assign_parameter_id}'";
                using (var command = GetSqlCommand(query3))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //int t_liq_para_id = dr.GetInt32(dr.GetOrdinal("liq_parameter_id"));
                            //string t_assigned_parameter = dr.GetString(dr.GetOrdinal("assigned_parameter"));
                            int t_assigned_parameter = dr.GetInt32(dr.GetOrdinal("parameter_master_id")); //vishv
                            //if (t_assigned_parameter != 0)
                            //{
                            //    assigned_para_list = t_assigned_parameter.ToList<string>();
                            //}
                            assigned_parameter_str = dr.GetInt32(dr.GetOrdinal("parameter_master_id")).ToString();

                            ViewState["assigned"] = assigned_parameter_str;
                            //System.Diagnostics.Debug.WriteLine((string)ViewState["assigned"]);
                            if (assigned_parameter_str.Length != 0)
                            {
                                assigned_para_list = assigned_parameter_str.Split(',').ToList<string>();
                            }
                            //passes
                            for (int i = 0; i < assigned_para_list.Count; i++)
                            {
                                int val = int.Parse(assigned_para_list[i]);
                                // int val = t_assigned_parameter;
                                if (checked_para_list.Count > 0)
                                {
                                    if (checked_para_list.Contains(i.ToString()))
                                    {
                                        parameterchecklist.Add(new parameterChecklist(dict[val], true));
                                    }
                                    else
                                    {
                                        parameterchecklist.Add(new parameterChecklist(dict[val], false));
                                    }
                                }
                                else
                                {
                                    parameterchecklist.Add(new parameterChecklist(dict[val], false));
                                }
                            }
                            // passes by valuef




                            for (int i = 0; i < assigned_para_list.Count; i++)
                            {
                                int val = int.Parse(assigned_para_list[i]);
                                if (input_list.Count > i)
                                {
                                    parametertextlist.Add(new parameterTextlist(dict[val], input_list[assigned_para_list.Count]));
                                }
                                else
                                {
                                    parametertextlist.Add(new parameterTextlist(dict[val], ""));
                                }
                            }
                        }
                    }
                    command.Connection.Close();
                }

                var query4 = $@"select color_name from exciseautomation.tab_color order by color_name asc";
                using (var command = GetSqlCommand(query4))
                {
                    command.Connection.Open();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string clr = dr.GetString(dr.GetOrdinal("color_name"));
                            colors.Add(clr);
                        }
                    }
                    command.Connection.Close();
                }
                colorDropdown.DataSource = colors;
                colorDropdown.DataBind();
                colorDropdown.Items.Insert(0, "");
                colorDropdown.SelectedValue = color;
                proofDropdown.SelectedValue = proof_type;
                parametergrid1.DataSource = parameterchecklist;
                parametergrid1.DataBind();
                parametergrid2.DataSource = parametertextlist;
                parametergrid2.DataBind();

                if (labdevice == 0)
                {
                    hydrotemperature = temperature.ToString();
                }
                else if (labdevice == 1)
                {
                    pyknotemperature = temperature.ToString();
                }
                if (labdevice == 0)
                {
                    labdeviceradiohydro.Checked = true;
                    labdeviceradiopykno.Checked = false;
                }
                else if (labdevice == 1)
                {
                    labdeviceradiohydro.Checked = false;
                    labdeviceradiopykno.Checked = true;
                }

                if (parametertest == 0)
                {
                    parameterpasses.Checked = true;
                    parameterpassesbyvalue.Checked = false;
                }
                else if (parametertest == 1)
                {
                    parameterpasses.Checked = false;
                    parameterpassesbyvalue.Checked = true;
                }

                if (parametertestresult == 0)
                {
                    testresultpassed.Checked = true;
                    testresultnotpassed.Checked = false;
                }
                else if (parametertestresult == 1)
                {
                    testresultpassed.Checked = false;
                    testresultnotpassed.Checked = true;
                }
                txtProofstr.Text = proof_strength.ToString();
                txtSmell.Text = smell.ToString();
                txtRemarks.Text = remarks.ToString();
                txtIndication.Text = indication;
                txtHydrotemp.Text = hydrotemperature;
                txtPyknometerempty.Text = pyknometerempty;
                txtPyknometerdmwater.Text = pyknometerdmwater;
                txtPyknometersample.Text = pyknometersample;
                txtPyknotemperature.Text = pyknotemperature;
                txtTestRemarks.Text = testremarks;
            }

            catch (Exception ex)
            {
                Response.Write("Contact Admin : " + ex.Message.ToString());
            }
            disable();
        }

    }
}