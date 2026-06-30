using System;

namespace FestivalAppv2.Models
{
    public class Locatie
    {
        //Propertoes
        public int LocatieId { get; private set; }
        public string LocatieNaam { get; private set; }
        public string LocatieOmschrijving { get; private set; }

        //Constructor voor klasse locatie
        public Locatie(int locatieId, string locatieNaam, string locatieOmschrijving)
        {
            LocatieId = locatieId;
            LocatieNaam = locatieNaam;
            LocatieOmschrijving = locatieOmschrijving;
        }

        //Geeft het locatieId terug
        public int GetLocatieId()
        {
            return LocatieId;
        }

        //Geeft de locatienaam terug
        public string GetLocatieNaam()
        {
            return LocatieNaam;
        }

        //Geeft de locatieomschrijving terug
        public string GetLocatieOmschrijving()
        {
            return LocatieOmschrijving;
        }

        //Toont de locatie als tekst
        public string ToonLocatie()
        {
            return LocatieId + " - " + LocatieNaam + " (" + LocatieOmschrijving + ")";
        }
    }
}