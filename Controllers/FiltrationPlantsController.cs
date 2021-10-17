using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{

    public class FiltrationPlantsController : Controller
    {
        // GET: Tubewells
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            int pumpsRunning = 0;
            double tankLevel = 0;
            DataTable dt = new DataTable();
            string S13query = "select count( DISTINCT r.resourceID )from tblEnergy s ";
            S13query += "inner join tblResource r on s.resourceID = r.resourceID ";
            S13query += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
            S13query += "where r.TemplateID = 68 and InsertionDateTime > DATEADD(MINUTE, -21, GETDATE()) ";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "select TOP(1) [Pump No. 1],[Pump No. 2],[Voltage],[Current],[Power Factor],[Frequency],[Power (KVA)],[Power (KVAR)],[Power (KW)],[Water Flow],[Chlorine Level],[Tank No. 1],[Tank No. 2],[TDS],[Manual Mode],[InsertionDateTime] ";
                    query += " from tblWaterFilteration order by ID DESC";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    sda.Fill(dt);
                    SqlCommand cmd1 = new SqlCommand(S13query, conn);
                    ViewBag.TotalStorageTank = 1;
                    int workingStorageTanks = Convert.ToInt32(cmd1.ExecuteScalar());
                    ViewBag.ActiveStorageTank = workingStorageTanks;
                    ViewBag.InactiveStorageTank = 1 - workingStorageTanks;
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }
            var data = new { status = "Success" };
            ViewBag.PumpsRunning = pumpsRunning.ToString();
            ViewBag.TankLevel = tankLevel.ToString();
            string chartdata1 = "";
            string[] arr = new string[16] ;


            chartdata1 += "[";
            if (Convert.ToDouble(dt.Rows[0][0]) > 0)
            {
                chartdata1 += "{\"category\":\"Pump No. 1\",\"tooltip\":\"" + Convert.ToDouble(dt.Rows[0][0])*10 + "\",\"value\":\"ON\"},";
                arr[0] = "ON";
            }
            else
            {
                chartdata1 += "{\"category\":\"Pump No. 1\",\"tooltip\":\"0\",\"value\":\"OFF\"},";
                arr[0] = "OFF";
            }
            if (Convert.ToDouble(dt.Rows[0][1]) > 0)
            {
                chartdata1 += "{\"category\":\"Pump No. 2\",\"tooltip\":\"" + Convert.ToDouble(dt.Rows[0][1]) * 10 + "\",\"value\":\"ON\"},";
                arr[1] = "ON";
            }
            else
            {
                chartdata1 += "{\"category\":\"Pump No. 2\",\"tooltip\":\"0\",\"value\":\"OFF\"},";
                arr[1] = "OFF";
            }
            chartdata1 += "{\"category\":\"Voltage\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][2]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][2]), 2) + "\"},";
            arr[2] = Math.Round(Convert.ToDouble(dt.Rows[0][2]), 2).ToString();
            chartdata1 += "{\"category\":\"Current\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][3]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][3]), 2) + "\"},";
            arr[3] = Math.Round(Convert.ToDouble(dt.Rows[0][3]), 2).ToString();
            chartdata1 += "{\"category\":\"Power Factor\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][4]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][4]), 2) + "\"},";
            arr[4] = Math.Round(Convert.ToDouble(dt.Rows[0][4]), 2).ToString();
            chartdata1 += "{\"category\":\"Frequency\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][5]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][5]), 2) + "\"},";
            arr[5] = Math.Round(Convert.ToDouble(dt.Rows[0][5]), 2).ToString();
            chartdata1 += "{\"category\":\"Power (KVA)\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][6]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][6]), 2) + "\"},";
            arr[6] = Math.Round(Convert.ToDouble(dt.Rows[0][6]), 2).ToString();
            chartdata1 += "{\"category\":\"Power (KVAR)\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][7]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][7]), 2) + "\"},";
            arr[7] = Math.Round(Convert.ToDouble(dt.Rows[0][7]), 2).ToString();
            chartdata1 += "{\"category\":\"Power (KW)\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][8]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][8]), 2) + "\"},";
            arr[8] = Math.Round(Convert.ToDouble(dt.Rows[0][8]), 2).ToString();
            chartdata1 += "{\"category\":\"Water Flow\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][9]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][9]), 2) + "\"},";
            arr[9] = Math.Round(Convert.ToDouble(dt.Rows[0][9]), 2).ToString();
            if (Convert.ToDouble(dt.Rows[0][10]) > 0)
            {
                chartdata1 += "{\"category\":\"Chlorine Level\",\"tooltip\":\"" + Convert.ToDouble(dt.Rows[0][10]) * 10 + "\",\"value\":\"HIGH\"},";
                arr[10] = "HIGH";
            }
            else
            {
                chartdata1 += "{\"category\":\"Chlorine Level\",\"tooltip\":\"0\",\"value\":\"LOW\"},";
                arr[10] = "LOW";
            }
            chartdata1 += "{\"category\":\"Tank No. 1\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][11]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][11]), 2) + " ft\"},";
            arr[11] = Math.Round(Convert.ToDouble(dt.Rows[0][11]), 2).ToString();
            chartdata1 += "{\"category\":\"Tank No. 2\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][12]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][12]), 2) + " ft\"},";
            arr[12] = Math.Round(Convert.ToDouble(dt.Rows[0][12]), 2).ToString();
            chartdata1 += "{\"category\":\"TDS\",\"tooltip\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][13]), 2) + "\",\"value\":\"" + Math.Round(Convert.ToDouble(dt.Rows[0][13]), 2) + "\"},";
            arr[13] = Math.Round(Convert.ToDouble(dt.Rows[0][13]), 2).ToString();
            if (Convert.ToDouble(dt.Rows[0][14]) == 1)
            {
                chartdata1 += "{\"category\":\"Mode\",\"tooltip\":\"10\",\"value\":\"Manual\"}]";
                arr[14] = "Manual";
            }
            else
            {
                chartdata1 += "{\"category\":\"Mode\",\"tooltip\":\"20\",\"value\":\"Auto\"}]";
                arr[14] = "Auto";
            }
            arr[15] = dt.Rows[0][15].ToString();
            ViewData["amChartData1"] = chartdata1;
            return View(arr.ToList());
        }

        public ActionResult ParameterizedData()
        {
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> parameterList = new List<string>();
            IList<string> resourceList = new List<string>();
            string paramsListQuery = "select p.ParameterName from tblParameter p inner join tblTemplateParameter tp on tp.ParameterID = p.ParameterID where tp.TemplateID = 68 order by p.paramOrder";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    DataTable dtParams = new DataTable();
                    conn.Open();
                    SqlDataAdapter sdaParams = new SqlDataAdapter(paramsListQuery, conn);
                    sdaParams.Fill(dtParams);
                    foreach (DataRow dr in dtParams.Rows)
                    {
                        parameterList.Add(dr["ParameterName"].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            foreach (var item in db.tblResources.AsEnumerable().Where(item => item.CompanyID == 4 && item.TemplateID == 68))
            {
                resourceList.Add(item.ResourceLocation);
            }
            ResourceAndParameterSelector rs = new ResourceAndParameterSelector();
            rs.resourceID = "Pattoki Filtration Plant";
            rs.parameterID = "Water Flow";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            ViewBag.ResourceList = resourceList;
            ViewBag.ParameterList = parameterList;
            return View(rs);
        }

        [HttpPost]
        public ActionResult ParameterizedData(FormCollection review)
        {
            string resource = review["resource"].ToString();
            string Parameter = review["parameter"].ToString();
            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> parameterList = new List<string>();
            IList<string> resourceList = new List<string>();
            string paramsListQuery = "select p.ParameterName from tblParameter p inner join tblTemplateParameter tp on tp.ParameterID = p.ParameterID where tp.TemplateID = 68 order by p.paramOrder";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    DataTable dtParams = new DataTable();
                    conn.Open();
                    SqlDataAdapter sdaParams = new SqlDataAdapter(paramsListQuery, conn);
                    sdaParams.Fill(dtParams);
                    foreach (DataRow dr in dtParams.Rows)
                    {
                        parameterList.Add(dr["ParameterName"].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            foreach (var item in db.tblResources.AsEnumerable().Where(item => item.CompanyID == 4 && item.TemplateID == 68))
            {
                resourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = resourceList;
            ViewBag.ParameterList = parameterList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedParameter = Parameter;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            ResourceAndParameterSelector rs = new ResourceAndParameterSelector();
            rs.resourceID = resource;
            rs.parameterID = Parameter;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _ParameterizedDataView(string resources, string parameter, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;

            DataTable dtVal = new DataTable();
            string parameterName = "";
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date.AddSeconds(-1);
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM" || tt_time == "12:00 am" || tt_time == "11:59 pm")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<FiltrationPlantTableData>();

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID, ParameterName from tblParameter where ParameterName = '" + parameter + "'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        parameterName = drRes["ParameterName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName from tblResource r where r.ResourceName = '" + resources + "' ";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"" + parameter + "\" },exportEnabled: true,";
                        string TheSelectedResource = "All Tubewells";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells ";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + "";
                        }
                        scriptString += "subtitles: [{text: \" " + parameter + " Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        if (drRes["ParameterName"].ToString() == "Pump No. 1" || drRes["ParameterName"].ToString() == "Pump No. 2")
                        {
                            scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
                            scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: OFF</b> at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: ON</b> at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
                        }
                        else
                        {
                            scriptString += "axisY:{labelFontSize: 15},";
                            scriptString += "toolTip: { shared: false },";
                        }
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 12},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            string Dashdtquery = ";WITH cte AS ( ";
                            Dashdtquery += "SELECT* FROM ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                            Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                            Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                            Dashdtquery += "s.InsertionDateTime as tim ";
                            Dashdtquery += "FROM tblEnergy s ";
                            Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                            Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                            Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                            Dashdtquery += "where ";
                            Dashdtquery += "r.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and ";
                            Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            Dashdtquery += ") ";
                            Dashdtquery += "AS SourceTable ";
                            Dashdtquery += "PIVOT ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SUM(pVal) FOR pID ";
                            Dashdtquery += "IN ";
                            Dashdtquery += "( ";
                            if (drRes["ParameterName"].ToString() == "Pump No. 1" || drRes["ParameterName"].ToString() == "Pump No. 2")
                            {
                                Dashdtquery += "[Pump No. 1] , [Pump No. 2] ";
                            }
                            else
                            {
                                Dashdtquery += "[Pump No. 1] , [Pump No. 2]  ,[" + drRes["ParameterName"].ToString() + "]  ";
                            }
                            Dashdtquery += ") ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "AS PivotTable ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "SELECT* FROM cte ";
                            Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                            Dashdtquery += "tim DESC";
                            string theQuery = Dashdtquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            sdaVal.Fill(dtVal);
                            if (drRes["ParameterName"].ToString() == "Pump No. 1" || drRes["ParameterName"].ToString() == "Pump No. 1")
                            {
                                scriptString += "{ type: \"area\", name: \"" + parameter + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\",  ";
                            }
                            else
                            {
                                scriptString += "{ type: \"line\", name: \"" + parameter + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            }
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["tim"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Math.Abs(Convert.ToDouble(drVal[drRes["ParameterName"].ToString()]))));
                                dt = Convert.ToDateTime(drVal["tim"]);
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";

                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                        //TubewellParameterChartClass tableData1 = getAllSpellsForTubewellParameterizedChart(dtVal, parameterName, FinalTimeFrom, FinalTimeTo);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            FiltrationPlantTableData tableData = new FiltrationPlantTableData();
            if (dtVal.Rows.Count > 0)
            {
                try
                {
                    if (parameter == "Pump No. 1")
                    {
                        tableData = getAllSpellsForStorageParameterizedChartPumps(dtVal, parameterName, FinalTimeFrom, FinalTimeTo, 1);
                    }
                    else if (parameter == "Pump No. 2")
                    {
                        tableData = getAllSpellsForStorageParameterizedChartPumps(dtVal, parameterName, FinalTimeFrom, FinalTimeTo, 2);
                    }
                    else
                    {
                        tableData = getAllSpellsForTubewellParameterizedChart(dtVal, parameterName, FinalTimeFrom, FinalTimeTo);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                tableData.locationName = resources;
                tableData.parameterName = parameter;
                tableData.workingInHours = 0;
                tableData.totalHours = 0;
                tableData.availableHours = 0;
                tableData.nonAvailableHours = 0;
                tableData.workingInHoursManual = 0;
                tableData.workingInHoursRemote = 0;
                tableData.workingInHoursScheduling = 0;
                tableData.minValue = 0;
                tableData.maxValue = 0;
                tableData.avgVale = 0;
                tableData.avgOfAvailableHours = 0;
                tableData.avgOfNonAvailableHours = 0;
            }
            tableData.parameterName = parameter;
            tableData.fromDate = FinalTimeFrom.ToString();
            tableData.toDate = FinalTimeTo.ToString();
            tubewellDataList.Add(tableData);
            IEnumerable<FiltrationPlantTableData> itoms = (IEnumerable<FiltrationPlantTableData>)tubewellDataList;
            FiltrationPlantTableData itom = itoms.FirstOrDefault();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string outputString = serializer.Serialize(itom).ToString();
            ViewData["amChartData"] = outputString;
            return PartialView(tubewellDataList);
        }
        public FiltrationPlantTableData getAllSpellsForStorageParameterizedChartPumps(DataTable dt, string ParameterName, DateTime timeFrom, DateTime timeTo, int pumpNumber)
        {
            var tableData = new FiltrationPlantTableData();
            var spelldata = new StoragePump1SpellData();
            string pumpNumberName = "";
            string pumpModeName = "";
            string location = dt.Rows[0]["Location"].ToString();
            if (pumpNumber == 1)
            {
                pumpNumberName = "Pump No. 1";
            }
            else if (pumpNumber == 2)
            {
                pumpNumberName = "Pump No. 2";
            }
            var cms = dt.Rows[0][pumpNumberName];
            double currentMotorStatus = 0;
            if (cms == DBNull.Value)
            {
                currentMotorStatus = 0;
            }
            else
            {
                currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0][pumpNumberName])), 2);
            }
            string currentTime = dt.Rows[0]["tim"].ToString();
            //timeFrom = Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["tim"].ToString());
            //timeTo = Convert.ToDateTime(dt.Rows[0]["tim"].ToString());

            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<StoragePump1SpellData> spellDataList = new List<StoragePump1SpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                var currV = dr[pumpNumberName];
                //var currVR = dr[pumpModeName];
                var wf = dr[ParameterName];
                double currValue = 0;
                double currValueRemote = 0;
                double FlowRate = 0;
                if (currV == DBNull.Value)
                { }
                else
                {
                    currValue = Math.Round((Convert.ToDouble(dr[pumpNumberName])), 2);
                }
                //if (currVR == DBNull.Value)
                //{ }
                //else
                //{
                //    currValueRemote = Math.Round((Convert.ToDouble(dr[pumpModeName])), 2);
                //}
                if (wf == DBNull.Value)
                { }
                else
                {
                    FlowRate = Math.Round((Convert.ToDouble(dr[ParameterName])), 2);
                }
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    if (currValueRemote == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                if (currValueRemote == 0)
                                {
                                    spelldata.SpellMode = 1;
                                }
                                else
                                {
                                    spelldata.SpellMode = 2;
                                }
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1)
                            {
                                spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.SpellPeriod == 0)
                                {
                                    spelldata.SpellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new StoragePump1SpellData();
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    if (currValueRemote == 0)
                                    {
                                        spelldata.SpellMode = 1;
                                    }
                                    else
                                    {
                                        spelldata.SpellMode = 2;
                                    }
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                if (currValueRemote == 0)
                                {
                                    spelldata.SpellMode = 1;
                                }
                                else
                                {
                                    spelldata.SpellMode = 2;
                                }
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.SpellPeriod == 0)
                                {
                                    spelldata.SpellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new StoragePump1SpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.SpellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.SpellPeriod == 0)
                    {
                        spelldata.SpellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            if (spellDataList.Count == 0)
            {
                tableData.locationName = location;
                tableData.workingInHours = 0;
                tableData.totalHours = 0;
                tableData.availableHours = 0;
                tableData.nonAvailableHours = 0;
                tableData.workingInHoursManual = 0;
                tableData.workingInHoursRemote = 0;
                tableData.workingInHoursScheduling = 0;
                tableData.minValue = 0;
                tableData.maxValue = 0;
                tableData.avgVale = 0;
                tableData.avgOfAvailableHours = 0;
                tableData.avgOfNonAvailableHours = 0;
            }
            if (0 == 0)
            {
                tableData.locationName = location;
                if (timeTo > DateTime.Now)
                {
                    timeTo = DateTime.Now;
                }
                tableData.totalHours = Math.Round((((timeTo - timeFrom).TotalMinutes) / 60), 2);
                if (tableData.totalHours == 23.98)
                {
                    tableData.totalHours = 24;
                }
                tableData.SpellTimeArray = new List<string>();
                tableData.SpellDataArray = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.SpellTimeArray.Add(dr["tim"].ToString());
                    tableData.SpellDataArray.Add(Convert.ToDouble(dr[ParameterName].ToString()));
                }
                tableData.workingInHoursManual = Math.Round(((spellDataList.Where(r => r.SpellMode == 1).Sum(i => i.SpellPeriod)) / 60), 2);
                tableData.workingInHoursRemote = Math.Round(((spellDataList.Where(r => r.SpellMode == 2).Sum(i => i.SpellPeriod)) / 60), 2);
                tableData.workingInHoursScheduling = Math.Round(((spellDataList.Where(r => r.SpellMode == 3).Sum(i => i.SpellPeriod)) / 60), 2);
                tableData.workingInHours = Math.Round(((Convert.ToDouble(tableData.workingInHoursManual) +
                Convert.ToDouble(tableData.workingInHoursRemote) +
                Convert.ToDouble(tableData.workingInHoursScheduling))), 2);
                tableData.nonWorkingInHours = Math.Round((tableData.totalHours - tableData.workingInHours), 2);
                tableData.availableHours = Math.Round((getAvailableHours(dt, ParameterName)), 2);
                tableData.nonAvailableHours = Math.Round((tableData.totalHours - tableData.availableHours), 2);
                if (tableData.nonAvailableHours == 0.02 && tableData.availableHours > 1)
                {
                    tableData.nonAvailableHours = 0;
                }
                if (tableData.availableHours >= 23.97)
                {
                    tableData.availableHours = 24;
                }
                double availableSum = 0;
                double unavailableSum = 0;
                int availableCount = 0;
                int unavailableCount = 0;
                List<double> valList = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr[pumpNumberName]) == 0)
                    {
                        unavailableSum += Convert.ToDouble(dr[ParameterName]);
                        unavailableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                    else
                    {
                        availableSum += Convert.ToDouble(dr[ParameterName]);
                        availableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                }
                if (availableCount == 0)
                {
                    availableCount = 1;
                }
                if (unavailableCount == 0)
                {
                    unavailableCount = 1;
                }
                tableData.avgOfAvailableHours = Math.Round((availableSum / availableCount), 2);
                tableData.avgOfNonAvailableHours = Math.Round((unavailableSum / unavailableCount), 2);


                if (ParameterName == "WaterFlow(Cusec).")
                {
                    tableData.avgOfAvailableHours = Math.Round((tableData.avgOfAvailableHours / 101.94), 2);
                    tableData.avgOfNonAvailableHours = Math.Round((tableData.avgOfNonAvailableHours / 101.94), 2);
                }

                tableData.minValue = Math.Round((valList.Min()), 2);
                tableData.maxValue = Math.Round((valList.Max()), 2);
                tableData.avgVale = Math.Round((valList.Average()), 2);

                tableData.totalHoursString = minutesToTime(tableData.totalHours * 60);
                tableData.workingInHoursString = minutesToTime(tableData.workingInHours * 60);
                tableData.nonWorkingInHoursString = minutesToTime(tableData.nonWorkingInHours * 60);
                tableData.workingInHoursRemoteString = minutesToTime(tableData.workingInHoursRemote * 60);
                tableData.workingInHoursManualString = minutesToTime(tableData.workingInHoursManual * 60);
                tableData.workingInHoursSchedulingString = minutesToTime(tableData.workingInHoursScheduling * 60);
                tableData.availableHoursString = minutesToTime(tableData.availableHours * 60);
                tableData.nonAvailableHoursString = minutesToTime(tableData.nonAvailableHours * 60);
                tableData.parameterName = ParameterName;
                if (tableData.workingInHoursString == "23 Hours, 58 Minutes" || tableData.workingInHoursString == "23 Hours, 57 Minutes") { tableData.workingInHoursString = "24 Hours, 0 Minutes"; }
                if (tableData.totalHoursString == "23 Hours, 58 Minutes" || tableData.totalHoursString == "23 Hours, 57 Minutes") { tableData.totalHoursString = "24 Hours, 0 Minutes"; }
                if (tableData.workingInHoursManualString == "23 Hours, 58 Minutes" || tableData.workingInHoursManualString == "23 Hours, 57 Minutes") { tableData.workingInHoursManualString = "24 Hours, 0 Minutes"; }
                if (tableData.workingInHoursRemoteString == "23 Hours, 58 Minutes" || tableData.workingInHoursRemoteString == "23 Hours, 57 Minutes") { tableData.workingInHoursRemoteString = "24 Hours, 0 Minutes"; }
                if (tableData.availableHoursString == "23 Hours, 58 Minutes" || tableData.availableHoursString == "23 Hours, 57 Minutes") { tableData.availableHoursString = "24 Hours, 0 Minutes"; }
                if (tableData.nonAvailableHoursString == "23 Hours, 58 Minutes" || tableData.nonAvailableHoursString == "23 Hours, 57 Minutes") { tableData.nonAvailableHoursString = "24 Hours, 0 Minutes"; }
                if (tableData.nonWorkingInHoursString == "23 Hours, 58 Minutes" || tableData.nonWorkingInHoursString == "23 Hours, 57 Minutes") { tableData.nonWorkingInHoursString = "24 Hours, 0 Minutes"; }
                if (tableData.workingInHours >= 23.95 && tableData.workingInHours <= 24) { tableData.workingInHours = 24; }
                if (tableData.nonWorkingInHours >= 23.95 && tableData.nonWorkingInHours <= 24) { tableData.nonWorkingInHours = 24; }
                if (tableData.totalHours >= 23.95 && tableData.totalHours <= 24) { tableData.totalHours = 24; }
                if (tableData.workingInHoursManual >= 23.95 && tableData.workingInHoursManual <= 24) { tableData.workingInHoursManual = 24; }
                if (tableData.workingInHoursRemote >= 23.95 && tableData.workingInHoursRemote <= 24) { tableData.workingInHoursRemote = 24; }
                if (tableData.availableHours >= 23.95 && tableData.availableHours <= 24) { tableData.availableHours = 24; }
                if (tableData.nonAvailableHours >= 23.95 && tableData.nonAvailableHours <= 24) { tableData.nonAvailableHours = 24; }
            }
            return tableData;
        }
        public ActionResult HMI()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            //string newResr = Session["TheResource"].ToString();
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ResourceSellector rs = new ResourceSellector();
            if (Session["TheResource"] != null)
            {
                rs.resourceName = Session["TheResource"].ToString();
                ViewBag.SelectedResource = rs.resourceName;
            }
            else
            {
                rs.resourceName = "C-II Block Johar Town";
            }
            rs.resourceType = "Tubewells";
            ViewBag.ResourceList = ResourceList;
            return View(rs);
        }
        [HttpPost]
        public ActionResult HMI(FormCollection hmi)
        {
            string resource = hmi["resource"].ToString();
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ResourceSellector rs = new ResourceSellector();
            rs.resourceType = "Tubewells";
            rs.resourceName = resource;
            ViewBag.SelectedResource = resource;
            return View(rs);
        }
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _HMIView(string resourceName, string resourceType)
        {
            ResourceSellector rs = new ResourceSellector();
            rs.resourceType = resourceType;
            rs.resourceName = resourceName;
            return PartialView(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        //[OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2000)]
        public PartialViewResult _TableRecords(string resourceName, string resourceType)
        {
            string resource = resourceName;
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            if (resource == null)
            {
                resource = "C-II Block Johar Town";
            }
            string getResID = "select ResourceID from tblResource where ResourceLocation = '" + resource + "' and TemplateID = 64 ";
            int ResID = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                SqlCommand newConC1 = new SqlCommand(getResID, conn);
                ResID = Convert.ToInt32(newConC1.ExecuteScalar());
                conn.Close();
            }
            Random random = new Random();
            ////////////////////////////////////////////////////////////////////////////////////////////////
            var cId = Convert.ToInt32(Session["CompanyID"]);
            var dt = new DataTable();
            //string query = ";with cteRowNumber as ( ";
            //query += " select r.ResourceID, r.ResourceLocation, CONCAT(r.ResourceLocation, ' : ',e.InsertionDateTime) AS Location,p.ParameterID,p.ParameterName, e.CompanyID, e.ParameterValue, ";
            //query += " row_number() over(partition by p.ParameterID, r.ResourceID,r.ResourceLocation order by e.ID desc) as RowNum";
            //query += " from tblEnergy e";
            //query += " inner join tblResource r on e.ResourceID = r.ResourceID";
            //query += " inner join tblParameter p on e.ParameterID = p.ParameterID";
            //query += " ) ";
            //query += " select * ";
            //query += " from cteRowNumber ";
            //query += " where RowNum = 1  and CompanyID = " + cId + "";
            //query += " and ResourceID = '" + ResID + "' ";
            //query += " order by ResourceID,ParameterID ";
            string query = "";
            query += "SELECT e.ID, r.ResourceLocation as Location, p.ParameterName, e.ParameterValue, e.InsertionDateTime ";
            query += "FROM tblEnergy e ";
            query += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
            query += "inner join tblResource r on e.ResourceID = r.ResourceID ";
            query += "WHERE ";
            query += "e.ResourceID = " + ResID + " AND ";
            query += "insertionDateTime = (Select max(insertionDateTime) from tblEnergy where ResourceID = " + ResID + ")";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var sda = new SqlDataAdapter(query, conn);
                try
                {
                    sda.Fill(dt);
                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            //string location = dt.Rows[0][1].ToString();
            //string time = dt.Rows[0][2].ToString().Split(':')[1] + ':' + dt.Rows[0][2].ToString().Split(':')[2];
            string location = dt.Rows[0][1].ToString();
            string time = dt.Rows[0][4].ToString();
            double delTime = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(time.ToString())).TotalMinutes;
            Pivot pvt = new Pivot(dt);
            DataTable pivotTable = new DataTable();
            pivotTable = pvt.PivotData("ParameterName", "ParameterValue", AggregateFunction.Sum, "Location");
            var db = new WASA_EMS_Entities();
            double V1N, V2N, V3N, I1, I2, I3, Frequency, PKVA, PF, AutoModeOn, PumpStatus, CurrentTrip, VoltageTrip, RemoteControl, ChlorineLevel, WaterExtracted, PKVAR, PKW, V12, V13, V23, PrimingLevel, averageVoltage, averageVoltageSecond, averageCurrent, TotalWorkingHour, Scheduling, Manual, Pressure, vib_m, vib_ms, vib_ms2 = 0;
            V1N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V1N." select row[1]).ElementAt(0)), 2);
            V2N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V2N." select row[1]).ElementAt(0)), 2);
            V3N = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V3N." select row[1]).ElementAt(0)), 2);
            I1 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I1." select row[1]).ElementAt(0)), 2);
            I2 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I2." select row[1]).ElementAt(0)), 2);
            I3 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "I3." select row[1]).ElementAt(0)), 2);
            Frequency = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Frequency." select row[1]).ElementAt(0)), 2);
            PKVA = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKVA." select row[1]).ElementAt(0)), 2);
            PF = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PF." select row[1]).ElementAt(0)), 2);
            AutoModeOn = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Remote." select row[1]).ElementAt(0)), 2);
            PumpStatus = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PumpStatus" select row[1]).ElementAt(0)), 2);
            CurrentTrip = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "CurrentTrip." select row[1]).ElementAt(0)), 2);
            VoltageTrip = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "VoltageTrip." select row[1]).ElementAt(0)), 2);
            RemoteControl = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Remote." select row[1]).ElementAt(0)), 2);
            ChlorineLevel = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "ChlorineLevel." select row[1]).ElementAt(0)), 2);
            WaterExtracted = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "WaterFlow(Cusec)." select row[1]).ElementAt(0)), 2);
            Scheduling = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "TimeSchedule." select row[1]).ElementAt(0)), 2);
            Manual = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Manual" select row[1]).ElementAt(0)), 2);
            //PKVAR, PKW, V12, V13, V23, PrimingLevel
            PKVAR = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKVAR." select row[1]).ElementAt(0)), 2);
            PKW = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PKW." select row[1]).ElementAt(0)), 2);
            V12 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V12" select row[1]).ElementAt(0)), 2);
            V13 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V13" select row[1]).ElementAt(0)), 2);
            V23 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "V23" select row[1]).ElementAt(0)), 2);
            PrimingLevel = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "PrimingLevel" select row[1]).ElementAt(0)), 2);
            vib_m = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_z" select row[1]).ElementAt(0)), 2);
            vib_ms = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_y" select row[1]).ElementAt(0)), 2);
            vib_ms2 = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "vib_x" select row[1]).ElementAt(0)), 2);
            Pressure = Math.Round(Convert.ToDouble((from row in pivotTable.AsEnumerable() where row.Field<string>("ParameterName") == "Pressure(Bar)" select row[1]).ElementAt(0)), 2);
            //var abbb = (from row in pivotTable.AsEnumerable()
            //     where row.Field<string>("ParameterName") == "V1N."
            //           //&& row.Field<int>("B") == 2
            //           //&& row.Field<int>("C") == 3
            //     select row[1]).ElementAt(0);
            //averageCurrent = (Convert.ToDouble(I1) + Convert.ToDouble(I2) + Convert.ToDouble(I3)).ToString();
            averageCurrent = Math.Round((I1 + I2 + I3) / 3, 2);
            averageVoltage = Math.Round((V1N + V2N + V3N) / 3, 2);
            averageVoltageSecond = Math.Round((V12 + V13 + V23) / 3, 2);

            ViewBag.parameterList = db.tblParameters.AsEnumerable().Where(item => item.CompanyID == cId);
            ViewBag.resourceList = db.tblResources.AsEnumerable().Where(item => item.CompanyID == cId);
            ViewBag.V1N = V1N;
            ViewBag.V2N = V2N;
            ViewBag.V3N = V3N;
            ViewBag.I1 = I1;
            ViewBag.I2 = I2;
            ViewBag.I3 = I3;
            ViewBag.Frequency = Frequency;
            ViewBag.PKVA = PKVA;
            ViewBag.PF = PF;
            ViewBag.AutoModeOn = AutoModeOn;
            ViewBag.PumpStatus = PumpStatus;
            ViewBag.CurrentTrip = CurrentTrip;
            ViewBag.VoltageTrip = VoltageTrip;
            ViewBag.RemoteControl = RemoteControl;
            ViewBag.ChlorineLevel = ChlorineLevel;
            ViewBag.WaterExtracted = WaterExtracted;
            ViewBag.PKVAR = PKVAR;
            ViewBag.PKW = PKW;
            ViewBag.V12 = V12;
            ViewBag.V13 = V13;
            ViewBag.V23 = V23;
            ViewBag.PrimingLevel = PrimingLevel;
            ViewBag.averageVoltage = averageVoltage;
            ViewBag.averageVoltageSecond = averageVoltageSecond;
            ViewBag.averageCurrent = averageCurrent;
            ViewBag.TotalWorkingHour = getWorkingHours(dt,1);
            ViewBag.Scheduling = Scheduling;
            int num = random.Next(100);
            ViewBag.FlowData = num;
            ViewBag.Location = location;
            ViewBag.RunTime = time;
            ViewBag.Manual = Manual;
            ViewBag.vib_m = vib_m;
            ViewBag.vib_ms = vib_ms;
            ViewBag.vib_ms2 = vib_ms2;
            ViewBag.Pressure = Pressure;
            ViewBag.DelTime = delTime;
            //return PartialView(pivotTable);
            ////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //int num = random.Next(100);
            //ViewBag.FlowData = num;
            //return PartialView();
            return PartialView(pivotTable);
        }
        public string getWorkingHours(DataTable dt, int pumpNumber)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            string columnName = "";
            if (pumpNumber == 1)
            {
                columnName = "Pump No. 1";
            }
            else
            {
                columnName = "Pump No. 2";
            }
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0][columnName])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            string location = dt.Rows[0]["Location"].ToString();
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr[columnName])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Water Flow"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Convert.ToDouble(Convert.ToInt32(Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes)));
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
                spellDataList.Add(spelldata);
            }
            if (spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.WorkingInHours = 0;
            }
            else
            {
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                tableData.WorkingInHours = Convert.ToDouble(pp.TotalMinutes);
            }
            return tableData.WorkingInHours.ToString();
        }
        TubewellDataClass getWaterFlowData(DataTable dt)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            string columnName = "Water Flow";
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0][columnName])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            string location = dt.Rows[0]["Location"].ToString();
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr[columnName])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["Water Flow"])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 0)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 0)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 0)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 100000)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 100000)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 0)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 0)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 100000)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 100000)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Convert.ToDouble(Convert.ToInt32(Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes)));
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
                spellDataList.Add(spelldata);
            }
            else
            {
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                double avgFlow = spellDataList[0].SpellDataArray.Average();
                tableData.waterDischarge = (pp.TotalSeconds * avgFlow).ToString();
                tableData.waterFlow = new List<double>();
                tableData.waterFlow = spellDataList[0].SpellDataArray;
                tableData.LogTime = spellDataList[0].SpellTimeArray;
            }
            tableData.LogTime.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                tableData.LogTime.Add(dr["tim"].ToString());
            }
            return tableData;
        }

        public string getHMIStatus(string resourceName)
        {
            var hmi = new HMIstatus();
            //double ret = -1;s
            string resource = resourceName;
            //string resourceType = ViewBag.resourceType.toString();
            //string getResID = "select ResourceID from tblResource where ResourceLocation = 'C-II Johar Town' ";
            //if (ViewBag.SelectedResource == null)
            //{
            //    resource = "C-II Johar Town";
            //}
            string query1 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 125 order by ID desc";
            string query2 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 129 order by ID desc";
            string query3 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 167 order by ID desc";
            string query4 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 130 order by ID desc";
            string query5 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 174 order by ID desc";
            string query6 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 115 order by ID desc";
            string query7 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 116 order by ID desc";
            string query8 = "select top(1) ParameterValue from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 117 order by ID desc";
            string querytime = "select top(1) insertionDateTime from tblEnergy where resourceID = (select ResourceID from tblResource where ResourceLocation = '" + resource + "' ) and ParameterID = 117 order by ID desc";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                var cmd1 = new SqlCommand(query1, conn);
                var cmd2 = new SqlCommand(query2, conn);
                var cmd3 = new SqlCommand(query3, conn);
                var cmd4 = new SqlCommand(query4, conn);
                var cmd5 = new SqlCommand(query5, conn);
                var cmd6 = new SqlCommand(query6, conn);
                var cmd7 = new SqlCommand(query7, conn);
                var cmd8 = new SqlCommand(query8, conn);
                var cmdtime = new SqlCommand(querytime, conn);
                try
                {
                    hmi.pumpStatus = Math.Abs(Convert.ToDouble(cmd1.ExecuteScalar()));
                    hmi.chlorineStatus = Math.Abs(Convert.ToDouble(cmd2.ExecuteScalar()));
                    hmi.primingStatus = Math.Abs(Convert.ToDouble(cmd3.ExecuteScalar()));
                    hmi.flowRate = Math.Round(Convert.ToDouble(cmd4.ExecuteScalar()), 1);
                    hmi.pressureRate = Math.Round(Convert.ToDouble(cmd5.ExecuteScalar()), 1);
                    hmi.v1n = Math.Round(Convert.ToDouble(cmd6.ExecuteScalar()), 1);
                    hmi.v2n = Math.Round(Convert.ToDouble(cmd7.ExecuteScalar()), 1);
                    hmi.v3n = Math.Round(Convert.ToDouble(cmd8.ExecuteScalar()), 1);
                    hmi.lastTime = cmdtime.ExecuteScalar().ToString();
                    hmi.delTime = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(hmi.lastTime.ToString())).TotalMinutes;

                }
                catch (Exception ex)
                {

                }
                conn.Close();
            }
            return JsonConvert.SerializeObject(hmi); ;
        }
        public ActionResult ParameterWiseReport()
        {
            //var tubewellDataList = new List<TubewellDataClass>();
            //DataTable dtParams = new DataTable();
            //string scriptString = "";
            //scriptString = "var chart = new CanvasJS.Chart(\"chartContainer1\", { animationEnabled: true, title:{ text: \"Tubewell All Parameters\" }, axisY:{labelFontSize: 10},axisX:{labelFontSize: 10}, toolTip: {fontSize: 10, shared: true }, data: [";
            //string getAllParametersQuery = "select p.ParameterID, p.ParameterName from tblParameter p inner join tblTemplateParameter tp on tp.ParameterID = p.ParameterID where tp.TemplateID = 64 order by p.paramOrder";
            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    try
            //    {
            //        conn.Open();
            //        SqlDataAdapter sdaParams = new SqlDataAdapter(getAllParametersQuery, conn);
            //        sdaParams.Fill(dtParams);
            //        foreach (DataRow dr in dtParams.Rows)
            //        {
            //            DataTable dtResource = new DataTable();
            //            scriptString += "{ type: \"stackedColumn\", name: \"" + dr["ParameterName"].ToString() + "\", showInLegend: false, dataPoints: [";
            //            string getAllResourceQuery = "select ResourceID, ResourceLocation from tblResource where TemplateID = 64 order by ResourceID";
            //            SqlDataAdapter sdaResource = new SqlDataAdapter(getAllResourceQuery, conn);
            //            sdaResource.Fill(dtResource);
            //            foreach (DataRow drRe in dtResource.Rows)
            //            {
            //                string getParameterValue = "select top(1)ParameterValue from tblEnergy where ResourceID = " + Convert.ToInt32(drRe["ResourceID"]) + " and ParameterID = " + Convert.ToInt32(dr["ParameterID"]) + " order by ID DESC";
            //                SqlCommand cmdVal = new SqlCommand(getParameterValue, conn);
            //                double val = Math.Abs(Convert.ToDouble(cmdVal.ExecuteScalar()));
            //                scriptString += "{ y: " + val + " , label: \"" + drRe["ResourceLocation"].ToString() + "\" },";
            //            }
            //            scriptString = scriptString.Remove(scriptString.Length - 1);
            //            scriptString += "]},";
            //        }
            //        scriptString = scriptString.Remove(scriptString.Length - 1);
            //        scriptString += "] })";
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //string NewscripString = scriptString;
            //NewscripString = NewscripString.Replace("&quot;", "\"");
            //ViewData["chartData"] = NewscripString;
            //DataTable dtRes = new DataTable();
            //int resourceID = 0;
            //////////////////////////////////////////////////////////////////////////
            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    try
            //    {
            //        conn.Open();
            //        string getResFromTemp = "";
            //        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
            //        SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
            //        dtRes.Clear();
            //        sdaRes.Fill(dtRes);
            //        string resourceLocation = "";
            //        string resourceSpecification = "";
            //        int ite = 0;
            //        //iterate through the list of resources within the desired set of resources chosen
            //        DataTable Dashdt = new DataTable();
            //        foreach (DataRow drRes in dtRes.Rows)
            //        {
            //            //getting resourceID 
            //            resourceID = Convert.ToInt32(drRes["ResourceID"]);
            //            //getting resourceLocation 
            //            resourceLocation = drRes["ResourceLocation"].ToString();
            //            resourceSpecification = drRes["ResourceSpecification"].ToString();
            //            //query will get the list of data available against given resourceID (latest first)
            //            string Dashdtquery = ";WITH cte AS ( ";
            //            Dashdtquery += "SELECT* FROM ";
            //            Dashdtquery += "( ";
            //            Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
            //            Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
            //            Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
            //            Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
            //            Dashdtquery += "s.InsertionDateTime as tim ,";
            //            Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 9,GETDATE ())) as DeltaMinutes ";
            //            Dashdtquery += "FROM tblEnergy s ";
            //            Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
            //            Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
            //            Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
            //            Dashdtquery += "where ";
            //            Dashdtquery += "r.ResourceID = " + resourceID + " and ";
            //            //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
            //            Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + DateTime.Now.Date + "', 103), 121)   ";
            //            Dashdtquery += ") ";
            //            Dashdtquery += "AS SourceTable ";
            //            Dashdtquery += "PIVOT ";
            //            Dashdtquery += "( ";
            //            Dashdtquery += "SUM(pVal) FOR pID ";
            //            Dashdtquery += "IN ";
            //            Dashdtquery += "( ";
            //            Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_M],[vib_Ms],[vib_Ms2] ";
            //            Dashdtquery += ") ";
            //            Dashdtquery += ")  ";
            //            Dashdtquery += "AS PivotTable ";
            //            Dashdtquery += ")  ";
            //            Dashdtquery += "SELECT* FROM cte ";
            //            Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
            //            Dashdtquery += "tim DESC";
            //            SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
            //            SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
            //            Dashdt.Clear();
            //            sda.Fill(Dashdt);
            //            if (Dashdt.Rows.Count > 0)
            //            {
            //                TubewellDataClass sd = getAllSpells(Dashdt, dtRes.Rows.IndexOf(drRes));
            //                tubewellDataList.Add(sd);
            //            }
            //            else
            //            {
            //                TubewellDataClass sd = new TubewellDataClass();
            //                sd.locationName = drRes["ResourceLocation"].ToString();
            //                sd.Specification = drRes["ResourceSpecification"].ToString();
            //                sd.pumpStatus = new List<double>();
            //                tubewellDataList.Add(sd);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Get stack trace for the exception with source file information
            //        var st = new StackTrace(ex, true);
            //        // Get the top stack frame
            //        var frame = st.GetFrame(0);
            //        // Get the line number from the stack frame
            //        var line = frame.GetFileLineNumber();
            //    }
            //    conn.Close();
            //}
            ////if (resources == "All")
            ////{
            ////    selectedResource = "All Tubewell Locations";
            ////}
            ////else
            ////{
            ////    selectedResource = "" + resources + " Tubewell";
            ////}
            ////Session["ReportTitle"] = "Energy Monitoring Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            //return PartialView(tubewellDataList);
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 5)]
        public PartialViewResult _ParameterReportView()
        {

            DataTable dtRes = new DataTable();
            DataTable dtReport = new DataTable();
            string getAllResourceQuery = "select ResourceID, ResourceLocation from tblResource where TemplateID = 64 order by ResourceID";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getAllResourceQuery, conn);
                    sdaRes.Fill(dtRes);
                    foreach (DataRow dr in dtRes.Rows)
                    {
                        //dtReport.Clear();
                        if (dtReport.Rows.Count < 1)
                        {
                            dtReport.Columns.Add("Location");
                        }
                        DataTable dtParamVals = new DataTable();
                        //string allParamsQueryForRes = "select p.ParameterName, e.ParameterValue from tblEnergy e left join tblParameter p on e.ParameterID = p.ParameterID left join tblResource r on e.ResourceID = r.ResourceID left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID left join tblTemplate t on r.TemplateID = t.TemplateID where e.InsertionDateTime = ( select max(InsertionDateTime) from tblEnergy where ResourceID = " + Convert.ToInt32(dr["ResourceID"]) + " ) and e.InsertionDateTime > dateadd(minute,-200,getdate()) and r.ResourceID = " + Convert.ToInt32(dr["ResourceID"]) + " order by p.ParamOrder";
                        string allParamsQueryForRes = "select p.ParameterName, e.ParameterValue, e.InsertionDateTime from tblEnergy e left join tblParameter p on e.ParameterID = p.ParameterID where e.InsertionDateTime = ( select max(InsertionDateTime) from tblEnergy where ResourceID = " + Convert.ToInt32(dr["ResourceID"]) + " and InsertionDateTime NOT IN ( select max(InsertionDateTime) from tblEnergy where ResourceID = " + Convert.ToInt32(dr["ResourceID"]) + ") ) and e.InsertionDateTime > dateadd(minute,-200,getdate()) and e.ResourceID = " + Convert.ToInt32(dr["ResourceID"]) + " order by p.ParamOrder";
                        SqlDataAdapter sdaParamVals = new SqlDataAdapter(allParamsQueryForRes, conn);
                        sdaParamVals.Fill(dtParamVals);
                        if (dtReport.Rows.Count < 1)
                        {
                            if (dtParamVals.Rows.Count == 0)
                            {
                                string getParamNames = "select p.ParameterName, null as ParameterValue from tblParameter p inner join tblTemplateParameter tp on p.ParameterID = tp.ParameterID where tp.TemplateID = 64";
                                SqlDataAdapter sdaParamNames = new SqlDataAdapter(getParamNames, conn);
                                sdaParamNames.Fill(dtParamVals);
                            }
                            foreach (DataRow drParamVals in dtParamVals.Rows)
                            {
                                string parameterValuesString = "";
                                if (drParamVals["ParameterName"].ToString() == "V1N.")
                                {
                                    parameterValuesString += "V1N";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "V2N.")
                                {
                                    parameterValuesString += "V2N";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "V3N.")
                                {
                                    parameterValuesString += "V3N";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "I1.")
                                {
                                    parameterValuesString += "I1";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "I2.")
                                {
                                    parameterValuesString += "I2";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "I3.")
                                {
                                    parameterValuesString += "I3";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "Frequency.")
                                {
                                    parameterValuesString += "Frequency";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PKVA.")
                                {
                                    parameterValuesString += "PKVA";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PF.")
                                {
                                    parameterValuesString += "Power Factor";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "Remote.")
                                {
                                    parameterValuesString += "Remote Mode";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PumpStatus")
                                {
                                    parameterValuesString += "Pump Status";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "CurrentTrip.")
                                {
                                    parameterValuesString += "Current Trip";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "VoltageTrip.")
                                {
                                    parameterValuesString += "Voltage Trip";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "TimeSchedule.")
                                {
                                    parameterValuesString += "Scheduling Mode";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "ChlorineLevel.")
                                {
                                    parameterValuesString += "Chlorine Level";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "WaterFlow(Cusec).")
                                {
                                    parameterValuesString += "Water Flow (cfs)";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PKVAR.")
                                {
                                    parameterValuesString += "PKVAR";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PKW.")
                                {
                                    parameterValuesString += "PKW";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "V12")
                                {
                                    parameterValuesString += "V12";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "V13")
                                {
                                    parameterValuesString += "V13";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "V23")
                                {
                                    parameterValuesString += "V23";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "PrimingLevel")
                                {
                                    parameterValuesString += "Priming Level";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "Pressure(Bar)")
                                {
                                    parameterValuesString += "Pressure(Bar)";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "Manual")
                                {
                                    parameterValuesString += "Manual Mode";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "IndoorLight")
                                {
                                    parameterValuesString += "Indoor Lights";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "OutdoorLight")
                                {
                                    parameterValuesString += "Outdoor Lights";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "Exhaust Fan")
                                {
                                    parameterValuesString += "Exhaust Fan";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "vib_x")
                                {
                                    parameterValuesString += "Vibration Velocity in (mm/s)";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "vib_y")
                                {
                                    parameterValuesString += "Vibration Acceleration in (m/s2)";
                                }
                                else if (drParamVals["ParameterName"].ToString() == "vib_z")
                                {
                                    parameterValuesString += "Vibration Displacement in (um)";
                                }
                                else
                                {
                                    parameterValuesString += drParamVals["ParameterName"].ToString();
                                }
                                dtReport.Columns.Add(parameterValuesString);
                            }
                        }
                        if (dtParamVals.Rows.Count == 0)
                        {
                            DataRow drNew = dtReport.NewRow();
                            drNew[0] = dr["ResourceLocation"].ToString();
                            dtReport.Rows.Add(drNew);
                        }
                        else
                        {
                            for (int i = 0; i < dtParamVals.Rows.Count; i++)
                            {

                                if (i == 0)
                                {
                                    DataRow drNew = dtReport.NewRow();
                                    drNew[0] = dr["ResourceLocation"].ToString();
                                    drNew[1] = dtParamVals.Rows[0][1];
                                    dtReport.Rows.Add(drNew);
                                }
                                else if (i == 28)
                                {
                                    if (dtParamVals.Rows[i][1] == DBNull.Value)
                                    {
                                        dtReport.Rows[dtRes.Rows.IndexOf(dr)][i + 1] = dtParamVals.Rows[i][1];
                                    }
                                    else
                                    {
                                        dtReport.Rows[dtRes.Rows.IndexOf(dr)][i + 1] = Math.Round(((Convert.ToDouble(dtParamVals.Rows[i][1])) * 0.3), 2);
                                    }
                                }
                                else if (i == 29)
                                {
                                    if (dtParamVals.Rows[i][1] == DBNull.Value)
                                    {
                                        dtReport.Rows[dtRes.Rows.IndexOf(dr)][i + 1] = dtParamVals.Rows[i][1];
                                    }
                                    else
                                    {
                                        dtReport.Rows[dtRes.Rows.IndexOf(dr)][i + 1] = Math.Round(((Convert.ToDouble(dtParamVals.Rows[i][1])) / 0.3), 2);
                                    }
                                }
                                else
                                {
                                    dtReport.Rows[dtRes.Rows.IndexOf(dr)][i + 1] = dtParamVals.Rows[i][1];
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string scriptString = "";
            scriptString += "var chart = new CanvasJS.Chart(\"chartContainer1\", { theme: \"light2\", animationEnabled: true,  title:{ text: \"PARAMETERS OF TUBEWELLS\" },exportEnabled: true, dataPointWidth: 30, axisY:{labelFontSize: 18, labelFormatter: function(){ return \" \"; }},axisX:{labelFontSize: 10}, toolTip: {fontSize: 11, fontWeight: \"bold\", shared: true }, data: [";
            for (int col = 1; col < dtReport.Columns.Count; col++)
            {
                scriptString += "{ type: \"stackedColumn\", name: \"" + dtReport.Columns[col].ColumnName + "\", showInLegend: false, dataPoints: [";
                for (int ro = 0; ro < dtReport.Rows.Count; ro++)
                {
                    var valObj = dtReport.Rows[ro][col];
                    if (!(valObj is DBNull))
                    {
                        double val = Math.Round(Math.Abs(Convert.ToDouble(dtReport.Rows[ro][col])), 2);
                        if (col < 5)
                        {
                            if (val == 0)
                            {
                                scriptString += "{ y:'<b>OFF</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                            else
                            {
                                scriptString += "{ y:'<b>ON</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                        }
                        else if (col == 19 || col == 20)
                        {
                            if (val == 0)
                            {
                                scriptString += "{ y:'<b>No Error</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                            else
                            {
                                scriptString += "{ y:'<b>Error</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                        }
                        else if (col == 21)
                        {
                            val = Math.Round((val / 101.94), 2);
                            //value = "\'<b>" + value + "</b>\'";
                            //scriptString += "{ y:" + val + " , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            scriptString += "{ y:" + val.ToString() + " , label: \"" + dtReport.Rows[ro][0] + "\" },";
                        }
                        else if (col == 23)
                        {
                            if (val == 0)
                            {
                                scriptString += "{ y:'<b>EMPTY</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                            else
                            {
                                scriptString += "{ y:'<b>FULL</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                        }
                        else if (col == 24)
                        {
                            if (val == 0)
                            {
                                scriptString += "{ y:'<b>HALF</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                            else
                            {
                                scriptString += "{ y:'<b>FULL</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                        }
                        else if (col == 25 || col == 26 || col == 27)
                        {
                            if (val == 0)
                            {
                                scriptString += "{ y:'<b>OFF</b>' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                            else
                            {
                                scriptString += "{ y:'ON' , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            }
                        }
                        else
                        {
                            string value = val.ToString();
                            //value = "\'<b>" + value + "</b>\'";
                            //scriptString += "{ y:" + val + " , label: \"" + dtReport.Rows[ro][0] + "\" },";
                            scriptString += "{ y:" + val.ToString() + " , label: \"" + dtReport.Rows[ro][0] + "\" },";
                        }
                    }
                    else
                    {
                        scriptString += "{ y: null , label: \"" + dtReport.Rows[ro][0] + "\" },";
                    }

                }
                scriptString += "]},";
            }
            scriptString += "] })";
            string NewscripString = scriptString;
            NewscripString = NewscripString.Replace("&quot;", "\"");
            ViewData["chartData"] = NewscripString;
            Session["ReportTitle"] = "Current Status of All Tubewell Locations at  " + DateTime.Now.AddHours(0).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " ";
            return PartialView(dtReport);
        }
        public ActionResult StatusReport()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 10)]
        public PartialViewResult _StatusReportView()
        {
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            int resourceID = 0;
            ////////////////////////////////////////////////////////////////////////
            ///
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Filtration Plants'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        Dashdtquery += "[Pump No. 1],[Pump No. 2],[Voltage],[Current],[Power Factor],[Frequency],[Power (KVA)],[Power (KVAR)],[Power (KW)],[Water Flow],[Chlorine Level],[Tank No. 1],[Tank No. 2],[TDS],[Manual Mode] ";
                        Dashdtquery += ") ";
                        Dashdtquery += ")  ";
                        Dashdtquery += "AS PivotTable ";
                        Dashdtquery += ")  ";
                        Dashdtquery += "SELECT* FROM cte ";
                        Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                        Dashdtquery += "tim DESC";

                        Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ,  td.[Pump No. 1] as [Pump No. 1], td.[Pump No. 2] as [Pump No. 2], td.[Voltage] as [Voltage], td.[Current] as [Current], td.[Power Factor] as [Power Factor], td.[Frequency] as [Frequency], td.[Power (KVA)] as [Power (KVA)], td.[Power (KVAR)] as [Power (KVAR)], td.[Power (KW)] as [Power (KW)], td.[Water Flow] as [Water Flow], td.[Chlorine Level] as [Chlorine Level], td.[Tank No. 1] as [Tank No. 1], td.[Tank No. 2] as [Tank No. 2], td.[TDS] as [TDS], td.[Manual Mode] as [Manual Mode] from tblWaterFilteration td inner join tblResource r on td.PlantId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) order by td.PlantId, tim DESC";

                        SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                        Dashdt.Clear();
                        sda.Fill(Dashdt);
                        
                        if (Dashdt.Rows.Count > 0)
                        {
                            //TubewellDataClass sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), DateTime.Now.Date);
                            //tubewellDataList.Add(sd);
                        }
                        else
                        {
                            TubewellDataClass sd = new TubewellDataClass();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            sd.pumpStatus = new List<double>();
                            tubewellDataList.Add(sd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }


            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'Pump No. 1'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'Pump No. 1'  and rtp.TemplateID = 68 order by cast(r.ResourceID as int) asc";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"PUMPS WORKING STATUS\" },exportEnabled: true,";
                        string TheSelectedResource = "Filtration Plant";
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data fetched from " + TheSelectedResource + " today   \" }],";
                        scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
                        scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: OFF</b> at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: ON</b> at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 12},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0)  ";
                            aquery += ") ";
                            aquery += "SELECT top 14000 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";



                            string theQuery = aquery;

                            theQuery = "select 190 as ParameterID, t.[Pump No. 1] as ParameterValue, t.InsertionDateTime from tblWaterFilteration t ";
                            theQuery += "where t.PlantId = " + Convert.ToInt32(drPar["ResourceID"]) + " and t.InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) order by InsertionDateTime ASC";


                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"area\", name: \"Pump No. 1\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\",  ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";
                            theQuery = "select 191 as ParameterID, t.[Pump No. 2] as ParameterValue, t.InsertionDateTime from tblWaterFilteration t ";
                            theQuery += "where t.PlantId = " + Convert.ToInt32(drPar["ResourceID"]) + " and t.InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,0,GETDATE())), 0) order by InsertionDateTime ASC";
                            sdaVal = new SqlDataAdapter(theQuery, conn);
                            dtVal.Clear();
                            sdaVal.Fill(dtVal);
                            dataPoints.Clear();
                            scriptString += "{ type: \"area\", name: \"Pump No. 2\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\",  ";
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////
            double p1wh = (Convert.ToDouble(getWorkingHours(Dashdt, 1)) / 60);
            double p2wh = (Convert.ToDouble(getWorkingHours(Dashdt, 2)) / 60);
            FiltrationPlantTableData spellData = new FiltrationPlantTableData();
            spellData.P1WorkingInHours = Convert.ToDouble(p1wh);
            spellData.P2WorkingInHours = Convert.ToDouble(p2wh);
            var pp = TimeSpan.FromMinutes(Convert.ToDouble(p1wh*60));
            int phour = (int)pp.TotalHours;
            int pmin = (int)pp.Minutes;
            int psec = (int)pp.Seconds;
            string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
            spellData.workingHoursTodayP1 = pstr;
            pp = TimeSpan.FromMinutes(Convert.ToDouble(p2wh * 60));
            phour = (int)pp.TotalHours;
            pmin = (int)pp.Minutes;
            psec = (int)pp.Seconds;
            pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
            spellData.workingHoursTodayP2 = pstr;
            string abc = Dashdt.Rows[0][10].ToString();
            spellData.pumpStatus1 = new List<double>();
            spellData.pumpStatus2 = new List<double>();
            spellData.ChlorineLevel = new List<double>();
            spellData.TankLevel1ft = new List<double>();
            spellData.TankLevel2ft = new List<double>();
            spellData.Voltage = new List<double>();
            spellData.Current = new List<double>();
            spellData.PF = new List<double>();
            spellData.FreqHz = new List<double>();
            spellData.PKVA = new List<double>();
            spellData.PKVAR = new List<double>();
            spellData.PKW = new List<double>();
            spellData.waterFlow = new List<double>();
            spellData.TDS = new List<double>();
            spellData.manualStatus = new List<double>();
            spellData.pumpStatus1.Add(Convert.ToDouble(Dashdt.Rows[0][10].ToString()));
            spellData.pumpStatus2.Add(Convert.ToDouble(Dashdt.Rows[0][11].ToString()));
            spellData.Voltage.Add(Convert.ToDouble(Dashdt.Rows[0][12].ToString()));
            spellData.Current.Add(Convert.ToDouble(Dashdt.Rows[0][13].ToString()));
            spellData.PF.Add(Convert.ToDouble(Dashdt.Rows[0][14].ToString()));
            spellData.FreqHz.Add(Convert.ToDouble(Dashdt.Rows[0][15].ToString()));
            spellData.PKVA.Add(Convert.ToDouble(Dashdt.Rows[0][16].ToString()));
            spellData.PKVAR.Add(Convert.ToDouble(Dashdt.Rows[0][17].ToString()));
            spellData.PKW.Add(Convert.ToDouble(Dashdt.Rows[0][18].ToString()));
            spellData.waterFlow.Add(Convert.ToDouble(Dashdt.Rows[0][19].ToString()));
            spellData.ChlorineLevel.Add(Convert.ToDouble(Dashdt.Rows[0][20].ToString()));
            spellData.TankLevel1ft.Add(Convert.ToDouble(Dashdt.Rows[0][21].ToString()));
            spellData.TankLevel2ft.Add(Convert.ToDouble(Dashdt.Rows[0][22].ToString()));
            spellData.TDS.Add(Convert.ToDouble(Dashdt.Rows[0][23].ToString()));
            spellData.manualStatus.Add(Convert.ToDouble(Dashdt.Rows[0][24].ToString()));
            foreach (DataRow dr in Dashdt.Rows)
            {
                spellData.SpellTimeArray.Add(dr["tim"].ToString());
                spellData.SpellDataArray.Add(Convert.ToDouble(dr[10].ToString()));
                spellData.SpellDataArray2.Add(Convert.ToDouble(dr[11].ToString()));
            }
            Session["ReportTitle"] = "Current Status of Pattoki Filtration Plant at  " + DateTime.Now.AddHours(0).ToString("dd'/'MM'/'yyyy HH:mm:ss") + " (Reflected for Today)";
            var filtrationPlantDataList = new List<FiltrationPlantTableData>();
            filtrationPlantDataList.Add(spellData);
            IEnumerable<FiltrationPlantTableData> itoms = (IEnumerable<FiltrationPlantTableData>)filtrationPlantDataList;
            FiltrationPlantTableData itom = itoms.FirstOrDefault();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string outputString = serializer.Serialize(itom).ToString();
            ViewData["amChartData"] = outputString;
            return PartialView(filtrationPlantDataList);
        }
        public ActionResult WaterDischargeReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 68))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Filteration Plants";
            //rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult WaterDischargeReport(FormCollection review)
        {
            string resource = review["resource"].ToString();
            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 68))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Filteration Plants";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _WaterDischargeReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date.AddSeconds(-1);
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<FiltrationPlantTableData>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources.ToLower() == "all" || resources.ToLower() == "")
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification,r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Filtration Plants'";
                    }
                    else
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Filtration Plants' and r.ResourceName = '" + resources + "'";
                    }
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen

                    scriptString += "var chart1 = new CanvasJS.Chart(\"chartContainer1\", {";
                    scriptString += "theme: \"light2\",";
                    scriptString += "animationEnabled: true,";
                    scriptString += "zoomEnabled: true, ";
                    scriptString += "title: {text: \"WATER FLOW (m3/h)\" },exportEnabled: true,";
                    string TheSelectedResource = "All Filteration Plants";
                    if (resources == "All")
                    {
                        TheSelectedResource = "All Filteration Plants ";
                    }
                    else
                    {
                        TheSelectedResource = "" + resources + " ";
                    }
                    //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                    scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                    scriptString += "axisY: {suffix: \" m3/h\" },";
                    //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                    scriptString += "toolTip: { shared: false },";
                    scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 12},";
                    scriptString += " data: [";

                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        //string Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes , td.[V1N] as [V1N.] ,td.[V2N] AS [V2N.],td.[V3N] AS [V3N.],td.[I1] as [I1.] ,td.[I2] as [I2.] ,td.[I3] as [I3.] ,td.[Frequency] as [Frequency.], td.[PKVA] as [PKVA.] ,td.[PF] as [PF.]  ,td.[Remote] as [Remote.] ,td.[PumpStatus] ,td.[CurrentTrip] as [CurrentTrip.] , td.[VolatgeTrip] as [VoltageTrip.] ,td.[TimeSchedule] as [TimeSchedule.] ,td.[ChlorineLevel] as [ChlorineLevel.], td.[WaterFlow] as [WaterFlow(Cusec).] ,td.[PKVAR] as [PKVAR.] ,td.[PKW] as [PKW.],td.[V12] ,td.[V13] ,td.[V23] , td.[PrimpingLevel] as [PrimingLevel],td.[PressureBar] as [Pressure(Bar)] ,td.[Manual] ,td.[vib_z] ,td.[vib_y] ,td.[vib_x] from tblTubewellsTest td inner join tblResource r on td.TubeWellId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and td.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) order by td.TubeWellId, tim DESC";
                        string Dashdtquery = "";

                        Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes , td.[Pump No. 1] as [Pump No. 1], td.[Pump No. 2] as [Pump No. 2], td.[Voltage] as [Voltage], td.[Current] as [Current], td.[Power Factor] as [Power Factor], td.[Frequency] as [Frequency], td.[Power (KVA)] as [Power (KVA)], td.[Power (KVAR)] as [Power (KVAR)], td.[Power (KW)] as [Power (KW)], td.[Water Flow] as [Water Flow], td.[Chlorine Level] as [Chlorine Level], td.[Tank No. 1] as [Tank No. 1], td.[Tank No. 2] as [Tank No. 2], td.[TDS] as [TDS], td.[Manual Mode] as [Manual Mode] from tblWaterFilteration td inner join tblResource r on td.PlantId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and td.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) order by td.PlantId, tim DESC";



                        SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                        Dashdt.Clear();
                        sda.Fill(Dashdt);

                        scriptString += "{ type: \"area\", name: \"" + resourceLocation + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} m3/h</strong> at {x}\", ";
                        List<DataPoint> dataPoints = new List<DataPoint>();
                        if (Dashdt.Rows.Count > 0)
                        {
                            FiltrationPlantTableData sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), FinalTimeFrom);
                            sd.workingInHours = sd.P1WorkingInHours + sd.P2WorkingInHours;
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            FiltrationPlantTableData sd = new FiltrationPlantTableData();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            //sd.Specification = drRes["ResourceSpecification"].ToString();
                            sd.pumpStatus1 = new List<double>();
                            tubewellDataList.Add(sd);
                        }
                        foreach (DataRow drVal in Dashdt.Rows)
                        {
                            dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["tim"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["Water Flow"])));
                        }
                        scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                        scriptString += "},";
                    }
                    scriptString = scriptString.Remove(scriptString.Length - 1);
                    scriptString = scriptString + "]";
                    scriptString = scriptString + "}";
                    scriptString += ");";
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Filteration Plants";
            }
            else
            {
                selectedResource = "" + resources + " ";
            }
            Session["ReportTitle"] = "Water Discharge Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            var tubewellData = getWaterFlowData(Dashdt);
            FiltrationPlantTableData spellData = new FiltrationPlantTableData();
            spellData.waterFlow = tubewellData.waterFlow;
            spellData.SpellTimeArray = tubewellData.LogTime;
            spellData.waterDischarge = tubewellData.waterDischarge;
            spellData.LogTime = tubewellData.LogTime;
            spellData.locationName = selectedResource;
            tubewellDataList.Clear();
            spellData.avgWaterFlow = spellData.waterFlow.Average();
            tubewellDataList.Add(spellData);
            IEnumerable<FiltrationPlantTableData> itoms = (IEnumerable<FiltrationPlantTableData>)tubewellDataList;
            FiltrationPlantTableData itom = itoms.FirstOrDefault();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string outputString = serializer.Serialize(itom).ToString();
            ViewData["amChartData"] = outputString;
            return PartialView(tubewellDataList);
        }
        
        public ActionResult EnergyMonitoringReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 68))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Filteration Plants";
            //rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult EnergyMonitoringReport(FormCollection review)
        {
            string resource = "";
            if (review["resource"] == null)
            {
                resource = "All";
            }
            else
            {
                resource = review["resource"].ToString();
                string[] resourcesArray = resource.Split(',');
                string newOne = "";
                for (int i = 0; i < resourcesArray.Count(); i++)
                {
                    newOne += "'";
                    newOne += resourcesArray[i];
                    newOne += "',";
                }
                newOne = newOne.Remove(newOne.Length - 1, 1);
                resource = newOne;
            }

            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 68))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Filteration Plants";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        [ValidateInput(false)]
        public PartialViewResult _EnergyMonitoringReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            resources = System.Web.HttpUtility.HtmlDecode(resources);
            DateTime FinalTimeFrom = DateTime.Now;
            if (resources == "")
            {
                resources = "All";
            }
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                CultureInfo culture = new CultureInfo("en-US");
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date;
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time.ToUpper() == "12:00 AM" || tt_time.ToUpper() == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<FiltrationPlantTableData>();
            int resourceID = 0;


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources.ToLower() == "all")
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Filtration Plants'";
                    }
                    else
                    {
                        getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Filtration Plants' and r.ResourceName in (" + resources + ")";
                    }
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    string resourceSpecification = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    foreach (DataRow drRes in dtRes.Rows)
                    {
                        //getting resourceID 
                        resourceID = Convert.ToInt32(drRes["ResourceID"]);
                        //getting resourceLocation 
                        resourceLocation = drRes["ResourceLocation"].ToString();
                        resourceSpecification = drRes["ResourceSpecification"].ToString();
                        //query will get the list of data available against given resourceID (latest first)
                        //string Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes , td.[V1N] as [V1N.] ,td.[V2N] AS [V2N.],td.[V3N] AS [V3N.],td.[I1] as [I1.] ,td.[I2] as [I2.] ,td.[I3] as [I3.] ,td.[Frequency] as [Frequency.], td.[PKVA] as [PKVA.] ,td.[PF] as [PF.]  ,td.[Remote] as [Remote.] ,td.[PumpStatus] ,td.[CurrentTrip] as [CurrentTrip.] , td.[VolatgeTrip] as [VoltageTrip.] ,td.[TimeSchedule] as [TimeSchedule.] ,td.[ChlorineLevel] as [ChlorineLevel.], td.[WaterFlow] as [WaterFlow(Cusec).] ,td.[PKVAR] as [PKVAR.] ,td.[PKW] as [PKW.],td.[V12] ,td.[V13] ,td.[V23] , td.[PrimpingLevel] as [PrimingLevel],td.[PressureBar] as [Pressure(Bar)] ,td.[Manual] ,td.[vib_z] ,td.[vib_y] ,td.[vib_x] from tblTubewellsTest td inner join tblResource r on td.TubeWellId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and td.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) order by td.TubeWellId, tim DESC";
                        string Dashdtquery = ";WITH cte AS ( ";
                        Dashdtquery += "SELECT* FROM ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                        Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                        Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                        Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                        Dashdtquery += "s.InsertionDateTime as tim ,";
                        Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                        Dashdtquery += "FROM tblEnergy s ";
                        Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                        Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                        Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                        Dashdtquery += "where ";
                        Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                        Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                        Dashdtquery += ") ";
                        Dashdtquery += "AS SourceTable ";
                        Dashdtquery += "PIVOT ";
                        Dashdtquery += "( ";
                        Dashdtquery += "SUM(pVal) FOR pID ";
                        Dashdtquery += "IN ";
                        Dashdtquery += "( ";
                        //Dashdtquery += "[Pump No. 1],[Pump No. 2],[Voltage],[Current],[Power Factor],[Frequency],[Power (KVA)],[Power (KVAR)],[Power (KW)],[Water Flow],[Chlorine Level],[Tank No. 1],[Tank No. 2],[TDS] ";
                        Dashdtquery += "[PlantId],[Pump No. 1],[Pump No. 2],[Voltage],[Current],[Power Factor],[Frequency],[Power (KVA)],[Power (KVAR)],[Power (KW)],[Water Flow],[Chlorine Level],[Tank No. 1],[Tank No. 2],[TDS] ";
                        Dashdtquery += ") ";
                        Dashdtquery += ")  ";
                        Dashdtquery += "AS PivotTable ";
                        Dashdtquery += ")  ";
                        Dashdtquery += "SELECT* FROM cte ";
                        Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                        Dashdtquery += "tim DESC";

                        Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes , td.[Pump No. 1] as [Pump No. 1], td.[Pump No. 2] as [Pump No. 2], td.[Voltage] as [Voltage], td.[Current] as [Current], td.[Power Factor] as [Power Factor], td.[Frequency] as [Frequency], td.[Power (KVA)] as [Power (KVA)], td.[Power (KVAR)] as [Power (KVAR)], td.[Power (KW)] as [Power (KW)], td.[Water Flow] as [Water Flow], td.[Chlorine Level] as [Chlorine Level], td.[Tank No. 1] as [Tank No. 1], td.[Tank No. 2] as [Tank No. 2], td.[TDS] as [TDS] from tblWaterFilteration td inner join tblResource r on td.PlantId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and td.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) order by td.PlantId, tim DESC";



                        SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                        Dashdt.Clear();
                        sda.Fill(Dashdt);
                        if (Dashdt.Rows.Count > 0)
                        {
                            FiltrationPlantTableData sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), FinalTimeFrom);
                            sd.workingInHours = sd.P1WorkingInHours + sd.P2WorkingInHours;
                            tubewellDataList.Add(sd);
                        }
                        else
                        {
                            FiltrationPlantTableData sd = new FiltrationPlantTableData();
                            sd.locationName = drRes["ResourceLocation"].ToString();
                            //sd.Specification = drRes["ResourceSpecification"].ToString();
                            sd.pumpStatus1 = new List<double>();
                            tubewellDataList.Add(sd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Locations";
            }
            else
            {
                selectedResource = "" + resources + "";
            }
            Session["ReportTitle"] = "Energy Monitoring Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";

            #region


            ////////////////////////////////////////////////////////////////////////
            ///



            string scriptString = "";


            ///////////////////////////

            scriptString = "var chart1 = new CanvasJS.Chart(\"chartContainer1\", { theme: \"light2\", animationEnabled: true, title:{ text: \"Energy Monitoring Stats\" },exportEnabled: true, dataPointWidth: 30, subtitles: [{text: \" Energy Data Fetched from " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }], axisY:{labelFontSize: 10, labelFormatter: function(){ return \" \"; }},axisX:{labelFontSize: 10}, legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 11, horizontalAlign: \"center\"}, toolTip: {fontSize: 12, fontWeight: \"bold\", shared: true }, data: [";





            scriptString += "{ type: \"stackedColumn\", name: \"Pump 1 Working Hours\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    scriptString += "{ y: '<b>" + Math.Round((item.P1WorkingInHours), 1) + "</b>' , label: \"" + item.locationName + "\" },";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                }
            }
            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Pump 2 Working Hours\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    scriptString += "{ y: '<b>" + Math.Round((item.P2WorkingInHours), 1) + "</b>' , label: \"" + item.locationName + "\" },";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                }
            }
            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Power Factor\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double pff = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            pff += item.PF[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    pff = Math.Round((pff / counter), 2);
                    scriptString += "{ y: '<b>" + pff + "<b>' , label: \"" + item.locationName + "\" },";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                }
            }

            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Average Voltage V\", showInLegend: true, dataPoints: [";
            string chartdata1 = "[";
            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double v1ns = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            v1ns += item.Voltage[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    v1ns = Math.Round((v1ns / counter), 2);
                    double averagevln = v1ns;
                    scriptString += "{ y: " + averagevln + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Avg. Voltage\",\"tooltip\":\"" + Math.Round((averagevln), 2) + "\",\"value\":\"" + Math.Round((averagevln), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Avg. Voltage\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }

            scriptString += "]},";

            scriptString += "{ type: \"stackedColumn\", name: \"Average Current\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double i1s = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            i1s += item.Current[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    i1s = Math.Round((i1s / counter), 2);
                    double averageis = i1s;
                    scriptString += "{ y: " + averageis + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Avg. Current\",\"tooltip\":\"" + Math.Round((averageis), 2) + "\",\"value\":\"" + Math.Round((averageis), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Avg. Current\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }
            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Frequency (Hz)\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double freqs = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            freqs += item.FreqHz[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    freqs = Math.Round((freqs / counter), 2);
                    scriptString += "{ y: " + freqs + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Frequency\",\"tooltip\":\"" + Math.Round((freqs), 2) + "\",\"value\":\"" + Math.Round((freqs), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Frequency\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }
            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Average PKVA\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double pkvas = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] == 1)
                        {
                            pkvas += item.PKVA[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    pkvas = Math.Round((pkvas / counter), 2);
                    scriptString += "{ y: " + pkvas + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKVA\",\"tooltip\":\"" + Math.Round((pkvas), 2) + "\",\"value\":\"" + Math.Round((pkvas), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKVA\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }

            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Average PKVAR\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double pkvars = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            pkvars += item.PKVAR[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    pkvars = Math.Round((pkvars / counter), 2);
                    scriptString += "{ y: " + pkvars + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKVAR\",\"tooltip\":\"" + Math.Round((pkvars), 2) + "\",\"value\":\"" + Math.Round((pkvars), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKVAR\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }

            scriptString += "]},";
            scriptString += "{ type: \"stackedColumn\", name: \"Average PKW\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double pkws = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus1[i] >= 1)
                        {
                            pkws += item.PKW[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    pkws = Math.Round((pkws / counter), 2);
                    scriptString += "{ y: " + pkws + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKW\",\"tooltip\":\"" + Math.Round((pkws), 2) + "\",\"value\":\"" + Math.Round((pkws), 2) + "\"},";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"PKW\",\"tooltip\":\"0\",\"value\":\"0\"},";
                }
            }
            scriptString += "]},";

            scriptString += "{ type: \"stackedColumn\", name: \"Units Consumed\", showInLegend: true, dataPoints: [";

            foreach (var item in tubewellDataList)
            {
                if (item.workingInHours != null)
                {
                    double pkws = 0;
                    int counter = 0;
                    for (int i = 0; i < item.pumpStatus1.Count; i++)
                    {
                        if (item.pumpStatus1[i] + item.pumpStatus2[i] >= 1)
                        {
                            pkws += item.PKW[i];
                            counter++;
                        }
                        else
                        {

                        }
                    }
                    if (counter == 0)
                    {
                        counter = 1;
                    }
                    pkws = Math.Round((pkws / counter), 2);
                    double units = Math.Round((pkws * (item.workingInHours)), 2);
                    scriptString += "{ y: " + units + " , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Units Consumed\",\"tooltip\":\"" + Math.Round((units), 0) + "\",\"value\":\"" + Math.Round((units), 0) + "\"}]";
                }
                else
                {
                    scriptString += "{ y: null , label: \"" + item.locationName + "\" },";
                    chartdata1 += "{\"category\":\"Unit Consumed\",\"tooltip\":\"0\",\"value\":\"0\"}]";
                }
            }
            scriptString += "]}";

            scriptString += "] })";

            #endregion

            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            ViewData["amChartData1"] = chartdata1;

            ////////////////////////////////////////////////////////////////////////



            return PartialView(tubewellDataList);
        }

        public ActionResult EnergyMonitoringReport2()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            //rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult EnergyMonitoringReport2(FormCollection review)
        {
            string resource = "";
            if (review["resource"] == null)
            {
                resource = "All";
            }
            else
            {
                resource = review["resource"].ToString();
                string[] resourcesArray = resource.Split(',');
                string newOne = "";
                for (int i = 0; i < resourcesArray.Count(); i++)
                {
                    newOne += "'";
                    newOne += resourcesArray[i];
                    newOne += "',";
                }
                newOne = newOne.Remove(newOne.Length - 1, 1);
                resource = newOne;
            }

            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            //ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }

        public ActionResult RemoteStatusReport()
        {
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 68))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Filtration Plants";
            //rs.resourceName = "All";
            rs.dateFrom = "";
            rs.timeFrom = "";
            rs.dateTo = "";
            rs.timeTo = "";
            return View(rs);
        }
        [HttpPost]
        public ActionResult RemoteStatusReport(FormCollection review)
        {
            string resource = review["resource"].ToString();
            DateTime dateFrom = DateTime.Parse(review["dateFrom"].ToString());
            DateTime dateTo = DateTime.Parse(review["dateTo"].ToString());
            string df_date = dateFrom.ToString("d");
            string dt_date = dateTo.ToString("d");
            string TF = review["timeFrom"];
            string TT = review["timeTo"];
            string abc = review["timeFrom"];
            string[] abc1 = abc.Split(',');
            string a = abc1[0];
            if (abc1.Length > 1)
            {
                TF = abc1[1];
            }
            else
            {
                TF = abc1[0];
            }
            DataTable dt121 = new DataTable();
            Session["TimeFrom"] = TF;
            DateTime timeFrom = DateTime.Parse(TF);
            string cba = review["timeTo"];
            string[] cba1 = cba.Split(',');
            TT = cba1[0];
            DateTime timeTo = DateTime.Parse(TT);
            string tf_time = timeFrom.ToString("t");
            string tt_time = timeTo.ToString("t");
            if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
            {
                tt_time = "11:59:59 PM";
            }
            DateTime FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
            DateTime FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            int c_id = Convert.ToInt32(Session["CompanyID"]);
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            IList<string> ResourceList = new List<string>();
            ResourceList.Add("All");
            foreach (var item in db.tblResources.AsQueryable().Where(item => item.CompanyID == c_id & item.TemplateID == 64))
            {
                ResourceList.Add(item.ResourceLocation);
            }
            ViewBag.ResourceList = ResourceList;
            ViewBag.SelectedResource = resource;
            ViewBag.SelectedTimeFrom = TF;
            ViewBag.SelectedTimeTo = TT;
            ViewBag.SelectedTimeFrom = TF.ToString();
            ViewBag.SelectedTimeTo = TT.ToString();
            ViewBag.timeFrom = TF;
            ViewBag.timeTo = TT;
            ViewBag.dateFrom = df_date;
            ViewBag.dateTo = dt_date;
            RangeAndResourceSelector rs = new RangeAndResourceSelector();
            rs.resourceType = "Tubewells";
            rs.resourceName = resource;
            rs.dateFrom = dateFrom.ToString();
            rs.timeFrom = TF;
            rs.dateTo = dateTo.ToString();
            rs.timeTo = TT;
            return View(rs);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _RemoteStatusReportView(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date.AddSeconds(-1);
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<FiltrationPlantTableData>();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID from tblParameter where parameterName = 'PumpStatus'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        //string resName = drRes["resourceLocationName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        if (resources.ToLower() == "all")
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 order by cast(r.ResourceID as int) asc";
                        }
                        else
                        {
                            getParamsFromRes = "select r.ResourceID, r.ResourceName, rtp.TemplateID from tblResource r inner join tblTemplateParameter rtp on r.TemplateID = rtp.TemplateID inner join tblParameter p on rtp.ParameterID = p.ParameterID where p.ParameterName = 'PumpStatus'  and rtp.TemplateID = 64 and r.ResourceName = '" + resources + "'  order by cast(r.ResourceID as int) asc";
                        }
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"MODE STATUS REPORT\" },exportEnabled: true,";
                        string TheSelectedResource = "";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        //Session["ReportTitle"] = "Data Fetched for " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
                        scriptString += "subtitles: [{text: \" Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        //scriptString += "axisY: {suffix: \" \" },";
                        scriptString += "axisY: {labelFontSize: 10, labelFormatter: function(){ return \" \"; }},";
                        //scriptString += "axisY: {includeZero: false, prefix: \"\", labelFormatter: function(e){if(e.value == NaN){return \"No Data\";}else{return e.value;}} },";
                        //scriptString += "toolTip: { shared: false },";
                        //scriptString += "axisX: { xValueType: \"dateTime\", valueFormatString: \"hh:mm TT DD-MM-YYYY\" }, ";
                        scriptString += "toolTip: { shared: false , contentFormatter: function(e){ var str = \" \" ; for (var i = 0; i < e.entries.length; i++){ var utcSeconds = e.entries[i].dataPoint.x; var d = new Date(utcSeconds); if(e.entries[i].dataPoint.y == 0){ var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: OFF</b> at  \" + d.toLocaleString('en-IN'); str = str+temp; } else { var temp = e.entries[i].dataSeries.name + \" \" +\"<b>: ON</b> at \" + d.toLocaleString('en-IN'); str = str+temp; } } return (str); }},";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 12},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            //string parName = drPar["parameterName"].ToString();
                            string aquery = ";WITH CTE AS ( ";
                            aquery += "SELECT e.ParameterID, e.ParameterValue, e.InsertionDateTime,  ";
                            aquery += " RN = ROW_NUMBER() OVER(PARTITION BY e.ParameterID ";
                            aquery += "ORDER BY e.InsertionDateTime DESC) ";
                            aquery += "FROM tblEnergy e ";
                            aquery += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                            aquery += "WHERE e.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and e.ParameterID = " + Convert.ToInt32(drRes["ParameterID"]) + " and e.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and e.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            aquery += ") ";
                            aquery += "SELECT top 14000 ParameterID, ParameterValue, InsertionDateTime FROM CTE WHERE RN < 14401 Order by InsertionDateTime ASC";
                            string theQuery = aquery;

                            theQuery = "select 125 as ParameterID, t.PumpStatus as ParameterValue, t.InsertionDateTime from tblTubewellsTest t ";
                            theQuery += "where t.TubeWellId = " + Convert.ToInt32(drPar["ResourceID"]) + " and t.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and t.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121) order by InsertionDateTime ASC";

                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            DataTable dtVal = new DataTable();
                            sdaVal.Fill(dtVal);
                            //scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            scriptString += "{ type: \"area\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\",  ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                // if (dtVal.Rows.IndexOf(drVal) != 0)
                                // {
                                //  dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(dt).AddHours(-5).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5).AddMinutes(-1) - new DateTime(1970, 1, 1)).TotalMilliseconds), double.NaN));
                                // dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                // dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                // }
                                //else
                                //{
                                dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                                dt = Convert.ToDateTime(drVal["InsertionDateTime"]);
                                //}
                                //dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["InsertionDateTime"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Convert.ToDouble(drVal["ParameterValue"])));
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";
                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources == "All")
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceLocation = '" + resources + "' ";
                    }

                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    double noOfDays = ((Convert.ToDateTime(FinalTimeTo).Date) - (Convert.ToDateTime(FinalTimeFrom).Date)).TotalDays;
                    for (int daysCount = 0; daysCount <= noOfDays; daysCount++)
                    {
                        DateTime ftf = DateTime.Now;
                        DateTime ftt = DateTime.Now;
                        if (noOfDays == 0)
                        {
                            ftf = FinalTimeFrom;
                            ftt = FinalTimeTo;
                        }
                        if (daysCount == 0 && noOfDays > 0)
                        {
                            ftf = FinalTimeFrom;
                            ftt = FinalTimeFrom.Date.AddDays(daysCount + 1).AddSeconds(-1);
                        }
                        if (daysCount > 0 && noOfDays > 0 && daysCount != noOfDays)
                        {
                            ftf = FinalTimeFrom.Date.AddDays(daysCount);
                            ftt = FinalTimeFrom.Date.AddDays(daysCount + 1).AddSeconds(-1);
                        }
                        if (noOfDays > 0 && daysCount == noOfDays)
                        {
                            ftf = FinalTimeFrom.Date.AddDays(daysCount);
                            ftt = FinalTimeTo;
                        }
                        foreach (DataRow drRes in dtRes.Rows)
                        {
                            //getting resourceID 
                            resourceID = Convert.ToInt32(drRes["ResourceID"]);
                            //getting resourceLocation 
                            resourceLocation = drRes["ResourceLocation"].ToString();
                            //query will get the list of data available against given resourceID (latest first)
                            string Dashdtquery = ";WITH cte AS ( ";
                            Dashdtquery += "SELECT* FROM ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                            Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                            Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                            Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                            Dashdtquery += "s.InsertionDateTime as tim ,";
                            Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                            Dashdtquery += "FROM tblEnergy s ";
                            Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                            Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                            Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                            Dashdtquery += "where ";
                            Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                            //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                            Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftf + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftt + "', 103), 121)  ";
                            Dashdtquery += ") ";
                            Dashdtquery += "AS SourceTable ";
                            Dashdtquery += "PIVOT ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SUM(pVal) FOR pID ";
                            Dashdtquery += "IN ";
                            Dashdtquery += "( ";
                            Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_z],[vib_y],[vib_x] ";
                            Dashdtquery += ") ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "AS PivotTable ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "SELECT* FROM cte ";
                            Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                            Dashdtquery += "tim DESC";

                            Dashdtquery = "select r.resourceName as Location, r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, r.ResourceID, td.[InsertionDateTime] as tim, DATEDIFF(minute, td.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes , td.[V1N] as [V1N.] ,td.[V2N] AS [V2N.],td.[V3N] AS [V3N.],td.[I1] as [I1.] ,td.[I2] as [I2.] ,td.[I3] as [I3.] ,td.[Frequency] as [Frequency.], td.[PKVA] as [PKVA.] ,td.[PF] as [PF.]  ,td.[Remote] as [Remote.] ,td.[PumpStatus] ,td.[CurrentTrip] as [CurrentTrip.] , td.[VolatgeTrip] as [VoltageTrip.] ,td.[TimeSchedule] as [TimeSchedule.] ,td.[ChlorineLevel] as [ChlorineLevel.], td.[WaterFlow] as [WaterFlow(Cusec).] ,td.[PKVAR] as [PKVAR.] ,td.[PKW] as [PKW.],td.[V12] ,td.[V13] ,td.[V23] , td.[PrimpingLevel] as [PrimingLevel],td.[PressureBar] as [Pressure(Bar)] ,td.[Manual] ,td.[vib_z] ,td.[vib_y] ,td.[vib_x] from tblTubewellsTest td inner join tblResource r on td.TubeWellId = r.ResourceID where r.ResourceID = " + resourceID + " and td.InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftf + "', 103), 121) and td.InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftt + "', 103), 121) order by td.TubeWellId, tim DESC";

                            SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                            SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                            Dashdt.Clear();
                            sda.Fill(Dashdt);
                            if (Dashdt.Rows.Count > 0)
                            {
                                FiltrationPlantTableData sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), ftf);
                                //sd.noOfDays = Convert.ToInt32(noOfDays);
                                tubewellDataList.Add(sd);
                            }
                            else
                            {
                                FiltrationPlantTableData sd = new FiltrationPlantTableData();
                                sd.locationName = drRes["ResourceLocation"].ToString();
                                sd.pumpStatus1 = new List<double>();
                                //sd.I1 = null;
                                //sd.I2 = null;
                                //sd.I3 = null;
                                //sd.manualStatus = null;
                                //sd.pkva = null;
                                //sd.pkvar = null;
                                //sd.pkw = null;
                                //sd.powerFactor = null;
                                //sd.pressure = null;
                                //sd.primingTankLevel = null;
                                //sd.remoteControll = null;
                                //sd.schedulingStatus = null;
                                //sd.V12 = null;
                                //sd.V13 = null;
                                //sd.V1N = null;
                                //sd.V23 = null;
                                //sd.V2N = null;
                                //sd.V3N = null;
                                //sd.voltageTrip = null;
                                //sd.waterDischarge = null;
                                //sd.waterFlow = null;
                                //sd.workingHoursToday = null;
                                sd.workingHoursTodayP1 = "-";
                                //sd.workingHoursTodayRemote = "-";
                                //sd.workingHoursTodayScheduling = "-";
                                //sd. = ftf.ToShortDateString();
                                //sd.noOfDays = Convert.ToInt32(noOfDays);
                                tubewellDataList.Add(sd);
                            }
                        }
                    }
                    //iterate through the list of resources within the desired set of resources chosen

                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewell Locations";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Mode Status Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            return PartialView(tubewellDataList);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _RemoteStatusReportView2(string resources, string datFrom, string timFrom, string datTo, string timTo)
        {

            var tubewellDataList = new List<FiltrationPlantTableData>();
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date.AddSeconds(-1);
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            int resourceID = 0;

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";

            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;

            ////////////////////////////////////////////////////////////////////////


            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "";
                    if (resources == "All")
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    }
                    else
                    {
                        getResFromTemp += "select DISTINCT r.ResourceID, r.ResourceLocation, r.ResourceSpecification from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells' and r.ResourceLocation = '" + resources + "' ";
                    }

                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string resourceLocation = "";
                    int ite = 0;
                    double noOfDays = ((Convert.ToDateTime(FinalTimeTo).Date) - (Convert.ToDateTime(FinalTimeFrom).Date)).TotalDays;
                    for (int daysCount = 0; daysCount <= noOfDays; daysCount++)
                    {
                        DateTime ftf = DateTime.Now;
                        DateTime ftt = DateTime.Now;
                        if (noOfDays == 0)
                        {
                            ftf = FinalTimeFrom;
                            ftt = FinalTimeTo;
                        }
                        if (daysCount == 0 && noOfDays > 0)
                        {
                            ftf = FinalTimeFrom;
                            ftt = FinalTimeFrom.Date.AddDays(daysCount + 1).AddSeconds(-1);
                        }
                        if (daysCount > 0 && noOfDays > 0 && daysCount != noOfDays)
                        {
                            ftf = FinalTimeFrom.Date.AddDays(daysCount);
                            ftt = FinalTimeFrom.Date.AddDays(daysCount + 1).AddSeconds(-1);
                        }
                        if (noOfDays > 0 && daysCount == noOfDays)
                        {
                            ftf = FinalTimeFrom.Date.AddDays(daysCount);
                            ftt = FinalTimeTo;
                        }
                        foreach (DataRow drRes in dtRes.Rows)
                        {
                            //getting resourceID 
                            resourceID = Convert.ToInt32(drRes["ResourceID"]);
                            //getting resourceLocation 
                            resourceLocation = drRes["ResourceLocation"].ToString();
                            //query will get the list of data available against given resourceID (latest first)
                            string Dashdtquery = ";WITH cte AS ( ";
                            Dashdtquery += "SELECT* FROM ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                            Dashdtquery += " r.ResourceSpecification AS specifications, r.WaterLevel_m, r.PumpingWaterLevel_hpl, r.RatedDischarge_Q, r.RatedHead_H, r.Discharge_Dia_Dd, ";
                            Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                            Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                            Dashdtquery += "s.InsertionDateTime as tim ,";
                            Dashdtquery += "DATEDIFF(minute, s.InsertionDateTime, DATEADD(hour, 0,GETDATE ())) as DeltaMinutes ";
                            Dashdtquery += "FROM tblEnergy s ";
                            Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                            Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                            Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                            Dashdtquery += "where ";
                            Dashdtquery += "r.ResourceID = " + resourceID + " and ";
                            //Dashdtquery += "InsertionDateTime > DATEADD(day, DATEDIFF(day, 0, DATEADD(hour,10,GETDATE())), 0) ";
                            Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftf + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + ftt + "', 103), 121)  ";
                            Dashdtquery += ") ";
                            Dashdtquery += "AS SourceTable ";
                            Dashdtquery += "PIVOT ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SUM(pVal) FOR pID ";
                            Dashdtquery += "IN ";
                            Dashdtquery += "( ";
                            Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_z],[vib_y],[vib_x] ";
                            Dashdtquery += ") ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "AS PivotTable ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "SELECT* FROM cte ";
                            Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                            Dashdtquery += "tim DESC";
                            SqlCommand cmd = new SqlCommand(Dashdtquery, conn);
                            SqlDataAdapter sda = new SqlDataAdapter(Dashdtquery, conn);
                            Dashdt.Clear();
                            sda.Fill(Dashdt);
                            if (Dashdt.Rows.Count > 0)
                            {
                                FiltrationPlantTableData sd = getAllSpellsForRemoteStatus(Dashdt, dtRes.Rows.IndexOf(drRes), ftf);
                                //sd.noOfDays = Convert.ToInt32(noOfDays);
                                tubewellDataList.Add(sd);
                            }
                            else
                            {
                                FiltrationPlantTableData sd = new FiltrationPlantTableData();
                                sd.locationName = drRes["ResourceLocation"].ToString();
                                sd.pumpStatus1 = new List<double>();
                                //sd.I1 = null;
                                //sd.I2 = null;
                                //sd.I3 = null;
                                //sd.manualStatus = null;
                                //sd.pkva = null;
                                //sd.pkvar = null;
                                //sd.pkw = null;
                                //sd.powerFactor = null;
                                //sd.pressure = null;
                                //sd.primingTankLevel = null;
                                //sd.remoteControll = null;
                                //sd.schedulingStatus = null;
                                //sd.V12 = null;
                                //sd.V13 = null;
                                //sd.V1N = null;
                                //sd.V23 = null;
                                //sd.V2N = null;
                                //sd.V3N = null;
                                //sd.voltageTrip = null;
                                //sd.waterDischarge = null;
                                //sd.waterFlow = null;
                                //sd.workingHoursToday = null;
                                sd.workingHoursTodayP1 = "-";
                                //sd.workingHoursTodayRemote = "-";
                                //sd.workingHoursTodayScheduling = "-";
                                //sd.logDate = ftf.ToShortDateString();
                                //sd.noOfDays = Convert.ToInt32(noOfDays);
                                tubewellDataList.Add(sd);
                            }
                        }
                    }
                    //iterate through the list of resources within the desired set of resources chosen

                }
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                }
                conn.Close();
            }
            string selectedResource = "";
            if (resources == "All")
            {
                selectedResource = "All Tubewell Locations";
            }
            else
            {
                selectedResource = "" + resources + " Tubewell";
            }
            Session["ReportTitle"] = "Mode Status Report of " + selectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewData["AllData"] = serializer.Serialize(tubewellDataList);
            //JsonConvert.SerializeObject(tubewellDataList);
            return PartialView(tubewellDataList);
        }
        public void report()
        {

        }
        public TubewellDataClass getAllSpells(DataTable dt, int order)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            string specs = dt.Rows[0]["specifications"].ToString();
            string WaterLevel_m = dt.Rows[0]["WaterLevel_m"].ToString();
            string PumpingWaterLevel_hpl = dt.Rows[0]["PumpingWaterLevel_hpl"].ToString();
            string RatedDischarge_Q = dt.Rows[0]["RatedDischarge_Q"].ToString();
            string RatedHead_H = dt.Rows[0]["RatedHead_H"].ToString();
            string Discharge_Dia_Dd = dt.Rows[0]["Discharge_Dia_Dd"].ToString();
            var cms = dt.Rows[0]["PumpStatus"];
            double currentMotorStatus = 0;
            if (cms == DBNull.Value)
            {
                currentMotorStatus = 0;
            }
            else
            {
                currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            }
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = 0;
            var dm = dt.Rows[0]["DeltaMinutes"];
            if (dm == DBNull.Value)
            {
                DeltaMinutes = 0;
            }
            else
            {
                DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            }
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = 0;
                var cv = dr["PumpStatus"];
                if (cv == DBNull.Value)
                {
                    currValue = 0;
                }
                else
                {
                    currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                }
                //double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                var fr = dr["WaterFlow(Cusec)."];
                double FlowRate = 0;
                if (fr == DBNull.Value)
                {
                    FlowRate = 0;
                }
                else
                {
                    FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
                }
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(lastTime).Date.ToString();
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(lastTime).Date.ToString();
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.spellDate = Convert.ToDateTime(currTime).Date.ToString();
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.spellDate = Convert.ToDateTime(curtm).Date.ToString();
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                tableData.Vibration_m = new List<double>();
                tableData.Vibration_m_s = new List<double>();
                tableData.Vibration_m_s_2 = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.WaterLevel_m = WaterLevel_m;
                    tableData.PumpingWaterLevel_hpl = PumpingWaterLevel_hpl;
                    tableData.RatedDischarge_Q = RatedDischarge_Q;
                    tableData.RatedHead_H = RatedHead_H;
                    tableData.Discharge_Dia_Dd = Discharge_Dia_Dd;

                    var ps = dr["PumpStatus"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus.Add(tableData.pumpStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    }

                    ps = dr["WaterFlow(Cusec)."];
                    if (ps == DBNull.Value)
                    {
                        tableData.waterFlow.Add(tableData.waterFlow.LastOrDefault());
                    }
                    else
                    {
                        tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    }

                    ps = dr["ChlorineLevel."];
                    if (ps == DBNull.Value)
                    {
                        tableData.chlorineLevel.Add(tableData.chlorineLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    }

                    ps = dr["PF."];
                    if (ps == DBNull.Value)
                    {
                        tableData.powerFactor.Add(tableData.powerFactor.LastOrDefault());
                    }
                    else
                    {
                        tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    }

                    ps = dr["V1N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V1N.Add(tableData.V1N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    }

                    ps = dr["V2N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V2N.Add(tableData.V2N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    }

                    ps = dr["V3N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V3N.Add(tableData.V3N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    }

                    ps = dr["V12"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V12.Add(tableData.V12.LastOrDefault());
                    }
                    else
                    {
                        tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    }

                    ps = dr["V13"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V13.Add(tableData.V13.LastOrDefault());
                    }
                    else
                    {
                        tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    }

                    ps = dr["V23"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V23.Add(tableData.V23.LastOrDefault());
                    }
                    else
                    {
                        tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    }

                    ps = dr["VoltageTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.voltageTrip.Add(tableData.voltageTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    }

                    ps = dr["I1."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I1.Add(tableData.I1.LastOrDefault());
                    }
                    else
                    {
                        tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    }

                    ps = dr["I2."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I2.Add(tableData.I2.LastOrDefault());
                    }
                    else
                    {
                        tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    }

                    ps = dr["I3."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I3.Add(tableData.I3.LastOrDefault());
                    }
                    else
                    {
                        tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    }

                    ps = dr["CurrentTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.currentTrip.Add(tableData.currentTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    }

                    ps = dr["Frequency."];
                    if (ps == DBNull.Value)
                    {
                        tableData.frequency.Add(tableData.frequency.LastOrDefault());
                    }
                    else
                    {
                        tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    }

                    ps = dr["PKVA."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkva.Add(tableData.pkva.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    }

                    ps = dr["PKVAR."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkvar.Add(tableData.pkvar.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    }

                    ps = dr["PKW."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkw.Add(tableData.pkw.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    }

                    ps = dr["Remote."];
                    if (ps == DBNull.Value)
                    {
                        tableData.remoteControll.Add(tableData.remoteControll.LastOrDefault());
                    }
                    else
                    {
                        tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    }

                    ps = dr["TimeSchedule."];
                    if (ps == DBNull.Value)
                    {
                        tableData.schedulingStatus.Add(tableData.schedulingStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    }

                    ps = dr["Manual"];
                    if (ps == DBNull.Value)
                    {
                        tableData.manualStatus.Add(tableData.manualStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    }

                    ps = dr["PrimingLevel"];
                    if (ps == DBNull.Value)
                    {
                        tableData.primingTankLevel.Add(tableData.primingTankLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    }

                    ps = dr["Pressure(Bar)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pressure.Add(tableData.pressure.LastOrDefault());
                    }
                    else
                    {
                        tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    }


                    object value = dr["vib_x"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_x"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_y"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_z"]));
                    }
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.accWaterDischargePerDay = "0";
                }
                else
                {
                    int s = 0;
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.DefaultIfEmpty().Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                tableData.pumpStatus = new List<double>();
                tableData.pumpStatus = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.chlorineLevel = new List<double>();
                tableData.powerFactor = new List<double>();
                tableData.V1N = new List<double>();
                tableData.V2N = new List<double>();
                tableData.V3N = new List<double>();
                tableData.V12 = new List<double>();
                tableData.V13 = new List<double>();
                tableData.V23 = new List<double>();
                tableData.voltageTrip = new List<double>();
                tableData.I1 = new List<double>();
                tableData.I2 = new List<double>();
                tableData.I3 = new List<double>();
                tableData.currentTrip = new List<double>();
                tableData.frequency = new List<double>();
                tableData.pkva = new List<double>();
                tableData.pkvar = new List<double>();
                tableData.pkw = new List<double>();
                tableData.remoteControll = new List<double>();
                tableData.schedulingStatus = new List<double>();
                tableData.manualStatus = new List<double>();
                tableData.primingTankLevel = new List<double>();
                tableData.pressure = new List<double>();
                tableData.Vibration_m = new List<double>();
                tableData.Vibration_m_s = new List<double>();
                tableData.Vibration_m_s_2 = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    tableData.Specification = specs;
                    tableData.WaterLevel_m = WaterLevel_m;
                    tableData.PumpingWaterLevel_hpl = PumpingWaterLevel_hpl;
                    tableData.RatedDischarge_Q = RatedDischarge_Q;
                    tableData.RatedHead_H = RatedHead_H;
                    tableData.Discharge_Dia_Dd = Discharge_Dia_Dd;

                    var ps = dr["PumpStatus"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus.Add(tableData.pumpStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus.Add(Convert.ToInt32(dr["PumpStatus"]));
                    }

                    ps = dr["WaterFlow(Cusec)."];
                    if (ps == DBNull.Value)
                    {
                        tableData.waterFlow.Add(tableData.waterFlow.LastOrDefault());
                    }
                    else
                    {
                        tableData.waterFlow.Add(Convert.ToDouble(dr["WaterFlow(Cusec)."]));
                    }

                    ps = dr["ChlorineLevel."];
                    if (ps == DBNull.Value)
                    {
                        tableData.chlorineLevel.Add(tableData.chlorineLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.chlorineLevel.Add(Convert.ToDouble(dr["ChlorineLevel."]));
                    }

                    ps = dr["PF."];
                    if (ps == DBNull.Value)
                    {
                        tableData.powerFactor.Add(tableData.powerFactor.LastOrDefault());
                    }
                    else
                    {
                        tableData.powerFactor.Add(Convert.ToDouble(dr["PF."]));
                    }

                    ps = dr["V1N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V1N.Add(tableData.V1N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V1N.Add(Convert.ToDouble(dr["V1N."]));
                    }

                    ps = dr["V2N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V2N.Add(tableData.V2N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V2N.Add(Convert.ToDouble(dr["V2N."]));
                    }

                    ps = dr["V3N."];
                    if (ps == DBNull.Value)
                    {
                        tableData.V3N.Add(tableData.V3N.LastOrDefault());
                    }
                    else
                    {
                        tableData.V3N.Add(Convert.ToDouble(dr["V3N."]));
                    }

                    ps = dr["V12"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V12.Add(tableData.V12.LastOrDefault());
                    }
                    else
                    {
                        tableData.V12.Add(Convert.ToDouble(dr["V12"]));
                    }

                    ps = dr["V13"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V13.Add(tableData.V13.LastOrDefault());
                    }
                    else
                    {
                        tableData.V13.Add(Convert.ToDouble(dr["V13"]));
                    }

                    ps = dr["V23"];
                    if (ps == DBNull.Value)
                    {
                        tableData.V23.Add(tableData.V23.LastOrDefault());
                    }
                    else
                    {
                        tableData.V23.Add(Convert.ToDouble(dr["V23"]));
                    }

                    ps = dr["VoltageTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.voltageTrip.Add(tableData.voltageTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.voltageTrip.Add(Convert.ToDouble(dr["VoltageTrip."]));
                    }

                    ps = dr["I1."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I1.Add(tableData.I1.LastOrDefault());
                    }
                    else
                    {
                        tableData.I1.Add(Convert.ToDouble(dr["I1."]));
                    }

                    ps = dr["I2."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I2.Add(tableData.I2.LastOrDefault());
                    }
                    else
                    {
                        tableData.I2.Add(Convert.ToDouble(dr["I2."]));
                    }

                    ps = dr["I3."];
                    if (ps == DBNull.Value)
                    {
                        tableData.I3.Add(tableData.I3.LastOrDefault());
                    }
                    else
                    {
                        tableData.I3.Add(Convert.ToDouble(dr["I3."]));
                    }

                    ps = dr["CurrentTrip."];
                    if (ps == DBNull.Value)
                    {
                        tableData.currentTrip.Add(tableData.currentTrip.LastOrDefault());
                    }
                    else
                    {
                        tableData.currentTrip.Add(Convert.ToDouble(dr["CurrentTrip."]));
                    }

                    ps = dr["Frequency."];
                    if (ps == DBNull.Value)
                    {
                        tableData.frequency.Add(tableData.frequency.LastOrDefault());
                    }
                    else
                    {
                        tableData.frequency.Add(Convert.ToDouble(dr["Frequency."]));
                    }

                    ps = dr["PKVA."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkva.Add(tableData.pkva.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkva.Add(Convert.ToDouble(dr["PKVA."]));
                    }

                    ps = dr["PKVAR."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkvar.Add(tableData.pkvar.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkvar.Add(Convert.ToDouble(dr["PKVAR."]));
                    }

                    ps = dr["PKW."];
                    if (ps == DBNull.Value)
                    {
                        tableData.pkw.Add(tableData.pkw.LastOrDefault());
                    }
                    else
                    {
                        tableData.pkw.Add(Convert.ToDouble(dr["PKW."]));
                    }

                    ps = dr["Remote."];
                    if (ps == DBNull.Value)
                    {
                        tableData.remoteControll.Add(tableData.remoteControll.LastOrDefault());
                    }
                    else
                    {
                        tableData.remoteControll.Add(Convert.ToDouble(dr["Remote."]));
                    }

                    ps = dr["TimeSchedule."];
                    if (ps == DBNull.Value)
                    {
                        tableData.schedulingStatus.Add(tableData.schedulingStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.schedulingStatus.Add(Convert.ToDouble(dr["TimeSchedule."]));
                    }

                    ps = dr["Manual"];
                    if (ps == DBNull.Value)
                    {
                        tableData.manualStatus.Add(tableData.manualStatus.LastOrDefault());
                    }
                    else
                    {
                        tableData.manualStatus.Add(Convert.ToDouble(dr["Manual"]));
                    }

                    ps = dr["PrimingLevel"];
                    if (ps == DBNull.Value)
                    {
                        tableData.primingTankLevel.Add(tableData.primingTankLevel.LastOrDefault());
                    }
                    else
                    {
                        tableData.primingTankLevel.Add(Convert.ToDouble(dr["PrimingLevel"]));
                    }

                    ps = dr["Pressure(Bar)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pressure.Add(tableData.pressure.LastOrDefault());
                    }
                    else
                    {
                        tableData.pressure.Add(Convert.ToDouble(dr["Pressure(Bar)"]));
                    }

                    object value = dr["vib_x"];
                    if (value == DBNull.Value)
                    {
                        tableData.Vibration_m_s_2.Add(0);
                        tableData.Vibration_m_s.Add(0);
                        tableData.Vibration_m.Add(0);
                    }
                    else
                    {
                        tableData.Vibration_m_s_2.Add(Convert.ToDouble(dr["vib_x"]));
                        tableData.Vibration_m_s.Add(Convert.ToDouble(dr["vib_y"]));
                        tableData.Vibration_m.Add(Convert.ToDouble(dr["vib_z"]));
                    }
                }
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Where(item => item > -1).Average());
                tableData.accWaterDischargePerDay = ((tableData.WorkingInHours) * avgWaterFlow).ToString();
            }
            return tableData;
        }

        public FiltrationPlantTableData getAllSpellsForRemoteStatus(DataTable dt, int order, DateTime ftf)
        {
            var tableData = new FiltrationPlantTableData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();

            if (dt.Rows.Count > 0)
            {
                tableData.pumpStatus1 = new List<double>();
                tableData.pumpStatus2 = new List<double>();
                tableData.waterFlow = new List<double>();
                tableData.ChlorineLevel = new List<double>();
                tableData.PF = new List<double>();
                tableData.Voltage = new List<double>();
                tableData.Current = new List<double>();
                tableData.FreqHz = new List<double>();
                tableData.PKVA = new List<double>();
                tableData.PKVAR = new List<double>();
                tableData.PKW = new List<double>();
                tableData.TDS = new List<double>();
                tableData.TankLevel1ft = new List<double>();
                tableData.TankLevel2ft = new List<double>();
                tableData.LogTime = new List<string>();

                tableData.locationName = location;
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.locationName = location;
                    var ps = dr["Pump No. 1"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus1.Add(tableData.pumpStatus1.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus1.Add(Convert.ToInt32(dr["Pump No. 1"]));
                    }
                    ps = dr["Pump No. 2"];
                    if (ps == DBNull.Value)
                    {
                        tableData.pumpStatus2.Add(tableData.pumpStatus1.LastOrDefault());
                    }
                    else
                    {
                        tableData.pumpStatus2.Add(Convert.ToInt32(dr["Pump No. 2"]));
                    }
                    ps = dr["Water Flow"];
                    if (ps == DBNull.Value)
                    {
                        tableData.waterFlow.Add(tableData.waterFlow.LastOrDefault());
                    }
                    else
                    {
                        tableData.waterFlow.Add(Convert.ToDouble(dr["Water Flow"]));
                    }

                    ps = dr["Chlorine Level"];
                    if (ps == DBNull.Value)
                    {
                        tableData.ChlorineLevel.Add(1.0);
                    }
                    else
                    {
                        tableData.ChlorineLevel.Add(Convert.ToDouble(dr["Chlorine Level"]));
                    }

                    ps = dr["Power Factor"];
                    if (ps == DBNull.Value)
                    {
                        tableData.PF.Add(tableData.PF.LastOrDefault());
                    }
                    else
                    {
                        tableData.PF.Add(Convert.ToDouble(dr["Power Factor"]));
                    }

                    ps = dr["Voltage"];
                    if (ps == DBNull.Value)
                    {
                        tableData.Voltage.Add(tableData.Voltage.LastOrDefault());
                    }
                    else
                    {
                        tableData.Voltage.Add(Convert.ToDouble(dr["Voltage"]));
                    }


                    ps = dr["Current"];
                    if (ps == DBNull.Value)
                    {
                        tableData.Current.Add(tableData.Current.LastOrDefault());
                    }
                    else
                    {
                        tableData.Current.Add(Convert.ToDouble(dr["Current"]));
                    }

                    ps = dr["Frequency"];
                    if (ps == DBNull.Value)
                    {
                        tableData.FreqHz.Add(tableData.FreqHz.LastOrDefault());
                    }
                    else
                    {
                        tableData.FreqHz.Add(Convert.ToDouble(dr["Frequency"]));
                    }

                    ps = dr["Power (KVA)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.PKVA.Add(tableData.PKVA.LastOrDefault());
                    }
                    else
                    {
                        tableData.PKVA.Add(Convert.ToDouble(dr["Power (KVA)"]));
                    }

                    ps = dr["Power (KVAR)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.PKVAR.Add(tableData.PKVAR.LastOrDefault());
                    }
                    else
                    {
                        tableData.PKVAR.Add(Convert.ToDouble(dr["Power (KVAR)"]));
                    }

                    ps = dr["Power (KW)"];
                    if (ps == DBNull.Value)
                    {
                        tableData.PKW.Add(tableData.PKW.LastOrDefault());
                    }
                    else
                    {
                        tableData.PKW.Add(Convert.ToDouble(dr["Power (KW)"]));
                    }

                    ps = dr["TDS"];
                    if (ps == DBNull.Value)
                    {
                        tableData.TDS.Add(tableData.PKVA.LastOrDefault());
                    }
                    else
                    {
                        tableData.TDS.Add(Convert.ToDouble(dr["TDS"]));
                    }

                    ps = dr["Tank No. 1"];
                    if (ps == DBNull.Value)
                    {
                        tableData.TankLevel1ft.Add(tableData.PKVAR.LastOrDefault());
                    }
                    else
                    {
                        tableData.TankLevel1ft.Add(Convert.ToDouble(dr["Tank No. 1"]));
                    }

                    ps = dr["Tank No. 2"];
                    if (ps == DBNull.Value)
                    {
                        tableData.TankLevel2ft.Add(tableData.PKVAR.LastOrDefault());
                    }
                    else
                    {
                        tableData.TankLevel2ft.Add(Convert.ToDouble(dr["Tank No. 2"]));
                    }

                    ps = dr["tim"];
                    if (ps == DBNull.Value)
                    {
                        tableData.LogTime.Add(tableData.LogTime.LastOrDefault());
                    }
                    else
                    {
                        tableData.LogTime.Add(dr["tim"].ToString());
                    }
                }
                double vw = 0; double vnw = 0; double iw = 0; double inw = 0; double pfw = 0; double pfnw = 0; double fw = 0; double fnw = 0; double pkvaw = 0; double pkvanw = 0; double pkvarw = 0; double pkvarnw = 0; double pkww = 0; double pkwnw = 0;
                int wentries = 0; int nwentries = 0;
                for (int i = 0; i<tableData.pumpStatus1.Count; i++)
                {
                    if (tableData.pumpStatus1[i] + tableData.pumpStatus2[i] > 0)
                    {
                        vw += tableData.Voltage[i];
                        iw += tableData.Current[i];
                        pfw += tableData.PF[i];
                        fw += tableData.FreqHz[i];
                        pkvaw += tableData.PKVA[i];
                        pkvarw += tableData.PKVAR[i];
                        pkww += tableData.PKW[i];
                        wentries++;
                    }
                    else
                    {
                        vnw += tableData.Voltage[i];
                        inw += tableData.Current[i];
                        pfnw += tableData.PF[i];
                        fnw += tableData.FreqHz[i];
                        pkvanw += tableData.PKVA[i];
                        pkvarnw += tableData.PKVAR[i];
                        pkwnw += tableData.PKW[i];
                        nwentries++;
                    }
                }
                tableData.avgVoltageWorking = vw/wentries;
                tableData.avgCurrentWorking = iw / wentries;
                tableData.avgPFWorking = pfw / wentries;
                tableData.avgFrequencyWorking = fw / wentries;
                tableData.avgPKVAWorking = pkvaw / wentries;
                tableData.avgPKVARWorking = pkvarw / wentries;
                tableData.avgPKWWorking = pkww / wentries;

                tableData.avgVoltageNonWorking = vnw / nwentries;
                tableData.avgCurrentNonWorking = inw / nwentries;
                tableData.avgPFNonWorking = pfnw / nwentries;
                tableData.avgFrequencyNonWorking = fnw / nwentries;
                tableData.avgPKVANonWorking = pkvanw / nwentries;
                tableData.avgPKVARNonWorking = pkvarnw / nwentries;
                tableData.avgPKWNonWorking = pkwnw / nwentries;

                if (dt.Rows.Count == 0)
                {
                    tableData.WorkingHoursToday = "0 Hours, 0 Minutes";
                    tableData.waterDischarge = "0 Cusecs";
                    tableData.workingHoursTodayP1 = "0";
                    tableData.workingHoursTodayP2 = "0";
                    tableData.logDate = ftf.ToShortDateString();
                }
                else
                {
                    tableData.locationName = location;
                }
            }
            double p1wh = (Convert.ToDouble(getWorkingHours(dt, 1)) / 60);
            double p2wh = (Convert.ToDouble(getWorkingHours(dt, 2)) / 60);

            tableData.P1WorkingInHours = p1wh;
            tableData.P2WorkingInHours = p2wh;

            return tableData;
        }

        public string minutesToTime(double minutes)
        {
            var pTime = TimeSpan.FromMinutes(minutes);
            int phour = (int)pTime.TotalHours;
            int pmin = (int)pTime.Minutes;
            int psec = (int)pTime.Seconds;
            string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
            return pstr;
        }
        public string SwitchON(string id)
        {
            return "";
        }

        public ActionResult Tubewell()
        {
            return View();
        }

        public TubewellDataClass getWorkingHoursFromDatatable(DataTable dt, string ParameterName)
        {
            var tableData = new TubewellDataClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0]["PumpStatus"])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            double DeltaMinutes = Convert.ToDouble(dt.Rows[0]["DeltaMinutes"]);
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr["PumpStatus"])), 2);
                //double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr["WaterFlow(Cusec)."])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (DeltaMinutes > 28800)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1)
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            if (/*DeltaMinutes > 1440 ||*/ spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {

                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                if (spellDataList.Count == 0)
                {
                    tableData.accWaterDischargePerDay = "0";
                }
                else
                {
                    double avgWaterFlow = spellDataList.DefaultIfEmpty().Average(x => x.SpellDataArray.DefaultIfEmpty().Average());
                    if (avgWaterFlow == 0)
                    {
                        avgWaterFlow = 1;
                    }
                    tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.DefaultIfEmpty().Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
                    //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                    //tableData.accWaterDischargePerDay = ((tableData.waterFlow.Sum(x => Convert.ToDouble(x)) / Convert.ToDouble(tableData.workingHoursToday)) * 60).ToString();
                }
            }
            else
            {
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                int phour = (int)pp.TotalHours;
                int pmin = (int)pp.Minutes;
                int psec = (int)pp.Seconds;
                string pstr = " " + phour.ToString() + " Hours, " + pmin.ToString() + " Minutes";
                tableData.workingHoursToday = pstr;
                tableData.WorkingInHours = Math.Round(Convert.ToDouble(TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod))).TotalMinutes) / 60, 2);
                //tableData.workingHoursToday = spellDataList.Sum(i => i.spellPeriod).ToString();
                double avgWaterFlow = spellDataList.Average(x => x.SpellDataArray.Average());
                tableData.accWaterDischargePerDay = (((Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)) / 60) * 102) * avgWaterFlow).ToString();
            }
            return tableData;
        }

        public FiltrationPlantTableData getAllSpellsForTubewellParameterizedChart(DataTable dt, string ParameterName, DateTime timeFrom, DateTime timeTo)
        {
            var tableData = new FiltrationPlantTableData();
            var spelldata = new TubewellSpellData();
            string location = dt.Rows[0]["Location"].ToString();
            double cms = 0;
            var cms1 = dt.Rows[0]["Pump No. 1"];
            double currentMotorStatus = 0;
            if (cms1 == DBNull.Value)
            {
                currentMotorStatus += 0;
            }
            else
            {
                currentMotorStatus += Math.Round((Convert.ToDouble(dt.Rows[0]["Pump No. 1"])), 2);
            }
            var cms2 = dt.Rows[0]["Pump No. 2"];
            if (cms2 == DBNull.Value)
            {
                currentMotorStatus += 0;
            }
            else
            {
                currentMotorStatus += Math.Round((Convert.ToDouble(dt.Rows[0]["Pump No. 1"])), 2);
            }
            cms = currentMotorStatus;
            string currentTime = dt.Rows[0]["tim"].ToString();
            //timeFrom = Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["tim"].ToString());
            //timeTo = Convert.ToDateTime(dt.Rows[0]["tim"].ToString());

            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                var currV1 = dr["Pump No. 1"];
                var currV2 = dr["Pump No. 2"];
                //var currVR = dr["Remote."];
                //var currVM = dr["Manual"];
                //var currVT = dr["TimeSchedule."];
                var wf = dr[ParameterName];
                double currValue = 0;
                double currValue1 = 0;
                double currValue2 = 0;
                double currValueRemote = 0;
                double currValueManual = 0;
                double currValueScheduling = 0;
                double FlowRate = 0;
                if (currV1 == DBNull.Value)
                { }
                else
                {
                    currValue1 = Math.Round((Convert.ToDouble(dr["Pump No. 1"])), 2);
                }
                if (currV2 == DBNull.Value)
                { }
                else
                {
                    currValue2 = Math.Round((Convert.ToDouble(dr["Pump No. 2"])), 2);
                }
                currValue = currValue1 + currValue2;
                //if (currVM == DBNull.Value)
                //{ }
                //else
                //{
                //    currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //}
                //if (currVR == DBNull.Value)
                //{ }
                //else
                //{
                //    currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //}
                //if (currVT == DBNull.Value)
                //{ }
                //else
                //{
                //    currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                //}
                if (wf == DBNull.Value)
                { }
                else
                {
                    FlowRate = Math.Round((Convert.ToDouble(dr[ParameterName])), 2);
                }
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus < 1)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Manual Mode 1
                                    else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Remote Mode 2
                                    else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 2;
                                    }
                                    // Scheduling Mode 3
                                    else
                                    {
                                        spelldata.spellMode = 3;
                                    }
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Manual Mode 1
                                else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Remote Mode 2
                                else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 2;
                                }
                                // Scheduling Mode 3
                                else
                                {
                                    spelldata.spellMode = 3;
                                }
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue < 1)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Manual Mode 1
                                    else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 1;
                                    }
                                    // Remote Mode 2
                                    else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                    {
                                        spelldata.spellMode = 2;
                                    }
                                    // Scheduling Mode 3
                                    else
                                    {
                                        spelldata.spellMode = 3;
                                    }
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                if (currValueManual == 0 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Manual Mode 1
                                else if (currValueManual == 1 && currValueRemote == 0 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 1;
                                }
                                // Remote Mode 2
                                else if (currValueManual == 0 && currValueRemote == 1 && currValueScheduling == 0)
                                {
                                    spelldata.spellMode = 2;
                                }
                                // Scheduling Mode 3
                                else
                                {
                                    spelldata.spellMode = 3;
                                }
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue < 1 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 2 (uncleared/ ponding continues)
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            if (spellDataList.Count == 0)
            {
                tableData.locationName = location;
                tableData.workingInHours = 0;
                tableData.totalHours = 0;
                tableData.availableHours = 0;
                tableData.nonAvailableHours = 0;
                tableData.workingInHoursManual = 0;
                tableData.workingInHoursRemote = 0;
                tableData.workingInHoursScheduling = 0;
                tableData.minValue = 0;
                tableData.maxValue = 0;
                tableData.avgVale = 0;
                tableData.avgOfAvailableHours = 0;
                tableData.avgOfNonAvailableHours = 0;
            }
            if (dt.Rows.Count > 0)
            {
                tableData.locationName = location;
                if (timeTo > DateTime.Now)
                {
                    timeTo = DateTime.Now;
                }
                tableData.totalHours = Math.Round((((timeTo - timeFrom).TotalMinutes) / 60), 2);
                if (tableData.totalHours == 23.98)
                {
                    tableData.totalHours = 24;
                }
                tableData.SpellDataArray = new List<double>();
                tableData.SpellTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.SpellTimeArray.Add(dr["tim"].ToString());
                    tableData.SpellDataArray.Add(Convert.ToDouble(dr[ParameterName].ToString()));
                }
                tableData.workingInHoursManual = Math.Round(((spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHoursRemote = Math.Round(((spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHoursScheduling = Math.Round(((spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHours = Math.Round(((Convert.ToDouble(tableData.workingInHoursManual) +
                Convert.ToDouble(tableData.workingInHoursRemote) +
                Convert.ToDouble(tableData.workingInHoursScheduling))), 2);
                tableData.nonWorkingInHours = Math.Round((tableData.totalHours - tableData.workingInHours), 2);
                tableData.availableHours = Math.Round((getAvailableHours(dt, ParameterName)), 2);
                tableData.nonAvailableHours = Math.Round((tableData.totalHours - tableData.availableHours), 2);
                if (tableData.nonAvailableHours == 0.02 && tableData.availableHours > 1)
                {
                    tableData.nonAvailableHours = 0;
                }
                if (tableData.availableHours == 23.98)
                {
                    tableData.availableHours = 24;
                }
                double availableSum = 0;
                double unavailableSum = 0;
                int availableCount = 0;
                int unavailableCount = 0;
                List<double> valList = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["Pump No. 1"]) + Convert.ToInt32(dr["Pump No. 2"]) == 0)
                    {
                        unavailableSum += Convert.ToDouble(dr[ParameterName]);
                        unavailableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                    else
                    {
                        availableSum += Convert.ToDouble(dr[ParameterName]);
                        availableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                }
                if (availableCount == 0)
                {
                    availableCount = 1;
                }
                if (unavailableCount == 0)
                {
                    unavailableCount = 1;
                }
                tableData.avgOfAvailableHours = Math.Round((availableSum / availableCount), 2);
                tableData.avgOfNonAvailableHours = Math.Round((unavailableSum / unavailableCount), 2);


                if (ParameterName == "WaterFlow(Cusec).")
                {
                    tableData.avgOfAvailableHours = Math.Round((tableData.avgOfAvailableHours / 101.94), 2);
                    tableData.avgOfNonAvailableHours = Math.Round((tableData.avgOfNonAvailableHours / 101.94), 2);
                }

                tableData.minValue = Math.Round((valList.Min()), 2);
                tableData.maxValue = Math.Round((valList.Max()), 2);
                tableData.avgVale = Math.Round((valList.Average()), 2);

                tableData.totalHoursString = minutesToTime(tableData.totalHours * 60);
                tableData.workingInHoursString = minutesToTime(tableData.workingInHours * 60);
                tableData.nonWorkingInHoursString = minutesToTime(tableData.nonWorkingInHours * 60);
                tableData.workingInHoursRemoteString = minutesToTime(tableData.workingInHoursRemote * 60);
                tableData.workingInHoursManualString = minutesToTime(tableData.workingInHoursManual * 60);
                tableData.workingInHoursSchedulingString = minutesToTime(tableData.workingInHoursScheduling * 60);
                tableData.availableHoursString = minutesToTime(tableData.availableHours * 60);
                tableData.nonAvailableHoursString = minutesToTime(tableData.nonAvailableHours * 60);
                tableData.parameterName = ParameterName;
            }
            else
            {
                tableData.locationName = location;
                if (timeTo > DateTime.Now)
                {
                    timeTo = DateTime.Now;
                }
                tableData.totalHours = Math.Round((((timeTo - timeFrom).TotalMinutes) / 60), 2);
                if (tableData.totalHours == 23.98)
                {
                    tableData.totalHours = 24;
                }
                tableData.SpellDataArray = new List<double>();
                tableData.SpellTimeArray = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    tableData.SpellTimeArray.Add(dr["tim"].ToString());
                    tableData.SpellDataArray.Add(Convert.ToDouble(dr[ParameterName].ToString()));
                }
                tableData.workingInHoursManual = Math.Round(((spellDataList.Where(r => r.spellMode == 1).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHoursRemote = Math.Round(((spellDataList.Where(r => r.spellMode == 2).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHoursScheduling = Math.Round(((spellDataList.Where(r => r.spellMode == 3).Sum(i => i.spellPeriod)) / 60), 2);
                tableData.workingInHours = Math.Round(((Convert.ToDouble(tableData.workingInHoursManual) +
                Convert.ToDouble(tableData.workingInHoursRemote) +
                Convert.ToDouble(tableData.workingInHoursScheduling))), 2);
                tableData.nonWorkingInHours = Math.Round((tableData.totalHours - tableData.workingInHours), 2);
                tableData.availableHours = Math.Round((getAvailableHours(dt, ParameterName)), 2);
                tableData.nonAvailableHours = Math.Round((tableData.totalHours - tableData.availableHours), 2);
                if (tableData.nonAvailableHours == 0.02 && tableData.availableHours > 1)
                {
                    tableData.nonAvailableHours = 0;
                }
                if (tableData.availableHours == 23.98)
                {
                    tableData.availableHours = 24;
                }
                double availableSum = 0;
                double unavailableSum = 0;
                int availableCount = 0;
                int unavailableCount = 0;
                List<double> valList = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["Pump No. 1"]) + Convert.ToInt32(dr["Pump No. 2"]) == 0)
                    {
                        unavailableSum += Convert.ToDouble(dr[ParameterName]);
                        unavailableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                    else
                    {
                        availableSum += Convert.ToDouble(dr[ParameterName]);
                        availableCount += 1;
                        valList.Add(Convert.ToDouble(dr[ParameterName]));
                    }
                }
                if (availableCount == 0)
                {
                    availableCount = 1;
                }
                if (unavailableCount == 0)
                {
                    unavailableCount = 1;
                }
                tableData.avgOfAvailableHours = Math.Round((availableSum / availableCount), 2);
                tableData.avgOfNonAvailableHours = Math.Round((unavailableSum / unavailableCount), 2);


                if (ParameterName == "WaterFlow(Cusec).")
                {
                    tableData.avgOfAvailableHours = Math.Round((tableData.avgOfAvailableHours / 101.94), 2);
                    tableData.avgOfNonAvailableHours = Math.Round((tableData.avgOfNonAvailableHours / 101.94), 2);
                }

                tableData.minValue = Math.Round((valList.Min()), 2);
                tableData.maxValue = Math.Round((valList.Max()), 2);
                tableData.avgVale = Math.Round((valList.Average()), 2);

                tableData.totalHoursString = minutesToTime(tableData.totalHours * 60);
                tableData.workingInHoursString = minutesToTime(tableData.workingInHours * 60);
                tableData.nonWorkingInHoursString = minutesToTime(tableData.nonWorkingInHours * 60);
                tableData.workingInHoursRemoteString = minutesToTime(tableData.workingInHoursRemote * 60);
                tableData.workingInHoursManualString = minutesToTime(tableData.workingInHoursManual * 60);
                tableData.workingInHoursSchedulingString = minutesToTime(tableData.workingInHoursScheduling * 60);
                tableData.availableHoursString = minutesToTime(tableData.availableHours * 60);
                tableData.nonAvailableHoursString = minutesToTime(tableData.nonAvailableHours * 60);
                tableData.parameterName = ParameterName;
            }
            //foreach (DataRow dr in dt.Rows)
            //{
            //    tableData.SpellTimeArray.Add(dr["tim"].ToString());
            //    tableData.SpellDataArray.Add(Convert.ToDouble(dr[ParameterName]));
            //    //tableData.waterFlow.Add(Convert.ToDouble(dr[ParameterName]));
            //}
            tableData.minValue = tableData.SpellDataArray.Min();
            tableData.maxValue = tableData.SpellDataArray.Max();
            tableData.avgVale = tableData.SpellDataArray.Average();
            return tableData;
        }

        public double getAvailableHours(DataTable dt, string ParameterName)
        {
            var tableData = new TubewellParameterChartClass();
            var spelldata = new TubewellSpellData();
            //int resourceID = Convert.ToInt32(dt.Rows[0]["resourceID"]);
            string location = dt.Rows[0]["Location"].ToString();
            double currentMotorStatus = Math.Round((Convert.ToDouble(dt.Rows[0][ParameterName])), 2);
            string currentTime = dt.Rows[0]["tim"].ToString();
            bool S = false;
            bool E = false;
            bool T = true;
            bool F = false;
            int spell = 0;
            List<TubewellSpellData> spellDataList = new List<TubewellSpellData>();
            string curtm = "";
            foreach (DataRow dr in dt.Rows)
            {
                double currValue = Math.Round((Convert.ToDouble(dr[ParameterName])), 2);
                //double currValueRemote = Math.Round((Convert.ToDouble(dr["Remote."])), 2);
                //double currValueManual = Math.Round((Convert.ToDouble(dr["Manual"])), 2);
                //double currValueScheduling = Math.Round((Convert.ToDouble(dr["TimeSchedule."])), 2);
                double FlowRate = Math.Round((Convert.ToDouble(dr[ParameterName])), 2);
                string currTime = dr["tim"].ToString();
                string clearaceTime = "";
                //start scenario 3 (inactive)
                if (0 > 1)
                {

                }
                // end  scenario 3 (inactive)
                else
                {
                    //start scenario 1 (No Ponding since many time/cleared/ zero received (find out what is the last ponding time if any))
                    if (currentMotorStatus == 0)
                    {
                        if (E == F && S == F)
                        {
                            if (currValue == 0)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue == 0 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                //int indexMax = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value > b.Value) ? a : b).Index;
                                //int indexMin = !spelldata.SpellDataArray.Any() ? 0 : spelldata.SpellDataArray.Select((value, index) => new { Value = value, Index = index }).Aggregate((a, b) => (a.Value < b.Value) ? a : b).Index;
                                //spelldata.spellMaxTime = spelldata.SpellTimeArray.ElementAt(indexMax);
                                //spelldata.spellMinTime = spelldata.SpellTimeArray.ElementAt(indexMin);
                                //spelldata.SpellMax = spelldata.SpellDataArray.DefaultIfEmpty().Max();
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                //spelldata.spellFlowDown = Math.Round(spelldata.SpellMax / spelldata.spellPeriod, 2);
                                //spelldata.spellFlowUp = Math.Round(spelldata.SpellMax / Math.Abs((Convert.ToDateTime(spelldata.spellMaxTime) - Convert.ToDateTime(spelldata.SpellStartTime)).TotalMinutes), 2);
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                    // end  scenario 1 (No Ponding since many time/cleared/ zero received)
                    //////////////////////////////////////////////////////////////////////
                    //start scenario 2 (uncleared/ ponding continues (find out when the ponding is started))
                    else
                    {
                        if (E == F && S == F)
                        {
                            if (currValue == 0)
                            {
                                if (spelldata.SpellDataArray.Count > 0)
                                {
                                    string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                    double lastvalue = spelldata.SpellDataArray.LastOrDefault();
                                    E = T;
                                    S = T;
                                    spelldata.SpellDataArray.Add(lastvalue);
                                    spelldata.SpellTimeArray.Add(lastTime);
                                    spelldata.SpellEndTime = currTime;
                                    clearaceTime = currTime;
                                }

                            }
                            else
                            {
                                E = T;
                                spell = spell + 1;
                                spelldata.SpellNumber = spell;
                                spelldata.SpellDataArray.Add(FlowRate);
                                spelldata.SpellTimeArray.Add(currTime);
                                spelldata.SpellEndTime = currTime;
                                clearaceTime = currTime;

                            }
                        }
                        else if (E == T && S == F)
                        {
                            if (currValue == 0 || dr == dt.Rows[dt.Rows.Count - 1])
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = lastTime;
                                    S = T;
                                }
                                else
                                {

                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                            }
                            else
                            {
                                string lastTime = spelldata.SpellTimeArray.LastOrDefault().ToString();
                                if (((Convert.ToDateTime(lastTime)) - (Convert.ToDateTime(currTime))).TotalMinutes > 10)
                                {
                                    spelldata.SpellStartTime = currTime;
                                    S = T;
                                }
                                else
                                {
                                    spelldata.SpellDataArray.Add(FlowRate);
                                    spelldata.SpellTimeArray.Add(currTime);
                                }
                            }
                        }
                        if (E == T && S == T)
                        {
                            E = F;
                            S = F;
                            if (spelldata.SpellDataArray.Count > 1 /*&& spelldata.SpellDataArray.Sum() > 0*/)
                            {
                                spelldata.spellPeriod = Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes);
                                if (spelldata.spellPeriod == 0)
                                {
                                    spelldata.spellPeriod = 1;
                                }
                                spellDataList.Add(spelldata);
                                spelldata = new TubewellSpellData();
                                string s = JsonConvert.SerializeObject(spellDataList);
                            }
                        }
                    }
                }
                curtm = currTime;
            }
            if (spellDataList.Count < 1)
            {
                if (spelldata.SpellDataArray.Count > 0)
                {
                    spelldata.SpellStartTime = curtm;
                    spelldata.spellPeriod = Convert.ToDouble(Convert.ToInt32(Math.Abs((Convert.ToDateTime(spelldata.SpellStartTime) - Convert.ToDateTime(spelldata.SpellEndTime)).TotalMinutes)));
                    if (spelldata.spellPeriod == 0)
                    {
                        spelldata.spellPeriod = 1;
                    }
                    spellDataList.Add(spelldata);
                }
            }
            string c = JsonConvert.SerializeObject(spellDataList);
            if (spelldata.SpellDataArray.Count == 0)
            {
                spelldata.SpellDataArray.Add(currentMotorStatus);
                spelldata.SpellTimeArray.Add(currentTime);
                spelldata.SpellStartTime = currentTime;
                spelldata.SpellEndTime = currentTime;
            }
            double availableHours = 0;
            if (spelldata.SpellDataArray.Count == 0 || spellDataList.Count == 0)
            {
                availableHours = 0;
            }
            else
            {
                var pp = TimeSpan.FromMinutes(Convert.ToDouble(spellDataList.Sum(i => i.spellPeriod)));
                availableHours = Convert.ToDouble(pp.TotalMinutes) / 60;
            }
            return availableHours;
        }


        [HttpGet]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 20)]
        public PartialViewResult _TubewellDashboardLocationWise(string resources, string parameter, string datFrom, string timFrom, string datTo, string timTo)
        {
            if (parameter == "")
            {
                parameter = "all";
            }
            DateTime FinalTimeFrom = DateTime.Now;
            DateTime FinalTimeTo = DateTime.Now;

            DataTable dtVal = new DataTable();
            string parameterName = "";
            if (datFrom == "" && timFrom == "" && datTo == "" && timTo == "")
            {
                FinalTimeFrom = DateTime.Now.AddHours(0).Date;
                FinalTimeTo = DateTime.Now.AddHours(0).AddDays(1).Date.AddSeconds(-1);
            }
            else
            {
                DateTime dateFrom = DateTime.Parse(datFrom);
                DateTime dateTo = DateTime.Parse(datTo);
                string df_date = dateFrom.ToString("d");
                string dt_date = dateTo.ToString("d");
                string TF = timFrom;
                string TT = timTo;
                string abc = timFrom;
                string[] abc1 = abc.Split(',');
                string a = abc1[0];
                if (abc1.Length > 1)
                {
                    TF = abc1[1];
                }
                else
                {
                    TF = abc1[0];
                }
                DataTable dt121 = new DataTable();
                Session["TimeFrom"] = TF;
                DateTime timeFrom = DateTime.Parse(TF);
                string cba = timTo;
                string[] cba1 = cba.Split(',');
                TT = cba1[0];
                DateTime timeTo = DateTime.Parse(TT);
                string tf_time = timeFrom.ToString("t");
                string tt_time = timeTo.ToString("t");
                if (tt_time == "12:00 AM" || tt_time == "11:59 PM")
                {
                    tt_time = "11:59:59 PM";
                }
                FinalTimeFrom = Convert.ToDateTime(df_date + " " + tf_time);
                FinalTimeTo = Convert.ToDateTime(dt_date + " " + tt_time);
            }
            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<FiltrationPlantTableData>();

            ////////////////////////////////////////////////////////////////////////

            string scriptString = "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string getResFromTemp = "select ParameterID, ParameterName from tblParameter where ParameterDescription = '" + parameter + "'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    DataTable dtRes1 = new DataTable();
                    sdaRes.Fill(dtRes1);
                    int ite = 0;
                    foreach (DataRow drRes in dtRes1.Rows)
                    {
                        parameterName = drRes["ParameterName"].ToString();
                        ite += 1;
                        string getParamsFromRes = "";
                        getParamsFromRes = "select r.ResourceID, r.ResourceName from tblResource r where r.ResourceName = '" + resources + "' ";
                        SqlDataAdapter sdaPar = new SqlDataAdapter(getParamsFromRes, conn);
                        DataTable dtPar = new DataTable();
                        sdaPar.Fill(dtPar);
                        scriptString += "var chart" + ite + " = new CanvasJS.Chart(\"chartContainer" + ite + "\", {";
                        scriptString += "theme: \"light2\",";
                        scriptString += "animationEnabled: true,";
                        scriptString += "zoomEnabled: true, ";
                        scriptString += "title: {text: \"" + parameter + "\" },exportEnabled: true,";
                        string TheSelectedResource = "All Tubewells";
                        if (resources == "All")
                        {
                            TheSelectedResource = "All Tubewells ";
                        }
                        else
                        {
                            TheSelectedResource = "" + resources + " Tubewell";
                        }
                        scriptString += "subtitles: [{text: \" " + parameter + " Data Fetched from " + TheSelectedResource + " between " + FinalTimeFrom + " to " + FinalTimeTo + "  \" }],";
                        scriptString += "axisY:{labelFontSize: 15},";
                        scriptString += "toolTip: { shared: false },";
                        scriptString += "legend: { cursor: \"pointer\", itemclick: toogleDataSeries, fontSize: 12},";
                        scriptString += " data: [";
                        foreach (DataRow drPar in dtPar.Rows)
                        {
                            string Dashdtquery = ";WITH cte AS ( ";
                            Dashdtquery += "SELECT* FROM ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
                            Dashdtquery += "r.ResourceID, p.ParameterName AS pID, ";
                            Dashdtquery += "CAST(s.ParameterValue AS NUMERIC(18,2)) AS pVal, ";
                            Dashdtquery += "s.InsertionDateTime as tim ";
                            Dashdtquery += "FROM tblEnergy s ";
                            Dashdtquery += "inner join tblResource r on s.ResourceID = r.ResourceID ";
                            Dashdtquery += "inner join tblParameter p on s.ParameterID = p.ParameterID ";
                            Dashdtquery += "inner join tblTemplate rt on r.TemplateID = rt.TemplateID ";
                            Dashdtquery += "where ";
                            Dashdtquery += "r.ResourceID = " + Convert.ToInt32(drPar["ResourceID"]) + " and ";
                            Dashdtquery += "InsertionDateTime >= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeFrom + "', 103), 121) and InsertionDateTime <= CONVERT(CHAR(24), CONVERT(DATETIME, '" + FinalTimeTo + "', 103), 121)  ";
                            Dashdtquery += ") ";
                            Dashdtquery += "AS SourceTable ";
                            Dashdtquery += "PIVOT ";
                            Dashdtquery += "( ";
                            Dashdtquery += "SUM(pVal) FOR pID ";
                            Dashdtquery += "IN ";
                            Dashdtquery += "( ";
                            Dashdtquery += "[V1N.],[V2N.],[V3N.],[I1.],[I2.],[I3.],[Frequency.],[PKVA.],[PF.],[Remote.],[PumpStatus],[CurrentTrip.],[VoltageTrip.],[TimeSchedule.],[ChlorineLevel.],[WaterFlow(Cusec).],[PKVAR.],[PKW.],[V12],[V13],[V23],[PrimingLevel],[Pressure(Bar)],[Manual],[vib_z],[vib_y],[vib_x] ";
                            Dashdtquery += ") ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "AS PivotTable ";
                            Dashdtquery += ")  ";
                            Dashdtquery += "SELECT* FROM cte ";
                            Dashdtquery += "order by cast(ResourceID as INT) ASC, ";
                            Dashdtquery += "tim DESC";
                            string theQuery = Dashdtquery;
                            SqlDataAdapter sdaVal = new SqlDataAdapter(theQuery, conn);
                            sdaVal.Fill(dtVal);
                            scriptString += "{ type: \"line\", name: \"" + drPar["ResourceName"].ToString() + "\", showInLegend: true,  markerSize: 1, xValueType: \"dateTime\", xValueFormatString: \"hh:mm TT DD-MM-YYYY\", yValueFormatString: \"#,##0.##\", toolTipContent: \"{label}<br/>{name}, <strong>{y} </strong> at {x}\", ";
                            List<DataPoint> dataPoints = new List<DataPoint>();
                            DateTime dt = DateTime.Now;
                            foreach (DataRow drVal in dtVal.Rows)
                            {
                                if (parameter == "Vibration Acceleration in (m/s2)")
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["tim"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Math.Round(Math.Abs(Convert.ToDouble(drVal[drRes["ParameterName"].ToString()]) * 0.3), 2)));
                                    dt = Convert.ToDateTime(drVal["tim"]);
                                }
                                else if (parameter == "Vibration Displacement in (um)")
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["tim"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Math.Round(Math.Abs(Convert.ToDouble(drVal[drRes["ParameterName"].ToString()]) / 0.3), 2)));
                                    dt = Convert.ToDateTime(drVal["tim"]);
                                }
                                else
                                {
                                    dataPoints.Add(new DataPoint(Convert.ToDouble((long)(Convert.ToDateTime(drVal["tim"]).AddHours(-5) - new DateTime(1970, 1, 1)).TotalMilliseconds), Math.Abs(Convert.ToDouble(drVal[drRes["ParameterName"].ToString()]))));
                                    dt = Convert.ToDateTime(drVal["tim"]);
                                }
                            }
                            scriptString += "dataPoints: " + Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints) + "";
                            scriptString += "},";

                        }
                        scriptString = scriptString.Remove(scriptString.Length - 1);
                        scriptString = scriptString + "]";
                        scriptString = scriptString + "}";
                        scriptString += ");";
                        //TubewellParameterChartClass tableData1 = getAllSpellsForTubewellParameterizedChart(dtVal, parameterName, FinalTimeFrom, FinalTimeTo);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string NewscripString = scriptString;
            ViewData["chartData"] = NewscripString;
            FiltrationPlantTableData tableData = new FiltrationPlantTableData();
            if (dtVal.Rows.Count > 0)
            {
                tableData = getAllSpellsForTubewellParameterizedChart(dtVal, parameterName, FinalTimeFrom, FinalTimeTo);
            }
            else
            {
                tableData.locationName = resources;
                tableData.parameterName = parameter;
                tableData.workingInHours = 0;
                tableData.totalHours = 0;
                tableData.availableHours = 0;
                tableData.nonAvailableHours = 0;
                tableData.workingInHoursManual = 0;
                tableData.workingInHoursRemote = 0;
                tableData.workingInHoursScheduling = 0;
                tableData.minValue = 0;
                tableData.maxValue = 0;
                tableData.avgVale = 0;
                tableData.avgOfAvailableHours = 0;
                tableData.avgOfNonAvailableHours = 0;
            }
            tableData.parameterName = parameter;
            tubewellDataList.Add(tableData);
            return PartialView(tubewellDataList);
        }



    }
}