using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace salesmanager.Models
{
    public class Client
    {
        public int id
        {
            get;
            set;
        }
        public int saleID
        {
            get;
            set;
        }
        public string clientname
        {
            get;
            set;
        }
        public string industry
        {
            get;
            set;
        }
        public string address
        {
            get;
            set;
        }
        public string area
        {
            get;
            set;
        }
        public int areavalue
        {
            get;
            set;
        }
        public string clientfrom
        {
            get;
            set;
        }
        public int clientfromvalue
        {
            get;
            set;
        }
        public string contact
        {
            get;
            set;
        }
        public string contactphone
        {
            get;
            set;
        }
        public string presigntime
        {
            get;
            set;
        }
        public string signtime
        {
            get;
            set;
        }
        public string preamt
        {
            get;
            set;
        }
        public string amt
        {
            get;
            set;
        }
        public string depart
        {
            get;
            set;
        }
        public string salename
        {
            get;
            set;
        }
    }
}