namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public interface ITestPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Address { get; set; }
    string City { get; set; }
    string State { get; set; }
    string PostalCode { get; set; }
    string SSN { get; set; }  //decided to not worry about phone number for now.
    int Age { get; set; }
    long CreditCardNumber { get; set; }
    bool IsActive { get; set; } //since i have this, might as well use it.
    string EmailAddress { get; set; }
}