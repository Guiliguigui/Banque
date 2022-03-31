using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Banque.Helpers;

namespace Banque.Models
{
    public class Client
    {
        private int id;
        private string nom;
        private string prenom;
        private string email;
        private string telephone;        

        public int Id { get => id; set => id = value; }
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-zA-Z\séèë\-_]*$", ErrorMessage = "Invalid FirstName")]
        public string Nom { get => nom; set =>nom = value; }
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-zA-Z\séèë\-_]*$", ErrorMessage = "Invalid LastName")]
        public string Prenom { get => prenom; set => prenom = value; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9\.\-_]+)@([a-zA-Z0-9\-_]+)(\.)?([a-zA-Z0-9\-_]+)?(\.){1}([a-zA-Z]{2,11})$", ErrorMessage = "Invalid Email Address")]
        public string Email { get => email; set => email = value; }
        [Required]
        [RegularExpression(@"^([0|\+33|33]+)(\.|\-|\s)?([1-9]{1})((\.|\-|\s)?[0-9]{2}){4}$", ErrorMessage = "Invalid Phone Number")]
        public string Telephone { get => telephone; set => telephone = Tools.FormatPhone(value); }
        public IEnumerable<Compte> Comptes { get; set; }

        public Client()
        {
            Comptes = new List<Compte>();
        }
    }
}
