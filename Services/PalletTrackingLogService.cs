using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Services
{
    public class PalletTrackingLogService : IPalletTrackingLogService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        GeneralUtilities util = new GeneralUtilities();

        public async Task AddPalletTrackingLogAsync(CompareTwoCodesJson data)
        {
            var trackingLog = new PalletTrackingLog();
            trackingLog.DateTime = data.TimeOfInteraction;
            trackingLog.Action = data.Action;
            trackingLog.PalletId = data.Pallet.Id;
            trackingLog.PalletState = data.Pallet.State;
            trackingLog.PalletLocation = data.Pallet.Location;
            trackingLog.ForkliftId = data.ForkliftId;
            trackingLog.UserId = data.UserId;

            context.PalletTrackingLog.Add(trackingLog);
            await context.SaveChangesAsync();
        }

        public async Task<object> GetAllTrackingLogsAsync()
        {
            context = util.RemakeContext(context);
            var trackingLogs = await context.PalletTrackingLog.ToListAsync();
            return util.WrapListOfEntities(trackingLogs);
        }


    }
}
