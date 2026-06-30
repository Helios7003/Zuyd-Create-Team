using FestivalAppv2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace FestivalAppv2.Data
{
    public class OptredenRepository
    {
        //Verbinding met de database
        private string connectionString;

        //Maakt de connection string met het ingevoerde wachtwoord
        public OptredenRepository(string databaseWachtwoord)
        {
            connectionString = @"Data Source=vallisnexusdatabeseserver.database.windows.net;Initial Catalog=vallisnexus_database;User ID=Joshua;Password="
                + databaseWachtwoord +
                @";Encrypt=True;TrustServerCertificate=True;";
        }

        //Haalt het volledige programma op
        public List<Optreden> GetAlleOptredens()
        {
            //Hier komen alle optredens in te staan
            List<Optreden> optredens = new List<Optreden>();

            //Verbinding maken met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query haalt optredens op met de artiest en het podium erbij
                string query = @"
                SELECT 
                    o.optredenId,
                    o.datum,
                    o.starttijd,
                    o.eindtijd,
                    a.artiestId,
                    a.artiestnaam,
                    a.genre,
                    p.podiumId,
                    p.podiumnaam
                FROM OPTREDEN o
                INNER JOIN ARTIEST a ON o.artiestId = a.artiestId
                INNER JOIN PODIUM p ON o.podiumId = p.podiumId
                ORDER BY o.datum, o.starttijd";

                //SQL command klaarzetten
                SqlCommand command = new SqlCommand(query, connection);

                //Resultaten uit de database lezen
                SqlDataReader reader = command.ExecuteReader();

                //Door alle gevonden optredens heen gaan
                while (reader.Read())
                {
                    //Van elke rij wordt een Optreden object gemaakt
                    Optreden optreden = LeesOptreden(reader);

                    //Het optreden wordt toegevoegd aan de lijst
                    optredens.Add(optreden);
                }
            }

            return optredens;
        }

        //Zoekt optredens op artiestnaam
        public List<Optreden> ZoekOpArtiest(string zoekterm)
        {
            //Lijst voor de gevonden optredens
            List<Optreden> optredens = new List<Optreden>();

            //Verbinding maken met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query zoekt op een deel van de artiestnaam
                string query = @"
                SELECT 
                    o.optredenId,
                    o.datum,
                    o.starttijd,
                    o.eindtijd,
                    a.artiestId,
                    a.artiestnaam,
                    a.genre,
                    p.podiumId,
                    p.podiumnaam
                FROM OPTREDEN o
                INNER JOIN ARTIEST a ON o.artiestId = a.artiestId
                INNER JOIN PODIUM p ON o.podiumId = p.podiumId
                WHERE a.artiestnaam LIKE @zoekterm
                ORDER BY o.datum, o.starttijd";

                //SQL command aanmaken
                SqlCommand command = new SqlCommand(query, connection);

                //De zoekterm wordt meegegeven aan de query
                command.Parameters.AddWithValue("@zoekterm", "%" + zoekterm + "%");

                //Resultaten lezen
                SqlDataReader reader = command.ExecuteReader();

                //Alle gevonden resultaten omzetten naar objecten
                while (reader.Read())
                {
                    Optreden optreden = LeesOptreden(reader);
                    optredens.Add(optreden);
                }
            }

            return optredens;
        }

        //Filtert optredens op podium
        public List<Optreden> FilterOpPodium(string podiumnaam)
        {
            //Lijst voor optredens van het gekozen podium
            List<Optreden> optredens = new List<Optreden>();

            //Verbinding maken met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Deze query filtert op podiumnaam
                string query = @"
                SELECT 
                    o.optredenId,
                    o.datum,
                    o.starttijd,
                    o.eindtijd,
                    a.artiestId,
                    a.artiestnaam,
                    a.genre,
                    p.podiumId,
                    p.podiumnaam
                FROM OPTREDEN o
                INNER JOIN ARTIEST a ON o.artiestId = a.artiestId
                INNER JOIN PODIUM p ON o.podiumId = p.podiumId
                WHERE p.podiumnaam LIKE @podiumnaam
                ORDER BY o.datum, o.starttijd";

                //SQL command klaarzetten
                SqlCommand command = new SqlCommand(query, connection);

                //Podiumnaam meegeven aan de query
                command.Parameters.AddWithValue("@podiumnaam", "%" + podiumnaam + "%");

                //Resultaten ophalen
                SqlDataReader reader = command.ExecuteReader();

                //Elke database rij wordt een Optreden object
                while (reader.Read())
                {
                    Optreden optreden = LeesOptreden(reader);
                    optredens.Add(optreden);
                }
            }

            return optredens;
        }

        //Zet een database rij om naar een Optreden object
        private Optreden LeesOptreden(SqlDataReader reader)
        {
            //Gegevens van het optreden uit de database halen
            int optredenId = Convert.ToInt32(reader["optredenId"]);
            DateTime datum = Convert.ToDateTime(reader["datum"]);
            TimeSpan starttijd = (TimeSpan)reader["starttijd"];
            TimeSpan eindtijd = (TimeSpan)reader["eindtijd"];

            //Gegevens van de artiest uit de database halen
            int artiestId = Convert.ToInt32(reader["artiestId"]);
            string artiestnaam = reader["artiestnaam"].ToString();
            string genre = reader["genre"].ToString();

            //Gegevens van het podium uit de database halen
            int podiumId = Convert.ToInt32(reader["podiumId"]);
            string podiumnaam = reader["podiumnaam"].ToString();

            //Artiest en podium object maken
            Artiest artiest = new Artiest(artiestId, artiestnaam, genre);
            Podium podium = new Podium(podiumId, podiumnaam);

            //Het complete optreden object wordt teruggegeven
            return new Optreden(optredenId, artiest, podium, datum, starttijd, eindtijd);
        }
    }
}