using FestivalAppv2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace FestivalAppv2.Data
{
    public class TaakRepository
    {
        //Connection string wordt gevuld met het wachtwoord uit Program.cs
        private string connectionString;

        //Maakt de connection string met het ingevoerde wachtwoord
        public TaakRepository(string databaseWachtwoord)
        {
            connectionString = @"Data Source=vallisnexusdatabeseserver.database.windows.net;Initial Catalog=vallisnexus_database;User ID=vallisnexus_app ;Password="
                + databaseWachtwoord +
                @";Encrypt=True;TrustServerCertificate=True;";
        }

        //Haalt de taken op die bij een vrijwilliger horen
        public List<Taak> GetTakenVoorVrijwilliger(int vrijwilligerId)
        {
            //Lijst voor de taken van deze vrijwilliger
            List<Taak> taken = new List<Taak>();

            //Databaseverbinding maken
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query haalt taken op met de locatie erbij
                string query = @"
                SELECT
                    t.taakId,
                    t.taakNaam,
                    t.datum,
                    t.beginTijd,
                    t.eindTijd,
                    t.korteOmschrijving,
                    t.volledigeOmschrijving,
                    t.extraInstructie,
                    t.status,
                    l.locatieId,
                    l.locatieNaam,
                    l.locatieOmschrijving
                FROM TAAK t
                INNER JOIN LOCATIE l ON t.locatieId = l.locatieId
                WHERE t.vrijwilligerId = @vrijwilligerId
                ORDER BY t.datum, t.beginTijd";

                //Query klaarzetten
                SqlCommand command = new SqlCommand(query, connection);

                //VrijwilligerId wordt meegegeven aan de query
                command.Parameters.AddWithValue("@vrijwilligerId", vrijwilligerId);

                //Resultaten uit de database lezen
                SqlDataReader reader = command.ExecuteReader();

                //Elke rij wordt omgezet naar een Taak object
                while (reader.Read())
                {
                    Taak taak = LeesTaak(reader);

                    //Taak toevoegen aan de lijst
                    taken.Add(taak);
                }
            }

            return taken;
        }

        //Haalt de volledige details van één taak op
        public Taak GetTaakDetails(int taakId)
        {
            //Taak is eerst leeg totdat er iets gevonden wordt
            Taak taak = null;

            //Verbinding openen met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query haalt één taak op met alle details
                string query = @"
                SELECT
                    t.taakId,
                    t.taakNaam,
                    t.datum,
                    t.beginTijd,
                    t.eindTijd,
                    t.korteOmschrijving,
                    t.volledigeOmschrijving,
                    t.extraInstructie,
                    t.status,
                    l.locatieId,
                    l.locatieNaam,
                    l.locatieOmschrijving
                FROM TAAK t
                INNER JOIN LOCATIE l ON t.locatieId = l.locatieId
                WHERE t.taakId = @taakId";

                //Command maken voor deze taak
                SqlCommand command = new SqlCommand(query, connection);

                //TaakId meegeven zodat alleen deze taak wordt opgehaald
                command.Parameters.AddWithValue("@taakId", taakId);

                //Resultaat van de query lezen
                SqlDataReader reader = command.ExecuteReader();

                //Als de taak bestaat, wordt er een Taak object gemaakt
                if (reader.Read())
                {
                    taak = LeesTaak(reader);
                }
            }

            return taak;
        }

        //Voegt een nieuwe taak toe aan de database
        public void VoegTaakToe(Taak taak, int vrijwilligerId)
        {
            //Controleert of de verplichte gegevens zijn ingevuld
            if (!taak.IsVolledigIngevuld())
            {
                Console.WriteLine("Niet alle verplichte taakgegevens zijn ingevuld.");
                return;
            }

            //De locatie komt uit het Taak object
            int locatieId = taak.GetLocatie().GetLocatieId();

            //Databaseverbinding starten
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query voegt een taak toe
                string query = @"
                INSERT INTO TAAK
                (
                    vrijwilligerId,
                    locatieId,
                    taakNaam,
                    datum,
                    beginTijd,
                    eindTijd,
                    korteOmschrijving,
                    volledigeOmschrijving,
                    extraInstructie,
                    status
                )
                VALUES
                (
                    @vrijwilligerId,
                    @locatieId,
                    @taakNaam,
                    @datum,
                    @beginTijd,
                    @eindTijd,
                    @korteOmschrijving,
                    @volledigeOmschrijving,
                    @extraInstructie,
                    @status
                )";

                //Command klaarzetten om de taak op te slaan
                SqlCommand command = new SqlCommand(query, connection);

                //Waardes van de taak worden meegegeven aan de query
                command.Parameters.AddWithValue("@vrijwilligerId", vrijwilligerId);
                command.Parameters.AddWithValue("@locatieId", locatieId);
                command.Parameters.AddWithValue("@taakNaam", taak.GetTaakNaam());
                command.Parameters.AddWithValue("@datum", taak.GetDatum());
                command.Parameters.AddWithValue("@beginTijd", taak.GetBeginTijd());
                command.Parameters.AddWithValue("@eindTijd", taak.GetEindTijd());
                command.Parameters.AddWithValue("@korteOmschrijving", taak.GetKorteOmschrijving());
                command.Parameters.AddWithValue("@volledigeOmschrijving", taak.GetVolledigeOmschrijving());
                command.Parameters.AddWithValue("@extraInstructie", taak.GetExtraInstructie());
                command.Parameters.AddWithValue("@status", taak.GetStatus());

                //Taak wordt toegevoegd aan de database
                command.ExecuteNonQuery();

                Console.WriteLine("Taak is toegevoegd.");
            }
        }

        //Wijzigt de status van een taak
        public void WijzigStatus(int taakId, string status)
        {
            //Verbinding maken met SQL Server
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query past de status van de taak aan
                string query = @"
                UPDATE TAAK
                SET status = @status,
                    afgemeldOp = @afgemeldOp
                WHERE taakId = @taakId";

                //Query klaarzetten voor de statuswijziging
                SqlCommand command = new SqlCommand(query, connection);

                //TaakId en nieuwe status worden meegegeven
                command.Parameters.AddWithValue("@taakId", taakId);
                command.Parameters.AddWithValue("@status", status);

                //Als de taak afgemeld wordt, wordt de tijd opgeslagen
                if (status == "Afgemeld")
                {
                    command.Parameters.AddWithValue("@afgemeldOp", DateTime.Now);
                }
                else
                {
                    command.Parameters.AddWithValue("@afgemeldOp", DBNull.Value);
                }

                //Controleren of er echt een taak is aangepast
                int aantalRijenAangepast = command.ExecuteNonQuery();

                if (aantalRijenAangepast == 0)
                {
                    Console.WriteLine("Geen taak gevonden met dit ID.");
                }
                else
                {
                    Console.WriteLine("Taakstatus is aangepast naar: " + status);
                }
            }
        }

        //Haalt alle locaties op uit de database
        public List<Locatie> GetLocaties()
        {
            //Hier komen de locaties in te staan
            List<Locatie> locaties = new List<Locatie>();

            //Verbinding openen met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query haalt alle locaties op
                string query = @"
                SELECT
                    locatieId,
                    locatieNaam,
                    locatieOmschrijving
                FROM LOCATIE
                ORDER BY locatieNaam";

                //Command maken voor de locatie query
                SqlCommand command = new SqlCommand(query, connection);

                //Locatiegegevens lezen
                SqlDataReader reader = command.ExecuteReader();

                //Iedere rij wordt een Locatie object
                while (reader.Read())
                {
                    int locatieId = Convert.ToInt32(reader["locatieId"]);
                    string locatieNaam = reader["locatieNaam"].ToString();
                    string locatieOmschrijving = reader["locatieOmschrijving"].ToString();

                    Locatie locatie = new Locatie(locatieId, locatieNaam, locatieOmschrijving);

                    //Locatie toevoegen aan de lijst
                    locaties.Add(locatie);
                }
            }

            return locaties;
        }

        //Zet een database rij om naar een Taak object
        private Taak LeesTaak(SqlDataReader reader)
        {
            //Locatiegegevens uit de database halen
            int locatieId = Convert.ToInt32(reader["locatieId"]);
            string locatieNaam = reader["locatieNaam"].ToString();
            string locatieOmschrijving = reader["locatieOmschrijving"].ToString();

            //Locatie object maken
            Locatie locatie = new Locatie(locatieId, locatieNaam, locatieOmschrijving);

            //Taakgegevens uit de database halen
            int taakId = Convert.ToInt32(reader["taakId"]);
            string taakNaam = reader["taakNaam"].ToString();
            DateTime datum = Convert.ToDateTime(reader["datum"]);
            TimeSpan beginTijd = (TimeSpan)reader["beginTijd"];
            TimeSpan eindTijd = (TimeSpan)reader["eindTijd"];
            string korteOmschrijving = reader["korteOmschrijving"].ToString();
            string volledigeOmschrijving = reader["volledigeOmschrijving"].ToString();
            string extraInstructie = reader["extraInstructie"].ToString();
            string status = reader["status"].ToString();

            //Het Taak object wordt teruggegeven
            return new Taak(
                taakId,
                taakNaam,
                datum,
                beginTijd,
                eindTijd,
                locatie,
                korteOmschrijving,
                volledigeOmschrijving,
                extraInstructie,
                status
            );
        }
    }
}