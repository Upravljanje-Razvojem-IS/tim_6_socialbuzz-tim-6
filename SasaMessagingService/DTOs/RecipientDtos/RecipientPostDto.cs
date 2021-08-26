using System;

namespace SasaMessagingService.DTOs.RecipientDtos
{
    public class RecipientPostDto
    {
        /// <summary>
        /// MessageId
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// AccountId
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
