using System;
using System.Collections.Generic;

namespace SasaMessagingService.MockData
{
    public static class UserData
    {

        public static List<MockAccounts> GetUsers()
        {
            return new List<MockAccounts>
            {
                new MockAccounts
                {
                    Id = Guid.Parse("b35050e7-4484-4328-9b16-34ea1f98c7fa"),
                    Username = "Sasa"
                },
                new MockAccounts
                {
                    Id = Guid.Parse("cedc44a9-1687-4514-ac10-8eb5c7a30774"),
                    Username = "Srdjan"
                },
                new MockAccounts
                {
                    Id = Guid.Parse("f04f3c73-fddc-430c-ad8d-da47f89f4bcd"),
                    Username = "Milan"
                },

            };
        }
    }
}
