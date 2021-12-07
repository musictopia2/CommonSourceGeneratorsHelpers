namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public class SamplePerson : ITestPerson
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string SSN { get; set; } = "";
    public int Age { get; set; }
    public long CreditCardNumber { get; set; }
    public bool IsActive { get; set; }
    public string EmailAddress { get; set; } = "";
    public override string ToString()
    {
        return "FirstName: " + FirstName +
            " Last Name: " + LastName +
            " Address: " + Address +
            " City: " + City +
            " State: " + State +
            " Postal Code: " + PostalCode +
            " SSN: " + SSN +
            " Age: " + Age +
            " CCN: " + CreditCardNumber +
            " Email Address: " + EmailAddress +
            " Is Active: " + IsActive.ToString();
    }
}