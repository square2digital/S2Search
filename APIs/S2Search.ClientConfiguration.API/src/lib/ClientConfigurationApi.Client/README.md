# Client Configuration API Client
> Provides a library to interact with the Client Configuration API

## Getting Started
To build the SDKs for the Client Configuration API, follow the below steps:
1. First install AutoRest via `npm` (`npm install -g autorest@latest`)
2. Open a console and `cd` to the `spec` directory (this is needed for the next command to ensure the spec file is output to the correct directory)
3. Run the API and in the console invoke the following command:- `iwr https://localhost:5005/swagger/v1/swagger.json -o clientConfiguration-api-spec.yml`
4. Next in the console `cd` to the `ClientConfigurationApi.Client` directory
5. Invoke the following:- `autorest README.md --version=3.0.6274`

## Troubleshooting

For AutoRest to work correctly you need to ensure that the swagger decorated verbs have the operation name and ID (if applicable) set in the attribute

`[HttpGet]`

becomes

`[HttpGet(Name ="GetAPIStatus")]`


## Configuration
The following are the settings for this using this API with AutoRest.

``` yaml
# specify the version of Autorest to use
# currently you have to pass the version as a command line arg to target a different version:
# e.g. autorest README.md --version=3.0.6274
version: 3.0.6274
csharp: true
v3: true
input-file: ../../../spec/clientConfiguration-api-spec.yml
output-folder: AutoRest
clear-output-folder: true
library-name: S2Search.ClientConfigurationApi.Client
namespace: S2Search.ClientConfigurationApi.Client.AutoRest
title: ClientConfigurationApi.Client
add-credentials: true

```