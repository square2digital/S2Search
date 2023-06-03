# Customer Resource API
## Overview
This API is used internally within S2 Search to administer a customers search resources.
It has endpoints that enable us to administer the following:
* Search Indexes -> Individual Azure Search Indexes
* Search Instances -> Azure Search Services
* Feed configuration
* Notification rules
* Search interface
* Synonyms

## Getting Started
This API connects to a SQL database as well as an Azure Storage account with queue storage.
In order to develop locally you will need the following:
* A local copy of the [Customer Resource Database](https://dev.azure.com/S2-Search/S2%20Search/_git/S2Search.CustomerResourceStore.DB)
* Azure Storage Explorer -> [Link](https://azure.microsoft.com/en-us/features/storage-explorer/)
* Azure Storage Emulator <sup><sub>(Included when installing the Azure Storage Explorer)<sub><sup>

Once you have all the dependencies, when you start the project it will open up to the swagger UI where you can explore the API's functionality.