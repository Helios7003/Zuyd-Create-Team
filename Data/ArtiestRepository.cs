using FestivalAppv2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace FestivalAppv2.Data
{
    public class ArtiestRepository
    {
        //Connection string (Apart database account nog koppelen)
        private string connectionString = @"Data Source=vallisnexusdatabeseserver.database.windows.net;Initial Catalog=vallisnexus_database;User ID=Joshua;Password=<WACHTWOORD>;Encrypt=True;TrustServerCertificate=True;";

        //Deze methode haalt alle artiesten uit de database
        public List<Artiest> GetAlleArtiesten()
        {
            //Hier komen alle artiesten in te staan
            List<Artiest> artiesten = new List<Artiest>();

            //Hier maak ik verbinding met de database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Verbinding openen
                connection.Open();

                //Query voor artiesten ophalen
                string query = "SELECT artiestId, artiestnaam, genre FROM ARTIEST ORDER BY artiestId";

                //Query klaarzetten
                SqlCommand command = new SqlCommand(query, connection);

                //Hier worden de gegevens gelezen
                SqlDataReader reader = command.ExecuteReader();

                //Zolang er gegevens zijn blijft hij doorlezen
                while (reader.Read())
                {
                    //Hier haal de gegevens uit de database rij
                    int artiestId = Convert.ToInt32(reader["artiestId"]);
                    string artiestnaam = reader["artiestnaam"].ToString();
                    string genre = reader["genre"].ToString();

                    //Van de databasegegevens maak ik een artiest object
                    Artiest artiest = new Artiest(artiestId, artiestnaam, genre);

                    //Het artiest object wordt toegevoegd aan de lijst
                    artiesten.Add(artiest);
                }
            }
            return artiesten;
        }

        //Deze methode voegt artiesten toe aan onze database
        public void VoegArtiestToe(Artiest artiest)
        {
            //Verbinding maken met database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //De verbinding wordt geopend
                connection.Open();

                //Deze query voegt een artiest toe
                string query = "INSERT INTO ARTIEST (artiestnaam, genre) VALUES (@artiestnaam, @genre)";

                //query klaargezet
                SqlCommand command = new SqlCommand(query, connection);

                //Parameter waardes meegeven aan query
                command.Parameters.AddWithValue("@artiestnaam", artiest.GetNaam());
                command.Parameters.AddWithValue("@genre", artiest.GetGenre());

                //QUery uitvoeren
                command.ExecuteNonQuery();
            }
        }

        //Deze methode wijzigt een bestaande artiest
        public void WijzigArtiest(Artiest artiest)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Deze query past de naam en het genre aan
                string query = "UPDATE ARTIEST SET artiestnaam = @artiestnaam, genre = @genre WHERE artiestId = @artiestId";

                //query klaargezet
                SqlCommand command = new SqlCommand(query, connection);

                //Hier geef ik de waardes mee aan de query
                command.Parameters.AddWithValue("@artiestId", artiest.GetArtiestId());
                command.Parameters.AddWithValue("@artiestnaam", artiest.GetNaam());
                command.Parameters.AddWithValue("@genre", artiest.GetGenre());

                //Query uitvoeren
                command.ExecuteNonQuery();
            }
        }

        //Deze methode verwijdert een artiest
        public void VerwijderArtiest(int artiestId)
        {
            //Verbinden met de DB
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Verbinding staat open
                connection.Open();

                //Verwijder optredens van het artiest (voor OptredenRepository)
                string deleteOptredens = "DELETE FROM OPTREDEN WHERE artiestId = @artiestId";

                //Delete query voor optredens klaarzetten
                SqlCommand commandOptredens = new SqlCommand(deleteOptredens, connection);

                //Hier geef ik het artiestId mee
                commandOptredens.Parameters.AddWithValue("@artiestId", artiestId);

                //Hier worden de optredens verwijderd
                commandOptredens.ExecuteNonQuery();

                //Daarna verwijder ik de artiest zelf
                string deleteArtiest = "DELETE FROM ARTIEST WHERE artiestId = @artiestId";

                //Hier wordt de delete query voor de artiest klaargezet
                SqlCommand commandArtiest = new SqlCommand(deleteArtiest, connection);

                //Hier geef ik weer hetzelfde artiestId mee
                commandArtiest.Parameters.AddWithValue("@artiestId", artiestId);

                //Hier wordt de artiest verwijderd
                commandArtiest.ExecuteNonQuery();
            }
        }
    }
}