using FestivalAppv2.Data;
using FestivalAppv2.Models;
using System;
using System.Collections.Generic;

namespace FestivalAppv2
{
    internal class Program
    {
        static ArtiestRepository artiestRepository;
        static OptredenRepository optredenRepository;

        static void Main(string[] args)
        {
            Console.Write("Voer het database wachtwoord in: ");
            string databaseWachtwoord = Console.ReadLine();

            artiestRepository = new ArtiestRepository(databaseWachtwoord);
            optredenRepository = new OptredenRepository(databaseWachtwoord);

            Console.WriteLine();
            Console.WriteLine("Database wachtwoord is ingesteld.");
            Console.WriteLine();
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
            Console.WriteLine("=== OptredenRepository ===");
            Console.WriteLine();

            Console.WriteLine("Programma bekijken:");
            ToonProgramma();

            Console.WriteLine();
            Console.WriteLine("Zoeken op artiest:");
            ZoekOpArtiest();

            Console.WriteLine();
            Console.WriteLine("Filteren op podium:");
            FilterOpPodium();

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

            //Controleert of het ingevulde ID wel een getal is
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
            string invoer = Console.ReadLine();

            int artiestId;

            //Controleert of het ingevulde ID wel een getal is
            if (!int.TryParse(invoer, out artiestId))
            {
                Console.WriteLine("Ongeldig ID ingevuld.");
                return;
            }

            //Verwijdert de artiest uit de database
            artiestRepository.VerwijderArtiest(artiestId);

            Console.WriteLine("Artiest is verwijderd.");
            Console.WriteLine();

            ToonArtiesten();
        }

        static void ToonProgramma()
        {
            Console.WriteLine("=== Programma bekijken ===");
            Console.WriteLine();

            //Haalt alle optredens op uit de database
            List<Optreden> optredens = optredenRepository.GetAlleOptredens();

            //Controleert of er optredens zijn gevonden
            if (optredens.Count == 0)
            {
                Console.WriteLine("Er zijn geen optredens gevonden.");
            }
            else
            {
                //Laat alle optredens onder elkaar zien
                foreach (Optreden optreden in optredens)
                {
                    Console.WriteLine(optreden.ToonOptreden());
                    Console.WriteLine();
                }
            }
        }

        static void ZoekOpArtiest()
        {
            Console.Write("Voer een artiestnaam in: ");
            string zoekterm = Console.ReadLine();

            //Zoekt optredens met de ingevulde artiestnaam
            List<Optreden> optredens = optredenRepository.ZoekOpArtiest(zoekterm);

            //Controleert of de zoekactie iets heeft gevonden
            if (optredens.Count == 0)
            {
                Console.WriteLine("Geen optredens gevonden voor deze artiest.");
            }
            else
            {
                //Toont de gevonden optredens
                foreach (Optreden optreden in optredens)
                {
                    Console.WriteLine(optreden.ToonOptreden());
                    Console.WriteLine();
                }
            }
        }

        static void FilterOpPodium()
        {
            Console.Write("Voer een podiumnaam in: ");
            string podiumnaam = Console.ReadLine();

            //Haalt alleen optredens op van het ingevulde podium
            List<Optreden> optredens = optredenRepository.FilterOpPodium(podiumnaam);

            //Controleert of er optredens zijn voor dit podium
            if (optredens.Count == 0)
            {
                Console.WriteLine("Geen optredens gevonden voor dit podium.");
            }
            else
            {
                //Toont de optredens van het gekozen podium
                foreach (Optreden optreden in optredens)
                {
                    Console.WriteLine(optreden.ToonOptreden());
                    Console.WriteLine();
                }
            }
        }
    }
}