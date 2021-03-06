using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using WASA_EMS.Models;

namespace WASA_EMS
{
    /// <summary>
    /// Summary description for saveData
    /// </summary>
    /// FOR WASA TTLOGIX
    [WebService(Namespace = "http://wasalhr.ttlogix.com/")]
    /// FOR WASA AMAZON WEB SERVER
    //[WebService(Namespace = "http://http://35.166.74.72/")]
    /// FOR LOCALHOST
    //[WebService(Namespace = "http://localhost/")]


    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class saveData : System.Web.Services.WebService
    {
        public int c_id;
        [WebMethod]
        public string insertData(string code, string val, string time)
        {
            string Message;
            BAL bal = new BAL();
            int result = bal.saveData(code, val, time);
            if (result == 1)
            {
                Message = "Details are inserted successfully";
            }
            else
            {
                Message = "Details are not inserted successfully";
            }
            return Message;
        }

        [WebMethod]
        public string getFiltrationPlantMarkers()
        {

            string tempName = "";
            string datetimed = "";
            string markers = "[";
            string parameterValuesString = "";
            string q1 = "";
            q1 += "select top(28)  r.ResourceLocation,  t.TemplateName, p.ParameterName, e.ParameterValue, e.InsertionDateTime  from tblEnergy e ";
            q1 += "left join tblParameter p on e.parameterID = p.parameterID ";
            q1 += "left join tblResource r on e.resourceID = r.resourceID ";
            q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
            q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = 1080) ";
            q1 += "and r.resourceID = 1080 order by p.paramOrder";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(q1, conn);
                    conn.Open();
                    using (SqlDataReader sdr1 = cmd.ExecuteReader())
                    {
                        while (sdr1.Read())
                        {
                            string valuee = "";
                            parameterValuesString += "";
                            if (sdr1["parameterName"].ToString() == "Pump No. 1")
                            {
                                parameterValuesString += "Pump No. 1 " + ": ";
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "OFF";
                                }
                                else
                                {
                                    valuee = "ON";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Pump No. 2")
                            {
                                parameterValuesString += "Pump No. 2 " + ": ";
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "OFF";
                                }
                                else
                                {
                                    valuee = "ON";
                                }
                            }
                            else if (sdr1["parameterName"].ToString() == "Chlorine Level")
                            {
                                parameterValuesString += "Chlorine Level " + ": ";
                                if (sdr1["parameterValue"].ToString() == "0")
                                {
                                    valuee = "LOW";
                                }
                                else
                                {
                                    valuee = "HIGH";
                                }
                            }
                            else if (sdr1["ParameterName"].ToString() == "Tank No. 1")
                            {
                                parameterValuesString += "Tank No. 1" + ": ";
                                valuee = Math.Round(Convert.ToDouble(sdr1["parameterValue"]), 1).ToString() + " ft";
                            }
                            else if (sdr1["ParameterName"].ToString() == "Tank No. 2")
                            {
                                parameterValuesString += "Tank No. 1" + ": ";
                                valuee = Math.Round(Convert.ToDouble(sdr1["parameterValue"]), 1).ToString() + " ft";
                            }
                            else if (sdr1["ParameterName"].ToString() == "Manual Mode")
                            {
                                parameterValuesString += "Mode " + ": ";
                                if (sdr1["parameterValue"].ToString() == "1")
                                {
                                    valuee = "Manual";
                                }
                                else
                                {
                                    valuee = "Auto";
                                }
                            }
                            else
                            {
                                parameterValuesString += sdr1["ParameterName"].ToString() + ": ";
                                valuee = Math.Round(Convert.ToDouble(sdr1["parameterValue"]), 2).ToString();
                            }
                            parameterValuesString += valuee;
                            parameterValuesString += "<br />";
                            datetimed = sdr1["InsertionDateTime"].ToString();
                        }
                    }
                    tempName = "F";
                    string newstring = "<b>Filtration Plant</b>";
                    newstring += "<br />";
                    newstring += "<b>Pattoki Filtration Plant</b>";
                    newstring += "<br />";
                    newstring += datetimed;
                    newstring += "<br />";
                    newstring += parameterValuesString;
                    TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                    double minu = Math.Abs(duration.TotalMinutes);
                    parameterValuesString = "";
                    markers += "{";
                    markers += string.Format("'Template': '{0}',", tempName);
                    markers += string.Format("'title': '{0}',", "Lawrence Road");
                    markers += string.Format("'lat':'{0}',", "31.395095");
                    markers += string.Format("'lnt':'{0}',", "74.362954");
                    markers += string.Format("'Delta': '{0}',", minu.ToString());
                    markers += string.Format("'description': '{0}'", newstring);
                    markers += "},";
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }

            }
            markers = markers.Remove(markers.Length - 1, 1);
            markers += "]";
            var data = new { status = "Success" };
            return markers;
        }

        [WebMethod]
        public string ShutDownPump(string sender, string v12, string v23, string v13, string i1, string i2, string i3)
        {
            string message = "";
            BAL bal = new BAL();
            int code = bal.shutDownPump(sender, v12, v23, v13, i1, i2, i3);
            if (code == 0)
            {
                message = "TURN OFF";
            }
            else
            {
                message = "ERROR";
            }
            return message;
        }

        [WebMethod]
        public string insertUpdateData(string code, string val, string time, string lat, string lng)
        {
            string Message;
            BAL bal = new BAL();
            int result = bal.saveData(code, val, time, lat, lng);
            if (result == 1)
            {
                Message = "Details are inserted successfully";
            }
            else
            {
                Message = "Details are not inserted successfully";
            }
            return Message;
        }
        [WebMethod]
        public string getRemoteStatus(string sender)
        {
            BAL bal = new BAL();
            string Mode = bal.getRemoteMode(sender);
            return Mode;
        }

        [WebMethod]
        public string ChangeTubewellWorkingMode(string sender, string v12, string v23, string v13, string i1, string i2, string i3)
        {
            BAL bal = new BAL();
            string result = bal.ChangeTubewellWorkingMode(sender, v12, v23, v13, i1, i2, i3);
            return result;
        }

        [WebMethod]
        public string TubewellVoltageError(string sender, string v12, string v23, string v13, string mode)
        {
            BAL bal = new BAL();
            string result = bal.voltageError(sender, v12, v23, v13, mode);
            return result;
        }

        [WebMethod]
        public string TubewellCurrentError(string sender, string i1, string i2, string i3)
        {
            BAL bal = new BAL();
            string result = bal.currentError(sender, i1, i2, i3);
            return result;
        }

        public double average(int a, int b)
        {
            double res = Convert.ToDouble(b) / 2 ;
            return res;
        }

        [WebMethod]
        public string getScheduleTime(string sender)
        {
            BAL bal = new BAL();
            string ScheduleTime = bal.getScheduleTime(sender);
            return ScheduleTime;
        }

        [WebMethod]
        public string getCurrentRange(string sender)
        {
            BAL bal = new BAL();
            string ScheduleTime = bal.getCurrentRange(sender);
            ScheduleTime = ScheduleTime.Replace("MinRange", "Lower");
            ScheduleTime = ScheduleTime.Replace("MaxRange", "Upper");
            return ScheduleTime;
        }

        [WebMethod]
        public string getVoltRange(string sender)
        {
            BAL bal = new BAL();
            string ScheduleTime = bal.getVoltageRange(sender);
            ScheduleTime = ScheduleTime.Replace("MinRange", "Lower");
            ScheduleTime = ScheduleTime.Replace("MaxRange", "Upper");
            return ScheduleTime;
        }

        [WebMethod]
        public string getSchedule(string sender)
        {
            BAL bal = new BAL();
            string ScheduleTime = bal.getScheduleTime(sender);
            ScheduleTime = ScheduleTime.Replace(":00", ":30");
            ScheduleTime = ScheduleTime.Replace("00:30", "00:00");
            return ScheduleTime;
        }

        [WebMethod]
        public string execQuery(string query)
        {
            BAL bal = new BAL();
            string result = bal.execQuery(query);
            return result;
        }

        [WebMethod]
        public double workingHoursToday(string sender)
        {

            DataTable dtRes = new DataTable();
            DataTable Dashdt = new DataTable();
            var tubewellDataList = new List<TubewellDataClass>();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string getResFromTemp = "select DISTINCT r.ResourceID, r.ResourceLocation from tblResource r inner join tblTemplate rt on r.TemplateID = rt.TemplateID where rt.TemplateName = 'Tubewells'";
                    SqlDataAdapter sdaRes = new SqlDataAdapter(getResFromTemp, conn);
                    dtRes.Clear();
                    sdaRes.Fill(dtRes);
                    string getResId = "select ResourceID from tblResource where MobileNumber = '" + sender + "'";
                    var cmdResId = new SqlCommand(getResId, conn);
                    int resourceID = Convert.ToInt32(cmdResId.ExecuteScalar());
                    string resourceLocation = "";
                    int ite = 0;
                    //iterate through the list of resources within the desired set of resources chosen
                    //getting resourceID 
                    //getting resourceLocation 
                    //query will get the list of data available against given resourceID (latest first)
                    string Dashdtquery = ";WITH cte AS ( ";
                    Dashdtquery += "SELECT* FROM ";
                    Dashdtquery += "( ";
                    Dashdtquery += "SELECT DISTINCT r.resourceName AS Location, ";
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
                    Dashdtquery += "[PumpStatus],[WaterFlow(Cusec).] ";
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
                        TubewellDataClass sd = getWorkingHours(Dashdt);
                        tubewellDataList.Add(sd);
                    }
                    else
                    {
                        TubewellDataClass sd = new TubewellDataClass();
                        sd.locationName = "";
                        sd.pumpStatus = new List<double>();
                        tubewellDataList.Add(sd);
                    }
                }
                catch (Exception ex)
                {

                }
                conn.Close();
                double hhours = 0;
                if (tubewellDataList.Count > 0)
                {
                    hhours = Convert.ToDouble(tubewellDataList.FirstOrDefault().WorkingInHours);
                }
                else
                {

                }
                return hhours;
            }
        }


        public TubewellDataClass getWorkingHours(DataTable dt)
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

        //[WebMethod(CacheDuration = 120)]
        [WebMethod]
        public string GetData()
        {
            int c_id = 4;
            List<object> lists = new List<object>();
            //int c_id = Convert.ToInt32(Session["CompanyID"]);
            string tempName = "";
            string query = "select TemplateID, TemplateName from tblTemplate where CompanyID = " + c_id + "";
            query += " and TemplateID in (select TemplateID from tblResource) ";
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd2 = new SqlCommand(query))
                {
                    cmd2.Connection = con1;
                    con1.Open();
                    using (SqlDataReader sdr = cmd2.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["TemplateName"].ToString(),
                                Value = sdr["TemplateID"].ToString()
                            });
                        }
                    }
                    con1.Close();
                }
            }
            string parameterValuesString = "";
            string datetimed = "";
            string markers = "[";
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            string q = "select r.ResourceLocation, r.ResourceID, r.CooridatesGoogle, t.TemplateName from tblResource r left join tblTemplate t on r.TemplateID = t.TemplateID  where  t.TemplateID = 64 and t.CompanyID = " + c_id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(q, conn);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DataTable dt = new DataTable();
                            //string q1 = "select distinct r.ResourceLocation,  t.TemplateName, p.ParameterName, p.paramOrder, e.ParameterValue, e.InsertionDateTime, rms.CurrentMotorOnOffStatus from tblEnergy e ";
                            //q1 += "left join tblParameter p on e.ParameterID = p.ParameterID ";
                            //q1 += "left join tblResource r on e.ResourceID = r.ResourceID ";
                            //q1 += "left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID ";
                            //q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
                            //q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = " + sdr["ResourceID"] + ")";
                            //q1 += "and r.ResourceID = " + sdr["ResourceID"] + "";
                            //q1 += " and t.TemplateID = 64 order by p.paramOrder";

                            string q1 = "";
                            q1 += "select top(1) r.ResourceLocation, 'Tubewell' as TemplateName, ";
                            q1 += "CASE t.PumpStatus When 0 then 'OFF' else 'ON' END as [Pump Status] ,  ";
                            q1 += "CASE t.[Manual] When 0 then 'OFF' else 'ON' END as [Manual Mode],  ";
                            q1 += "CASE t.[Remote] When 0 then 'OFF' else 'ON' END as [Remote Mode],  ";
                            q1 += "CASE t.TimeSchedule When 0 then 'OFF' else 'ON' END as [Scheduling Mode],  ";
                            q1 += "round(t.V12, 2) as V12, round(t.V13, 2) as V13, round(t.V23, 2) as V23, round(t.V1N, 2) as V1N, round(t.V2N, 2) as V2N, round(t.V3N, 2) as V3N, ";
                            q1 += "round(t.I1, 2) as I1, round(t.I2, 2) as I2, round(t.I3, 2) as I3, round(t.PF, 2) as [Power Factor], round(t.Frequency, 2) as Frequency, ";
                            q1 += "round(t.PKVA, 2) as PKVA, round(t.PKW, 2) as PKW, round(t.PKVAR, 2) as PKVAR, ";
                            q1 += "CASE t.VolatgeTrip When 0 then 'No Error' else 'Error' END as [Voltage Trip],  ";
                            q1 += "CASE t.CurrentTrip When 0 then 'No Error' else 'Error' END as [Current Trip],  ";
                            q1 += "cast(round(t.WaterFlow/101.94,2) as numeric(36,2)) as [Water Flow (cfs)], round(t.PressureBar,2) as [Pressure(Bar)], ";
                            q1 += "CASE t.PrimpingLevel When 0 then 'LOW' else 'FULL' END as [Priming Level],  ";
                            q1 += "CASE t.ChlorineLevel When 0 then 'HALF' else 'FULL' END as [Chlorine Level],  ";
                            q1 += "CASE t.IndoorLight When 0 then 'OFF' else 'ON' END as [Indoor Lights],  ";
                            q1 += "CASE t.OutdoorLight When 0 then 'OFF' else 'ON' END as [Outdoor Lights],  ";
                            q1 += "CASE t.ExhustFan When 0 then 'OFF' else 'ON' END as [Exhaust Fan],  ";
                            q1 += "round(t.vib_x,2) as [Vibration Velocity in (mm/s)],round(t.vib_y,2)  as [Vibration Acceleration in (m/s2)], round(t.vib_z,2) as [Vibration Displacement in (um)], ";
                            q1 += "t.InsertionDateTime ";
                            q1 += "from tblTubewellsTest t ";
                            q1 += "inner join tblResource r on t.TubeWellId = r.ResourceID ";
                            q1 += "where t.TubeWellId = " + sdr["ResourceID"] + " ";
                            q1 += "order by t.ID DESC ";

                            SqlCommand cmd1 = new SqlCommand(q1, conn);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                            dt.Clear();
                            sda.Fill(dt);
                            int optionStatus = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                int number = 0;
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (number == 0)
                                    {
                                        tempName = dr[dc].ToString();
                                    }
                                    else if (number == 1)
                                    {

                                    }
                                    else if (number == 2)
                                    {
                                        parameterValuesString += dc.ColumnName + " : " + dr[dc].ToString() + "<br />";
                                        if (dr[dc].ToString() == "OFF")
                                        {
                                            optionStatus = 0;
                                        }
                                        else
                                        {
                                            optionStatus = 1;
                                        }
                                    }
                                    else if (number > 31)
                                    {
                                        datetimed = dr[dc].ToString();
                                    }
                                    else
                                    {
                                        parameterValuesString += dc.ColumnName + " : " + dr[dc].ToString() + "<br />";
                                    }
                                    number++;
                                }
                            }
                            string theStatus = "False";
                            if (optionStatus > 0)
                            {
                                theStatus = "True";
                            }
                            tempName = sdr["TemplateName"].ToString().Substring(0, 1);
                            string templName = sdr["TemplateName"].ToString();
                            if (templName == "Tubewells")
                            {
                                templName = "Tubewell";
                            }
                            string newstring = "<b>" + templName + "</b>";
                            newstring += "<br />";
                            newstring += "<b>" + sdr["ResourceLocation"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += datetimed;
                            newstring += "<br />";
                            newstring += parameterValuesString;
                            TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                            double minu = duration.TotalMinutes;
                            parameterValuesString = "";
                            markers += "{";
                            markers += string.Format("'Status': '{0}',", theStatus);
                            markers += string.Format("'Template': '{0}',", tempName);
                            markers += string.Format("'title': '{0}',", sdr["ResourceLocation"]);
                            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
                            markers += string.Format("'lat':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[0]);
                            markers += string.Format("'lnt':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[1]);
                            //markers += string.Format("'parameter':'{0}',", sdr1["ParameterName"]);
                            //markers += string.Format("'value':'{0}'", sdr1["ParameterValue"]);
                            markers += string.Format("'DelTime': '{0}',", minu);
                            markers += string.Format("'description': '{0}'", newstring);
                            markers += "},";
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }

            string p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, ptime, ptime2, plevel1, plevel2 = "";
            int pp1 = 0;
            int pp2 = 0;
            int pp3 = 0;
            int pp4 = 0;
            int pp5 = 0;
            int pp6 = 0;
            int pp7 = 0;
            int pp8 = 0;
            int pp9 = 0;
            int pp10 = 0;
            double tlevel1 = 0;
            double tlevel2 = 0;
            p1 = "select top(1) ParameterValue from tblEnergy where ParameterID = 144 order by ID DESC";
            p2 = "select top(1) ParameterValue from tblEnergy where ParameterID = 145 order by ID DESC";
            p3 = "select top(1) ParameterValue from tblEnergy where ParameterID = 146 order by ID DESC";
            p4 = "select top(1) ParameterValue from tblEnergy where ParameterID = 147 order by ID DESC";
            p5 = "select top(1) ParameterValue from tblEnergy where ParameterID = 148 order by ID DESC";
            p6 = "select top(1) ParameterValue from tblEnergy where ParameterID = 149 order by ID DESC";
            p7 = "select top(1) ParameterValue from tblEnergy where ParameterID = 150 order by ID DESC";
            p8 = "select top(1) ParameterValue from tblEnergy where ParameterID = 151 order by ID DESC";
            p9 = "select top(1) ParameterValue from tblEnergy where ParameterID = 152 order by ID DESC";
            p10 = "select top(1) ParameterValue from tblEnergy where ParameterID = 153 order by ID DESC";
            ptime = "select top(1) InsertionDateTime from tblEnergy where ParameterID = 153 order by ID DESC";
            ptime2 = "select top(1) InsertionDateTime from tblEnergy where ParameterID = 144 order by ID DESC";
            plevel1 = "select top(1) ParameterValue from tblEnergy where ParameterID = 175 order by ID DESC";
            plevel2 = "select top(1) ParameterValue from tblEnergy where ParameterID = 176 order by ID DESC";

            //////////////////////////////////////////////////////////////////////////////////////////////////
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(p1, conn);
                    SqlCommand cmd2 = new SqlCommand(p2, conn);
                    SqlCommand cmd3 = new SqlCommand(p3, conn);
                    SqlCommand cmd4 = new SqlCommand(p4, conn);
                    SqlCommand cmd5 = new SqlCommand(p5, conn);
                    SqlCommand cmd6 = new SqlCommand(p6, conn);
                    SqlCommand cmd7 = new SqlCommand(p7, conn);
                    SqlCommand cmd8 = new SqlCommand(p8, conn);
                    SqlCommand cmd9 = new SqlCommand(p9, conn);
                    SqlCommand cmd10 = new SqlCommand(p10, conn);
                    SqlCommand cmdtime = new SqlCommand(ptime, conn);
                    SqlCommand cmdtime2 = new SqlCommand(ptime2, conn);
                    SqlCommand cmdlevel1 = new SqlCommand(plevel1, conn);
                    SqlCommand cmdlevel2 = new SqlCommand(plevel2, conn);
                    pp1 = Convert.ToInt32(cmd1.ExecuteScalar());
                    pp2 = Convert.ToInt32(cmd2.ExecuteScalar());
                    pp3 = Convert.ToInt32(cmd3.ExecuteScalar());
                    pp4 = Convert.ToInt32(cmd4.ExecuteScalar());
                    pp5 = Convert.ToInt32(cmd5.ExecuteScalar());
                    pp6 = Convert.ToInt32(cmd6.ExecuteScalar());
                    pp7 = Convert.ToInt32(cmd7.ExecuteScalar());
                    pp8 = Convert.ToInt32(cmd8.ExecuteScalar());
                    pp9 = Convert.ToInt32(cmd9.ExecuteScalar());
                    pp10 = Convert.ToInt32(cmd10.ExecuteScalar());
                    ptime = cmdtime.ExecuteScalar().ToString();
                    ptime2 = cmdtime2.ExecuteScalar().ToString();
                    tlevel1 = Convert.ToDouble(cmdlevel1.ExecuteScalar());
                    tlevel2 = Convert.ToDouble(cmdlevel2.ExecuteScalar());
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                if (tlevel1 == 0)
                {
                    plevel1 = "Empty";
                }
                else
                {
                    plevel1 = Math.Round(tlevel1, 1).ToString() + " ft";
                }
                if (tlevel2 == 0)
                {
                    plevel2 = "Empty";
                }
                else
                {
                    plevel2 = Math.Round(tlevel2, 1).ToString() + " ft";
                }
                if (pp1 == 0)
                {
                    p1 = "OFF";
                }
                else
                {
                    p1 = "ON";
                }
                if (pp2 == 0)
                {
                    p2 = "OFF";
                }
                else
                {
                    p2 = "ON";
                }
                if (pp3 == 0)
                {
                    p3 = "OFF";
                }
                else
                {
                    p3 = "ON";
                }
                if (pp4 == 0)
                {
                    p4 = "OFF";
                }
                else
                {
                    p4 = "ON";
                }
                if (pp5 == 0)
                {
                    p5 = "OFF";
                }
                else
                {
                    p5 = "ON";
                }
                if (pp6 == 0)
                {
                    p6 = "OFF";
                }
                else
                {
                    p6 = "ON";
                }
                if (pp7 == 0)
                {
                    p7 = "OFF";
                }
                else
                {
                    p7 = "ON";
                }
                if (pp8 == 0)
                {
                    p8 = "OFF";
                }
                else
                {
                    p8 = "ON";
                }
                if (pp9 == 0)
                {
                    p9 = "OFF";
                }
                else
                {
                    p9 = "ON";
                }
                if (pp10 == 0)
                {
                    p10 = "OFF";
                }
                else
                {
                    p10 = "ON";
                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }
            string theStatusp = "False";
            if (pp1 + pp2 + pp3 + pp4 + pp5 + pp6 + pp7 + pp8 + pp9 + pp10 > 0)
            {
                theStatusp = "True";
            }

            parameterValuesString = "Pump No. 1 : " + p1;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 2 : " + p2;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 3 : " + p3;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 4 : " + p4;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 5 : " + p5;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 6 : " + p6;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 7 : " + p7;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 8 : " + p8;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 9 : " + p9;
            parameterValuesString += "<br />";
            parameterValuesString += "Pump No. 10 : " + p10;
            parameterValuesString += "<br />";
            parameterValuesString += "Wet Well Level No. 1 : " + plevel1;
            parameterValuesString += "<br />";
            parameterValuesString += "Wet Well Level No. 2 : " + plevel2;
            parameterValuesString += "<br />";

            if (Convert.ToDateTime(ptime2) > Convert.ToDateTime(ptime))
            {
                datetimed = ptime2;
            }
            else
            {
                datetimed = ptime;
            }

            tempName = "D";
            string newstringp = "<b>Disposal Station</b>";
            newstringp += "<br />";
            newstringp += "<b>Shaukat Khanum Disposal Station</b>";
            newstringp += "<br />";
            newstringp += datetimed;
            newstringp += "<br />";
            newstringp += parameterValuesString;
            TimeSpan durationp = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
            double minup = durationp.TotalMinutes;
            parameterValuesString = "";
            markers += "{";
            markers += string.Format("'Status': '{0}',", theStatusp);
            markers += string.Format("'Template': '{0}',", tempName);
            markers += string.Format("'title': '{0}',", "Shaukat Khanum Disposal Station");
            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
            markers += string.Format("'lat':'{0}',", "31.430021");
            markers += string.Format("'lnt':'{0}',", "74.252829");
            markers += string.Format("'DelTime': '{0}',", minup);
            markers += string.Format("'description': '{0}'", newstringp);
            markers += "},";

            //////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            string qres = "select r.ResourceLocation, r.ResourceID, r.CooridatesGoogle, t.TemplateName from tblResource r left join tblTemplate t on r.TemplateID = t.TemplateID  where  t.TemplateID = 67 and t.CompanyID = " + c_id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(qres, conn);
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DataTable dt = new DataTable();
                            string q1 = "select distinct r.ResourceLocation,  t.TemplateName, p.ParameterName, p.paramOrder, e.ParameterValue, e.InsertionDateTime,rms.CurrentMotorOnOffStatus from tblEnergy e ";
                            q1 += "left join tblParameter p on e.ParameterID = p.ParameterID ";
                            q1 += "left join tblResource r on e.ResourceID = r.ResourceID ";
                            q1 += "left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID ";
                            q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
                            q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = " + sdr["ResourceID"] + ")";
                            q1 += "and r.ResourceID = " + sdr["ResourceID"] + "";
                            q1 += " and t.TemplateID = 67 order by p.paramOrder";
                            SqlCommand cmd1 = new SqlCommand(q1, conn);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                            dt.Clear();
                            sda.Fill(dt);
                            int optionStatus = 0;
                            int manual = 0;
                            int remote = 0;
                            int scheduling = 0;
                            int pumpStatus = 0;
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                while (sdr1.Read())
                                {
                                    string valuee = "";
                                    parameterValuesString += "";
                                    if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus1")
                                    {
                                        parameterValuesString += "Submersible Pump : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus2")
                                    {
                                        parameterValuesString += "Filtered Water Pump : ";
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus3")
                                    {
                                        //parameterValuesString += "Fresh Water Pump : ";
                                    }
                                    else
                                    {
                                        parameterValuesString += sdr1["ParameterName"].ToString() + ": ";
                                    }
                                    //parameterValuesString += ssdr1["ParameterName"].ToString() + ": ";
                                    if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus1")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus2")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "RecyclePumpStatus3")
                                    {
                                        //if (sdr1["ParameterValue"].ToString() == "0")
                                        //{
                                        //    valuee = "OFF";
                                        //}
                                        //else
                                        //{
                                        //    valuee = "ON";
                                        //    optionStatus += 1;
                                        //}
                                    }
                                    else
                                    {
                                        valuee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    parameterValuesString += valuee;
                                    parameterValuesString += "<br />";
                                    datetimed = sdr1["InsertionDateTime"].ToString();
                                    //optionStatus = sdr1["CurrentMotorOnOffStatus"].ToString();
                                }
                            }
                            string theStatus = "False";
                            if (optionStatus > 0)
                            {
                                theStatus = "True";
                            }
                            tempName = sdr["TemplateName"].ToString().Substring(0, 1);
                            string templName = sdr["TemplateName"].ToString();
                            if (templName == "Recycling Plants")
                            {
                                templName = "Recycling Plant";
                            }
                            string newstring = "<b>" + templName + "</b>";
                            newstring += "<br />";
                            newstring += "<b>" + sdr["ResourceLocation"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += datetimed;
                            newstring += "<br />";
                            newstring += parameterValuesString;
                            TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                            double minu = duration.TotalMinutes;
                            parameterValuesString = "";
                            markers += "{";
                            markers += string.Format("'Status': '{0}',", theStatus);
                            markers += string.Format("'Template': '{0}',", tempName);
                            markers += string.Format("'title': '{0}',", sdr["ResourceLocation"]);
                            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
                            markers += string.Format("'lat':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[0]);
                            markers += string.Format("'lnt':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[1]);
                            //markers += string.Format("'parameter':'{0}',", sdr1["ParameterName"]);
                            //markers += string.Format("'value':'{0}'", sdr1["ParameterValue"]);
                            markers += string.Format("'DelTime': '{0}',", minu);
                            markers += string.Format("'description': '{0}'", newstring);
                            markers += "},";
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            markers += "]";
            return markers;
        }

        [WebMethod]
        public int getTimeScheduleMode(string sender)
        {
            int mode = -1;
            string getTimeScheduleMode = "select Mode from tblSetMode where ResourceID = (select ResourceID from tblResource where MobileNumber = '" + sender + "')";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    var cmdResId = new SqlCommand(getTimeScheduleMode, conn);
                    mode = Convert.ToInt32(cmdResId.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
            }
            return mode;
        }




        [WebMethod]
        public double getVibM(string sender)
        {
            double val = -1;
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string query_m = "select top(1) e.ParameterValue from tblEnergy e ";
                    query_m += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                    query_m += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
                    query_m += "where p.ParameterName = 'vib_M' ";
                    query_m += "and e.ParameterValue IS NOT NULL ";
                    query_m += "and r.ResourceID = (select ResourceID from tblResource where MobileNumber = '" + sender + "') ";
                    query_m += "order by ID DESC ";
                    SqlCommand cmd_m = new SqlCommand(query_m, conn1);
                    val = Convert.ToDouble(cmd_m.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
            }
            return val;
        }

        [WebMethod]
        public double getVibMS(string sender)
        {
            double val = -1;
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string query_m = "select top(1) e.ParameterValue from tblEnergy e ";
                    query_m += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                    query_m += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
                    query_m += "where p.ParameterName = 'vib_Ms' ";
                    query_m += "and e.ParameterValue IS NOT NULL ";
                    query_m += "and r.ResourceID = (select ResourceID from tblResource where MobileNumber = '" + sender + "') ";
                    query_m += "order by ID DESC ";
                    SqlCommand cmd_m = new SqlCommand(query_m, conn1);
                    val = Convert.ToDouble(cmd_m.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
            }
            return val;
        }

        [WebMethod]
        public double getVibMS2(string sender)
        {
            double val = -1;
            using (SqlConnection conn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    string query_m = "select top(1) e.ParameterValue from tblEnergy e ";
                    query_m += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                    query_m += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
                    query_m += "where p.ParameterName = 'vib_Ms2' ";
                    query_m += "and e.ParameterValue IS NOT NULL ";
                    query_m += "and r.ResourceID = (select ResourceID from tblResource where MobileNumber = '" + sender + "') ";
                    query_m += "order by ID DESC ";
                    SqlCommand cmd_m = new SqlCommand(query_m, conn1);
                    val = Convert.ToDouble(cmd_m.ExecuteScalar());
                }
                catch (Exception ex)
                {

                }
            }
            return val;
        }
        
        [WebMethod]
        public string GetDataDisposals()
        {
            int c_id = 4;
            List<object> lists = new List<object>();
            //int c_id = Convert.ToInt32(Session["CompanyID"]);
            string tempName = "";
            string query = "select TemplateID, TemplateName from tblTemplate where CompanyID = " + c_id + "";
            query += " and TemplateID in (select TemplateID from tblResource) ";
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd2 = new SqlCommand(query))
                {
                    cmd2.Connection = con1;
                    con1.Open();
                    using (SqlDataReader sdr = cmd2.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["TemplateName"].ToString(),
                                Value = sdr["TemplateID"].ToString()
                            });
                        }
                    }
                    con1.Close();
                }
            }
            string PG = "";
            string P1 = "";
            string P2 = "";
            string P3 = "";
            string P4 = "";
            string P5 = "";
            string P6 = "";
            string P7 = "";
            string P8 = "";
            string P9 = "";
            string P10 = "";
            string P1value = "";
            string P2value = "";
            string P3value = "";
            string P4value = "";
            string P5value = "";
            string P6value = "";
            string P7value = "";
            string P8value = "";
            string P9value = "";
            string P10value = "";
            string PL = "";
            string PF = "";
            string VM = "";
            string CL = "";
            string LS = "";
            string CT = "";
            string VT = "";
            string Auto = "";
            string Time = "";
            string Remote = "";
            string PGvaluee = "";
            string PFvaluee = "";
            string PLvaluee = "";
            string VMvaluee = "";
            string LSvaluee = "";
            string CLvaluee = "";

            string CTvaluee = "";
            string VTvaluee = "";
            string parameterValuesString = "";
            string datetimed = "";
            string markers = "[";
            WASA_EMS_Entities db = new WASA_EMS_Entities();
            string q = "select r.ResourceLocation, r.ResourceID, r.CooridatesGoogle, t.TemplateName from tblResource r left join tblTemplate t on r.TemplateID = t.TemplateID  where t.CompanyID = " + c_id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(q, conn);
                    using (SqlDataReader sdr = cmd3.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string q1 = "select distinct r.ResourceLocation,  t.TemplateName, p.ParameterName, e.ParameterValue, e.InsertionDateTime, rms.CurrentMotorOnOffStatus from tblEnergy e ";
                            q1 += "left join tblParameter p on e.ParameterID = p.ParameterID ";
                            q1 += "left join tblResource r on e.ResourceID = r.ResourceID ";
                            q1 += "left join tblRemoteSensor rms on r.ResourceID = rms.ResourceID ";
                            q1 += "left join tblTemplate t on r.TemplateID = t.TemplateID ";
                            q1 += "where e.InsertionDateTime = (select max(InsertionDateTime) from tblEnergy where ResourceID = " + sdr["ResourceID"] + ")";
                            q1 += "and r.ResourceID = " + sdr["ResourceID"] + "";
                            SqlCommand cmd1 = new SqlCommand(q1, conn);
                            int optionStatus = 0;
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                while (sdr1.Read())
                                {
                                    string valuee = "";






                                    parameterValuesString += "";
                                    parameterValuesString += sdr1["ParameterName"].ToString() + ": ";

                                    if (sdr1["ParameterName"].ToString() == "Manual")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            Auto = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Auto = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus1.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P1 = "OFF";
                                        }
                                        else
                                        {
                                            P1 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus2.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P2 = "OFF";
                                        }
                                        else
                                        {
                                            P2 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus3.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P3 = "OFF";
                                        }
                                        else
                                        {
                                            P3 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus4.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P4 = "OFF";
                                        }
                                        else
                                        {
                                            P4 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus5.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P5 = "OFF";
                                        }
                                        else
                                        {
                                            P5 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus6.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P6 = "OFF";
                                        }
                                        else
                                        {
                                            P6 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus7.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P7 = "OFF";
                                        }
                                        else
                                        {
                                            P7 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus8.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P8 = "OFF";
                                        }
                                        else
                                        {
                                            P8 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatus9.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P9 = "OFF";
                                        }
                                        else
                                        {
                                            P9 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PumpStatuss10.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            P10 = "OFF";
                                        }
                                        else
                                        {
                                            P10 = "ON";
                                            optionStatus += 1;
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "CurrentTrip.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "No Error";
                                        }
                                        else
                                        {
                                            valuee = "Error";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "VoltageTrip.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "No Error";
                                        }
                                        else
                                        {
                                            valuee = "Error";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Remote.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            Remote = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Remote = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "TimeSchedule.")
                                    {
                                        if (sdr1["ParameterValue"].ToString() == "0")
                                        {
                                            valuee = "OFF";
                                            Time = "OFF";
                                        }
                                        else
                                        {
                                            valuee = "ON";
                                            Time = "ON";
                                        }
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "Pressure(Bar)")
                                    {
                                        PGvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PF.")
                                    {
                                        PFvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "PrimingLevel")
                                    {
                                        PLvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "vib_y")
                                    {
                                        VMvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "ChlorineLevel.")
                                    {
                                        CLvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "OutdoorLight")
                                    {
                                        LSvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "VoltageTrip.")
                                    {
                                        VTvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else if (sdr1["ParameterName"].ToString() == "CurrentTrip.")
                                    {
                                        CTvaluee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }
                                    else
                                    {
                                        valuee = Math.Round(Convert.ToDouble(sdr1["ParameterValue"]), 2).ToString();
                                    }


                                    PF += PFvaluee;
                                    PL += PLvaluee;
                                    VM += VMvaluee;
                                    CL += CLvaluee;
                                    LS += LSvaluee;
                                    VT += VTvaluee;
                                    CT += CTvaluee;
                                    parameterValuesString += valuee;
                                    parameterValuesString += "<br />";
                                    datetimed = sdr1["InsertionDateTime"].ToString();
                                    //optionStatus = sdr1["CurrentMotorOnOffStatus"].ToString();
                                }
                            }
                            string theStatus = "False";
                            if (optionStatus > 0)
                            {
                                theStatus = "True";
                            }


                            tempName = sdr["TemplateName"].ToString().Substring(0, 1);

                            string newstring = "<b>" + sdr["TemplateName"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += "<b>" + sdr["ResourceLocation"].ToString() + "</b>";
                            newstring += "<br />";
                            newstring += datetimed;
                            newstring += "<br />";
                            newstring += parameterValuesString;

                            TimeSpan duration = (Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Pakistan Standard Time").ToString()) - Convert.ToDateTime(datetimed.ToString()));
                            double minu = duration.TotalMinutes;
                            parameterValuesString = "";

                            markers += "{";


                            markers += string.Format("'P': '{0}',", PGvaluee);
                            markers += string.Format("'P1': '{0}',", P1);
                            markers += string.Format("'P2': '{0}',", P2);
                            markers += string.Format("'P3': '{0}',", P3);
                            markers += string.Format("'P4': '{0}',", P4);
                            markers += string.Format("'P5': '{0}',", P5);
                            markers += string.Format("'P6': '{0}',", P6);
                            markers += string.Format("'P7': '{0}',", P7);
                            markers += string.Format("'P8': '{0}',", P8);
                            markers += string.Format("'P9': '{0}',", P9);
                            markers += string.Format("'P10': '{0}',", P10);
                            markers += string.Format("'Status': '{0}',", theStatus);
                            markers += string.Format("'Template': '{0}',", tempName);
                            markers += string.Format("'title': '{0}',", sdr["ResourceLocation"]);
                            markers += string.Format("'Dtae': '{0}',", datetimed);
                            //markers += string.Format("'time': '{0}',", sdr1["InsertionDateTime"]);
                            markers += string.Format("'lat':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[0]);
                            markers += string.Format("'lnt':'{0}',", sdr["CooridatesGoogle"].ToString().Split(',')[1]);
                            //markers += string.Format("'parameter':'{0}',", sdr1["ParameterName"]);
                            //markers += string.Format("'value':'{0}'", sdr1["ParameterValue"]);
                            markers += string.Format("'DelTime': '{0}',", minu);
                            markers += string.Format("'description': '{0}'", newstring);
                            markers += "},";
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
                //return new SelectList(theResourceTypes, "Value", "Text", "id");
            }

            markers += "]";
            return markers;
        }
        
        

        [WebMethod]
        public string WhatsAppDataDisposal()
        {
            BAL bal = new BAL();
            string disposalData = bal.DisposalLastDayReport();
            return disposalData;
        }

        [WebMethod]
        public string WhatsAppDisposalToday()
        {
            BAL bal = new BAL();
            string disposalData = bal.DisposalCurrentDayReport();
            return disposalData;
        }

        [WebMethod]
        public string DisposalData()
        {
            BAL bal = new BAL();
            string disposalData = bal.DisposalCurrentDayReport();
            return disposalData;
        }


        [WebMethod]

        public string WhatsAppDataTubewell()
        {
            BAL bal = new BAL();
            string tubewellData = bal.TubewellLastDayReport();
            return tubewellData;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, XmlSerializeString = false, ResponseFormat = ResponseFormat.Json)]
        public string TubewellsData()
        {
            BAL bal = new BAL();
            string tubewellData = bal.TubewellCurrentDayReport();
            return tubewellData;
        }


        [WebMethod]
        public string WhatsAppDataRecyclingPlant()
        {
            BAL bal = new BAL();
            string recyclingData = bal.RecyclingPlantLastDayReport();
            return recyclingData;
        }

        [WebMethod]
        public string RecyclingPlantData()
        {
            BAL bal = new BAL();
            string recyclingData = bal.RecyclingPlantCurrentDayReport();
            return recyclingData;
        }

        [WebMethod]
        public string PondingDashboardData()
        {
            BAL bal = new BAL();
            string pondingData = bal.getPondingTableListUpdated2();
            return pondingData;
        }

        [WebMethod]
        public string RainDashboardData()
        {
            BAL bal = new BAL();
            string rainData = bal.getRainTableList();
            return rainData;
        }

    }
}



