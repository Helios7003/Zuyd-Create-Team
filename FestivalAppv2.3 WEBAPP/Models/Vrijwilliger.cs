using System;

namespace FestivalAppv2.Models
{
    public class Vrijwilliger : Persoon
    {
        //Properties
        public int VrijwilligerId { get; private set; }
        public string Email { get; private set; }
        public Vrijwilliger(int vrijwilligerId, string naam, string email) : base(naam)
        {
            VrijwilligerId = vrijwilligerId;
            Email = email;
        }

        //Geeft het vrijwilligerId terug
        public int GetVrijwilligerId()
        {
            return VrijwilligerId;
        }

        //Return email
        public string GetEmail()
        {
            return Email;
        }

        //Toont de informatie van de vrijwilliger
        public override string ToonInfo()
        {
            return VrijwilligerId + " - " + Naam + " (" + Email + ")";
        }
    }
}