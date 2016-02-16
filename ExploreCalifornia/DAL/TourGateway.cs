using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExploreCalifornia.Models;
using System.Data.Entity;

namespace ExploreCalifornia.DAL
{
    public class TourGateway : DataGateway<Tour>
    {

        public IEnumerable<Tour> SelectAllSortByPrice()
        {
            return data.OrderBy(t => t.Price).ToList();
        }

 
    }
}