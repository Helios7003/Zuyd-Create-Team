using System;

namespace FestivalAppv2.Models
{
    public class Persoon
    {
        //Properties
        public string Naam { get; private set; }

        //Constructor voor naam
        public Persoon(string naam)
        {

            Naam = naam;
        }

        //Geeft de naam terug
        public string GetNaam()
        {
            return Naam;
        }

        //Past de naam aan
        public void SetNaam(string naam)
        {
            //Controleert of de naam niet leeg is
            if (naam != "")
            {
                Naam = naam;
            }
        }

        //Toont de informatie van een persoon
        public virtual string ToonInfo()
        {
            return "Naam: " + Naam;

        }
    }
}