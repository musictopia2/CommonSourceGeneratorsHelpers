using CommonBasicLibraries.CollectionClasses;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
public partial class RandomGenerator
{
    internal static class Data
    {
        public const string Numbers = "0123456789";
        public const string CharsLower = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string CharsUpper = CharsLower.ToUpperInvariant();
        public const string HexPool = Numbers + "abcdef";
        public static readonly BasicList<(string Month, string Abb, string Code, int Day)> Months =
            new()
            {
                ("January", "Jan", "01", 31),
                ("February", "Feb", "02", 28),
                ("March", "Mar", "03", 31),
                ("April", "Apr", "04", 30),
                ("May", "May", "05", 31),
                ("June", "Jun", "06", 30),
                ("July", "Jul", "07", 31),
                ("August", "Aug", "08", 31),
                ("September", "Sep", "09", 30),
                ("October", "Oct", "10", 31),
                ("November", "Nov", "11", 30),
                ("December", "Dec", "12", 31)
            };
        public static readonly BasicList<string> Tlds = new()
        {
            "com",
            "org",
            "edu",
            "gov",
            "net",
        };
        public static readonly BasicList<(string Name, string Abb)> StreetSuffixes = new()
        {
            ("Avenue", "Ave"),
            ("Boulevard", "Blvd"),
            ("Center", "Ctr"),
            ("Circle", "Cir"),
            ("Court", "Ct"),
            ("Drive", "Dr"),
            ("Extension", "Ext"),
            ("Glen", "Gln"),
            ("Grove", "Grv"),
            ("Heights", "Hts"),
            ("Highway", "Hwy"),
            ("Junction", "Jct"),
            ("Key", "Key"),
            ("Lane", "Ln"),
            ("Loop", "Loop"),
            ("Manor", "Mnr"),
            ("Mill", "Mill"),
            ("Park", "Park"),
            ("Parkway", "Pkwy"),
            ("Pass", "Pass"),
            ("Path", "Path"),
            ("Pike", "Pike"),
            ("Place", "Pl"),
            ("Plaza", "Plz"),
            ("Point", "Pt"),
            ("Ridge", "Rdg"),
            ("River", "Riv"),
            ("Road", "Rd"),
            ("Square", "Sq"),
            ("Street", "St"),
            ("Terrace", "Ter"),
            ("Trail", "Trl"),
            ("Turnpike", "Tpke"),
            ("View", "Vw"),
            ("Way", "Way")
        };
        public static readonly BasicList<(string Company, string Abb, string Code, int Digits)> CcTypes = new()
        {
            ("American Express", "amex", "37", 15),
            ("Discover Card", "discover", "6011", 16),
            ("Mastercard", "mc", "51", 16),
            ("Visa", "visa", "4", 16),
        };
    }
}