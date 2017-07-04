using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using collectBooksManageList.Models;
using System.Data.SqlClient;
using System.Data;

namespace collectBooksManageList.Controllers
{
    public class HomeController : Controller
    {
       private collectBooksManageListEntities db = new collectBooksManageListEntities();
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Registered newData)
        {
            
            ViewBag.userId = newData.userId;
            ViewBag.userPassword = newData.userPassword;
            SqlConnection cn = new SqlConnection(@"server=.\SQLExpress_2;database=collectBooksManageList;Integrated Security=true");
            
            SqlCommand cmd = new SqlCommand("select * from Registered where userId=@id AND userPassword=@pwd", cn);
          
            cmd.Parameters.AddWithValue("@id", newData.userId);
            cmd.Parameters.AddWithValue("@pwd", newData.userPassword);
            
            cn.Open();
            int num = Convert.ToInt32(cmd.ExecuteScalar());
                if (num == 0)
                {
                    ViewBag.show = "ID or pwd Error";
                    return View();

                }
                
            
            Session["userId"] = newData.userId;         
            return RedirectToAction("BookList", "Home");

        }
       
        public ActionResult BookList(string s)
        {
            s = Session["userId"].ToString();
            if (s == "")
            {
                return RedirectToAction("Login", "Home");
            }

            var query = from o in db.BookLists
                        where o.U_Id == s
                        select o;

            List<BookList> data = query.ToList<BookList>();
            return View(data);

            //List<BookList> bk = db.BookLists.ToList<BookList>();
            //return View();
        }
        public ActionResult Registered()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Registered(Registered newData)
        {
            ViewBag.userId = newData.userId;
            ViewBag.userPassword = newData.userPassword;
           
            if (string.IsNullOrEmpty(newData.userId) ||
                string.IsNullOrEmpty(newData.userPassword.ToString())
               )
            {
                return View();
            }
            SqlConnection cn = new SqlConnection(@"server=.\SQLExpress_2;database=collectBooksManageList;Integrated Security=true");

            SqlCommand cmd = new SqlCommand("select * from Registered where userId=@id", cn);
            cmd.Parameters.AddWithValue("@id", newData.userId);
            cn.Open();
            int num = Convert.ToInt32(cmd.ExecuteScalar());
            cn.Close();
            if (num == 0)
            {
                db.Registereds.Add(newData);
                db.SaveChanges();

                return RedirectToAction("Login");
               

            }
            else
            {
                ViewBag.show = "ID重複";
                return View();
            }
        }
        public ActionResult EditBook()
        {
            if (Session["userId"].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
           
        }

        [HttpPost]
        public ActionResult EditBook(BookList book)
        {
            Session["userId"] = book.U_Id;
            ViewBag.bookName = book.bookName;
            ViewBag.episode = book.episode;
            if (string.IsNullOrEmpty(book.bookName)
                )
            {
                ViewBag.show = "請輸入書名或集數";
                return View();
            }
            else
            {
                SqlConnection cn = new SqlConnection(@"server=.\SQLExpress_2;database=collectBooksManageList;Integrated Security=true");

                SqlCommand cmd = new SqlCommand("select * from BookList where bookName=@name AND episode=@no", cn);
                cmd.Parameters.AddWithValue("@name", book.bookName);
                cmd.Parameters.AddWithValue("@no", book.episode);
                cn.Open();
                int num = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                if (num == 0)
                {
                    db.BookLists.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("BookList");


                }
                else
                {
                    ViewBag.show = "書籍重複登記";
                    return View();
                }
            }
            
        }
        public ActionResult Logout()
        {
            Session["userID"] = "";

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Delete(int? id)
        {
          
            BookList bk = db.BookLists.Find(id);
            db.BookLists.Remove(bk);
            db.SaveChanges();
            return RedirectToAction("BookList");
        }
    }
}