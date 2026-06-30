using System;

namespace FestivalAppv2.Models
{
    //Klasse voor een podium
    public class Podium
    {
        //Properties
        public int PodiumId { get; private set; }
        public string Podiumnaam { get; private set; }

        //Constructor voor podium
        public Podium(int podiumId, string podiumnaam)
        {
            PodiumId = podiumId;
            Podiumnaam = podiumnaam;
        }

        //Geeft het podiumId terug
        public int GetPodiumId()
        {
            return PodiumId;
        }

        //Geeft de podiumnaam terug
        public string GetPodiumnaam()
        {
            return Podiumnaam;
        }

        //Toont het podium als tekst
        public string ToonPodium()
        {
            return PodiumId + " - " + Podiumnaam;
        }
    }
}