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
        static TaakRepository taakRepository;

        static void Main(string[] args)
        {
            Console.Write("Voer het database wachtwoord in: ");
            string databaseWachtwoord = Console.ReadLine();

            artiestRepository = new ArtiestRepository(databaseWachtwoord);
            optredenRepository = new OptredenRepository(databaseWachtwoord);
            taakRepository = new TaakRepository(databaseWachtwoord);

            Console.WriteLine();
            Console.WriteLine("Databaseverbinding wordt gecontroleerd.");
            Console.WriteLine();

            try
            {
                ToonArtiesten();
            }
            catch
            {
                Console.WriteLine("Er is iets fout gegaan met de databaseverbinding.");
                Console.WriteLine("Controleer het wachtwoord en probeer het opnieuw.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Database wachtwoord is ingevoerd.");
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
            Console.WriteLine("=== TaakRepository ===");
            Console.WriteLine();

            Console.WriteLine("Taken bekijken per vrijwilliger:");
            ToonTakenVoorVrijwilliger();

            Console.WriteLine();
            Console.WriteLine("Taakdetails bekijken:");
            ToonTaakDetails();

            Console.WriteLine();
            Console.WriteLine("Locaties bekijken:");
            ToonLocaties();

            Console.WriteLine();
            Console.WriteLine("Taak toevoegen:");
            VoegTaakToe();

            Console.WriteLine();
            Console.WriteLine("Taak afmelden:");
            MeldTaakAf();

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

            Console.Write("Weet je zeker dat je deze artiest wilt verwijderen? ja/nee: ");
            string bevestiging = Console.ReadLine();

            //Controleert of de gebruiker echt wil verwijderen
            if (bevestiging.ToLower() == "ja")
            {
                artiestRepository.VerwijderArtiest(artiestId);
                Console.WriteLine("Artiest is verwijderd.");
            }
            else
            {
                Console.WriteLine("Artiest is niet verwijderd.");
            }

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

        static void ToonTakenVoorVrijwilliger()
        {
            Console.Write("Voer het vrijwilliger ID in: ");
            string invoer = Console.ReadLine();

            int vrijwilligerId;

            //Controleert of het ingevulde ID een getal is
            if (!int.TryParse(invoer, out vrijwilligerId))
            {
                Console.WriteLine("Ongeldig vrijwilliger ID ingevuld.");
                return;
            }

            //Haalt de taken op van de gekozen vrijwilliger
            List<Taak> taken = taakRepository.GetTakenVoorVrijwilliger(vrijwilligerId);

            //Controleert of er taken zijn gevonden
            if (taken.Count == 0)
            {
                Console.WriteLine("Er zijn geen taken gevonden voor deze vrijwilliger.");
            }
            else
            {
                //Toont de taken van de vrijwilliger
                foreach (Taak taak in taken)
                {
                    Console.WriteLine(taak.ToonKort());
                    Console.WriteLine();
                }
            }
        }

        static void ToonTaakDetails()
        {
            Console.Write("Voer het taak ID in: ");
            string invoer = Console.ReadLine();

            int taakId;

            //Controleert of het ingevulde ID geldig is
            if (!int.TryParse(invoer, out taakId))
            {
                Console.WriteLine("Ongeldig taak ID ingevuld.");
                return;
            }

            //Haalt de details van de taak op
            Taak taak = taakRepository.GetTaakDetails(taakId);

            //Controleert of de taak bestaat
            if (taak == null)
            {
                Console.WriteLine("Geen taak gevonden met dit ID.");
            }
            else
            {
                Console.WriteLine(taak.ToonDetails());
            }
        }

        static void ToonLocaties()
        {
            Console.WriteLine("=== Locaties bekijken ===");
            Console.WriteLine();

            //Haalt alle locaties op uit de database
            List<Locatie> locaties = taakRepository.GetLocaties();

            //Controleert of er locaties zijn
            if (locaties.Count == 0)
            {
                Console.WriteLine("Er zijn geen locaties gevonden.");
            }
            else
            {
                //Toont alle locaties onder elkaar
                foreach (Locatie locatie in locaties)
                {
                    Console.WriteLine(locatie.ToonLocatie());
                    Console.WriteLine();
                }
            }
        }

        static void VoegTaakToe()
        {
            Console.Write("Vrijwilliger ID: ");
            string vrijwilligerInvoer = Console.ReadLine();

            int vrijwilligerId;

            //Controleert of vrijwilliger ID een getal is
            if (!int.TryParse(vrijwilligerInvoer, out vrijwilligerId))
            {
                Console.WriteLine("Ongeldig vrijwilliger ID ingevuld.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Beschikbare locaties:");
            ToonLocaties();

            Console.Write("Locatie ID: ");
            string locatieInvoer = Console.ReadLine();

            int locatieId;

            //Controleert of locatie ID een getal is
            if (!int.TryParse(locatieInvoer, out locatieId))
            {
                Console.WriteLine("Ongeldig locatie ID ingevuld.");
                return;
            }

            //Zoekt de gekozen locatie op
            Locatie gekozenLocatie = null;
            List<Locatie> locaties = taakRepository.GetLocaties();

            foreach (Locatie locatie in locaties)
            {
                if (locatie.GetLocatieId() == locatieId)
                {
                    gekozenLocatie = locatie;
                }
            }

            if (gekozenLocatie == null)
            {
                Console.WriteLine("Geen locatie gevonden met dit ID.");
                return;
            }

            Console.Write("Taaknaam: ");
            string taakNaam = Console.ReadLine();

            Console.Write("Datum (bijvoorbeeld 2026-07-02): ");
            string datumInvoer = Console.ReadLine();

            DateTime datum;

            //Controleert of de datum geldig is
            if (!DateTime.TryParse(datumInvoer, out datum))
            {
                Console.WriteLine("Ongeldige datum ingevuld.");
                return;
            }

            Console.Write("Begintijd (bijvoorbeeld 09:00): ");
            string beginTijdInvoer = Console.ReadLine();

            TimeSpan beginTijd;

            //Controleert of de begintijd geldig is
            if (!TimeSpan.TryParse(beginTijdInvoer, out beginTijd))
            {
                Console.WriteLine("Ongeldige begintijd ingevuld.");
                return;
            }

            Console.Write("Eindtijd (bijvoorbeeld 10:00): ");
            string eindTijdInvoer = Console.ReadLine();

            TimeSpan eindTijd;

            //Controleert of de eindtijd geldig is
            if (!TimeSpan.TryParse(eindTijdInvoer, out eindTijd))
            {
                Console.WriteLine("Ongeldige eindtijd ingevuld.");
                return;
            }

            Console.Write("Korte omschrijving: ");
            string korteOmschrijving = Console.ReadLine();

            Console.Write("Volledige omschrijving: ");
            string volledigeOmschrijving = Console.ReadLine();

            Console.Write("Extra instructie: ");
            string extraInstructie = Console.ReadLine();

            //Nieuwe taak krijgt standaard status Openstaand
            string status = "Openstaand";

            //Maakt een nieuw Taak object
            Taak taak = new Taak(
                0,
                taakNaam,
                datum,
                beginTijd,
                eindTijd,
                gekozenLocatie,
                korteOmschrijving,
                volledigeOmschrijving,
                extraInstructie,
                status
            );

            //Voegt de taak toe aan de database
            taakRepository.VoegTaakToe(taak, vrijwilligerId);
        }

        static void MeldTaakAf()
        {
            Console.Write("Voer het taak ID in dat je wilt afmelden: ");
            string invoer = Console.ReadLine();

            int taakId;

            //Controleert of het ingevulde ID geldig is
            if (!int.TryParse(invoer, out taakId))
            {
                Console.WriteLine("Ongeldig taak ID ingevuld.");
                return;
            }

            Console.Write("Weet je zeker dat je deze taak wilt afmelden? ja/nee: ");
            string bevestiging = Console.ReadLine();

            //Controleert of de vrijwilliger de taak echt wil afmelden
            if (bevestiging.ToLower() == "ja")
            {
                taakRepository.WijzigStatus(taakId, "Afgemeld");
            }
            else
            {
                Console.WriteLine("Taak is niet afgemeld.");
            }
        }
    }
}