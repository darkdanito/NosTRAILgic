﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Xml;
using HtmlAgilityPack;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of TrailMeetup View            *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 14/03/2016                                                                 *
     ************************************************************************************/
    [Authorize]
    public class UploadController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();
        private Location location;

        // GET: Upload/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Upload/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase MuseumFile, HttpPostedFileBase MonumentFile, HttpPostedFileBase HistoricSiteFile)
        {
            Boolean skipLocation = false;

            try
            {
                //Check If MuseumFile and Etc is not emptyy before continue
                if (MuseumFile != null)
                {
                    System.Diagnostics.Debug.WriteLine("Upload Musuem Files");

                    string path = Server.MapPath("~/Content/KML/" + MuseumFile.FileName);
                    MuseumFile.SaveAs(path);
                    //System.Diagnostics.Debug.WriteLine(path);

                    //Delete all previous musuem
                    var deleteLocation = db.Locations.Where(location => location.Category == "Musuem");
                    db.Locations.RemoveRange(deleteLocation);
                    db.SaveChanges();

                    //System.Diagnostics.Debug.WriteLine("Delete DB");

                    // TODO: Add insert logic here
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(path);
                    XmlNodeList nameList = xmlDoc.GetElementsByTagName("Placemark");
                    XmlNodeList descList = xmlDoc.GetElementsByTagName("description");
                    XmlNodeList corrdinateList = xmlDoc.GetElementsByTagName("coordinates");
                    //for (int placeIndex = 0; placeIndex < 1; placeIndex++)
                    for (int placeIndex = 0; placeIndex < nameList.Count; placeIndex++)
                    {
                        location = new Location();
                        location.Name = nameList[placeIndex].ChildNodes[0].InnerText;
                        string[] corrdinate = corrdinateList[placeIndex].InnerText.Split(',');
                        location.Longitude = float.Parse(corrdinate[0]);
                        location.Latitude = float.Parse(corrdinate[1]); 

                         HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(descList[placeIndex].InnerText);
                        HtmlNode[] nodes = doc.DocumentNode.SelectNodes("//td").ToArray();
                        for (int descIndex = 0; descIndex < nodes.Length; descIndex++)
                        {
                            if (nodes[descIndex].InnerHtml.Equals("ADDRESSPOSTALCODE"))
                            {
                                string postalCode = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                                if (HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml).Equals("<Null>"))
                                {
                                    //location.PostalCode = 0; // AreaCode and PostalCode = 0 if Null 
                                    skipLocation = true; //Skip
                                }
                                else
                                {
                                    location.PostalCode = Int32.Parse(postalCode);
                                    location.AreaCode = location.PostalCode / 10000; //get the first 2 digit
                                }

                            }
                            else if (nodes[descIndex].InnerHtml.Equals("DESCRIPTION"))
                            {
                                location.Description = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("HYPERLINK"))
                            {
                                var doc2 = new HtmlDocument();
                                doc2.LoadHtml(nodes[descIndex + 1].InnerHtml);
                                var anchor = doc.DocumentNode.SelectSingleNode("//a");
                                var href = anchor.GetAttributeValue("href", null);
                                location.HyperLink = href.ToString();
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("PHOTOURL"))
                            {
                                var doc2 = new HtmlDocument();
                                doc2.LoadHtml(nodes[descIndex + 1].InnerHtml);
                                var anchor = doc.DocumentNode.SelectSingleNode("//a");
                                var href = anchor.GetAttributeValue("href", null);
                                location.ImageLink = href.ToString();
                            }
                        }

                        location.Category = "Musuem";

                        //Test Display
                        //System.Diagnostics.Debug.WriteLine(location.Name);
                        //System.Diagnostics.Debug.WriteLine(location.Description);
                        //System.Diagnostics.Debug.WriteLine(location.HyperLink);
                        //System.Diagnostics.Debug.WriteLine(location.ImageLink);
                        //System.Diagnostics.Debug.WriteLine(location.AreaCode.ToString());
                        //System.Diagnostics.Debug.WriteLine(location.PostalCode.ToString());
                        //System.Diagnostics.Debug.WriteLine(location.Latitude.ToString());
                        //System.Diagnostics.Debug.WriteLine(location.Longitude.ToString());
                        //System.Diagnostics.Debug.WriteLine(location.Category);
                        if (!skipLocation) { 
                            db.Locations.Add(location);
                            db.SaveChanges();
                        }
                        else
                        {
                            skipLocation = false;
                        }
                    }                    
                }

                //Check If MonumentFile and Etc is not emptyy before continue
                if (MonumentFile != null)
                {
                    System.Diagnostics.Debug.WriteLine("Upload Monument Files");

                    string path = Server.MapPath("~/Content/KML/" + MonumentFile.FileName);
                    MonumentFile.SaveAs(path);
                    //System.Diagnostics.Debug.WriteLine(path);

                    //Delete all previous Monument
                    var deleteLocation = db.Locations.Where(location => location.Category == "Monument");
                    db.Locations.RemoveRange(deleteLocation);
                    db.SaveChanges();

                    // TODO: Add insert logic here
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(path);
                    XmlNodeList nameList = xmlDoc.GetElementsByTagName("Placemark");
                    XmlNodeList descList = xmlDoc.GetElementsByTagName("description");
                    XmlNodeList corrdinateList = xmlDoc.GetElementsByTagName("coordinates");
                    //for (int placeIndex = 0; placeIndex < 1; placeIndex++)
                    for (int placeIndex = 0; placeIndex < nameList.Count; placeIndex++)
                    {
                        location = new Location();
                        location.Name = nameList[placeIndex].ChildNodes[0].InnerText;
                        string[] corrdinate = corrdinateList[placeIndex].InnerText.Split(',');
                        location.Longitude = float.Parse(corrdinate[0]);
                        location.Latitude = float.Parse(corrdinate[1]);

                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(descList[placeIndex].InnerText);
                        HtmlNode[] nodes = doc.DocumentNode.SelectNodes("//td").ToArray();
                        for (int descIndex = 0; descIndex < nodes.Length; descIndex++)
                        {
                            if (nodes[descIndex].InnerHtml.Equals("ADDRESSPOSTALCODE"))
                            {
                                string postalCode = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                                if (HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml).Equals("<Null>"))
                                {
                                    //location.PostalCode = 0; // AreaCode and PostalCode = 0 if Null 
                                    skipLocation = true; //Skip
                                }
                                else
                                {
                                    location.PostalCode = Int32.Parse(postalCode);
                                    location.AreaCode = location.PostalCode / 10000; //get the first 2 digit
                                }
                                
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("DESCRIPTION"))
                            {
                                location.Description = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("PHOTOURL"))
                            {
                                var doc2 = new HtmlDocument();
                                doc2.LoadHtml(nodes[descIndex + 1].InnerHtml);
                                var anchor = doc.DocumentNode.SelectSingleNode("//a");
                                var href = anchor.GetAttributeValue("href", null);
                                location.ImageLink = href.ToString();
                            }
                        }

                        location.HyperLink = "<Null>";
                        location.Category = "Monument";

                        if (!skipLocation)
                        {
                            db.Locations.Add(location);
                            db.SaveChanges();
                        }
                        else
                        {
                            skipLocation = false;
                        }
                    }
                }

                //Check If MuseumFile and Etc is not emptyy before continue
                if (HistoricSiteFile != null)
                {
                    System.Diagnostics.Debug.WriteLine("Upload HistoricSite Files");

                    string path = Server.MapPath("~/Content/KML/" + HistoricSiteFile.FileName);
                    HistoricSiteFile.SaveAs(path);
                    //System.Diagnostics.Debug.WriteLine(path);

                    //Delete all previous musuem
                    var deleteLocation = db.Locations.Where(location => location.Category == "HistoricSite");
                    db.Locations.RemoveRange(deleteLocation);
                    db.SaveChanges();

                    //System.Diagnostics.Debug.WriteLine("Delete DB");

                    // TODO: Add insert logic here
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(path);
                    XmlNodeList nameList = xmlDoc.GetElementsByTagName("Placemark");
                    XmlNodeList descList = xmlDoc.GetElementsByTagName("description");
                    XmlNodeList corrdinateList = xmlDoc.GetElementsByTagName("coordinates");
                    //for (int placeIndex = 0; placeIndex < 1; placeIndex++)
                    for (int placeIndex = 0; placeIndex < nameList.Count; placeIndex++)
                    {
                        location = new Location();
                        location.Name = nameList[placeIndex].ChildNodes[0].InnerText;
                        string[] corrdinate = corrdinateList[placeIndex].InnerText.Split(',');
                        location.Longitude = float.Parse(corrdinate[0]);
                        location.Latitude = float.Parse(corrdinate[1]);

                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(descList[placeIndex].InnerText);
                        HtmlNode[] nodes = doc.DocumentNode.SelectNodes("//td").ToArray();
                        for (int descIndex = 0; descIndex < nodes.Length; descIndex++)
                        {
                            if (nodes[descIndex].InnerHtml.Equals("ADDRESSPOSTALCODE"))
                            {
                                string postalCode = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                                if (HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml).Equals("<Null>"))
                                {
                                    //location.PostalCode = 0; // AreaCode and PostalCode = 0 if Null
                                    skipLocation= true; // Skip 
                                }
                                else
                                {
                                    location.PostalCode = Int32.Parse(postalCode);
                                    location.AreaCode = location.PostalCode / 10000; //get the first 2 digit
                                }
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("DESCRIPTION"))
                            {
                                location.Description = HttpUtility.HtmlDecode(nodes[descIndex + 1].InnerHtml);
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("HYPERLINK"))
                            {
                                var doc2 = new HtmlDocument();
                                doc2.LoadHtml(nodes[descIndex + 1].InnerHtml);
                                var anchor = doc.DocumentNode.SelectSingleNode("//a");
                                var href = anchor.GetAttributeValue("href", null);
                                location.HyperLink = href.ToString();
                            }
                            else if (nodes[descIndex].InnerHtml.Equals("PHOTOURL"))
                            {
                                var doc2 = new HtmlDocument();
                                doc2.LoadHtml(nodes[descIndex + 1].InnerHtml);
                                var anchor = doc.DocumentNode.SelectSingleNode("//a");
                                var href = anchor.GetAttributeValue("href", null);
                                location.ImageLink = href.ToString();
                            }
                        }

                        location.Category = "HistoricSite";

                        if (!skipLocation)
                        {
                            db.Locations.Add(location);
                            db.SaveChanges();
                        }
                        else
                        {
                            skipLocation = false;
                        }
                    }
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
