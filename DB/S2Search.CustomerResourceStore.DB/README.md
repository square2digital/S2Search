# Customer Resource Store
This is a database project to hold all of our SQL objects relating to Customer Resources.
It will hold data relating to:
- Feed Configuration
- Notifications
- Notification Rules
- Search Index Configuration
- Search Interface Configuration
- Theme Configuration
## Installation
If installing a fresh copy of the database. Open this project and publish the database to your local SQL instance.
To populate some sample data, run the following scripts:
- [SQL Scripts/CreateData.sql](https://dev.azure.com/S2-Search/S2%20Search/_git/S2Search.CustomerResourceStore.DB?path=%2FS2Search.CustomerResourceStore.DB%2FSQL%20Scripts%2FCreateData.sql)
- [SQL Scripts/CreateData-Notifications.sql](https://dev.azure.com/S2-Search/S2%20Search/_git/S2Search.CustomerResourceStore.DB?path=%2FS2Search.CustomerResourceStore.DB%2FSQL%20Scripts%2FCreateData-Notifications.sql)
- [SQL Scripts/CreateData-CustomerPricingTiers.sql](https://dev.azure.com/S2-Search/S2%20Search/_git/S2Search.CustomerResourceStore.DB?path=%2FS2Search.CustomerResourceStore.DB%2FSQL%20Scripts%2FCreateData-CustomerPricingTiers.sql)

If updating an existing local instance, use the schema compare feature in Visual Studio to update your local SQL instance.