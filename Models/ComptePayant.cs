using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Banque.Models
{
    public class ComptePayant : Compte
    {
        private decimal coutOperation;
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [Range(0.01, 50.0)]
        public decimal CoutOperation { get => coutOperation; set => coutOperation = value; }

        public ComptePayant() : base() { }
    }
}
