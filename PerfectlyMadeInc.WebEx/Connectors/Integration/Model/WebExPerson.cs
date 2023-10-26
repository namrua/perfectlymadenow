namespace PerfectlyMadeInc.WebEx.Connectors.Integration.Model
{
    #region models

    /// <summary>
    /// Personal information of person
    /// </summary>
    public class WebExPerson
    {
        // public properties
        public long AttendeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }

    }

    #endregion

}
