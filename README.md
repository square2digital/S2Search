# S2 Search

<p align="center"><a href="https://www.square2digital.com/s2-search/" target="_blank" rel="noopener noreferrer"><img src="https://s2storagedev.blob.core.windows.net/assets/logos/Square2Digital-Logo-2024.png" alt="S2 Search Logo" width="30%"></a></p>

---

[![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=flat&logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![JavaScript](https://img.shields.io/badge/--F7DF1E?logo=javascript&logoColor=000)](https://www.javascript.com/)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![Next.js](https://img.shields.io/badge/Next-black?style=flat&logo=next.js&logoColor=white)](https://nextjs.org/)
[![React](https://img.shields.io/badge/react-%2320232a.svg?style=flat&logo=react&logoColor=%2361DAFB)](https://reactjs.org/)
[![Azure](https://badgen.net/badge/icon/azure?icon=azure&label)](https://azure.microsoft.com)
[![Docker](https://badgen.net/badge/icon/docker?icon=docker&label)](https://docker.com/)
[![Kubernetes](https://img.shields.io/badge/kubernetes-%23326ce5.svg?style=flat&logo=kubernetes&logoColor=white)](https://kubernetes.io/)
[![Elasticsearch](https://img.shields.io/badge/-ElasticSearch-005571?style=flat&logo=elasticsearch)](https://www.elastic.co/)
[![TailwindCSS](https://img.shields.io/badge/tailwindcss-%2338B2AC.svg?style=flat&logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)

## 🚀 Project Overview

**S2 Search** is an enterprise-grade search platform specifically designed for the automotive industry. It provides a modern, responsive search experience for vehicle stock management with seamless integration capabilities for existing applications.

### ✨ Key Highlights

- **🔍 Dual Search Technology**: Full support for both Elasticsearch and Azure Cognitive Services
- **⚡ Modern TypeScript UI**: Latest Next.js 13+ with TypeScript and Tailwind CSS
- **📱 Responsive Design**: Clean, adaptive UI across all devices and screen sizes
- **🎨 Fully Customizable**: Complete branding and functionality customization
- **📈 Enterprise Scalable**: Built to handle high-volume enterprise demands
- **☁️ Flexible Deployment**: Support for cloud (SaaS) and on-premises installations
- **📊 Rich Analytics**: Comprehensive search insights and business intelligence

### 🎯 Main Features

1. **Modern React/TypeScript UIs** - Next.js 13+ with App Router and TypeScript
2. **Admin Portal** - Complete management interface for search instances
3. **Multi-Format Support** - CSV and SFTP integration for stock management
4. **Search Analytics** - Rich user search analytics and business insights
5. **Elasticsearch Integration** - Full Elasticsearch 8.x support
6. **Azure Cognitive Services** - Native Azure AI Search capabilities
7. **Redis Caching** - Lightning-fast response times with intelligent caching
8. **Kubernetes Ready** - Production-ready container orchestration
9. **Development Automation** - PowerShell scripts for rapid local setup

## 📁 Folder Structure

```
S2Search/
├── APIs/                          # REST API Services
│   ├── Admin/                     # Administrative APIs
│   │   ├── S2Search.Admin.API/
│   │   ├── S2Search.ClientConfiguration.API/
│   │   └── S2Search.CustomerResource.API/
│   └── Search/                    # Search Engine APIs
│       └── SearchAPIs/
│           ├── AzureCognitiveServices/
│           └── ElasticSearch/
├── AzureFunctions/                # Serverless Functions (.NET 8)
│   ├── S2Search.FeedServices.Function/
│   ├── S2Search.SearchInsights.Function/
│   └── S2Search.SFTPGoServices.Function/
├── Cache/                         # Caching Layer
│   └── S2Search.CacheManager.App/
├── Common/                        # Shared Libraries (.NET Standard 2.0)
│   ├── S2Search.Common.Database.Sql.Dapper/
│   ├── S2Search.Common.Models/
│   └── S2Search.Common.Providers/
├── DB/                           # Database Projects
│   └── S2Search.CustomerResourceStore.DB/
├── FTP/                          # File Transfer Services
│   ├── S2Search.SFTPGo.App/
│   └── S2Search.SFTPGo.Client/
├── Importer/                     # Data Import Services
│   └── S2Search.Search.Importer/
├── K8s/                         # Kubernetes Infrastructure
│   ├── K8s.Helm/                # Helm Charts
│   ├── K8s.Local.Development.Environment/
│   └── S2Search.Infrastructure/  # Infrastructure as Code
└── UIs/                         # Frontend Applications
    ├── AdminUI/                 # Admin Interface
    │   └── S2Search.Admin.NextJS.ReactUI/
    └── SearchUIs/               # Search Interfaces
        ├── AzureCognitiveServices/
        ├── ElasticSearch/       # Legacy React UI
        └── ElasticSearch.2023/  # ⭐ New TypeScript UI
            └── elastic.ui.2023/ # Next.js 13 + TypeScript + Tailwind
```

## 🛠️ Technologies Used

### Frontend Technologies

- **Next.js 13+** - App Router with React Server Components
- **TypeScript 5.1+** - Type-safe development
- **React 18** - Latest React with concurrent features
- **Tailwind CSS 3.3** - Utility-first CSS framework
- **Material-UI (MUI) 5** - React component library
- **Redux** - Predictable state management

### Backend Technologies

- **.NET 7.0/8.0** - Modern cross-platform framework
- **ASP.NET Core** - High-performance web APIs
- **Azure Functions v4** - Serverless computing platform
- **Dapper** - Lightweight ORM for data access
- **.NET Standard 2.0** - Cross-platform compatibility

### Search & Data Technologies

- **Elasticsearch 8.x** - Full-text search and analytics
- **Azure Cognitive Search** - AI-powered cloud search
- **Redis** - In-memory data structure store
- **SQL Server** - Primary relational database
- **MySQL** - Alternative database support

### Infrastructure & DevOps

- **Docker** - Application containerization
- **Kubernetes** - Container orchestration platform
- **Helm** - Kubernetes package management
- **Azure DevOps** - Complete CI/CD pipelines
- **PowerShell 7+** - Cross-platform automation
- **SFTP** - Secure file transfer protocol

## ⚡ Quick Start

### Prerequisites

- **.NET 7.0/8.0 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **Node.js 18+** and npm - [Download here](https://nodejs.org/)
- **Docker Desktop** - [Download here](https://www.docker.com/products/docker-desktop)
- **Kubernetes** - Docker Desktop or minikube
- **PowerShell 7+** - [Download here](https://github.com/PowerShell/PowerShell)

### 🔧 Setup Instructions

#### 1. Clone the Repository

```powershell
git clone https://github.com/square2digital/S2Search.git
cd S2Search
```

#### 2. Choose Your Search Technology

**🔍 For Elasticsearch:**

- ✅ Perfect for on-premises deployment
- ✅ Works with AWS, GCP, or any cloud platform
- ✅ Full feature set with complete control

**☁️ For Azure Cognitive Services:**

- ✅ Optimized for Azure cloud deployment
- ✅ Access to all S2 Search platform features
- ✅ AI-powered search capabilities

#### 3. New TypeScript UI Development (Recommended)

**For the latest TypeScript UI:**

```powershell
cd "UIs\SearchUIs\ElasticSearch.2023\elastic.ui.2023"
npm install
npm run dev
```

**Access at:** http://localhost:3000

#### 4. Local Development Environment

Navigate to the Kubernetes development environment:

```powershell
cd "K8s\K8s.Local.Development.Environment"
```

**Deploy Full Platform:**

```powershell
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeConfigAPI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeCRAPI $true `
  -includeRedis $true `
  -includeSftpGo $true `
  -includeElastic $true `
  -deleteAllImages $true `
  -deleteS2Namespace $true
```

**Deploy Specific Components:**

```powershell
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeElasticAPI $true `
  -includeRedis $true `
  -includeElastic $true
```

#### 5. Legacy UI Development

**For React JavaScript UIs:**

```powershell
# Azure Cognitive Services UI
cd "UIs\SearchUIs\AzureCognitiveServices\S2Search.Search.NextJS.ReactUI\S2Search"
npm install
npm run dev

# Elasticsearch UI (Legacy)
cd "UIs\SearchUIs\ElasticSearch\S2Search.Elastic.NextJS.ReactUI\S2Search"
npm install
npm run dev

# Admin UI
cd "UIs\AdminUI\S2Search.Admin.NextJS.ReactUI\s2admin"
npm install
npm run dev
```

#### 6. Backend API Development

**Build and run APIs:**

```powershell
# Example: Elasticsearch Search API
cd "APIs\Search\SearchAPIs\ElasticSearch\S2Search.Elastic.API"
dotnet restore
dotnet build
dotnet run --project Search

# Example: Azure Cognitive Services API
cd "APIs\Search\SearchAPIs\AzureCognitiveServices\S2Search.Search.API"
dotnet restore
dotnet build
dotnet run --project Search
```

### 🌐 Access Points

After successful deployment:

| Service               | URL                           | Description                              |
| --------------------- | ----------------------------- | ---------------------------------------- |
| **New TypeScript UI** | http://localhost:3000         | Modern TypeScript-based search interface |
| **Legacy Search UI**  | http://localhost:3001         | Original React search interface          |
| **Admin UI**          | http://localhost:3002         | Administrative dashboard                 |
| **Search API**        | http://localhost:5000/swagger | Search API documentation                 |
| **Admin API**         | http://localhost:5001/swagger | Admin API documentation                  |

## 🤝 Contributing

We welcome contributions from the community! Please follow these guidelines:

### Getting Started

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Make** your changes following our coding standards
4. **Write** or update tests as needed
5. **Ensure** all tests pass and code builds successfully
6. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
7. **Push** to the branch (`git push origin feature/AmazingFeature`)
8. **Open** a Pull Request

### 📝 Code Standards

- **C#**: Follow .NET coding conventions and use async/await patterns
- **TypeScript/JavaScript**: Use ESLint, Prettier, and TypeScript strict mode
- **React**: Follow React best practices and use functional components
- **Testing**: Write unit tests for new functionality
- **Documentation**: Update API documentation for changes
- **Architecture**: Follow the existing project structure

### 🐛 Issue Reporting

- Use the [issue tracker](https://github.com/square2digital/S2Search/issues) to report bugs
- Provide detailed reproduction steps and environment information
- Include screenshots or code snippets when helpful
- Add appropriate labels and assignees

### 🔄 Pull Request Process

1. **Describe** the problem and solution clearly in PR description
2. **Reference** relevant issue numbers using `#issue-number`
3. **Ensure** all CI/CD checks pass successfully
4. **Request** review from appropriate maintainers
5. **Update** documentation and CHANGELOG if needed

For detailed contributing guidelines, see [CONTRIBUTING.md](CONTRIBUTING.md).

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

- **Jonathan Gilmartin** - [@JGilmartin-S2](https://github.com/JGilmartin-S2) - _Project Lead & Architecture_

## 🔗 Links & Resources

- **🌐 Company Website**: [Square2 Digital](https://www.square2digital.com/s2-search/)
- **📚 Documentation**: [docs.s2search.com](https://docs.s2search.com) _(Coming Soon)_
- **🔧 API Reference**: [api.s2search.com/swagger](https://api.s2search.com/swagger) _(Coming Soon)_
- **📞 Enterprise Support**: [info@square2digital.com](mailto:info@square2digital.com)

## 🆕 What's New

### Version 2023 Updates

- ⭐ **New TypeScript UI**: Modern Next.js 13+ with App Router
- 🎨 **Tailwind CSS**: Utility-first styling approach
- ⚡ **Performance**: Enhanced loading speeds and responsiveness
- 🔧 **Developer Experience**: Improved tooling and development workflow
- 🐳 **Containerization**: Updated Docker configurations
- ☸️ **Kubernetes**: Enhanced orchestration setup

---

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Made with ❤️](https://img.shields.io/badge/Made%20with-❤️-red.svg)](https://github.com/square2digital/S2Search)
[![GitHub issues](https://img.shields.io/github/issues/square2digital/S2Search)](https://github.com/square2digital/S2Search/issues)
[![GitHub stars](https://img.shields.io/github/stars/square2digital/S2Search)](https://github.com/square2digital/S2Search/stargazers)

> **💡 Note**: This project is actively maintained and developed by Square2 Digital. For enterprise support, custom implementations, and licensing inquiries, please contact [info@square2digital.com](mailto:info@square2digital.com)

---

### 🚀 Enterprise Ready

S2 Search is production-ready and currently powers vehicle search platforms for automotive dealerships worldwide. Contact us for enterprise deployment, custom integrations, and professional support services.
