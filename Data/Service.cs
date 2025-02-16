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
        public async Task<T?> getDocAsync<T>(string collection, string id)
        {
            var docRef = _firestoreDb.Collection(collection).Document(id);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<T>();
            }

            return default;
        }
        public async Task<List<T>> getAllDocsAsync<T>(string collection)
        {
            var query = _firestoreDb.Collection(collection);
            var snapshot = await query.GetSnapshotAsync();

            List<T> lista = new List<T>();

            foreach (var doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    var obj = doc.ConvertTo<T>();
                    lista.Add(obj);
                }
            }

            return lista;
        }
    }
}
