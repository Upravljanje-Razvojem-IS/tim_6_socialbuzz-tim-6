﻿using System;

namespace SasaMessagingService.DTOs.MessageStatusDtos
{
    public class MessageStatusGetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
    }
}