using airen.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace airen.Controllers
{
    public class AirenController : Controller
    {
        // GET: Airen

        public ActionResult Index()
        {
            if (Request.Cookies["Myuser"] != null)
            {
                Response.Redirect("/Airen/IndexManage");
                return null;
            }
            return View();
        }
        public ActionResult SuperManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
            {
                var db = new airenEntities();
                var list = from s in db.user where s.Type != "管理员" orderby s.ID select s;
                string html = "";
                string online;
                //int page;
                int i = 0;
                //if (Request["page"] != null)
                //    page = int.Parse(Request["page"]);
                //else page = 1;
                if (list.Count() > 0)
                {
                    //if (page == 1)
                    //{
                    //    page = 0;
                    //    i = page * 10;
                    //}
                    //else i = (page - 1) * 10;
                    foreach (var item in list.Take(10))
                    {
                        i++;
                        if (item.IsOnline == "在线")
                        {
                            online = "<td style='color:green'>" + item.IsOnline + "</td>";
                        }
                        else
                        {
                            online = "<td style='color:red'>" + item.IsOnline + "</td>";
                        }
                        html += "<tr class='dat'><td>" + i + "</td><td>" + item.Phone + "</td><td>" + item.Name + "</td><td>" + item.Sex + "</td><td>" + item.Age + "</td> <td>" + item.Profession + "</td><td>" + item.Type + "</td>" + online + "</tr>";
                    }
                    ViewData["td"] = html;
                }
                ViewData["count"] = list.Count();
                //if (page == 0)
                //    page = 1;
                //ViewData["pageIndex"] = page;
                return View();
            }
            Response.Redirect("/");
            return null;
        }
        public ActionResult SuperIndex()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
                return View();
            Response.Redirect("/");
            return null;
        }

        public ActionResult NewsManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
            {
                
                var db = new airenEntities();
                var list = from s in db.news orderby s.ID select s;
                string html = "";
                // string content;
                //int page;
                int i = 0;
                //if (Request["page"] != null)
                //    page = int.Parse(Request["page"]);
                //else page = 1;
                if (list.Count() > 0)
                {
                    //if (page == 1)
                    //{
                    //    page = 0;
                    //    i = page * 10;
                    //}
                    //else i = (page - 1) * 10;
                    foreach (var item in list.Take(10))
                    {
                        i++;
                        //if (item.Content.Length > 100)
                        //    content = item.Content.Substring(0, 100);
                        //else
                        //    content = item.Content;
                        html += "<tr class='dat'><td class='ckbox'><input type='checkbox' /></td><td>" + i + "</td><td style='display:none'>" + item.ID + "</td><td>" + item.Title + "</td><td>" + item.Author + "</td><td>" + item.AddTime.ToString("yyyy-MM-dd") + "</td><td style='display:none'>" + item.Content + "</td></tr>";
                    }
                    ViewData["td"] = html;
                }
                ViewData["count"] = list.Count();
                //if (page == 0)
                //    page = 1;
                //ViewData["pageIndex"] = page;
                return View();
            }
            Response.Redirect("/");
            return null;
        }
        public ActionResult IndexManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
                return View();
            Response.Redirect("/");
            return null;
        }
        public ActionResult UserManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
            {
                if (Request.Cookies["Myuser"] != null)
                    ViewData["phone"] = Request.Cookies["Myuser"]["phone"];
                else ViewData["phone"] = Session["phone"];
                //airenEntities db = new airenEntities();
                //var user = db.user.Where(u => u.Phone == Session["phone"].ToString()).FirstOrDefault();      
                return View();
            }
            Response.Redirect("/");
            return null;
        }
        public ActionResult PwdManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
            {
                if (Request.Cookies["Myuser"] != null)
                    ViewData["phone"] = Request.Cookies["Myuser"]["phone"];
                else ViewData["phone"] = Session["phone"];
                return View();
            }
            Response.Redirect("/");
            return null;
        }
        public ActionResult PhotoManage()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
                return View();
            Response.Redirect("/");
            return null;
        }
        public ActionResult PhotoView()
        {
            if (Request.Cookies["Myuser"] != null || Session["phone"] != null)
            {
                //string userid;
                //if (Request.Cookies["Myuser"] != null)
                //    userid = Request.Cookies["Myuser"]["userid"];
                // else userid = Session["id"].ToString();
                //airenEntities db = new airenEntities();
                //var list = from s in db.file where s.UserID == userid select s;
                //ViewData["userid"] = userid;
                return View();
            }
            Response.Redirect("/");
            return null;
        }

        [ValidateInput(false)]
        public string donews()
        {

            string title = Request["title"];
            string author = Request["author"];
            DateTime addtime = DateTime.Parse(Request["addtime"]);
            string content = Request["content"];

            var db = new airenEntities();
            //id 为空 添加
            if (string.IsNullOrWhiteSpace(Request["id"]))
            {
                news news = new news();
                news.Title = title;
                news.Author = author;
                news.AddTime = addtime;
                news.Content = content;
                db.news.Add(news);
                db.SaveChanges();
                return "success";
            }
            //id 不为空 修改
            else
            {
                int id = int.Parse(Request["id"]);
                var nws = db.news.Where(n => n.ID == id).FirstOrDefault();
                nws.Title = title;
                nws.Author = author;
                nws.AddTime = addtime;
                nws.Content = content;
                db.SaveChanges();
                return "success1";
            }
        }
        public string doview()
        {
            airenEntities db = new airenEntities();
            int count = int.Parse(Request["count"]);
            //int id = int.Parse(Request["id"]);
            // var list = from s in db.file where s.UserID == UserID && s.ID > 40 select s;
            List<file> list = new List<file>();
            MySqlConnection conn = DB.MBT();
            conn.Open();
            string sql = "select * from file where UserID=" + Request.Cookies["Myuser"]["userid"] + " LIMIT " + count + ",3";
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DataTable table = ds.Tables[0];
            string pic = "";
            foreach (DataRow item in table.Rows)
            {
                pic += DataRowToModel(item).FileName + ",";
            }
            return pic;
        }

        public file DataRowToModel(DataRow dr)
        {

            file model = new file();
            if (!dr.IsNull("ID"))
            {
                model.ID = int.Parse(dr["ID"].ToString()); model.FileName = dr["FileName"].ToString(); model.FilePath = dr["FilePath"].ToString(); model.UserID = dr["UserID"].ToString();
            }
            return model;
        }

        public string doupload()
        {
            string url = "";
            HttpPostedFileBase _file;
            airenEntities db = new airenEntities();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                _file = Request.Files[i];
                if (_file.ContentLength > 0)
                {
                    url = Server.MapPath("~/upload/") + _file.FileName;
                    _file.SaveAs(url);
                    file fi = new file();
                    fi.FileName = _file.FileName;
                    fi.FilePath = url;
                    fi.UserID = Request.Cookies["Myuser"]["userid"];
                    db.file.Add(fi);
                    db.SaveChanges();
                }
            }
            return "";
        }

        public string doinfo()
        {
            string phone = Request["phone"];
            string name = Request["name"];
            string bday = Request["bday"];
            string age = Request["age"];
            string pro = Request["pro"];
            string sex = Request["sex"];
            if (!string.IsNullOrWhiteSpace(phone))
            {
                airenEntities db = new airenEntities();
                //var info = from p in db.user
                //           where p.Phone == phone && p.Password == pwd
                //           select p;
                var list = db.user.Where(p => p.Phone == phone).First();
                if (list != null)
                {
                    list.Name = name;
                    if (!string.IsNullOrWhiteSpace(bday))
                        list.Birthday = DateTime.Parse(bday);
                    list.Age = age;
                    list.Profession = pro;
                    list.Sex = sex;
                    db.SaveChanges();
                    return "success";
                }
                else
                    return "error";
            }
            else
            {
                return "error";
            }
        }

        public string dopwd()
        {
            string phone = Request["phone"];
            string pwd = Request["pwd"];
            if (!string.IsNullOrWhiteSpace(pwd))
            {
                airenEntities db = new airenEntities();
                //var info = from p in db.user
                //           where p.Phone == phone && p.Password == pwd
                //           select p;
                var list = db.user.Where(p => p.Phone == phone).FirstOrDefault();
                if (list != null)
                {
                    list.Password = pwd;
                    list.Confirmpwd = pwd;
                    db.SaveChanges();
                    return "success";
                }
                else
                    return "error";
            }
            else
            {
                return "error";
            }
        }

        public string doReg()
        {
            string phone = Request["phone"];
            string pwd = Request["pwd"];
            if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(pwd))
            {
                airenEntities db = new airenEntities();
                user user = new user();
                user.Phone = phone;
                user.Password = pwd;
                user.Confirmpwd = pwd;
                user.Type = "普通用户";
                db.user.Add(user);
                db.SaveChanges();
                return "success";
            }
            else
            {
                return "error";
            }
        }

        public string dolog()
        {
            string phone = Request["phone"];
            string pwd = Request["pwd"];

            string UserID;
            if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrWhiteSpace(pwd))
            {
                airenEntities db = new airenEntities();
                //var info = from p in db.user
                //           where p.Phone == phone && p.Password == pwd
                //           select p;
                var list = db.user.Where(p => p.Phone == phone && p.Password == pwd).FirstOrDefault();
                if (list != null)
                {
                    Session["phone"] = phone;
                    Session["pwd"] = pwd;
                    UserID = list.ID.ToString();
                    Session["id"] = UserID;
                    list.IsOnline = "在线";
                    HttpCookie cookie = new HttpCookie("Myuser");
                    cookie.Values["phone"] = phone;
                    cookie.Values["pwd"] = pwd;
                    cookie.Values["userid"] = UserID;
                    //cookie.Values.Add("username", username);
                    //cookie.Values.Add("pwd", pwd);
                    cookie.Expires = DateTime.Now.AddSeconds(600);
                    Response.AppendCookie(cookie);
                    db.SaveChanges();
                    if (list.Type == "管理员")
                        return "super";
                    return "success";
                }
                else
                    return "error";
            }
            else
                return "error";

        }

        public string exit()
        {
            var db = new airenEntities();
            string phone;
            if (Session["phone"] != null)
                phone = Session["phone"].ToString();
            else
                phone = Request.Cookies["Myuser"]["phone"].ToString();
            var user = db.user.Where(p => p.Phone == phone).FirstOrDefault();
            user.IsOnline = "离线";
            db.SaveChanges();
            return "success";
        }
        public string outuser()
        {
            var db = new airenEntities();
            string phone = Request["phone"];
            var user = db.user.Where(p => p.Phone == phone).FirstOrDefault();
            user.IsOnline = "离线";
            db.SaveChanges();
            return "success";
        }

        public string delete() {
            string ids = Request["ids"];
            var db = new airenEntities();
            foreach(var item in ids.Split(','))
            {
                int id = Convert.ToInt32(item);
                var nws = db.news.Where(n => n.ID == id).FirstOrDefault();
                db.news.Remove(nws);
            }
            db.SaveChanges();
            return "success";
        }
        public string dopage()
        {
            var db = new airenEntities();
            int page = int.Parse(Request["page"]);
            var list = from s in db.user where s.Type != "管理员" orderby s.ID select s;
            string html = "";
            string online;
            //int page;
            int i = (page - 1) * 10;
            //if (Request["page"] != null)
            //    page = int.Parse(Request["page"]);
            //else page = 1;
            if (list.Count() > 0)
            {
                //if (page == 1)
                //{
                //    page = 0;
                //    i = page * 10;
                //}
                //else i = (page - 1) * 10;
                foreach (var item in list.Skip(i).Take(10))
                {
                    i++;
                    if (item.IsOnline == "在线")
                    {
                        online = "<td style='color:green'>" + item.IsOnline + "</td>";
                    }
                    else
                    {
                        online = "<td style='color:red'>" + item.IsOnline + "</td>";
                    }
                    html += "<tr class='dat'><td>" + i + "</td><td>" + item.Phone + "</td><td>" + item.Name + "</td><td>" + item.Sex + "</td><td>" + item.Age + "</td> <td>" + item.Profession + "</td><td>" + item.Type + "</td>" + online + "</tr>";
                }
                //ViewData["td"] = html;
            }
            //if (page == 0)
            //    page = 1;
            return html;
        }

        public string donewspage()
        {
            var db = new airenEntities();
            int page = int.Parse(Request["page"]);
            var list = from s in db.news orderby s.ID select s;
            string html = "";
            //string content;
            //int page;
            int i = (page - 1) * 10;
            //if (Request["page"] != null)
            //    page = int.Parse(Request["page"]);
            //else page = 1;
            if (list.Count() > 0)
            {
                //if (page == 1)
                //{
                //    page = 0;
                //    i = page * 10;
                //}
                //else i = (page - 1) * 10;
                foreach (var item in list.Skip(i).Take(10))
                {
                    i++;
                    //if (item.Content.Length > 100)
                    //    content = item.Content.Substring(0, 100);
                    //else
                    //    content = item.Content;
                    html += "<tr class='dat'><td class='ckbox'><input type='checkbox' /></td><td>" + i + "</td><td style='display:none'>" + item.ID + "</td><td>" + item.Title + "</td><td>" + item.Author + "</td><td>" + item.AddTime.ToString("yyyy-MM-dd") + "</td><td style='display:none'>" + item.Content + "</td></tr>";
                }
                //ViewData["td"] = html;
            }
            ViewData["count"] = list.Count();
            //if (page == 0)
            //    page = 1;
            return html;
        }
        public int gettotal()
        {
            var db = new airenEntities();
            var list = from s in db.news orderby s.ID select s;
            int count = list.Count();
            return count;
        }



    }
}