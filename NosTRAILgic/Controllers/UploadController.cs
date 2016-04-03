using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Xml;
using HtmlAgilityPack;
using System.Net;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of Upload of KML into the      *
     *              location database                                                   *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 14/03/2016                                                                 *
     ************************************************************************************/
    [Authorize(Roles = "Admin")]
    public class UploadController : GeneralController<Location>
    {
        private Location location;
        UploadGateway uploadGateway = new UploadGateway();

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
                    System.Diagnostics.Debug.WriteLine("Upload Museum Files");

                    string path = Server.MapPath("~/Content/KML/" + MuseumFile.FileName);
                    MuseumFile.SaveAs(path);
                    //System.Diagnostics.Debug.WriteLine(path);

                    //Delete all previous Museum
                    uploadGateway.removePreviousMuseum();

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

                        location.Category = "Museum";

                        if (!skipLocation) {
                            uploadGateway.Insert(location);
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
                    uploadGateway.removePreviousMonument();

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
                            uploadGateway.Insert(location);
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

                    //Delete all previous HistoricSite
                    uploadGateway.removePreviousHistoricSite();

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
                            uploadGateway.Insert(location);
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
