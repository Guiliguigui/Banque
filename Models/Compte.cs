using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banque.Models
{
    public class Compte
    {
        private int id;
        private decimal solde;
        private Client client;

        public int Id { get => id; set => id = value; }
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [Range(-5000.0, 1000000000.0)]
        public decimal Solde { get => solde; set => solde = value; }
        [Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get => client; set => client = value; }
        public IEnumerable<Operation> Operations { get; set; }
        public string Discriminator { get; set; }

        public Compte()
        {
            Operations = new List<Operation>();
        }
    }
}
