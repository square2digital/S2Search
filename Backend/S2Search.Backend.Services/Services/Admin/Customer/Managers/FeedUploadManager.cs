using Microsoft.AspNetCore.Http;
using S2Search.Backend.Domain.Constants;
using S2Search.Backend.Domain.Customer.Constants;
using S2Search.Backend.Domain.Customer.Enums;
using S2Search.Backend.Domain.Customer.SearchResources.Notifications;
using S2Search.Backend.Domain.Interfaces.Repositories;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class FeedUploadManager : IFeedUploadManager
    {
        private readonly IFeedUploadValidationManager _feedUploadValidator;
        private readonly IFeedUploadDestinationManager _feedUploadDestination;
        private readonly IBlobClientProvider _blobClientProvider;
        private readonly INotificationRepository _notificationRepo;
        private readonly ISearchIndexRepository _searchIndexRepo;
        public FeedUploadManager(IFeedUploadValidationManager feedUploadValidator,
                                 IFeedUploadDestinationManager feedUploadDestination,
                                 IBlobClientProvider blobClientProvider,
                                 INotificationRepository notificationRepo,
                                 ISearchIndexRepository searchIndexRepo)
        {
            _feedUploadValidator = feedUploadValidator ?? throw new ArgumentNullException(nameof(feedUploadValidator));
            _feedUploadDestination = feedUploadDestination ?? throw new ArgumentNullException(nameof(feedUploadDestination));
            _blobClientProvider = blobClientProvider ?? throw new ArgumentNullException(nameof(blobClientProvider));
            _notificationRepo = notificationRepo ?? throw new ArgumentNullException(nameof(notificationRepo));
            _searchIndexRepo = searchIndexRepo ?? throw new ArgumentNullException(nameof(searchIndexRepo));
        }

        public async Task<(bool, string)> UploadFileAsync(Guid customerId, Guid searchIndexId, IFormFile file)
        {
            var isValidResult = await _feedUploadValidator.IsValid(file);

            if (!isValidResult.Item1)
            {
                return isValidResult;
            }

            var searchIndex = await _searchIndexRepo.GetAsync(customerId, searchIndexId);

            var isZip = file.FileName.EndsWith(AcceptedFileTypes.ZipFile, StringComparison.OrdinalIgnoreCase);

            var uploadDestination = _feedUploadDestination.GetDestination(customerId, searchIndex.IndexName, isZip, true);
            var blobName = $"{uploadDestination}/{file.FileName}";

            var blobClient = _blobClientProvider.Get(ConnectionStrings.BlobStorage, FeedUploadDestinations.FeedContainer, blobName);
            var blobExists = await blobClient.ExistsAsync();

            if (blobExists)
            {
                return (false, "The file has already been uploaded and is awaiting processing. Please wait for the file to be processed before uploading another.");
            }

            await blobClient.UploadAsync(file.OpenReadStream());
            
            var feedUploadNotification = CreateFeedUploadNotification(searchIndexId);
            await _notificationRepo.AddNotificationAsync(feedUploadNotification);

            return (true, "");
        }

        private Notification CreateFeedUploadNotification(Guid searchIndexId)
        {
            return new Notification()
            {
                SearchIndexId = searchIndexId,
                Recipients = "",
                Event = NotificationTriggerType.Feed_Manual_Upload.GetDescription(),
                Category = NotificationCategory.Feed.GetDescription(),
                TransmitType = NotificationTransmitType.System.GetDescription()
            };
        }
    }
}
