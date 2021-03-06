﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace WebService.Models
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

        [Column("is_loop_route")]
        public bool IsLoopRoute { get; set; }

        [Column("origin")]
        public string Origin { get; set; }

        [Column("destination")]
        public string Destination { get; set; }

        [Column("distance")]
        public decimal Distance { get; set; }


        public ApplicationUser User { get; set; } // FK user_id
    }

}