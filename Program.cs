using FestivalAppv2.Data;
using FestivalAppv2.Models;
using System;
using System.Collections.Generic;

namespace FestivalAppv2
{
    internal class Program
    {
        static ArtiestRepository artiestRepository = new ArtiestRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("=== ArtiestRepository ===");
            Console.WriteLine();

            ToonArtiesten();

            Console.WriteLine();
            Console.WriteLine("Artiest toevoegen:");
            VoegArtiestToe();

            Console.WriteLine();
            Console.WriteLine("Artiest wijzigen:");
            WijzigArtiest();

            Console.WriteLine();
            Console.WriteLine("Artiest verwijderen:");
            VerwijderArtiest();

            Console.WriteLine();
            Console.WriteLine("Druk op een toets om af te sluiten.");
            Console.ReadKey();
        }

        static void ToonArtiesten()
        {
            Console.WriteLine("=== Artiesten bekijken ===");
            Console.WriteLine();

            //Haalt alle artiesten op uit de database
            List<Artiest> artiesten = artiestRepository.GetAlleArtiesten();

            //Controleert of er artiesten zijn
            if (artiesten.Count == 0)
            {
                Console.WriteLine("Er zijn geen artiesten gevonden.");
            }
            else
            {
                //Toont alle artiesten
                foreach (Artiest artiest in artiesten)
                {
                    Console.WriteLine(artiest.ToonInfo());
                    Console.WriteLine();
                }
            }
        }

        static void VoegArtiestToe()
        {
            Console.Write("Naam van de artiest: ");
            string naam = Console.ReadLine();

            Console.Write("Genre van de artiest: ");
            string genre = Console.ReadLine();

            //Maakt een nieuw artiest object
            Artiest artiest = new Artiest(0, naam, genre);

            //Voegt de artiest toe aan de database
            artiestRepository.VoegArtiestToe(artiest);

            Console.WriteLine("Artiest is toegevoegd.");
            Console.WriteLine();

            ToonArtiesten();
        }

        static void WijzigArtiest()
        {
            ToonArtiesten();

Console.Write("Welke artiest ID wil je wijzigen: ");
string invoer = Console.ReadLine();

int artiestId;

if (!int.TryParse(invoer, out artiestId))
{
    Console.WriteLine("Ongeldig ID ingevuld.");
    return;
}

            Console.Write("Nieuwe naam: ");
            string naam = Console.ReadLine();

            Console.Write("Nieuw genre: ");
            string genre = Console.ReadLine();

            //Maakt een artiest object met de nieuwe gegevens
            Artiest artiest = new Artiest(artiestId, naam, genre);

            //Wijzigt de artiest in de database
            artiestRepository.WijzigArtiest(artiest);

            Console.WriteLine("Artiest is gewijzigd.");
            Console.WriteLine();

            ToonArtiesten();
        }

        static void VerwijderArtiest()
        {
            ToonArtiesten();

            Console.Write("Welke artiest ID wil je verwijderen: ");
            int artiestId = Convert.ToInt32(Console.ReadLine());

            //Verwijdert de artiest uit de database
            artiestRepository.VerwijderArtiest(artiestId);

            Console.WriteLine("Artiest is verwijderd.");
            Console.WriteLine();

            ToonArtiesten();
        }
    }
}