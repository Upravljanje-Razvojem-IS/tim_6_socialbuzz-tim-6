using SasaMessagingService.Entities;
using System;

namespace SasaMessagingService.DTOs.MessageDtos
{
    public class MessageConfirmationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Status id
        /// </summary>
        public Guid StatusId { get; set; }
        /// <summary>
        /// status obj
        /// </summary>
        public MessageStatus Status { get; set; }
        /// <summary>
        /// sender id
        /// </summary>
        public Guid SenderId { get; set; }
    }
}
