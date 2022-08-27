//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using Home.Common;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.IO;
//using System.Text;

//namespace Home.Azure.Functions
//{
//    public static class GenerateAlexaNews
//    {


//        public class NewsItem
//        {
//            public string uid { get; set; }
//            public string updateDate { get; set; }
//            public string titleText { get; set; }
//            public string mainText { get; set; }
//            public string redirectionUrl { get; set; }
//        }

//        [FunctionName("GenerateAlexaNews")]
//        public static void Run([TimerTrigger("0 */30 * * * *"
//#if DEBUG
//            ,RunOnStartup =true
//#endif
//            )] TimerInfo myTimer, ILogger log)
//        {
//            var cfg = System.Environment.GetEnvironmentVariable("AppConfig", EnvironmentVariableTarget.Process);
//            ConfigurationSettingsHelper.Init(cfg);

//            NewsItem item = new NewsItem()
//            {
//                uid = "LESNEWSDEHOMEAUTOMATION",
//                titleText = DateTime.Today.ToString("D"),
//                mainText = "Ceci est un essai !",
//                updateDate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.f") + "Z",
//                redirectionUrl = "https://home.anzin.carbenay.me/"
//            };


//            using (var str = new MemoryStream())
//            {
//                using (var wri = new StreamWriter(str, Encoding.UTF8, -1, true))
//                {
//                    wri.Write(JsonConvert.SerializeObject(new NewsItem[] { item }));
//                }

//                str.Seek(0, SeekOrigin.Begin);

//                string cnStorage = ConfigurationSettingsHelper.GetAzureStorageConnectionString();

//                var container = new BlobContainerClient(cnStorage, "alexa");
//                container.CreateIfNotExists();
//                container.SetAccessPolicy(PublicAccessType.BlobContainer);
//                var blob = container.GetBlobClient("news/homebrief.json");
                
//                var file = blob.Upload(str, new BlobHttpHeaders()
//                {
//                    ContentType = "application/json"
//                }, conditions: null);
                
//            }
//        }
//    }
//}
