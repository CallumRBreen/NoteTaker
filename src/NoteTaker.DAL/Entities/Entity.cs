using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTaker.DAL.Entities
{
    public class Entity
    {
        public string Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }
    }
}
