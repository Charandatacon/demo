﻿<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Npgsql;
using System.Linq;


public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        // level mapping for each column
        string[] levelValueMapping = new string[] { "District", "Thana" };


        string drillLevel = context.Request["drillLevel"];
        string query = "";
        string label = "";
        if (string.IsNullOrEmpty(drillLevel))
        {
            drillLevel = "0";
            // build custom query 
            // parameter: column to be fetch
            query = BuildQuery(levelValueMapping[(Convert.ToInt16(drillLevel))]);
        }
        else
        {
            drillLevel = (Convert.ToInt16(drillLevel) + 1).ToString();
            label = context.Request["label"];
            // build custom query 
            // parameter: column to be fetch, previously clicked value, previous level column name
            query = BuildQuery(levelValueMapping[(Convert.ToInt16(drillLevel))], label, levelValueMapping[(Convert.ToInt16(drillLevel) - 1)]);
        }

        DataTable dt = new DataTable();
        // establish DB connection and fetch chart data
        GetChartData(ref dt, query);
        // from DB data create chart compatible json
        string chartJsonData = ProcessChartData(dt, levelValueMapping[(Convert.ToInt16(drillLevel))], drillLevel, levelValueMapping.Length);
        // send response
        context.Response.Write(chartJsonData);



    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    private string BuildQuery(string columnName)
    {
        string query;
        query = "select " + columnName + ", count(seizureno) as SeizureNoCount" + " from exciseautomation.casemgmt_basicinformation group by  " + columnName;
        return query;
    }
    private string BuildQuery(string columnName, string parentValue, string parentName)
    {
        string query;
        query = "select " + columnName + ", count(seizureno) as SeizureNoCount" + " from exciseautomation.casemgmt_basicinformation where " + parentName + "= '" + parentValue + "' Group by " + columnName;
        return query;

    }
    private string ProcessChartData(DataTable dt, string columnName, string drillLevel, int maxLevel)
    {
        StringBuilder jsonData = new StringBuilder();
        StringBuilder data = new StringBuilder();
        // store chart config name-config value pair

        Dictionary<string, string> chartConfig = new Dictionary<string, string>();

        string linkParam = "newchart-jsonurl-Handler/ThanaHandler.ashx?label={0}&drillLevel={1}";

        chartConfig.Add("caption", "Total Seizures by " + columnName); // caption will change dynamically based on chart label
        chartConfig.Add("xAxisName", columnName); // xaxis name will chnage dynamically based on chart label
        chartConfig.Add("yAxisName", "Total Seizures");
        chartConfig.Add("numberSuffix", "");
        chartConfig.Add("theme", "fusion");

        chartConfig.Add("showvalues", "1");
        chartConfig.Add("animation", "1");
        chartConfig.Add("animationDuration", "5");
        chartConfig.Add("exportEnabled", "1");
        chartConfig.Add("showHoverEffect", "1");
        chartConfig.Add("plotHoverEffect", "1");

        // json data to use as chart data source
        jsonData.Append("{\"chart\":{");
        foreach (var config in chartConfig)
        {
            jsonData.AppendFormat("\"{0}\":\"{1}\",", config.Key, config.Value);
        }
        jsonData.Replace(",", "},", jsonData.Length - 1, 1);
        data.Append("\"data\":[");

        //iterate through data table to build data object
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt16(drillLevel) < maxLevel - 1)
                {
                    string link = string.Format(linkParam, row[0].ToString(), drillLevel);
                    data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\", \"link\": \"{2}\"}},", row[0].ToString(), row[1].ToString(), link);
                }
                else // for last level, link attribute will not be added
                {
                    data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", row[0].ToString(), row[1].ToString());
                }
            }
        }
        data.Replace(",", "]", data.Length - 1, 1);

        jsonData.Append(data.ToString());
        jsonData.Append("}");
        return jsonData.ToString();
    }

    private void GetChartData(ref DataTable dt, string query)
    {

        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["CASEMGMT"].ConnectionString;
        NpgsqlConnection cn = new NpgsqlConnection(connstring);



        using (NpgsqlConnection con = new NpgsqlConnection(connstring))
        {
            con.Open();
            using (NpgsqlCommand command = new NpgsqlCommand())
            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, cn))
            {
                // fill data table
                da.Fill(dt);

            }

        }
    }

}