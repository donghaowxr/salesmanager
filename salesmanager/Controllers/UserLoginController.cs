using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SchoolMateCommon;
using System.Data;

namespace salesmanager.Controllers
{
    public class UserLoginController : Controller
    {
        //
        // GET: /UserLogin/

        public ActionResult Index()
        {
            return View();
        }

        //登录action
        public ActionResult UserLogin()
        {
            return View();
        }

        //登录验证
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public ActionResult ProcessLogin()
        {
            //获取用户输入的用户名和密码
            string username = Request["username"];
            string password = Request["password"];
            string depart = "";
            string respassowrd = "";
            int status = 1;
            int id = 0;
            //判断用户输入是否为空
            if (!String.IsNullOrEmpty(username) && username != null)
            {
                if (!String.IsNullOrEmpty(password) && password != null)
                {
                    SqlParameter[] usernamepars = { new SqlParameter("@username", username) };
                    var loginres = SQLHelper.ExecuteReader(connStr, System.Data.CommandType.StoredProcedure, "ProcessLogin", usernamepars);
                    //判断用户名是否正确
                    if (loginres.HasRows == true)
                    {
                        while (loginres.Read())
                        {
                            id = Convert.ToInt32(loginres["id"]);
                            status = Convert.ToInt32(loginres["status"]);
                            respassowrd = loginres["password"].ToString();
                            depart = loginres["depart"].ToString();
                        }
                        //判断密码是否正确
                        if (password == respassowrd)
                        {
                            //用户名密码正确（写入登录时间）
                            DateTime dt = DateTime.Now;
                            string logintime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            SqlParameter[] logintimepars = { new SqlParameter("@username", username), new SqlParameter("@logintime", logintime) };
                            var ds = SQLHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "logintime_Add", logintimepars);
                            var result = new { code = "0", status = status.ToString() };
                            Session["username"] = username;
                            Session["logintime"] = logintime;
                            Session["status"] = status;
                            Session["id"] = id;
                            Session["depart"] = depart;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        //密码错误返回状态码3
                        else
                        {
                            var result = new { code = "3" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //用户名错误返回状态码1
                    else
                    {
                        var result = new { code = "1" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                //密码为空返回状态码4
                else
                {
                    var result = new { code = "4" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                //如果用户输入为空，返回状态码2
                var result = new { code = "2" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
