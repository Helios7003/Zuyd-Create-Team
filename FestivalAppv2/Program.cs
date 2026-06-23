using FestivalAppv2.Models;
using System;

namespace FestivalAppv2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Test FestivalAppv2 ===");
            Console.WriteLine();

            //Test objecten aanmaken
            Artiest artiest = new Artiest(1, "Lil Kleine", "rap");


            Podium podium = new Podium(1, "Main stage");

            Optreden optreden = new Optreden(
                1,
                artiest,
                podium,
                new DateTime(2026, 7, 3),
                new TimeSpan(12, 00, 00),
                new TimeSpan(13, 00, 00)

            );

            Locatie locatie = new Locatie(1, "locatie test sndjsbdjns ", "Test23");

            Vrijwilliger vrijwilliger = new Vrijwilliger(1, "Test vrijwilliger", "job@gmail.com");

            Taak taak = new Taak(
                1,
                "Test taak",
                new DateTime(2026, 7, 3),
                new TimeSpan(14, 00, 00),
                new TimeSpan(15, 00, 00),
                locatie,
                "korte omschrijving",
                "volledige omschrijving",
                "extra instructie",
                "Ingepland"
            );

            //Test methodes
            Console.WriteLine("Test Artiest ToonInfo:");
            Console.WriteLine(artiest.ToonInfo());
            Console.WriteLine();

            Console.WriteLine("Test Vrijwilliger ToonInfo:");
            Console.WriteLine(vrijwilliger.ToonInfo());
            Console.WriteLine();

            Console.WriteLine("Test Podium ToonPodium:");
            Console.WriteLine(podium.ToonPodium());
            Console.WriteLine();

            Console.WriteLine("Test Optreden ToonOptreden:");
            Console.WriteLine(optreden.ToonOptreden());
            Console.WriteLine();

            Console.WriteLine("Test Locatie ToonLocatie:");
            Console.WriteLine(locatie.ToonLocatie());
            Console.WriteLine();

            Console.WriteLine("Test Taak ToonDetails:");
            Console.WriteLine(taak.ToonDetails());
            Console.WriteLine();

            Console.WriteLine("Test Taak IsVolledigIngevuld:");
            Console.WriteLine(taak.IsVolledigIngevuld());
            Console.WriteLine();

            Console.WriteLine("Test Taak MeldAf:");
            taak.MeldAf();
            Console.WriteLine(taak.ToonKort());

            Console.ReadKey();
        }
    }
}