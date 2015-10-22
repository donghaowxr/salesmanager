using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace salesmanager.Models
{
    public class UserLogin
    {
        public int id
        {
            get;
            set;
        }
        public string username
        {
            get;
            set;
        }
        public string password
        {
            get;
            set;
        }
        public string salename
        {
            get;
            set;
        }
        public string phone
        {
            get;
            set;
        }
        public string tel
        {
            get;
            set;
        }
        public int status
        {
            get;
            set;
        }
        public int parentID
        {
            get;
            set;
        }
        public string logintime
        {
            get;
            set;
        }
        public string depart
        {
            get;
            set;
        }
    }
}