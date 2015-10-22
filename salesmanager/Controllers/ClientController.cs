using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SchoolMateCommon;
using salesmanager.Models;

namespace salesmanager.Controllers
{
    public class ClientController : BaseController
    {
        //
        // GET: /Client/

        public ActionResult Index()
        {
            return View();
        }
        //客户添加
        public ActionResult addClient()
        {
            ViewData["id"] = Session["id"];
            return View();
        }
        //客户添加处理
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public ActionResult ClientAdd()
        {
            string clientname = Request["clientname"].ToString();
            if (String.IsNullOrEmpty(clientname) || clientname == null) {
                var result = new { code = "2" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int saleID = Convert.ToInt32(Request["saleID"]);
                string industry = Request["industry"].ToString();
                string address = Request["address"].ToString();
                string area = Request["area"].ToString();
                int areavalue = Convert.ToInt32(Request["areavalue"]);
                string clientfrom = Request["clientfrom"].ToString();
                string contact = Request["contact"].ToString();
                string contactphone = Request["contactphone"].ToString();
                string presigntime = Request["presigntime"].ToString();
                string signtime = Request["signtime"].ToString();
                string preamt = Request["preamt"].ToString();
                string amt = Request["amt"].ToString();
                string salename = Session["username"].ToString();
                string depart = "";
                SqlParameter[] departparam = { new SqlParameter("@id", saleID) };
                var departres = SQLHelper.ExecuteReader(connStr, System.Data.CommandType.StoredProcedure, "select_depart", departparam);
                if (departres.HasRows == true)
                {
                    while (departres.Read())
                    {
                        depart = departres["depart"].ToString();
                    }
                }
                SqlParameter[] clientparam = { new SqlParameter("@saleID", saleID), new SqlParameter("@clientname", clientname), new SqlParameter("@industry", industry), new SqlParameter("@address", address), new SqlParameter("@area", area),new SqlParameter("@areavalue",areavalue), new SqlParameter("@clientfrom", clientfrom), new SqlParameter("@contact", contact), new SqlParameter("@contactphone", contactphone), new SqlParameter("@presigntime", presigntime), new SqlParameter("@signtime", signtime), new SqlParameter("@preamt", preamt), new SqlParameter("@amt", amt), new SqlParameter("@depart", depart),new SqlParameter("@salename",salename) };
                var client_add = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "add_client", clientparam);
                //添加成功
                if (client_add > 0)
                {
                    var result = new { code = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //添加失败
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //销售员客户查询
        public ActionResult client_select()
        {
            ViewData["id"] = Session["id"];
            return View();
        }
        //销售员客户查询处理
        public ActionResult select_client()
        {
            int saleID = Convert.ToInt32(Request["saleID"]);
            SqlParameter[] saleIDparam = { new SqlParameter("@saleID", saleID) };
            var clientds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "sale_select_client", saleIDparam);
            if (clientds.Tables[0].Rows.Count > 0)
            {
                List<Client> clients = new List<Client>();
                for (int i = 0; i < clientds.Tables[0].Rows.Count; i++)
                {
                    Client client = new Client();
                    client.id = Convert.ToInt32(clientds.Tables[0].Rows[i]["id"]);
                    client.clientname = clientds.Tables[0].Rows[i]["clientname"].ToString();
                    client.industry = clientds.Tables[0].Rows[i]["industry"].ToString();
                    client.address = clientds.Tables[0].Rows[i]["address"].ToString();
                    client.area = clientds.Tables[0].Rows[i]["area"].ToString();
                    client.contact = clientds.Tables[0].Rows[i]["contact"].ToString();
                    client.contactphone = clientds.Tables[0].Rows[i]["contactphone"].ToString();
                    client.presigntime = clientds.Tables[0].Rows[i]["presigntime"].ToString();
                    client.signtime = clientds.Tables[0].Rows[i]["signtime"].ToString();
                    client.preamt = clientds.Tables[0].Rows[i]["preamt"].ToString();
                    client.amt = clientds.Tables[0].Rows[i]["amt"].ToString();
                    clients.Add(client);
                }
                var result = new { code = "0", rows = clients };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //查看客户详情
        public ActionResult clienttotalmessage(string id)
        {
            ViewData["id"] = id;
            return View();
        }
        //查询客户详情处理
        public ActionResult totalmessageclient()
        {
            int id = Convert.ToInt32(Request["id"]);
            SqlParameter[] messageparam = { new SqlParameter("@id", id) };
            var totalmessageres = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "client_total_message", messageparam);
            if (totalmessageres.Tables[0].Rows.Count > 0)
            {
                List<Client> totalclient = new List<Client>();
                for (int i = 0; i < totalmessageres.Tables[0].Rows.Count; i++)
                {
                    Client client = new Client();
                    client.clientname = totalmessageres.Tables[0].Rows[i]["clientname"].ToString();
                    client.industry = totalmessageres.Tables[0].Rows[i]["industry"].ToString();
                    client.address = totalmessageres.Tables[0].Rows[i]["address"].ToString();
                    client.area = totalmessageres.Tables[0].Rows[i]["area"].ToString();
                    client.areavalue = Convert.ToInt32(totalmessageres.Tables[0].Rows[i]["areavalue"]);
                    client.clientfrom = totalmessageres.Tables[0].Rows[i]["clientfrom"].ToString();
                    client.contact = totalmessageres.Tables[0].Rows[i]["contact"].ToString();
                    client.contactphone = totalmessageres.Tables[0].Rows[i]["contactphone"].ToString();
                    client.presigntime = totalmessageres.Tables[0].Rows[i]["presigntime"].ToString();
                    client.signtime = totalmessageres.Tables[0].Rows[i]["signtime"].ToString();
                    client.preamt = totalmessageres.Tables[0].Rows[i]["preamt"].ToString();
                    client.amt = totalmessageres.Tables[0].Rows[i]["amt"].ToString();
                    client.salename = totalmessageres.Tables[0].Rows[i]["salename"].ToString();
                    totalclient.Add(client);
                }
                var result = new { code = "0", rows = totalclient };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //客户信息修改
        public ActionResult EditClient(string id)
        {
            ViewData["id"] = id;
            return View();
        }
        //客户信息修改处理
        public ActionResult ClientEdit()
        {
            string clientname = Request["clientname"].ToString();
            if (String.IsNullOrEmpty(clientname) || clientname == null)
            {
                var result = new { code = "2" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int id=Convert.ToInt32(Request["id"]);
                string industry = Request["industry"].ToString();
                string address = Request["address"].ToString();
                string area = Request["area"].ToString();
                string clientfrom = Request["clientfrom"].ToString();
                string contact = Request["contact"].ToString();
                string contactphone = Request["contactphone"].ToString();
                string presigntime = Request["presigntime"].ToString();
                string signtime = Request["signtime"].ToString();
                string preamt = Request["preamt"].ToString();
                string amt = Request["amt"].ToString();
                SqlParameter[] editclientparam = { new SqlParameter("@id", id), new SqlParameter("@clientname", clientname), new SqlParameter("@industry", industry), new SqlParameter("@address", address), new SqlParameter("@area", area), new SqlParameter("@clientfrom", clientfrom), new SqlParameter("@contact", contact), new SqlParameter("@contactphone", contactphone), new SqlParameter("@presigntime", presigntime), new SqlParameter("@signtime", signtime), new SqlParameter("@preamt", preamt), new SqlParameter("@amt", amt) };
                var editres = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "edit_client", editclientparam);
                if (editres > 0)
                {
                    var result = new { code = "0" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //客户模糊查询
        public ActionResult clientsearch()
        {
            string clientname = Request["clientname"].ToString();
            int saleID = Convert.ToInt32(Request["saleID"]);
            SqlParameter[] searchparam = { new SqlParameter("@saleID", saleID), new SqlParameter("@clientname", "%" + clientname + "%") };
            var clientds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "searchclientbyname", searchparam);
            if (clientds.Tables[0].Rows.Count > 0)
            {
                List<Client> clients = new List<Client>();
                for (int i = 0; i < clientds.Tables[0].Rows.Count; i++)
                {
                    Client client = new Client();
                    client.id = Convert.ToInt32(clientds.Tables[0].Rows[i]["id"]);
                    client.clientname = clientds.Tables[0].Rows[i]["clientname"].ToString();
                    client.industry = clientds.Tables[0].Rows[i]["industry"].ToString();
                    client.address = clientds.Tables[0].Rows[i]["address"].ToString();
                    client.area = clientds.Tables[0].Rows[i]["area"].ToString();
                    client.contact = clientds.Tables[0].Rows[i]["contact"].ToString();
                    client.contactphone = clientds.Tables[0].Rows[i]["contactphone"].ToString();
                    client.presigntime = clientds.Tables[0].Rows[i]["presigntime"].ToString();
                    client.signtime = clientds.Tables[0].Rows[i]["signtime"].ToString();
                    client.preamt = clientds.Tables[0].Rows[i]["preamt"].ToString();
                    client.amt = clientds.Tables[0].Rows[i]["amt"].ToString();
                    clients.Add(client);
                }
                var result = new { code = "0", rows = clients };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //销售管理员查询客户
        public ActionResult salemanageclient()
        {
            ViewData["id"] = Session["id"];
            return View();
        }
        //销售管理员查询客户处理
        public ActionResult clientsalemanager()
        {
            if (Convert.ToInt32(Session["status"]) == 0)
            {
                var allclientds= SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "allclient");
                if (allclientds.Tables[0].Rows.Count > 0)
                {
                    List<Client> clients = new List<Client>();
                    for (int i = 0; i < allclientds.Tables[0].Rows.Count; i++)
                    {
                        Client client = new Client();
                        client.id = Convert.ToInt32(allclientds.Tables[0].Rows[i]["id"]);
                        client.clientname = allclientds.Tables[0].Rows[i]["clientname"].ToString();
                        client.industry = allclientds.Tables[0].Rows[i]["industry"].ToString();
                        client.address = allclientds.Tables[0].Rows[i]["address"].ToString();
                        client.area = allclientds.Tables[0].Rows[i]["area"].ToString();
                        client.contact = allclientds.Tables[0].Rows[i]["contact"].ToString();
                        client.contactphone = allclientds.Tables[0].Rows[i]["contactphone"].ToString();
                        client.salename = allclientds.Tables[0].Rows[i]["salename"].ToString();
                        clients.Add(client);
                    }
                    var result = new { code = "0", rows = clients };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                int id = Convert.ToInt32(Request["id"]);
                SqlParameter[] idparam = { new SqlParameter("@id", id) };
                var clientds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "salemanagerclientsearch", idparam);
                if (clientds.Tables[0].Rows.Count > 0)
                {
                    List<Client> clients = new List<Client>();
                    for (int i = 0; i < clientds.Tables[0].Rows.Count; i++)
                    {
                        Client client = new Client();
                        client.id = Convert.ToInt32(clientds.Tables[0].Rows[i]["id"]);
                        client.clientname = clientds.Tables[0].Rows[i]["clientname"].ToString();
                        client.industry = clientds.Tables[0].Rows[i]["industry"].ToString();
                        client.address = clientds.Tables[0].Rows[i]["address"].ToString();
                        client.area = clientds.Tables[0].Rows[i]["area"].ToString();
                        client.contact = clientds.Tables[0].Rows[i]["contact"].ToString();
                        client.contactphone = clientds.Tables[0].Rows[i]["contactphone"].ToString();
                        client.salename = clientds.Tables[0].Rows[i]["salename"].ToString();
                        clients.Add(client);
                    }
                    var result = new { code = "0", rows = clients };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //销售管理员查询客户详情
        public ActionResult salemanagertotalmessage(string id)
        {
            ViewData["id"] = id;
            return View();
        }
        //销售管理员客户条件查询
        public ActionResult salemanagersearchclient()
        {
            if (Convert.ToInt32(Session["status"]) == 0)
            {
                string clientname = Request["clientname"].ToString();
                SqlParameter[] searchparam = { new SqlParameter("@clientname", "%" + clientname + "%") };
                var clientds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "allclientsearch", searchparam);
                if (clientds.Tables[0].Rows.Count > 0)
                {
                    List<Client> clients = new List<Client>();
                    for (int i = 0; i < clientds.Tables[0].Rows.Count; i++)
                    {
                        Client client = new Client();
                        client.id = Convert.ToInt32(clientds.Tables[0].Rows[i]["id"]);
                        client.clientname = clientds.Tables[0].Rows[i]["clientname"].ToString();
                        client.industry = clientds.Tables[0].Rows[i]["industry"].ToString();
                        client.address = clientds.Tables[0].Rows[i]["address"].ToString();
                        client.area = clientds.Tables[0].Rows[i]["area"].ToString();
                        client.contact = clientds.Tables[0].Rows[i]["contact"].ToString();
                        client.contactphone = clientds.Tables[0].Rows[i]["contactphone"].ToString();
                        client.salename = clientds.Tables[0].Rows[i]["salename"].ToString();
                        clients.Add(client);
                    }
                    var result = new { code = "0", rows = clients };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                int id = Convert.ToInt32(Request["id"]);
                string clientname = Request["clientname"].ToString();
                SqlParameter[] searchparam = { new SqlParameter("@id", id), new SqlParameter("@clientname", "%" + clientname + "%") };
                var clientds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "managerclientsearch", searchparam);
                if (clientds.Tables[0].Rows.Count > 0)
                {
                    List<Client> clients = new List<Client>();
                    for (int i = 0; i < clientds.Tables[0].Rows.Count; i++)
                    {
                        Client client = new Client();
                        client.id = Convert.ToInt32(clientds.Tables[0].Rows[i]["id"]);
                        client.clientname = clientds.Tables[0].Rows[i]["clientname"].ToString();
                        client.industry = clientds.Tables[0].Rows[i]["industry"].ToString();
                        client.address = clientds.Tables[0].Rows[i]["address"].ToString();
                        client.area = clientds.Tables[0].Rows[i]["area"].ToString();
                        client.contact = clientds.Tables[0].Rows[i]["contact"].ToString();
                        client.contactphone = clientds.Tables[0].Rows[i]["contactphone"].ToString();
                        client.salename = clientds.Tables[0].Rows[i]["salename"].ToString();
                        clients.Add(client);
                    }
                    var result = new { code = "0", rows = clients };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { code = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
    }
}
