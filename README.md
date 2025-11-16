[![Cache Manager Build Status](https://github.com/square2digital/S2Search/actions/workflows/build-cachemanager.yml/badge.svg)](https://github.com/square2digital/S2Search/actions/workflows/build-cachemanager.yml)
[![Backend Build Status](https://github.com/square2digital/S2Search/actions/workflows/build-S2Search-backend-api.yml/badge.svg)](https://github.com/square2digital/S2Search/actions/workflows/build-S2Search-backend-api.yml)
[![Search UI Build Status](https://github.com/square2digital/S2Search/actions/workflows/build-search-ui.yml/badge.svg)](https://github.com/square2digital/S2Search/actions/workflows/build-search-ui.yml)

# S2 Search

<p align="center"><a href="https://www.square2digital.com/s2-search/" target="_blank" rel="noopener noreferrer"><img src="https://s2storagedev.blob.core.windows.net/assets/logos/Square2Digital-Logo-2024.png" alt="S2 Search Logo" width="30%"></a></p>

---

[![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=flat&logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![Next.js](https://img.shields.io/badge/Next-black?style=flat&logo=next.js&logoColor=white)](https://nextjs.org/)
[![React](https://img.shields.io/badge/react-%2320232a.svg?style=flat&logo=react&logoColor=%2361DAFB)](https://reactjs.org/)
[![Azure](https://badgen.net/badge/icon/azure?icon=azure&label)](https://azure.microsoft.com)
[![Elasticsearch](https://img.shields.io/badge/-ElasticSearch-005571?style=flat&logo=elasticsearch)](https://www.elastic.co/)
[![Kubernetes](https://img.shields.io/badge/kubernetes-%23326ce5.svg?style=flat&logo=kubernetes&logoColor=white)](https://kubernetes.io/)

## 🚀 Project Overview

**S2 Search** is an enterprise-grade search platform specifically designed for the automotive industry. It provides a modern, responsive search experience for vehicle stock management with seamless integration capabilities for existing applications.

### ✨ Key Highlights

- **🔍 Dual Search Technology**: Full support for both Elasticsearch and Azure Cognitive Services
- **⚡ Modern TypeScript UI**: Latest Next.js 14+ with TypeScript and Material-UI
- **📱 Responsive Design**: Clean, adaptive UI across all devices and screen sizes
- **🎨 Fully Customizable**: Complete branding and functionality customization
- **📈 Enterprise Scalable**: Built to handle high-volume enterprise demands
- **☁️ Flexible Deployment**: Support for cloud (SaaS) and on-premises installations
- **📊 Rich Analytics**: Comprehensive search insights and business intelligence

### 🎯 Main Features

1. **Modern React/TypeScript UIs** - Next.js 14+ with App Router and TypeScript
2. **Admin Portal** - Complete management interface with [`S2Search.Admin.NextJS.ReactUI`](UIs/AdminUI/S2Search.Admin.NextJS.ReactUI)
3. **Multi-Format Support** - CSV and SFTP integration via [`FeedServices`](AzureFunctions/S2Search.Function.FeedServices)
4. **Search Analytics** - Rich user search analytics and business insights
5. **Elasticsearch Integration** - Full Elasticsearch 8.x support via [`S2Search.Elastic.API`](APIs/Search/SearchAPIs/ElasticSearch)
6. **Azure Cognitive Services** - Native Azure AI Search capabilities
7. **Redis Caching** - Lightning-fast response times with [`S2Search.CacheManager`](CacheManager/S2Search.CacheManager)
8. **Kubernetes Ready** - Production-ready container orchestration in [`K8s`](K8s)
9. **Development Automation** - PowerShell scripts for rapid local setup

## 📁 Folder Structure

```
S2Search/
├── APIs/                          # REST API Services
│   ├── Search/                    # Search Engine APIs
│   │   └── SearchAPIs/
│   │       ├── AzureCognitiveServices/
│   │       └── ElasticSearch/
│   └── S2Search.API/             # Main Backend API
├── AzureFunctions/               # Serverless Functions (.NET 8)
│   ├── S2Search.Function.FeedServices/      # Feed processing
│   ├── S2Search.Function.SearchInsights/   # Analytics
│   └── S2Search.SFTPGoServices.Function/    # SFTP management
├── CacheManager/                 # Redis Caching Layer
│   └── S2Search.CacheManager/
├── DB/                          # Database Scripts & Migrations
│   └── S2Search.DB/
├── Domain/                      # Shared Domain Models
│   └── S2Search.Backend.Domain/
├── FTP/                        # File Transfer Services
│   ├── S2Search.SFTPGo.App/
│   └── S2Search.SFTPGo.Client/
├── Importer/                   # Data Import Services
│   └── S2Search.Backend.Importer/
├── K8s/                       # Kubernetes Infrastructure
│   └── Legacy/K8s.Local.Development.Environment/
├── Services/                  # Business Logic Layer
│   └── S2Search.Backend.Services/
└── UIs/                      # Frontend Applications
    ├── AdminUI/              # Admin Interface
    │   └── S2Search.Admin.NextJS.ReactUI/
    └── SearchUIs/            # Search Interfaces
        ├── AzureCognitiveServices/
        │   └── S2Search.Search.NextJS.ReactUI/  # TypeScript UI
        ├── ElasticSearch/                       # Legacy React UI
        └── ElasticSearch.2023/                  # ⭐ New TypeScript UI
            └── elastic.ui.2023/                 # Next.js 14 + TypeScript
```

## 🛠️ Technologies Used

### Frontend Technologies

- **Next.js 14+** - App Router with React Server Components
- **TypeScript 5.x** - Type-safe development
- **React 18** - Latest React with concurrent features
- **Material-UI (MUI) 5** - React component library
- **Redux Toolkit** - State management with TypeScript

### Backend Technologies

- **.NET 8** - Modern cross-platform framework
- **ASP.NET Core** - High-performance web APIs
- **Azure Functions v4** - Serverless computing platform
- **Dapper** - Lightweight ORM for data access

### Search & Data Technologies

- **Elasticsearch 8.x** - Full-text search and analytics
- **Azure Cognitive Search** - AI-powered cloud search
- **Redis** - In-memory data structure store
- **SQL Server/PostgreSQL** - Primary relational databases

### Infrastructure & DevOps

- **Docker** - Application containerization
- **Kubernetes** - Container orchestration platform
- **PowerShell 7+** - Cross-platform automation
- **SFTP** - Secure file transfer protocol

## ⚡ Quick Start

### Prerequisites

- **.NET 8 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **Node.js 18+** and npm - [Download here](https://nodejs.org/)
- **Docker Desktop** - [Download here](https://www.docker.com/products/docker-desktop)
- **PowerShell 7+** - [Download here](https://github.com/PowerShell/PowerShell)

### 🔧 Setup Instructions

#### 1. Clone the Repository

```powershell
git clone https://github.com/square2digital/S2Search.git
cd S2Search
```

#### 2. New TypeScript UI Development (Recommended)

```powershell
# Latest TypeScript UI with Next.js 14
cd "UIs/SearchUIs/ElasticSearch.2023/elastic.ui.2023"
npm install
npm run dev
```

**Access at:** http://localhost:3000

#### 3. Legacy UI Development

```powershell
# Azure Cognitive Services UI (TypeScript)
cd "UIs/SearchUIs/AzureCognitiveServices/S2Search.Search.NextJS.ReactUI"
npm install
npm run dev

# Admin UI
cd "UIs/AdminUI/S2Search.Admin.NextJS.ReactUI"
npm install
npm run dev
```

#### 4. Backend API Development

```powershell
# Main Backend API
cd "APIs/S2Search.API"
dotnet restore
dotnet build
dotnet run

# Elasticsearch Search API
cd "APIs/Search/SearchAPIs/ElasticSearch/S2Search.Elastic.API"
dotnet restore
dotnet run
```

#### 5. Local Development Environment

```powershell
cd "K8s/Legacy/K8s.Local.Development.Environment"

# Deploy Full Platform
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeRedis $true `
  -includeElastic $true
```

### 🌐 Access Points

| Service               | URL                           | Description                              |
| --------------------- | ----------------------------- | ---------------------------------------- |
| **New TypeScript UI** | http://localhost:3000         | Modern TypeScript-based search interface |
| **Azure Search UI**   | http://localhost:3001         | TypeScript search interface              |
| **Admin UI**          | http://localhost:3002         | Administrative dashboard                 |
| **Main API**          | http://localhost:5000/swagger | Main backend API documentation           |
| **Search API**        | http://localhost:5001/swagger | Search API documentation                 |

## 🤝 Contributing

We welcome contributions from the community! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

### Getting Started

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Make** your changes following our coding standards
4. **Write** or update tests as needed
5. **Ensure** all tests pass and code builds successfully
6. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
7. **Push** to the branch (`git push origin feature/AmazingFeature`)
8. **Open** a Pull Request

## 📄 License

This project is licensed under a **Proprietary Commercial License** - see the [LICENSE](LICENSE) file for details.

## 👥 Maintainers

- **Jonathan Gilmartin** - [@JGilmartin-S2](https://github.com/JGilmartin-S2) - _Project Lead & Architecture_

## 🔗 Links & Resources

- **🌐 Company Website**: [Square2 Digital](https://www.square2digital.com/s2-search/)
- **📞 Enterprise Support**: [info@square2digital.com](mailto:info@square2digital.com)

---

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Made with ❤️](https://img.shields.io/badge/Made%20with-❤️-red.svg)](https://github.com/square2digital/S2Search)
