using SqlLocalDb;

namespace CodingArchitect.Spikes.NH.Persistance
{
    public static class SqlLocalDbInstanceProvider
    {
        public static LocalDatabase Instance = new LocalDatabase("NHTest");
    }
}
