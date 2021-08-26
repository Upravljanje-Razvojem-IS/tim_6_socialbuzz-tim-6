using System;

namespace SasaMessagingService.DTOs.RecipientDtos
{
    public class RecipientPutDto
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
