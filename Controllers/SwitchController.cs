﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class SwitchController : Controller
    {
        private WASA_EMS_Entities db = new WASA_EMS_Entities();

        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }

        public ActionResult SetMode()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("LogOn", "Account");
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult _ModeRecords()
        {
            var dt = new DataTable();
            var cId = Convert.ToInt32(Session["CompanyID"]);
            //var query = "select s.ID ID, r.ResourceLocation Resource, case when s.ModeManualAuto = 0 then 'Manual' ELSE 'AUTO' END as Mode, case when s.CurrentMotorOnOffStatus = 0 then   'OFF' ELSE 'ON' END as Status, case when s.CurrentMotorOnOffStatus = 0 then   'TURN ON' ELSE 'TURN OFF' END as Action from tblRemoteSensor s left join tblResource r on s.ResourceID = r.ResourceID where r.CompanyID = " + cId + "";
            var query = "";
            query += "select r.ResourceID, r.ResourceLocation, sm.Mode from tblSetMode sm ";
            query += " inner join tblResource r on sm.ResourceID = r.ResourceID ";
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    var da = new SqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    da.Fill(ds);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return PartialView(dt);
        }

        [HttpGet]
        public PartialViewResult _SwitchRecords()
        {
            var dt = new DataTable();
            var cId = Convert.ToInt32(Session["CompanyID"]);
            //var query = "select s.ID ID, r.ResourceLocation Resource, case when s.ModeManualAuto = 0 then 'Manual' ELSE 'AUTO' END as Mode, case when s.CurrentMotorOnOffStatus = 0 then   'OFF' ELSE 'ON' END as Status, case when s.CurrentMotorOnOffStatus = 0 then   'TURN ON' ELSE 'TURN OFF' END as Action from tblRemoteSensor s left join tblResource r on s.ResourceID = r.ResourceID where r.CompanyID = " + cId + "";
            var query = "";
            //query += "select r.ResourceID, r.ResourceLocation,p.ParameterID,p.ParameterName, cte.CompanyID, cte.ParameterValue, cte.InsertionDateTime, 1 as RowNum from ( select DISTINCT top(14) * from tblEnergy where ResourceID in (1068, 1069, 1070, 1071, 1072, 1073, 1074) and ParameterID in (124, 125) order by ID DESC) as cte inner join tblResource r on cte.ResourceID = r.ResourceID inner join tblParameter p on cte.ParameterID = p.ParameterID order by cte.ResourceID, cte.ParameterID select r.ResourceID, r.ResourceLocation,p.ParameterID,p.ParameterName, cte.CompanyID, cte.ParameterValue, cte.InsertionDateTime, 1 as RowNum from ( select DISTINCT top(14) * from tblEnergy where ResourceID in (1068, 1069, 1070, 1071, 1072, 1073, 1074) and ParameterID in (124, 125) order by ID DESC) as cte inner join tblResource r on cte.ResourceID = r.ResourceID inner join tblParameter p on cte.ParameterID = p.ParameterID order by cte.ResourceID, cte.ParameterID";
            //query += ";with cterownumber as ( ";
            //query += "select r.resourceid, r.resourcelocation,p.parameterid,p.parametername, e.companyid, e.parametervalue,   ";
            //query += "row_number() over(partition by p.parameterid, r.resourceid, r.resourcelocation order by e.id desc) as rownum ";
            //query += "from tblenergy e ";
            //query += "inner join tblresource r on e.resourceid = r.resourceid ";
            //query += "inner join tblparameter p on e.parameterid = p.parameterid ";
            //query += ")   ";
            //query += "select* ";
            //query += "from cterownumber ";
            //query += "where rownum = 1  and companyid = 4 ";
            //query += "and parameterid in (124, 125) ";
            //query += "order by resourceid, parameterid";
            query += "select ResourceID from tblResource where TemplateID = 64 order by ResourceID";
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    var dtResourcesWithID = new DataTable();
                    var da = new SqlDataAdapter(query, conn);
                    da.Fill(dtResourcesWithID);
                    dt.Columns.Add("ResourceID");
                    dt.Columns.Add("ResourceLocation");
                    dt.Columns.Add("ParameterID");
                    dt.Columns.Add("ParameterName");
                    dt.Columns.Add("CompanyID");
                    dt.Columns.Add("ParameterValue");
                    dt.Columns.Add("InsertionDateTime");
                    dt.Columns.Add("RowNum");
                    foreach (DataRow dr in dtResourcesWithID.Rows)
                    {
                        var queryRemote = "";
                        queryRemote += "select top(1) e.ResourceID, r.ResourceLocation, e.ParameterID, p.ParameterName, e.CompanyID,  e.ParameterValue, e.InsertionDateTime, 1 as RowNum ";
                        queryRemote += "from tblEnergy e ";
                        queryRemote += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                        queryRemote += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
                        queryRemote += "where e.ResourceID = " + dr["ResourceID"] + " and p.ParameterID = 124 order by ID DESC ";

                        var queryPump = "";
                        queryPump += "select top(1) e.ResourceID, r.ResourceLocation, e.ParameterID, p.ParameterName, e.CompanyID,  e.ParameterValue, e.InsertionDateTime, 1 as RowNum ";
                        queryPump += "from tblEnergy e ";
                        queryPump += "inner join tblResource r on e.ResourceID = r.ResourceID ";
                        queryPump += "inner join tblParameter p on e.ParameterID = p.ParameterID ";
                        queryPump += "where e.ResourceID = " + dr["ResourceID"] + " and p.ParameterID = 125 order by ID DESC";

                        var sdaRemote = new SqlDataAdapter(queryRemote, conn);
                        var dtRemote = new DataTable();
                        sdaRemote.Fill(dtRemote);
                        DataRow drRemote = dtRemote.Rows[0];

                        dt.ImportRow(drRemote);


                        var sdaPump = new SqlDataAdapter(queryPump, conn);
                        var dtPump = new DataTable();
                        sdaPump.Fill(dtPump);
                        DataRow drPump = dtPump.Rows[0];

                        dt.ImportRow(drPump);

                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return PartialView(dt);
        }

        [HttpGet]
        public PartialViewResult _SwitchRecords2()
        {
            var dt = new DataTable();
            var cId = Convert.ToInt32(Session["CompanyID"]);
            //var query = "select s.ID ID, r.ResourceLocation Resource, case when s.ModeManualAuto = 0 then 'Manual' ELSE 'AUTO' END as Mode, case when s.CurrentMotorOnOffStatus = 0 then   'OFF' ELSE 'ON' END as Status, case when s.CurrentMotorOnOffStatus = 0 then   'TURN ON' ELSE 'TURN OFF' END as Action from tblRemoteSensor s left join tblResource r on s.ResourceID = r.ResourceID where r.CompanyID = " + cId + "";
            var query = "";
            //query += "select r.ResourceID, r.ResourceLocation,p.ParameterID,p.ParameterName, cte.CompanyID, cte.ParameterValue, cte.InsertionDateTime, 1 as RowNum from ( select DISTINCT top(14) * from tblEnergy where ResourceID in (1068, 1069, 1070, 1071, 1072, 1073, 1074) and ParameterID in (124, 125) order by ID DESC) as cte inner join tblResource r on cte.ResourceID = r.ResourceID inner join tblParameter p on cte.ParameterID = p.ParameterID order by cte.ResourceID, cte.ParameterID select r.ResourceID, r.ResourceLocation,p.ParameterID,p.ParameterName, cte.CompanyID, cte.ParameterValue, cte.InsertionDateTime, 1 as RowNum from ( select DISTINCT top(14) * from tblEnergy where ResourceID in (1068, 1069, 1070, 1071, 1072, 1073, 1074) and ParameterID in (124, 125) order by ID DESC) as cte inner join tblResource r on cte.ResourceID = r.ResourceID inner join tblParameter p on cte.ParameterID = p.ParameterID order by cte.ResourceID, cte.ParameterID";
            query += ";with cterownumber as ( ";
            query += "select r.resourceid, r.resourcelocation,p.parameterid,p.parametername, e.companyid, e.parametervalue,   ";
            query += "row_number() over(partition by p.parameterid, r.resourceid, r.resourcelocation order by e.id desc) as rownum ";
            query += "from tblenergy e ";
            query += "inner join tblresource r on e.resourceid = r.resourceid ";
            query += "inner join tblparameter p on e.parameterid = p.parameterid ";
            query += ")   ";
            query += "select* ";
            query += "from cterownumber ";
            query += "where rownum = 1  and companyid = 4 ";
            query += "and parameterid in (124, 125) ";
            query += "order by resourceid, parameterid";
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    var da = new SqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    da.Fill(ds);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return PartialView(dt);
        }

        public ActionResult EditMode(int? id, string mode)
        {
            int mm = 0;
            if (mode.ToLower() == "true")
            {
                mode = "False";
            }
            else
            {
                mode = "True";
            }
            string query2 = "update tblSetMode set Mode = '" + mode + "' where ResourceID = " + Convert.ToInt32(id) + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query2, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
            if (mode == "True")
            {
                string query22 = "update tblRemoteSensor set ModeManualAuto = 1,CurrentMotorOnOffStatus = 0 where ResourceID = "+id+" "; ;
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query22, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return RedirectToAction("SetMode");
        }

        public ActionResult Edit(int? id)
        {
            int? Mode = null;
            int? Status = null;
            string queryr = "select ID from tblRemoteSensor where ResourceID = " + id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(queryr, conn);
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                }
            }
            string query = "select ModeManualAuto from tblRemoteSensor where ID = " + id + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    Mode = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                }
            }
            if (id != null && Mode != 0)
            {
                string query1 = "select CurrentMotorOnOffStatus from tblRemoteSensor where ID = " + id + "";
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query1, conn);
                        Status = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (Status != null && Mode != 0)
                {
                    int? NewStatus = null;
                    if (Status == 0)
                    {
                        NewStatus = 1;
                    }
                    else
                    {
                        NewStatus = 0;
                    }
                    string query2 = "update tblRemoteSensor set PreviousMotorOnOffStatus = " + Status + ", CurrentMotorOnOffStatus = " + NewStatus + " where ID = " + Convert.ToInt32(id) + "";
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query2, conn);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult MasterOn()
        {
            string query2 = "update tblRemoteSensor ";
            query2 += " set CurrentMotorOnOffStatus = 1 ";
            query2 += " from tblRemoteSensor rs ";
            query2 += " inner join tblResource r ";
            query2 += " on rs.ResourceID = r.ResourceID ";
            query2 += "  where r.CompanyID = " + Convert.ToInt32(Session["CompanyID"]) + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query2, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult MasterOff()
        {
            string query2 = "update tblRemoteSensor ";
            query2 += " set CurrentMotorOnOffStatus = 0 ";
            query2 += " from tblRemoteSensor rs ";
            query2 += " inner join tblResource r ";
            query2 += " on rs.ResourceID = r.ResourceID ";
            query2 += "  where r.CompanyID = " + Convert.ToInt32(Session["CompanyID"]) + "";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query2, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult InaugurationActivity()
        {
            return View();
        }
    }
}
