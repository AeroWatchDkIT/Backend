using Microsoft.AspNetCore.StaticFiles;
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

        public virtual object WrapListOfEntities(object entities)
        {
            var WrappedEntities = new
            {
                entities
            };
            return WrappedEntities;
        }

        public string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
