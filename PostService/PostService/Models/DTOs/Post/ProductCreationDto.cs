﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.DTOs.Post
{
    public class ProductCreationDto
    {
        /// <summary>
        /// Name of the post(product)
        /// </summary>
        [Required(ErrorMessage = "PostName is required.")]
        public String PostName { get; set; }

        /// <summary>
        /// Image of the post(product)
        /// </summary>
        public String PostImage { get; set; }

        /// <summary>
        /// Description of the post(product)
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Price of the post(product)
        /// </summary>
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater then 0")]
        public Double Price { get; set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        public String Currency { get; set; }

        /// <summary>
        /// Category to which the post(product) belongs
        /// </summary>
        [Required(ErrorMessage = "Category is required.")]
        public String Category { get; set; }

        /// <summary>
        /// Date when the post(product) was published
        /// </summary>
        //public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Id of the user's account who posted the post
        /// </summary>
        [Required(ErrorMessage = "AccountId is required.")]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Weight of the product
        /// </summary>
        public String Weight { get; set; }
    }
}
