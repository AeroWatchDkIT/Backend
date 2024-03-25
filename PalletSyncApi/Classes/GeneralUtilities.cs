using PalletSyncApi.Context;

namespace PalletSyncApi.Classes
{
    public class GeneralUtilities
    {
        public GeneralUtilities() { }

        public virtual PalletSyncDbContext RemakeContext(PalletSyncDbContext context)
        {
            context.Dispose();
            context = new PalletSyncDbContext();
            return context;
        }

        public object WrapListOfEntities(object entities)
        {
            var WrappedEntities = new
            {
                entities
            };
            return WrappedEntities;
        }
    }
}
