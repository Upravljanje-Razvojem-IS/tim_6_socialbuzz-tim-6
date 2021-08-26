using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SasaMessagingService.Entities
{
    public class Message
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Message content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Message date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// StatusId
        /// </summary>
        [ForeignKey("MessageStatus")]
        public Guid StatusId { get; set; }
        /// <summary>
        /// Status obj
        /// </summary>
        public MessageStatus Status { get; set; }
        /// <summary>
        /// Message sender id
        /// </summary>
        public Guid SenderId { get; set; }
    }
}
