using System;
using System.ComponentModel.DataAnnotations;

namespace SasaMessagingService.Entities
{
    public class MessageStatus
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Status name
        /// </summary>
        public string Status { get; set; }
    }
}
