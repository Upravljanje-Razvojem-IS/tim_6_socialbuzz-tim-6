using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SasaMessagingService.Entities
{
    public class Recipient
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// MessageId
        /// </summary>
        [ForeignKey("Message")]
        public Guid MessageId { get; set; }
        /// <summary>
        /// Message object
        /// </summary>
        public Message Message { get; set; }
        /// <summary>
        /// Account Id
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
