using System;

namespace FestivalAppv2.Models
{
    public class Artiest : Persoon
    {
        //Properties
        public int ArtiestId { get; private set; }
        public string Genre { get; private set; }

        //Constructor
        public Artiest(int artiestId, string artiestnaam, string genre) : base(artiestnaam)
        {
            ArtiestId = artiestId;
            Genre = genre;

        }

        //Geeft het artiestId terug
        public int GetArtiestId()
        {
            return ArtiestId;
        }

        //Geeft de artiestnaam terug
        public string GetArtiestnaam()

        {
            return Naam;
        }

        //Geeft het genre terug
        public string GetGenre()
        {

            return Genre;
        }

        //Past het genre aan
        public void SetGenre(string genre)
        {
            //Validatie: controleert of genre niet leeg is
            if (genre != "")
            {
                Genre = genre;
            }
        }

        //Toont de informatie van de artiest
        public override string ToonInfo()
        {
            return ArtiestId + " - " + Naam + " (" + Genre + ")";


        }
    }
}