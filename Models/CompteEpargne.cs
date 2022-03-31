using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Banque.Models
{
    public class CompteEpargne : Compte
    {
        private decimal taux;

        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        [Range(0.0,100.0)]
        public decimal Taux { get => taux; set => taux = value; }

        public CompteEpargne() : base() { }
    }
}
