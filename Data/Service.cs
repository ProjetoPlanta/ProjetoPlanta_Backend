using Google.Cloud.Firestore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace ProjetoPlanta_Backend.Data
{
    public class FirestoreService
    {
        private FirestoreDb _firestoreDb;

        public static string CriarID(string nome)
        {
            return nome.ToLower()
                       .Replace(" ", "-")
                       .Replace("á", "a")
                       .Replace("é", "e")
                       .Replace("í", "i")
                       .Replace("ó", "o")
                       .Replace("ú", "u")
                       .Replace("ç", "c");

        }

        public FirestoreService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"privatekey.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            _firestoreDb = FirestoreDb.Create("projetoplanta2025-84d0e");
        }
        public async Task AddDocAsync(string coll, string id, object data)
        {
            var docref = _firestoreDb.Collection(coll).Document(id);
            await docref.SetAsync(data);
        }
        public async Task getDocAsync(string coll, string id)
        {
            var docref = _firestoreDb.Collection(coll).Document(id);
            var snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                var doc = snap.ToDictionary();

            }
        }
    }
}
