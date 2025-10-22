# Introduction 
This project contains Azure Functions that handle the processing of data feeds from customers.

Currently it supports `.zip` files containing  `.csv` files
It uses Azure Blob Storage and Storage Queues to process feeds through the following stages:
1. Extraction
2. Validation
3. Processing

The `FeedMonitor` function watches for new feed file uploads and coordinates the next step in the process for the file. 

The `FeedMonitor` uses the folder structure as a way to identify which file belongs to which `CustomerId` and `SearchIndexId`.

# Local Development
To run this locally the following are required:
1.	Azure Storage Explorer
2.	Azure Storage Emulator
3.	CustomerResourceStore database connection (SQLExpress or Azure SQL DB)

First run the Azure Storage Emulator, this will enable emulated Blob Storage and Queues.

Make sure you have a `local.settings.json` file added to the function project that contains these values:

*The appsettings.json file can serve as an example.

```json
{
  ...
  "Values": {
    ...
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "ConnectionStrings:CustomerResourceStore": "Server=localhost\\sqlexpress;Database=CustomerResourceStore;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Next, ensure that you have the `CustomerResourceStore` database accessible either via SQL Express or update the connection string to point to a remote SQL database.

In Azure Storage Explorer, navigate to `Local & Attached` > `Storage Accounts` > `Emulator - Default Ports`.

Finally, expand `Blob Storage` and ensure you have a container named `feed-services` created, if not create one as this is where the `FeedMonitor` function watches.

Now you can run the function and use the `CustomerResourceAPI` `Feed/Upload` endpoint to manually upload feed files into the correct folder structure.

To simulate a feed file being uploaded to trigger the function, run the following script in Powershell and change the relevant values:

```ps

$fileName = "MARSHALLCARS-02102021-DMS14"

$LocalFile = "PATH_TO_MY_FILE\$($fileName).zip"
$SasToken = "MY_SAS_TOKEN"

$CustomerId = "37A0EB6C-FD38-4B11-9486-E61ED6745953"
$SearchIndexName = "marshall-cars-vehicles"

$BlobTarget = "http://127.0.0.1:10000/devstoreaccount1/feed-services/processing/extract/$($CustomerId)/$($SearchIndexName)/$($fileName).zip"
azcopy copy $LocalFile $($BlobTarget + $SasToken) --recursive=true --from-to LocalBlob

```