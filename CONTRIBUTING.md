# Contributing to S2Search

Thank you for your interest in contributing to S2Search! We welcome contributions from developers, DevOps engineers, and the broader community. This guide will help you get started with contributing to our enterprise search platform.

## 🚀 Quick Start

Before you begin, please:

1. ⭐ **Star the repository** if you find S2Search useful
2. 📖 **Read our [README](README.md)** to understand the project architecture
3. 🔍 **Search [existing issues](https://github.com/square2digital/S2Search/issues)** to avoid duplicates
4. 💬 **Join discussions** in our [GitHub Discussions](https://github.com/square2digital/S2Search/discussions)

## 🐛 Reporting Issues

### Before Submitting an Issue

- [ ] Check the [README](README.md) and project documentation
- [ ] Search [existing issues](https://github.com/square2digital/S2Search/issues) (including closed ones)
- [ ] Try the latest version to see if the issue has been resolved
- [ ] Check our [troubleshooting guides](K8s/Legacy/K8s.Local.Development.Environment/DEPLOYMENT.md#troubleshooting)

### How to Submit a Bug Report

When creating a bug report, please include:

```markdown
**Environment:**

- S2Search Version: [e.g., v1.2.0]
- OS: [e.g., Windows 11, Ubuntu 22.04, macOS 14]
- Kubernetes Version: [e.g., v1.28.0]
- Deployment Method: [Legacy K8s, Helm Chart, Docker]
- Browser: [if UI-related, e.g., Chrome 119, Firefox 120]

**Description:**
A clear and concise description of what the bug is.

**Steps to Reproduce:**

1. Go to '...'
2. Click on '....'
3. Scroll down to '....'
4. See error

**Expected Behavior:**
What you expected to happen.

**Actual Behavior:**
What actually happened.

**Screenshots/Logs:**
If applicable, add screenshots or log output to help explain your problem.

**Additional Context:**
Any other context about the problem here.
```

### Issue Labels

We use the following labels to categorize issues:

- 🐛 `bug` - Something isn't working
- ✨ `enhancement` - New feature or request
- 📚 `documentation` - Improvements or additions to documentation
- 🚀 `performance` - Performance-related improvements
- 🔒 `security` - Security-related issues
- 🏗️ `infrastructure` - K8s, Docker, deployment-related
- 🎨 `ui` - Frontend/UI related
- 🔧 `api` - Backend API related
- 🔍 `search` - Elasticsearch or Azure Cognitive Services related
- 🆘 `help-wanted` - Community help is appreciated
- 🥇 `good-first-issue` - Good for newcomers

## 🛠️ Development Setup

### Prerequisites

Before contributing code, ensure you have:

- **Node.js 20+** - For frontend development
- **.NET 8 SDK** - For backend API development
- **Docker Desktop** - For containerization
- **Kubernetes** - Docker Desktop with K8s enabled or minikube
- **Git** - Version control
- **PowerShell 7+** - For deployment scripts (Windows/macOS/Linux)

### Local Development Setup

1. **Fork and Clone**

   ```bash
   git clone https://github.com/YOUR-USERNAME/S2Search.git
   cd S2Search
   ```

2. **Frontend Setup** (Choose your area of focus)

   ```bash
   # Modern TypeScript UI (Next.js 16)
   cd UIs/SearchUIs/AzureCognitiveServices/S2Search.Search.NextJS.ReactUI
   npm install
   npm run dev

   # New TypeScript UI (Next.js 14)
   cd UIs/SearchUIs/ElasticSearch.2023/elastic.ui.2023
   npm install
   npm run dev

   # Admin UI
   cd UIs/AdminUI/S2Search.Admin.NextJS.ReactUI
   npm install
   npm run dev
   ```

3. **Backend Setup**

   ```bash
   # Main API
   cd APIs/S2Search.API
   dotnet restore
   dotnet build
   dotnet run

   # Search APIs
   cd APIs/Search/SearchAPIs/ElasticSearch/S2Search.Elastic.API
   dotnet restore
   dotnet run
   ```

4. **Full Local Deployment**
   ```powershell
   cd K8s/Legacy/K8s.Local.Development.Environment
   .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true
   ```

### Development Workflow

1. **Create a Feature Branch**

   ```bash
   git checkout -b feature/your-feature-name
   # or
   git checkout -b fix/issue-number-description
   ```

2. **Make Your Changes**

   - Follow our [coding standards](#coding-standards)
   - Write tests for new functionality
   - Update documentation as needed

3. **Test Your Changes**

   ```bash
   # Frontend tests
   npm run lint
   npm run type-check
   npm run test

   # Backend tests
   dotnet test

   # Full integration test
   cd K8s/Legacy/K8s.Local.Development.Environment
   .\deployment-script.ps1 # Test full deployment
   ```

4. **Commit and Push**

   ```bash
   git add .
   git commit -m "feat: add new search filtering capability"
   git push origin feature/your-feature-name
   ```

5. **Create Pull Request**
   - Use our [PR template](#pull-request-guidelines)
   - Link to relevant issues
   - Provide clear description of changes

## 🎯 Coding Standards

### General Principles

- **Code Quality** - Write clean, readable, and maintainable code
- **Type Safety** - Use TypeScript for all frontend code, strong typing in C#
- **Performance** - Consider performance implications of your changes
- **Security** - Follow security best practices
- **Documentation** - Document complex logic and public APIs

### Frontend Standards (TypeScript/React)

```typescript
// ✅ Good - Proper TypeScript with interfaces
interface SearchResult {
  id: string;
  title: string;
  description: string;
  price?: number;
}

const SearchResultCard: React.FC<{ result: SearchResult }> = ({ result }) => {
  return (
    <div className="search-result-card">
      <h3>{result.title}</h3>
      <p>{result.description}</p>
      {result.price && <span>${result.price}</span>}
    </div>
  );
};

// ❌ Avoid - Any types and unclear interfaces
const SearchCard = ({ data }: any) => {
  return <div>{data.stuff}</div>;
};
```

### Backend Standards (.NET 8/C#)

```csharp
// ✅ Good - Proper async/await patterns with error handling
public async Task<IActionResult> SearchVehiclesAsync(
    SearchRequest request,
    CancellationToken cancellationToken = default)
{
    try
    {
        var results = await _searchService.SearchAsync(request, cancellationToken);
        return Ok(results);
    }
    catch (SearchException ex)
    {
        _logger.LogError(ex, "Search failed for request {Request}", request);
        return BadRequest(new { Error = "Search failed", Details = ex.Message });
    }
}

// ❌ Avoid - Synchronous operations and poor error handling
public IActionResult Search(object request)
{
    var results = _searchService.Search(request); // Blocking call
    return Ok(results); // No error handling
}
```

### Kubernetes/Infrastructure Standards

```yaml
# ✅ Good - Proper resource limits and health checks
apiVersion: apps/v1
kind: Deployment
metadata:
  name: s2search-api
spec:
  template:
    spec:
      containers:
        - name: api
          image: s2search-api:latest
          resources:
            requests:
              memory: "256Mi"
              cpu: "250m"
            limits:
              memory: "512Mi"
              cpu: "500m"
          livenessProbe:
            httpGet:
              path: /health
              port: 80
            initialDelaySeconds: 30
            periodSeconds: 10
          readinessProbe:
            httpGet:
              path: /ready
              port: 80
            initialDelaySeconds: 5
            periodSeconds: 5
```

## 📝 Pull Request Guidelines

### PR Title Format

Use [Conventional Commits](https://www.conventionalcommits.org/) format:

- `feat: add new vehicle search filters`
- `fix: resolve search result pagination issue`
- `docs: update Kubernetes deployment guide`
- `perf: optimize Elasticsearch query performance`
- `refactor: improve search service architecture`
- `test: add integration tests for admin API`
- `ci: update GitHub Actions workflow`

### PR Description Template

```markdown
## Description

Brief description of the changes made.

## Type of Change

- [ ] 🐛 Bug fix (non-breaking change which fixes an issue)
- [ ] ✨ New feature (non-breaking change which adds functionality)
- [ ] 💥 Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] 📚 Documentation update
- [ ] 🏗️ Infrastructure/DevOps changes
- [ ] 🧪 Tests

## Testing

Describe the tests you ran to verify your changes:

- [ ] Frontend unit tests pass (`npm test`)
- [ ] Backend unit tests pass (`dotnet test`)
- [ ] Integration tests pass
- [ ] Manual testing performed
- [ ] K8s deployment tested

## Screenshots (if applicable)

Add screenshots for UI changes.

## Checklist

- [ ] My code follows the project's style guidelines
- [ ] I have performed a self-review of my code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
- [ ] Any dependent changes have been merged and published

## Related Issues

Fixes #(issue number)
Closes #(issue number)
Related to #(issue number)
```

### Review Process

1. **Automated Checks** - All CI/CD checks must pass
2. **Code Review** - At least one maintainer review required
3. **Testing** - Manual testing for critical changes
4. **Documentation** - Updates to documentation if needed
5. **Security Review** - Security-sensitive changes require additional review

## 🔧 Component-Specific Guidelines

### Frontend Development

**UI Components:**

- Use Material-UI (MUI) components consistently
- Follow responsive design principles
- Implement proper accessibility (ARIA labels, keyboard navigation)
- Use TypeScript interfaces for all props and state

**State Management:**

- Use Redux Toolkit for global state
- Prefer local state for component-specific data
- Implement proper error boundaries

**Performance:**

- Use React.memo for expensive components
- Implement lazy loading for routes
- Optimize bundle size with code splitting

### Backend Development

**API Design:**

- Follow RESTful principles
- Use proper HTTP status codes
- Implement comprehensive error handling
- Add Swagger/OpenAPI documentation

**Database:**

- Support both SQL Azure and PostgreSQL
- Use Entity Framework or Dapper appropriately
- Implement proper connection pooling
- Follow database migration best practices

**Security:**

- Validate all inputs
- Use proper authentication/authorization
- Implement rate limiting
- Follow OWASP guidelines

### Infrastructure Development

**Kubernetes:**

- Include resource limits and requests
- Implement health checks
- Use proper ConfigMaps and Secrets
- Follow security best practices

**Docker:**

- Use multi-stage builds
- Minimize image size
- Run as non-root user
- Include proper health checks

## 🌟 Recognition

We appreciate all contributions! Contributors will be recognized in:

- 📜 **Contributors section** in README
- 🎉 **Release notes** for significant contributions
- 💼 **LinkedIn recommendations** for substantial contributions (with permission)
- 🏆 **Special contributor badges** for ongoing contributors

## 📞 Getting Help

### Community Support

- 💬 **GitHub Discussions** - General questions and community help
- 🐛 **GitHub Issues** - Bug reports and feature requests
- 📧 **Email** - [info@square2digital.com](mailto:info@square2digital.com) for sensitive issues

### Maintainer Contact

- **Jonathan Gilmartin** - [@JGilmartin-S2](https://github.com/JGilmartin-S2) - Project Lead

### Response Times

We aim to respond to:

- 🚨 **Critical bugs**: Within 24 hours
- 🐛 **Bug reports**: Within 3-5 business days
- ✨ **Feature requests**: Within 1 week
- 📚 **Documentation**: Within 1 week
- 💬 **General questions**: Within 1 week

## 🔒 Security

### Reporting Security Issues

Please **DO NOT** open a public issue for security vulnerabilities. Instead:

1. Email us directly at [security@square2digital.com](mailto:security@square2digital.com)
2. Include a detailed description of the vulnerability
3. Provide steps to reproduce if applicable
4. We will respond within 48 hours

### Security Best Practices

When contributing:

- Never commit secrets, API keys, or passwords
- Use environment variables for configuration
- Follow OWASP security guidelines
- Implement proper input validation
- Use secure communication protocols

## 📄 License

By contributing to S2Search, you agree that your contributions will be licensed under the same proprietary license as the project. See [LICENSE](LICENSE) for details.

## 🎯 Roadmap

Interested in major features? Check our roadmap for upcoming work:

- 🔍 **Enhanced Search AI** - Advanced ML-powered search capabilities
- 🌐 **Multi-language Support** - Internationalization features
- 📊 **Advanced Analytics** - Real-time business intelligence dashboards
- 🔄 **GraphQL API** - Modern API layer with GraphQL
- 🏗️ **Multi-cloud Support** - AWS and GCP deployment options

## 🙏 Thank You

Thank you for contributing to S2Search! Your efforts help make enterprise search better for everyone. If you include the 🔍 emoji at the top of your issue or pull request, we'll know you've read this guide thoroughly!

---

_Built with ❤️ by the S2Search community_

## I want to report a problem or ask a question

Before submitting a new GitHub issue, please make sure to

- Check out [docs.fastlane.tools](https://docs.fastlane.tools)
- Check out the README pages on [this repo](https://github.com/fastlane/fastlane)
- Search for [existing GitHub issues](https://github.com/fastlane/fastlane/issues)

If the above doesn't help, please [submit an issue](https://github.com/fastlane/fastlane/issues) on GitHub and provide information about your setup, in particular the output of the `fastlane env` command.

**Note**: If you want to report a regression in _fastlane_ (something that has worked before, but broke with a new release), please mark your issue title as such using `[Regression] Your title here`. This enables us to quickly detect and fix regressions.

Some people might also use the [_fastlane_ tag on StackOverflow](https://stackoverflow.com/questions/tagged/fastlane), however we don’t actively monitor issues submitted there.

## I want to contribute to _fastlane_

- To start working on _fastlane_, check out [YourFirstPR.md][firstpr]
- You will need a Google account to sign the CLA when you make your first PR
- For some more advanced tooling and debugging tips, check out [ToolsAndDebugging.md](ToolsAndDebugging.md)

### Google Contributor License Agreement (CLA)

Upon your first pull request to _fastlane_, the [googlebot](https://github.com/googlebot) will ask you to sign the Google Contributor License Agreement. Once the CLA has been accepted, the PR will be available to merge and you will not be asked to sign it again unless your GitHub username or email address changes.

Contributions to this project must be accompanied by a Contributor License
Agreement. You (or your employer) retain the copyright to your contribution;
this simply gives us permission to use and redistribute your contributions as
part of the project. Head over to <https://cla.developers.google.com/> to see
your current agreements on file or to sign a new one.

You generally only need to submit a CLA once, so if you've already submitted one
(even if it was for a different project), you probably don't need to do it
again.

### New Actions

Please be aware that we don’t accept submissions for new actions at the moment. You can find more information about that [here][submit action].

## I want to help work on _fastlane_ by reviewing issues and PRs

Thanks! We would really appreciate the help! Feel free to read our document on how to [respond to issues and PRs][responding to prs] and also check out how to become a [core contributor][core contributor].

## Why did my issue/PR get closed?

It's not you, it's us! _fastlane_ and its related tools receive a lot of issues and PRs. In order to effectively work through them and give each the prompt attention it deserves, we need to keep a sharp focus on the work we have outstanding.

One way we do this is by closing issues that we don't feel are immediately actionable. This might mean that we need more information in order to investigate. Or, it might mean that we haven't been able to reproduce it using the provided info. In this case we might close the issue while we wait for others to reproduce the problem and possibly provide some more info that unlocks the mystery.

<a id="fastlane-bot"/>

Another way we do this is by having an [automated bot](https://github.com/fastlane/issue-bot) go through our issues and PRs. The main goal of the bot is to ensure that the issues are still relevant and reproducible. Issues can be opened, and later fall idle for a variety of reasons:

- The user later decided not to use _fastlane_
- A workaround was found, making it a low priority for the user
- The user changed projects and/or companies
- A new version of _fastlane_ has been released that fixed the problem

No matter the reason, the _fastlane_ bot will ask for confirmation that an issue is still relevant after two months of inactivity. If the ticket becomes active again, it will remain open. If another 10 days pass with no activity, however, the ticket will be automatically closed.

In any case, **a closed issue is not necessarily the end of the story!** If more info becomes available after an issue is closed, it can be reopened for further consideration.

One of the best ways we can keep _fastlane_ an approachable, stable, and dependable tool is to be deliberate about how we choose to modify it. If we don't adopt your changes or new feature into _fastlane,_ that doesn't mean it was bad work! It may be that the _fastlane_ philosophy about how to accomplish a particular task doesn't align well with your approach. The best way to make sure that your time is well spent in contributing to _fastlane_ is to **start your work** on a modification or new feature **by opening an issue to discuss the problem or shortcoming with the community**. The _fastlane_ maintainers will do our best to give early feedback about whether a particular goal and approach is likely to be something we want to adopt!

## Code of Conduct

Help us keep _fastlane_ open and inclusive. Please read and follow our [Code of Conduct][code of conduct].

## Above All, Thanks for Your Contributions

Thank you for reading to the end, and for taking the time to contribute to the project! If you include the 🔑 emoji at the top of the body of your issue or pull request, we'll know that you've given this your full attention and are doing your best to help!

## License

This project is licensed under the terms of the MIT license. See the [LICENSE][license] file.

> This project and all _fastlane_ tools are in no way affiliated with Apple Inc. This project is open source under the MIT license, which means you have full access to the source code and can modify it to fit your own needs. All _fastlane_ tools run on your own computer or server, so your credentials or other sensitive information will never leave your own computer. You are responsible for how you use _fastlane_ tools.

<!-- Links: -->

[code of conduct]: CODE_OF_CONDUCT.md
[core contributor]: CORE_CONTRIBUTOR.md
[license]: LICENSE
[tools and debugging]: ToolsAndDebugging.md
[vision]: VISION.md
[responding to prs]: RespondingToIssuesAndPullRequests.md
[plugins]: https://docs.fastlane.tools/plugins/create-plugin/
[firstpr]: YourFirstPR.md
[submit action]: https://docs.fastlane.tools/plugins/create-plugin/#submitting-the-action-to-the-fastlane-main-repo
