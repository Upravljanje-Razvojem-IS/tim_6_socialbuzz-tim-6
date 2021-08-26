using SasaMessagingService.Entities;
using System;

namespace SasaMessagingService.DTOs.RecipientDtos
{
    public class RecipientGetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// MessageId
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// Message object
        /// </summary>
        public Message Message { get; set; }
        /// <summary>
        /// AccountId
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
