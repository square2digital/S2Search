namespace Domain.Enums
{
    /// <summary>
    /// List of current SFTPGo providers:
    /// * `0` - Local filesystem
    /// * `1` - S3 Compatible Object Storage
    /// * `2` - Google Cloud Storage
    /// * `3` - Azure Blob Storage
    /// * `4` - Local filesystem encrypted
    /// * `5` - SFTP
    ///
    /// </summary>
    public enum SFTPGoFilesystemProvider
    {
        LocalFilesystem = 0,
        S3ObjectStorage = 1,
        GoogleCloudStorage = 2,
        AzureBlobStorage = 3,
        LocalFilesystemEncrypted = 4,
        SFTP = 5
    }
}
