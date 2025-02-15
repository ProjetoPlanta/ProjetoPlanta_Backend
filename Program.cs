using Google.Cloud.Firestore;
//aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

string path = AppDomain.CurrentDomain.BaseDirectory + @"privatekey.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
FirestoreDb db = FirestoreDb.Create("projetoplanta2025-84d0e");

async void getPlanta()
{
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
}
getPlanta();
Console.WriteLine("deu certo");
app.Run();

