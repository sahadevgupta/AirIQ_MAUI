using System;

namespace AirIQ.Models;

public class User
{
    public int AgencyId { get; set; }

    public string? AgencyName { get; set; }

    public string? ContactPerson { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public double Balance { get; set; }

    public string? EmailId { get; set; }

    public string? MobileNumber { get; set; }

    public string ContactPersonInitials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ContactPerson))
                return string.Empty;

            var words = ContactPerson
                .Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Concat(words.Select(w => char.ToUpperInvariant(w[0])));
        }
    }
}
