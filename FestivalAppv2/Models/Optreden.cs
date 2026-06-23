using System;

namespace FestivalAppv2.Models
{
    public class Optreden
    {
        //Properties
        public int OptredenId { get; private set; }
        public Artiest Artiest { get; private set; }
        public Podium Podium { get; private set; }
        public DateTime Datum { get; private set; }
        public TimeSpan Starttijd { get; private set; }
        public TimeSpan Eindtijd { get; private set; }

        //Constructor voor optreden
        public Optreden(int optredenId, Artiest artiest, Podium podium, DateTime datum, TimeSpan starttijd, TimeSpan eindtijd)
        {
            OptredenId = optredenId;
            Artiest = artiest;
            Podium = podium;
            Datum = datum;
            Starttijd = starttijd;
            Eindtijd = eindtijd;
        }

        //Geeft het optredenId terug
        public int GetOptredenId()
        {
            return OptredenId;
        }

        //Geeft de artiest terug
        public Artiest GetArtiest()
        {
            return Artiest;
        }

        //Geeft het podium terug
        public Podium GetPodium()
        {
            return Podium;
        }

        //Geeft de datum terug
        public DateTime GetDatum()
        {
            return Datum;
        }

        //Geeft de starttijd terug
        public TimeSpan GetStarttijd()
        {
            return Starttijd;
        }

        //Geeft de eindtijd terug
        public TimeSpan GetEindtijd()
        {
            return Eindtijd;
        }

        //Toont het optreden als tekst
        public string ToonOptreden()
        {
            return Datum.ToShortDateString() + " | " +
                   Starttijd.ToString(@"hh\:mm") + " - " +
                   Eindtijd.ToString(@"hh\:mm") + " | " +
                   Artiest.GetArtiestnaam() + " | " +
                   Podium.GetPodiumnaam();
        }
    }
}