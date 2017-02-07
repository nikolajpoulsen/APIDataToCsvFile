using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace APITest
{
    class Program
    {

        static void Main(string[] args)
        {
            Token token = getToken();
            Console.WriteLine(token.access_token);
            List<Boxlocations> bl = getBoxLocation(token);
            Console.WriteLine(bl.First().name);
        }

        static List<Boxlocations> getBoxLocation(Token token)
        {
            string url = "https://app.ic-meter.com/icm/api/boxlocations/1.0/list?access_token=" + token.access_token;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();

                        List<Boxlocations> obj = JsonConvert.DeserializeObject<List<Boxlocations>>(response);
                        return obj;
                    }
                }
            }
            return null;
        }

        static Token getToken()
        {
            string tokenUrl = "https://app.ic-meter.com/icm/oauth/token?client_id=trusted-client&grant_type=password&scope=trust&username=demo@ic-meter.com&password=demo";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tokenUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();                    
                        Token obj = JsonConvert.DeserializeObject<Token>(response);
                        return obj;                        
                    }
                }
            }
            return null;
        }
    }
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
    }

    public class Boxlocations
    {
        public string boxQR { get; set; }
        public string name { get; set; }
        public string room { get; set; }
        public double ceilingHeight { get; set; }
        public double floorArea { get; set; }
        public string timezone { get; set; }
        public string fromDate { get; set; }
        public string lastMeasurementDate { get; set; }
        public string ownership { get; set; }
    }
}
