using CodingArchitect.Spikes.NH.Persistance;

namespace CodingArchitect.Spikes.NH.Domain
{
    public static class TextStorageUtility
    {
        private static TextStorage emptyTextStorage;
        public static TextStorage EmptyTextStorage
        {
            get
            {
                if (emptyTextStorage == null)
                    lock(typeof(TextStorage))
                    {
                        if (emptyTextStorage == null)
                        {
                            LoadEmptyTextStorage();
                        }
                    }
                return emptyTextStorage;
            }
        }
        public static void LoadEmptyTextStorage()
        {
            var sessionFactory = Repository.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            {
                emptyTextStorage = session.Get<TextStorage>(0);
            }
        }
    }
}
