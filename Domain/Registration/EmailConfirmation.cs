using System;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class EmailConfirmation
        {
            public DateTime ConfirmationSentDate { get; set; }
            public string Token { get; set; }
            public DateTime? ConfirmationReceivedDate { get; set; }
        }
    }
}