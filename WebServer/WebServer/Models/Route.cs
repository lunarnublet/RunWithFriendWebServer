using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace WebServer.Models
{
    [Table("Routes")]
    public class Route
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("polyline")]
        public DbGeometry Polyline { get; set; }

        [Column("distance")]
        public decimal Distance { get; set; }


        public ApplicationUser User{ get; set; }
    }

}