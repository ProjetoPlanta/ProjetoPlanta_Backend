using System.Runtime.CompilerServices;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoPlanta_Backend.Controllers
{

    public class PlantaControllerr : Controller
    {
        public static async void getPlanta()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"privatekey.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb db = FirestoreDb.Create("projetoplanta2025-84d0e");

            DocumentReference docref = db.Collection("Plantas").Document("1");
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                Dictionary<string, object> planta = snap.ToDictionary();
                foreach (var item in planta)
                {
                    Console.WriteLine("{0}: {1}\n", item.Key, item.Value);
                }
            }
            Thread.Sleep(2000);
        }
        public static async void createPlanta()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"privatekey.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb db = FirestoreDb.Create("projetoplanta2025-84d0e");

            CollectionReference coll = db.Collection("Plantas");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                { "nome", "coco" },
                { "tipo", "planta" }

            };
            coll.AddAsync(data1);
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
