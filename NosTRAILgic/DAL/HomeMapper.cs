using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NosTRAILgic.DAL
{
    public class HomeMapper
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        public IQueryable<string> getSearchAutoComplete(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;

            return result;
        }
    }
}