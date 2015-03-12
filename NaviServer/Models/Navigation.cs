using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Navigation
    {
        private String id;
        private String startAdd ;
        private List<String> addresses = new List<string>();
        private String endAdd;


        public Navigation()
        {
        }

        public Navigation(String id, String startAdd, List<String> addresses, String endAdd)
        {
            this.id = id;
            this.startAdd = startAdd;
            this.addresses = addresses;
            this.endAdd = endAdd;
        }

        public void addAddress(String address)
        {
            addresses.Add(address);
        }

        public void addStartAdd(String address)
        {
            startAdd = address;
        }

        public void addEndAdd(String address)
        {
            endAdd = address;
        }

        public String getStartAdd()
        {
            return startAdd;
        }

        public String getEndAdd()
        {
            return endAdd;
        }

        public List<String> getAddresses()
        {
            return addresses;
        }

        public void removeFromAddresses(int position)
        {
            addresses.RemoveAt(position);
        }

        public String toString()
        {
            char[] delimiterChars = { ',' };
            String[] src = startAdd.Split(delimiterChars);
            String[] dst = endAdd.Split(delimiterChars);

            return src[0] + " - " + dst[0] + " : " + addresses.Count() + " נקודות ביניים";
        }

        public String getId()
        {
            return id;
        }

    }
}