using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;

namespace iCoffe.Shared
{
    public class CoffeeHouseNet
    {
        public string Name { set; get; }

        public string Logo { set; get; }

        public string Photo { set; get; }

        public List<string> Addresses { set; get; }

        public List<string> WorkingTime { set; get; }

        public List<string> Contacts { set; get; }
    }
}
