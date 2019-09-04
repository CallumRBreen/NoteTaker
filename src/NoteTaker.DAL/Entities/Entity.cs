using System;
using System.ComponentModel.DataAnnotations.Schema;
using NoteTaker.DAL.Utilities;

namespace NoteTaker.DAL.Entities
{
    public class Entity
    {
        public string Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        protected Entity()
        {
            Id = GuidHelper.GenerateSequential().ToString();
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }
    }
}
