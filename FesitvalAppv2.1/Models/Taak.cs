using System;

namespace FestivalAppv2.Models
{
    public class Taak
    {
        //Properties
        public int TaakId { get; private set; }
        public string TaakNaam { get; private set; }
        public DateTime Datum { get; private set; }
        public TimeSpan BeginTijd { get; private set; }
        public TimeSpan EindTijd { get; private set; }
        public Locatie Locatie { get; private set; }
        public string KorteOmschrijving { get; private set; }
        public string VolledigeOmschrijving { get; private set; }
        public string ExtraInstructie { get; private set; }
        public string Status { get; private set; }

        //Constructor voor taak
        public Taak(int taakId, string taakNaam, DateTime datum, TimeSpan beginTijd, TimeSpan eindTijd, Locatie locatie, string korteOmschrijving, string volledigeOmschrijving, string extraInstructie, string status)
        {
            TaakId = taakId;
            TaakNaam = taakNaam;
            Datum = datum;
            BeginTijd = beginTijd;
            EindTijd = eindTijd;
            Locatie = locatie;
            KorteOmschrijving = korteOmschrijving;
            VolledigeOmschrijving = volledigeOmschrijving;
            ExtraInstructie = extraInstructie;
            Status = status;
        }

        //Geeft het taakId terug
        public int GetTaakId()
        {
            return TaakId;
        }

        //Geeft de taaknaam terug
        public string GetTaakNaam()
        {
            return TaakNaam;
        }

        //Geeft de datum terug
        public DateTime GetDatum()
        {
            return Datum;
        }

        //Geeft de begintijd terug
        public TimeSpan GetBeginTijd()
        {
            return BeginTijd;
        }

        //Geeft de eindtijd terug
        public TimeSpan GetEindTijd()
        {
            return EindTijd;
        }

        //Geeft de locatie terug
        public Locatie GetLocatie()
        {
            return Locatie;
        }

        //Geeft de korte omschrijving terug
        public string GetKorteOmschrijving()
        {
            return KorteOmschrijving;
        }

        //Geeft de volledige omschrijving terug
        public string GetVolledigeOmschrijving()
        {
            return VolledigeOmschrijving;
        }

        //Geeft de extra instructie terug
        public string GetExtraInstructie()
        {
            return ExtraInstructie;
        }

        //Geeft de status terug
        public string GetStatus()
        {
            return Status;
        }

        //Controleert of verplichte velden zijn ingevuld
        public bool IsVolledigIngevuld()
        {
            if (TaakNaam == "" || KorteOmschrijving == "" || VolledigeOmschrijving == "")
            {
                return false;
            }

            return true;
        }

        //Past de status aan naar afgemeld
        public void MeldAf()
        {
            Status = "Afgemeld";
        }

        //Toont de taak kort
        public string ToonKort()
        {
            return TaakId + " - " + TaakNaam + " | " +
                   Datum.ToShortDateString() + " | " +
                   BeginTijd.ToString(@"hh\:mm") + " - " +
                   EindTijd.ToString(@"hh\:mm") + " | " +
                   Locatie.GetLocatieNaam() + " | " +
                   Status;
        }

        //Toont de taakdetails
        public string ToonDetails()
        {
            return "Taak: " + TaakNaam + "\n" +
                   "Datum: " + Datum.ToShortDateString() + "\n" +
                   "Tijd: " + BeginTijd.ToString(@"hh\:mm") + " - " + EindTijd.ToString(@"hh\:mm") + "\n" +
                   "Locatie: " + Locatie.GetLocatieNaam() + "\n" +
                   "Status: " + Status + "\n" +
                   "Korte omschrijving: " + KorteOmschrijving + "\n" +
                   "Volledige omschrijving: " + VolledigeOmschrijving + "\n" +
                   "Extra instructie: " + ExtraInstructie;
        }
    }
}