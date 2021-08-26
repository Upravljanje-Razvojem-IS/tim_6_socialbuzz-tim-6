using System;

namespace SasaMessagingService.DTOs.MessageDtos
{
    public class MessagePutDto
    {
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
        /// sender id
        /// </summary>
        public Guid SenderId { get; set; }
    }
}
