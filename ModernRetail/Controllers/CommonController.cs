﻿using BusinessLogicLayer.MenuBLS;
using EntityLayer.MenuHelperELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModernRetail.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public PartialViewResult _PartialMenu()
        {
            MenuBL menuBL = new MenuBL();
            ///Session["MRusergoup"] = "1";
            //Session["MREntryProfileType"] = "F";
            List<MenuListModel> ModelLsit = menuBL.GetUserMenuListByGroup();
            return PartialView(ModelLsit);
        }

        public ActionResult RedirectToLogin(string rurl)
        {
            string url = "/OMS/Login.aspx";

            if (!string.IsNullOrWhiteSpace(rurl))
            {
                url += "?rurl=" + rurl;
            }

            return Redirect(url);
        }
    }
}