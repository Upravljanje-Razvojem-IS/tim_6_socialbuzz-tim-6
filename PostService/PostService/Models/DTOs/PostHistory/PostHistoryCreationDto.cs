﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.PostHistory
{
    public class PostHistoryCreationDto
    {
        /// <summary>
        /// Price for the post in a certain period of time
        /// </summary>
        [Required]
        public Double Price { get; set; }

        /// <summary>
        /// Date to which a certain post's price applies
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Id of the post to which a certain price applies
        /// </summary>
        [Required]
        public Guid PostId { get; set; }
    }
}