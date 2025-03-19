using Google.Cloud.Firestore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using ProjetoPlanta_Backend.Models;

namespace ProjetoPlanta_Backend.Data
{
    public class FirestoreService
    {
        private FirestoreDb _firestoreDb;

        public async Task<T?> getDocByNomeAsync<T>(string collection, string nomePopular)
        {
            var query = _firestoreDb.Collection(collection)
                                    .WhereEqualTo("nomePopular", nomePopular);
            var snapshot = await query.GetSnapshotAsync();
            if (snapshot.Documents.Count > 0)
            {
                // Retorna o primeiro documento encontrado
                return snapshot.Documents[0].ConvertTo<T>();
            }
            return default;
        }

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
        public async Task<string> AddDocAsync(string collection, object data)
        {
            var collectionRef = _firestoreDb.Collection(collection);
            var docRef = await collectionRef.AddAsync(data);
            return docRef.Id;
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

        public async Task<List<T>> GetDocsByEmailAsync<T>(string collection, string email)
        {
            var query = _firestoreDb.Collection(collection)
                                    .WhereEqualTo("emailUsuario", email);

            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
            {
                return new List<T>(); // Retorna lista vazia se não encontrar nada
            }

            return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
        }

        public async Task<List<T>> GetDocsByTelefoneAsync<T>(string collection, string telefone)
        {
            var query = _firestoreDb.Collection(collection)
                                    .WhereEqualTo("telefoneUsuario", telefone);

            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
            {
                return new List<T>(); // Retorna lista vazia se não encontrar nada
            }

            return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
        }

        public async Task<List<T>> getAllDocsAsync<T>(string collection) where T : new()
        {
            var query = _firestoreDb.Collection(collection);
            var snapshot = await query.GetSnapshotAsync();

            List<T> lista = new List<T>();

            foreach (var doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    T obj = doc.ConvertTo<T>();

                    if (obj is Planta planta)
                    {
                        planta.id = doc.Id;
                    }

                    lista.Add(obj);
                }
            }

            return lista;
        }
        public async Task updateDocAsync(string collection, string id, object data)
        {
            var docRef = _firestoreDb.Collection(collection).Document(id);
            await docRef.SetAsync(data, SetOptions.MergeAll);
        }

        public async Task deleteDocAsync(string collection, string id)
        {
            var docRef = _firestoreDb.Collection(collection).Document(id);
            await docRef.DeleteAsync();
        }
    }


}
