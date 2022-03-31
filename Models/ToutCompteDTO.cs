using System.ComponentModel.DataAnnotations;

namespace Banque.Models
{
    public class ToutCompteDTO : Compte //me sert à créer tout types de comptes facilement (DTO = Data Transfert Object)
    {
        public ToutCompteDTO() : base() { }
        [DataType(DataType.Currency)]
        [Range(0.0, 50.0)]
        public decimal? CoutOperation { get; set; }
        [Range(0.0, 100.0)]
        public decimal? Taux { get; set; }
    }
}
