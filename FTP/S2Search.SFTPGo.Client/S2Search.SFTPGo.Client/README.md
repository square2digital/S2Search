# SFTPGo Client
> Provides a library to interact with the SFTPGo API

## Getting Started
To build the SDKs for My API, simply install AutoRest via `npm` (`npm install -g autorest`) and then run:
> `autorest README.md`


## Configuration
The following are the settings for this using this API with AutoRest.

``` yaml
# specify the version of Autorest to use
# e.g. version: 1.0.1-20170402
csharp: true
v3: true
input-file: ./spec/sftpgo-api-spec.yml
output-folder: AutoRest
clear-output-folder: true
library-name: S2Search.SFTPGo.Client
namespace: S2Search.SFTPGo.Client.AutoRest
title: SFTPGoClient
add-credentials: true

```