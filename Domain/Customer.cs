namespace CodingArchitect.Spikes.NH.Domain
{
    public class Customer
    {
        public virtual int CustomerNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual TextStorage Comment { get; set; }
    }
}
