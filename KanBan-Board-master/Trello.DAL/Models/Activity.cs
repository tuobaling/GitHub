namespace Trello.DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Activity")]
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int CardId { get; set; }

        public int PositionNo { get; set; }

        public virtual Card Card { get; set; }
    }
}
