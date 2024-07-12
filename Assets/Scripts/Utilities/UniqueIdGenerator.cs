namespace Platformer.Utilities
{
    public static class UniqueIdGenerator
    {
        private static int currentId = 0;

        public static int GenerateUniqueId()
        {
            return ++currentId;
        }
    }
}