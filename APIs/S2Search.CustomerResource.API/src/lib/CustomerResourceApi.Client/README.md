# Customer Resource API Client
> Provides a library to interact with the Customer Resource API

## Getting Started
To build the SDKs for the Customer Resource API, follow the below steps:
1. First install AutoRest via `npm` (`npm install -g autorest@latest`)
2. Open a console and `cd` to the `spec` directory (this is needed for the next command to ensure the spec file is output to the correct directory)
3. Run the API and in the console invoke the following command:- `iwr https://localhost:5001/swagger/v1/swagger.json -o customerResource-api-spec.yml`
4. Next in the console `cd` to the `CustomerResourceApi.Client` directory
5. Invoke the following:- `autorest README.md --version=3.0.6130`

## Configuration
The following are the settings for this using this API with AutoRest.

``` yaml
# specify the version of Autorest to use
# currently you have to pass the version as a command line arg to target a different version:
# e.g. autorest README.md --version=3.0.6130
# v 3.0.6130 needs to be used for this API at time of writing due to an autorest issue with multipart formdata
version: 3.0.6130
csharp: true
v3: true
input-file: ../../../spec/customerResource-api-spec.yml
output-folder: AutoRest
clear-output-folder: true
library-name: S2Search.CustomerResourceApi.Client
namespace: S2Search.CustomerResourceApi.Client.AutoRest
title: CustomerResourceApi.Client
add-credentials: true

```