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
    public class CustomerController : BaseController
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }
        //退出登录
        public ActionResult Exit()
        {
            if (Session["username"] != null)
            {
                Session.Remove("username");

            }
            return RedirectToAction("UserLogin", "UserLogin");
        }
        //验证登录跳转
        public ActionResult LoginRedirect()
        {
            string username = Session["username"].ToString();
            int status = Convert.ToInt32(Session["status"]);
            switch (status)
            {
                case 0:
                    return RedirectToAction("Admin", "Customer");
                    break;
                case 1:
                    return RedirectToAction("Admin", "Customer");
                    break;
                case 2:
                    return RedirectToAction("sales","Customer");
                    break;
                default:
                    return RedirectToAction("UserLogin", "UserLogin");
                    break;
            }
                
        }

        //管理员界面
        public ActionResult Admin()
        {
            ViewData["username"] = Session["username"];
            return View();
        }
        //销售界面
        public ActionResult sales()
        {
            ViewData["username"] = Session["username"];
            return View();
        }
        //角色管理页面
        public ActionResult RoleManager()
        {
            ViewData["id"] = Session["id"];
            ViewData["status"] = Session["status"];
            return View();
        }

        //获取子销售列表
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public ActionResult GetSaleList()
        {
            //获取当前用户的id
            int id = Convert.ToInt32(Request["id"]);
            SqlParameter[] saleparm = { new SqlParameter("@id", id) };
            var saleds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "getsalelist", saleparm);
            //如果存在子销售员
            //ds.Tables[0].Rows[i]["ID"]
            if (saleds.Tables[0].Rows.Count > 0)
            {
                List<UserLogin> saleList = new List<UserLogin>();
                for (int i = 0; i < saleds.Tables[0].Rows.Count; i++)
                {
                    UserLogin sale = new UserLogin();
                    sale.id = Convert.ToInt32(saleds.Tables[0].Rows[i]["id"]);
                    sale.username = saleds.Tables[0].Rows[i]["username"].ToString();
                    sale.phone = saleds.Tables[0].Rows[i]["phone"].ToString();
                    sale.tel = saleds.Tables[0].Rows[i]["tel"].ToString();
                    sale.depart = saleds.Tables[0].Rows[i]["depart"].ToString();
                    sale.logintime = saleds.Tables[0].Rows[i]["logintime"].ToString();
                    saleList.Add(sale);
                }
                //返回返回码0，子销售员列表
                var result = new { code = "0", rows = saleList };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //返回返回码1，没有子销售员
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //根据用户名查询
        public ActionResult SearchByName()
        {
            int parentId = Convert.ToInt32(Request["parentId"]);
            string searchName = Request["searchname"].ToString();
            //根据用户名模糊查询出子销售员
            SqlParameter[] searchParm = { new SqlParameter("@parentId", parentId), new SqlParameter("@username", "%" + searchName + "%") };
            var searchds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "name_likesearch", searchParm);
            //如果查询的子销售员存在
            if (searchds.Tables[0].Rows.Count > 0)
            {
                List<UserLogin> searchSaleList = new List<UserLogin>();
                for (int i = 0; i < searchds.Tables[0].Rows.Count; i++)
                {
                    UserLogin sale = new UserLogin();
                    sale.username = searchds.Tables[0].Rows[i]["username"].ToString();
                    sale.phone = searchds.Tables[0].Rows[i]["phone"].ToString();
                    sale.tel = searchds.Tables[0].Rows[i]["tel"].ToString();
                    sale.depart = searchds.Tables[0].Rows[i]["depart"].ToString();
                    sale.logintime = searchds.Tables[0].Rows[i]["logintime"].ToString();
                    searchSaleList.Add(sale);
                }
                //成功返回返回码0和子销售员列表
                var result = new { code = "0", rows = searchSaleList };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //返回返回码1，没有子销售员
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //添加销售管理员界面
        public ActionResult addSaleManager()
        {
            ViewData["id"] = Session["id"];
            ViewData["status"] = Session["status"];
            return View();
        }
        //添加销售管理员处理
        public ActionResult SaleManagerAdd()
        {
            string username = Request["username"].ToString();
            string phone = Request["phone"].ToString();
            string tel = Request["tel"].ToString();
            string depart = Request["depart"].ToString();
            string password = "123456";
            string salename = username;
            int parentID = Convert.ToInt32(Request["parentID"]);
            int parentstatus = Convert.ToInt32(Request["parentstatus"]);
            int status = parentstatus + 1;
            SqlParameter[] saleManagerParm = { new SqlParameter("@username", username), new SqlParameter("@password", password), new SqlParameter("@salename", salename), new SqlParameter("@phone", phone), new SqlParameter("@tel", tel), new SqlParameter("@status", status), new SqlParameter("@parentID", parentID), new SqlParameter("@depart", depart) };
            var saleManagerret = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "add_salemanager", saleManagerParm);
            //添加成功返回状态码0
            if (saleManagerret > 0)
            {
                var result = new { code = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //添加失败返回状态码1
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //添加销售
        public ActionResult addSale()
        {
            ViewData["id"] = Session["id"];
            ViewData["status"] = Session["status"];
            ViewData["depart"] = Session["depart"];
            return View();
        }
        //添加销售处理
        public ActionResult SaleAdd()
        {
            string username = Request["username"].ToString();
            string phone = Request["phone"].ToString();
            string tel = Request["tel"].ToString();
            int parentID = Convert.ToInt32(Request["parentID"]);
            int parentstatus = Convert.ToInt32(Request["status"]);
            string depart = Request["depart"].ToString();
            int status = parentstatus + 1;
            string password = "123456";
            string salename = username;
            SqlParameter[] saleParm = { new SqlParameter("@username", username), new SqlParameter("@password", password), new SqlParameter("@salename", salename), new SqlParameter("@phone", phone), new SqlParameter("@tel", tel), new SqlParameter("@status", status), new SqlParameter("@parentID", parentID), new SqlParameter("@depart", depart) };
            var saleret = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "add_salemanager", saleParm);
            //添加成功返回状态码0
            if (saleret > 0)
            {
                var result = new { code = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //添加失败返回状态码1
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //获取销售员详细信息
        public ActionResult SaleMessage()
        {
            int id = Convert.ToInt32(Request["id"]);
            SqlParameter[] saleMessageParam = { new SqlParameter("@id", id) };
            var saleMessageds = SQLHelper.ExecuteDataset(connStr, System.Data.CommandType.StoredProcedure, "sale_message", saleMessageParam);
            //如果有数据返回
            if (saleMessageds.Tables[0].Rows.Count > 0)
            {
                List<UserLogin> saleMessage = new List<UserLogin>();
                for (int i = 0; i < saleMessageds.Tables[0].Rows.Count; i++)
                {
                    UserLogin sale = new UserLogin();
                    sale.username = saleMessageds.Tables[0].Rows[i]["username"].ToString();
                    sale.phone = saleMessageds.Tables[0].Rows[i]["phone"].ToString();
                    sale.tel = saleMessageds.Tables[0].Rows[i]["tel"].ToString();
                    sale.depart = saleMessageds.Tables[0].Rows[i]["depart"].ToString();
                    saleMessage.Add(sale);
                }
                var result = new { code = "0", rows = saleMessage };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //销售管理员修改
        public ActionResult EditSaleManager(string id)
        {
            ViewData["id"] = Convert.ToInt32(id);
            return View();
        }
        //销售管理员修改处理
        public ActionResult SaleManagerEdit()
        {
            string username = Request["username"].ToString();
            string phone = Request["phone"].ToString();
            string tel = Request["tel"].ToString();
            string depart = Request["depart"].ToString();
            int id = Convert.ToInt32(Request["id"]);
            SqlParameter[] editparam = { new SqlParameter("@username", username), new SqlParameter("@phone", phone), new SqlParameter("@tel", tel), new SqlParameter("@depart", depart), new SqlParameter("@id", id) };
            var saleeditres = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "edit_sale", editparam);
            //修改成功返回成功码0
            if (saleeditres > 0)
            {
                var result = new { code = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //修改失败返回失败码1
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //销售员修改
        public ActionResult EditSale(string id)
        {
            ViewData["id"] = Convert.ToInt32(id);
            return View();
        }
        //销售员修改处理
        public ActionResult SaleEdit()
        {
            string username = Request["username"].ToString();
            string phone = Request["phone"].ToString();
            string tel = Request["tel"].ToString();
            string depart = Request["depart"].ToString();
            int id = Convert.ToInt32(Request["id"]);
            SqlParameter[] editparam = { new SqlParameter("@username", username), new SqlParameter("@phone", phone), new SqlParameter("@tel", tel), new SqlParameter("@depart", depart), new SqlParameter("@id", id) };
            var saleeditres = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "edit_sale", editparam);
            //修改成功返回成功码0
            if (saleeditres > 0)
            {
                var result = new { code = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //修改失败返回失败码1
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //个人中心
        public ActionResult selfcenter()
        {
            ViewData["id"] = Session["id"];
            return View();
        }
        //个人信息修改处理
        public ActionResult centerSelf()
        {
            string username = Request["username"].ToString();
            string password = Request["password"].ToString();
            string repassword = Request["repassword"].ToString();
            string phone = Request["phone"].ToString();
            string tel = Request["tel"].ToString();
            int id = Convert.ToInt32(Request["id"]);
            if ((!String.IsNullOrEmpty(password) && password != null) || (!String.IsNullOrEmpty(repassword) && repassword != null))
            {
                if (password == repassword)
                {
                    SqlParameter[] selfparam = { new SqlParameter("@username", username), new SqlParameter("@password", password), new SqlParameter("@phone", phone), new SqlParameter("@tel", tel), new SqlParameter("@id", id) };
                    var selfedit = SQLHelper.ExecuteNonQuery(connStr, System.Data.CommandType.StoredProcedure, "self_edit", selfparam);
                    if (selfedit > 0)
                    {
                        var result = new { code = "0" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    //修改失败
                    else
                    {
                        var result = new { code = "3" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                //密码确认不一致返回2
                else
                {
                    var result = new { code = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            //密码为空返回1
            else
            {
                var result = new { code = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
    }
}
