using Microsoft.Its.Domain;

namespace Domain
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public PostalCode PostalCode { get; set; }
    }

    public class State : String<State>
    {
        public State(string value) : base(value)
        {

        }
    }
    public class City : String<City>
    {
        public City(string value) : base(value)
        {

        }
    }

    public class PostalCode : String<PostalCode>
    {
        public PostalCode(string value) : base(value)
        {

        }
    }
}
