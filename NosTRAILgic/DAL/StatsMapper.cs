
using NosTRAILgic.Models;
using NosTRAILgic.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NosTRAILgic.DAL
{
    public class StatsMapper : DataGateway<Statistic>
    {
        public List<Statistic> GetStatsByTopSearchLocation(string valMonth)
        {
            var statsByTopSearchLocation = db.Database.SqlQuery<Statistic>("select top 5 Name, count(*) as No, l.ImageLink as Image from Searches s left join Locations l on s.Keyword = l.Name where MONTH(CONVERT(Date, DATE)) = '" + valMonth + "' group by Name, ImageLink ORDER BY No DESC").ToList();
            return statsByTopSearchLocation;
        }

        public List<Statistic> GetStatsByTopTrailContributor()
        {
            var statsByTopTrailContributor = db.Database.SqlQuery<Statistic>("select top 5 t.CreatorID as Name,count(j.TrailMeetupID) as Number from JoinTrails j right join TrailMeetups t on t.TrailMeetupID = j.TrailMeetupID GROUP BY t.CreatorID ORDER BY Number DESC").ToList();
            return statsByTopTrailContributor;
        }

        public List<Statistic> GetSearchLocationDay(string searchKeyword)
        {
            var searchLocationDay = db.Database.SqlQuery<Statistic>("select Name, count(DAY(CONVERT(Date, DATE))) as Number, DAY(CONVERT(Date, DATE)) as Date from CheckIns c left join Locations l on l.Name = c.LocationName where l.Name = '" + searchKeyword + "' group by l.Name, DAY(CONVERT(Date, DATE))").ToList();
            if (searchKeyword == "" || searchKeyword == null)
            {
                searchLocationDay = db.Database.SqlQuery<Statistic>("select Name, count(DAY(CONVERT(Date, DATE))) as Number, DAY(CONVERT(Date, DATE)) as Date from CheckIns c left join Locations l on l.Name = c.LocationName where l.Name = '' group by l.Name, DAY(CONVERT(Date, DATE))").ToList();
            }
            else
            {
                searchLocationDay = db.Database.SqlQuery<Statistic>("select Name, count(DAY(CONVERT(Date, DATE))) as Number, DAY(CONVERT(Date, DATE)) as Date from CheckIns c left join Locations l on l.Name = c.LocationName where l.Name = '" + searchKeyword + "' group by l.Name, DAY(CONVERT(Date, DATE))").ToList();
            }
            return searchLocationDay;
        }

        public List<Statistic> GetStatsByCheckInCurrentYear(string category)
        {
            var statsByCheckInCurrentYear = db.Database.SqlQuery<Statistic>("select Category as Name, YEAR(CONVERT(Date, DATE)) as Date, count(*) as Number from CheckIns c right join Locations l on l.Name = c.LocationName where Category = '" + category + "' and YEAR(CONVERT(Date, DATE)) = YEAR(CONVERT(Date, GETDATE())) group by Category, YEAR(CONVERT(Date, DATE))").ToList();
            return statsByCheckInCurrentYear;
        }

        public List<Statistic> GetStatsByCheckInPreviousYear(string category)
        {
            var statsByCheckInPreviousYear = db.Database.SqlQuery<Statistic>("select Category as Name, YEAR(CONVERT(Date, DATE)) as Date, count(*) as Number from CheckIns c right join Locations l on l.Name = c.LocationName where Category = '" + category + "' and YEAR(CONVERT(Date, DATE)) = YEAR(CONVERT(Date, GETDATE()))-1 group by Category, YEAR(CONVERT(Date, DATE))").ToList();
            return statsByCheckInPreviousYear;
        }

        public IQueryable<string> GetAutoCompleteResult(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;
            return result;
        }

    }
}