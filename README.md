# S2 Search

<p align="center"><a href="https://www.square2digital.com/s2-search/" target="_blank" rel="noopener noreferrer"><img src="https://s2storagedev.blob.core.windows.net/assets/logos/Square2Digital-Logo-2024.png" alt="S2 Search Logo" width="30%"></a></p>

---

[![JavaScript](https://img.shields.io/badge/--F7DF1E?logo=javascript&logoColor=000)](https://www.javascript.com/)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![Azure](https://badgen.net/badge/icon/azure?icon=azure&label)](https://azure.microsoft.com)
[![Docker](https://badgen.net/badge/icon/docker?icon=docker&label)](https://https://docker.com/)
[![Kubernetes](https://img.shields.io/badge/kubernetes-%23326ce5.svg?style=flat&logo=kubernetes&logoColor=white)](https://kubernetes.io/)
[![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=flat&logo=redis&logoColor=white)](https://redis.io/)

## 🚀 Project Overview

**S2 Search** is an enterprise-grade search platform specifically designed for the automotive industry. It provides a modern, responsive search experience for vehicle stock management that can be seamlessly integrated with existing applications.

### Key Highlights

- **Dual Search Technology Support**: Choose between Elasticsearch and Azure Cognitive Services
- **Modern React-Based UI**: Clean, responsive user experience across all devices
- **Fully Customizable**: Adapt branding and functionality to match your requirements
- **Scalable Architecture**: Built to handle enterprise-level demand
- **Cloud & On-Premises**: Flexible deployment options (SaaS or On-Premises)
- **Rich Analytics**: Comprehensive search insights and business intelligence

### 🎯 Main Features

1. **Modern React Search UIs** - Clean and responsive user experience
2. **Admin Portal** - Complete management interface for search instances
3. **Multi-Format Support** - CSV and FTP integration for stock management
4. **Search Analytics** - Rich user search analytics and insights
5. **Elasticsearch Support** - Full Elasticsearch integration
6. **Azure Cognitive Services** - Native Azure search capabilities
7. **Redis Caching** - Lightning-fast response times
8. **Kubernetes Orchestration** - Automated deployment for cloud and on-premises
9. **Development Automation** - PowerShell scripts for local environment setup

## 📁 Folder Structure

```
S2Search/
├── APIs/                          # REST API Services
│   ├── Admin/                     # Administrative APIs
│   │   ├── S2Search.Admin.API/
│   │   ├── S2Search.ClientConfiguration.API/
│   │   └── S2Search.CustomerResource.API/
│   └── Search/                    # Search Engine APIs
│       ├── AzureCognitiveServices/
│       └── ElasticSearch/
├── AzureFunctions/                # Serverless Functions
│   ├── S2Search.FeedServices.Function/
│   ├── S2Search.SearchInsights.Function/
│   └── S2Search.SFTPGoServices.Function/
├── Cache/                         # Caching Layer
│   └── S2Search.CacheManager.App/
├── Common/                        # Shared Libraries
│   ├── S2Search.Common.Database.Sql.Dapper/
│   ├── S2Search.Common.Models/
│   └── S2Search.Common.Providers/
├── DB/                           # Database Projects
│   └── S2Search.CustomerResourceStore.DB/
├── FTP/                          # File Transfer Services
│   ├── S2Search.MySQL.DB/
│   ├── S2Search.SFTPGo.App/
│   └── S2Search.SFTPGo.Client/
├── Importer/                     # Data Import Services
│   └── S2Search.Search.Importer/
├── Infrastructure/               # Infrastructure as Code
│   └── S2Search.Infrastructure/
├── K8s/                         # Kubernetes Configurations
│   ├── K8s.Helm/
│   └── K8s.Local.Development.Environment/
└── SearchUIs/                   # Frontend Applications
    ├── AzureCognitiveServices/
    └── ElasticSearch/
```

## 🛠️ Technologies Used

### Backend Technologies

- **.NET 7.0/8.0** - Core backend framework
- **ASP.NET Core** - Web API development
- **Azure Functions v4** - Serverless computing
- **Dapper** - Database access layer
- **Entity Framework** - ORM for data modeling

### Frontend Technologies

- **React 17** - UI library
- **Next.js 11** - React framework
- **TypeScript** - Type-safe JavaScript
- **Material-UI (MUI) 5** - Component library
- **Redux** - State management

### Search Technologies

- **Elasticsearch** - Full-text search engine
- **Azure Cognitive Search** - Cloud search service

### Infrastructure & DevOps

- **Docker** - Containerization
- **Kubernetes** - Container orchestration
- **Helm** - Kubernetes package manager
- **Azure DevOps** - CI/CD pipelines
- **Redis** - In-memory caching
- **SFTP** - Secure file transfer

### Database

- **SQL Server** - Primary database
- **MySQL** - Alternative database option

## ⚡ Quick Start

### Prerequisites

- **.NET 7.0 SDK** or later
- **Node.js 16+** and npm
- **Docker Desktop**
- **Kubernetes** (Docker Desktop or minikube)
- **PowerShell 7+** (for deployment scripts)

### 🔧 Setup Instructions

#### 1. Clone the Repository

```powershell
git clone https://github.com/square2digital/S2Search.git
cd S2Search
```

#### 2. Choose Your Search Technology

**For Elasticsearch:**

- Ideal for on-premises deployment
- Compatible with AWS, GCP, or any cloud platform
- Full feature set available

**For Azure Cognitive Services:**

- Optimized for Azure deployment
- Access to all S2 Search platform features
- Native cloud integration

#### 3. Local Development Environment

Navigate to the Kubernetes local development environment:

```powershell
cd "K8s\K8s.Local.Development.Environment"
```

**Deploy Full Platform:**

```powershell
.\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $true -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $true -includeRedis $true -includeSftpGo $true -includeElastic $true -deleteAllImages $true -deleteS2Namespace $true
```

**Deploy Specific Components Only:**

```powershell
.\deployment-script.ps1 -includeElasticUI $true -includeElasticAPI $true -includeRedis $true -includeElastic $true
```

#### 4. Frontend Development

**For React UIs:**

```powershell
# Azure Cognitive Services UI
cd "SearchUIs\AzureCognitiveServices\S2Search.Search.NextJS.ReactUI\S2Search"
npm install
npm run dev

# Elasticsearch UI
cd "SearchUIs\ElasticSearch\S2Search.Elastic.NextJS.ReactUI\S2Search"
npm install
npm run dev
```

#### 5. Backend Development

**Build and run APIs:**

```powershell
# Example: Azure Cognitive Services Search API
cd "APIs\Search\AzureCognitiveServices\S2Search.Search.API"
dotnet restore
dotnet build
dotnet run --project Search
```

### 🌐 Access Points

After successful deployment:

- **Search UI**: http://localhost:3000
- **Admin UI**: http://localhost:3001
- **Search API**: http://localhost:5000/swagger
- **Admin API**: http://localhost:5001/swagger

## 🤝 Contributing

We welcome contributions from the community! Please follow these guidelines:

### Getting Started

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Make your changes
4. Write or update tests as needed
5. Ensure all tests pass
6. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
7. Push to the branch (`git push origin feature/AmazingFeature`)
8. Open a Pull Request

### Code Standards

- Follow C# coding conventions for backend code
- Use ESLint and Prettier for frontend code formatting
- Write unit tests for new functionality
- Update documentation for API changes
- Follow the existing project structure

### Issue Reporting

- Use the [issue tracker](https://github.com/square2digital/S2Search/issues) to report bugs
- Provide detailed reproduction steps
- Include environment information
- Add appropriate labels

### Pull Request Process

1. Ensure your PR description clearly describes the problem and solution
2. Include the relevant issue number if applicable
3. Make sure all status checks pass
4. Request review from maintainers
5. Update documentation if needed

For more detailed contributing guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md).

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2023 Jonathan Gilmartin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

## 👥 Maintainers

- **Jonathan Gilmartin** - [@JGilmartin-S2](https://github.com/JGilmartin-S2)

## 🔗 Links

- [Square2 Digital Website](https://www.square2digital.com/s2-search/)
- [Documentation](https://docs.s2search.com) _(Coming Soon)_
- [API Reference](https://api.s2search.com/swagger) _(Coming Soon)_

---

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Made with ❤️](https://img.shields.io/badge/Made%20with-❤️-red.svg)](https://github.com/square2digital/S2Search)

> **Note**: This project is actively maintained and developed by Square2 Digital. For enterprise support and licensing inquiries, please contact [info@square2digital.com](mailto:info@square2digital.com).

---

[![JavaScript](https://img.shields.io/badge/--F7DF1E?logo=javascript&logoColor=000)](https://www.javascript.com/)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![Azure](https://badgen.net/badge/icon/azure?icon=azure&label)](https://azure.microsoft.com)
[![Docker](https://badgen.net/badge/icon/docker?icon=docker&label)](https://https://docker.com/)
[![NuGet](https://badgen.net/badge/icon/nuget?icon=nuget&label)](https://https://nuget.org/)
[![Npm](https://badgen.net/badge/icon/npm?icon=npm&label)](https://https://npmjs.com/)

# Overview

> An Enterprise Search platform designed for the Automotive Industry which provides a modern and responsive search experience for your Vehicle Stock. Supports both Elastic and Azure Cognitive Services and can be integrated with any existing application.

---

S2 Search provides a Modern and Responsive UX that looks great on any device. It’s simple and easy to use UI is fully customisable to perfectly fit your branding and vehicle stock profile. S2 Search can be scaled to support any demand. The S2 Search platform is highly available, supporting both SaaS and OnPrem.

S2 Search supports both Elastic and Azure Cognitive Services. The codebase provides APIs and UIs designed to work specifically with each search technology.

S2 Search consists of multiple components such as APIs. UIs. SQL Databases, Azure Functions, Storage Integrations and FTP support. Each component builds as docker containers which are deployed and orchestrated to Kubernetes.

S2 Search provides rich search insights that are designed to provide business intelligence on your stock. What are you customers searching for? Which search attributes are higher and lower, what is the trend? These insights will provide a valuable tool to ensure you are purchasing stock which match your customer sentiment.

# Main Features

1. Modern React Search UIs offering a clean and responsive UX.
2. Admin Portal to manage your search instances.
3. CSV and FTP support for stock management.
4. Search Insights that provide you a rich set of user search analytics
5. Support for Elastic Search
6. Support for Azure Cognitive Services
7. Integrates with Redis cache to provide lighting fast response speeds.
8. Automated deployment to K8s for both Cloud and OnPrem production environments
9. Automated PowerShell scripts to easily setup the local development environment.

## Elastic Search or Cognitive Services?

S2 Search supports both Elastic Search and Azure Cognitive Services. The statements below will help you decide which S2 Version will be better suited for your requirements.

Choose Elastic if…

- You want to deploy S2 Search OnPrem.
- Will be deploying to AWS, GCP or any cloud platform that is not Azure.

Choose Azure Cognitive services if…

- Will be deploying S2 Search to Azure.
- Want to utilize all features of the S2 Search platform.

## Table of Contents

- [Background](#background)
- [Install](#install)
- [Usage](#usage)
  - [Generator](#generator)
- [Badge](#badge)
- [Example Readmes](#example-readmes)
- [Related Efforts](#related-efforts)
- [Maintainers](#maintainers)
- [Contributing](#contributing)
- [License](#license)

## Background

Standard Readme started with the issue originally posed by [@maxogden](https://github.com/maxogden) over at [feross/standard](https://github.com/feross/standard) in [this issue](https://github.com/feross/standard/issues/141), about whether or not a tool to standardize readmes would be useful. A lot of that discussion ended up in [zcei's standard-readme](https://github.com/zcei/standard-readme/issues/1) repository. While working on maintaining the [IPFS](https://github.com/ipfs) repositories, I needed a way to standardize Readmes across that organization. This specification started as a result of that.

> Your documentation is complete when someone can use your module without ever
> having to look at its code. This is very important. This makes it possible for
> you to separate your module's documented interface from its internal
> implementation (guts). This is good because it means that you are free to
> change the module's internals as long as the interface remains the same.

> Remember: the documentation, not the code, defines what a module does.

~ [Ken Williams, Perl Hackers](http://mathforum.org/ken/perl_modules.html#document)

Writing READMEs is way too hard, and keeping them maintained is difficult. By offloading this process - making writing easier, making editing easier, making it clear whether or not an edit is up to spec or not - you can spend less time worrying about whether or not your initial documentation is good, and spend more time writing and using code.

By having a standard, users can spend less time searching for the information they want. They can also build tools to gather search terms from descriptions, to automatically run example code, to check licensing, and so on.

The goals for this repository are:

1. A well defined **specification**. This can be found in the [Spec document](spec.md). It is a constant work in progress; please open issues to discuss changes.
2. **An example README**. This Readme is fully standard-readme compliant, and there are more examples in the `example-readmes` folder.
3. A **linter** that can be used to look at errors in a given Readme. Please refer to the [tracking issue](https://github.com/JGilmartin-S2/S2Search/issues/5).
4. A **generator** that can be used to quickly scaffold out new READMEs. See [generator-standard-readme](https://github.com/RichardLitt/generator-standard-readme).
5. A **compliant badge** for users. See [the badge](#badge).

## Install

This project uses [node](http://nodejs.org) and [npm](https://npmjs.com). Go check them out if you don't have them locally installed.

```sh
$ npm install --global standard-readme-spec
```

## Usage

This is only a documentation package. You can print out [spec.md](spec.md) to your console:

```sh
$ standard-readme-spec
# Prints out the standard-readme spec
```

### Generator

To use the generator, look at [generator-standard-readme](https://github.com/RichardLitt/generator-standard-readme). There is a global executable to run the generator in that package, aliased as `standard-readme`.

## Maintainers

[@JonathanGilmatin](https://github.com/JGilmartin-S2).

## Contributing

Feel free to dive in! [Open an issue](https://github.com/JGilmartin-S2/S2Search/issues/new) or submit PRs.

If you wish to contribute to the project please select an issue form the [issue board](https://github.com/JGilmartin-S2/S2Search/issues/new) and submit a PR. Please ensure your code follows the Contributor Covenant Code of Conduct.

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-green.svg?style=flat-square)](http://makeapullrequest.com)

Standard Readme follows the [Contributor Covenant](http://contributor-covenant.org/version/1/3/0/) Code of Conduct.

## License

[MIT](LICENSE) © Jonathan Gilmartin
