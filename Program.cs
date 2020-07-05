using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Scrapper
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            // set storage file location
            string StoreFileName = "ShowCast_data.json";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            + "/" + StoreFileName;

            // this boolean is used to control whether all show pages to be collected
            bool b_collectAllPages = true;
            int startPage = 195;
            int endpage;
            int page2get = 10;

            if (b_collectAllPages)
            {
                // if yes, then the RESTclient will start from 0 and end until an 404 response is received
                startPage = 0;
                endpage = 1;
            }
            else
            {
                // other option is to specify a show page range
                endpage = startPage + page2get;
            }


            //Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // create an instance of Pagenation list which contains page id ,a list of shows and their cast info
            List<JsonStructure.Pagenation> LsPage = new List<JsonStructure.Pagenation>();


            for (int pageIndex = startPage; pageIndex < endpage; pageIndex++)
            {
                // create an Restclient instance for getting response from a webapi
                RESTClient rClient = new RESTClient();

                //create an instance single pagenation
                JsonStructure.Pagenation thisPage = new JsonStructure.Pagenation();

                // create an instance a list of show/cast according to the assignment requirement
                List<JsonStructure.ResponseShow> lsShow = new List<JsonStructure.ResponseShow>();

                // compose a url with variable pageindex
                rClient.endPoint = "http://api.tvmaze.com/shows?page=" + pageIndex.ToString();

                //1st step -  get response as json format from the api with this url
                string strJSON = string.Empty;
                strJSON = rClient.makeRequest();
                if (strJSON.Contains("error: (404)"))
                {
                    // exit the show page for loop once receiving not found error HTTP404
                    // this means the previous page was the last page that contains information
                    endpage = pageIndex;
                    break;
                }
                // deserialize json response into a list of shows
                List<JsonStructure.movie> shows = (List<JsonStructure.movie>)DeserializeToList<JsonStructure.movie>(strJSON);

                // 2nd step - get info of each show/movie on the show list
                for (int i = 0; i < shows.Count; i++)
                {
                    // tunned rate limited of api reponse
                    Thread.Sleep(200);
                    // map show id and show name
                    var showid = shows[i].id;
                    var showName = shows[i].name;

                    // construct url for each show with show id as variable
                    rClient.endPoint = "http://api.tvmaze.com/shows/" + showid.ToString() + "/cast";
                    var strJSON_cast = rClient.makeRequest();

                    // deserialized json response into list a cast for this show
                    List<JsonStructure.STcast> lscast = (List<JsonStructure.STcast>)DeserializeToList<JsonStructure.STcast>(strJSON_cast);

                    // sort the cast of this show by birthday descending 
                    var descListOb = lscast.OrderByDescending(x => x.Person.Birthday);
                    var lsCastDec = descListOb.ToList();

                    // create an instance of show/cast
                    JsonStructure.ResponseShow ThisShow = new JsonStructure.ResponseShow();
                    ThisShow.Id = showid;
                    ThisShow.Name = showName;

                    //create an instance of actor list and fill it with required actor info
                    List<JsonStructure.Actor> lsActor = new List<JsonStructure.Actor>();
                    for (int IndexActor = 0; IndexActor < lsCastDec.Count; IndexActor++)
                    {
                        var ActorName = lsCastDec[IndexActor].Person.Name;
                        var ActorBirthday = String.Format("{0:yyyy-MM-dd}", lsCastDec[IndexActor].Person.Birthday);
                        var ActorId = lsCastDec[IndexActor].Person.Id;

                        JsonStructure.Actor actor = new JsonStructure.Actor();
                        actor.Id = ActorId;
                        actor.Name = ActorName;
                        actor.Birthday = ActorBirthday;

                        lsActor.Add(actor);

                    }

                    ThisShow.Cast = lsActor;

                    // add this show with show name,id, list of cast(actor name, id, birthday)
                    lsShow.Add(ThisShow);

                    if (b_collectAllPages)
                    {
                        endpage += 1;
                    }
                }
                // complet page info (page id and list of show)
                thisPage.PageId = pageIndex;
                thisPage.LsShow = lsShow;
                Console.WriteLine("Page " + pageIndex.ToString() + " is done");

                // add this page to page list
                LsPage.Add(thisPage);

            }

            // last step - store all information by as json string in a text file
            string Json2Str = JsonConvert.SerializeObject(LsPage, Formatting.Indented);
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(Json2Str);

                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(Json2Str);
                }
            }

            // Stop timing.
            stopwatch.Stop();

            // Write result.
            Console.WriteLine("Time elapsed for " + (endpage - startPage).ToString() + "  pages: {0}", stopwatch.Elapsed);
            Console.ReadLine();

        }
        private static void Jsonparser()
        {



        }
        
      
        public static List<string> InvalidJsonElements;
        public static IList<T> DeserializeToList<T>(string jsonString)
        {
            InvalidJsonElements = null;
            var array = JArray.Parse(jsonString);
            IList<T> objectsList = new List<T>();
            foreach (var item in array)
            {
                try
                {
                    // CorrectElements
                    objectsList.Add(item.ToObject<T>());
                }
                catch (Exception ex)
                {
                    InvalidJsonElements = InvalidJsonElements ?? new List<string>();
                    InvalidJsonElements.Add(item.ToString());
                }
            }
            return objectsList;
        }
    }
}
