using Google.Cloud.Firestore;
using ProjetoPlanta_Backend.Controllers;

internal class Program
{
    private static void Main(string[] args)
    {
        PlantaControllerr.getPlanta();
        PlantaControllerr.createPlanta();
        Thread.Sleep(5000);
    }
}