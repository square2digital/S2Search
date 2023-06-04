# S2 Search

<p align="center"><a href="https://www.square2digital.com/s2-search/" target="_blank" rel="noopener noreferrer"><img src="https://www.square2digital.com/wp-content/uploads/2022/01/Square_2_Logo_Colour_Blue_White_BG.svg" alt="re-frame logo" width="30%"></a></p>

[![standard-readme compliant](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/JGilmartin-S2/S2Search)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

Technology

[![JavaScript](https://img.shields.io/badge/--F7DF1E?logo=javascript&logoColor=000)](https://www.javascript.com/)
[![.NET](https://img.shields.io/badge/--512BD4?logo=.net&logoColor=ffffff)](https://dotnet.microsoft.com/)
[![Azure](https://badgen.net/badge/icon/azure?icon=azure&label)](https://azure.microsoft.com)
[![Docker](https://badgen.net/badge/icon/docker?icon=docker&label)](https://https://docker.com/)
[![NuGet](https://badgen.net/badge/icon/nuget?icon=nuget&label)](https://https://nuget.org/)
[![Npm](https://badgen.net/badge/icon/npm?icon=npm&label)](https://https://npmjs.com/)

# Overview

> An Enterprise Search platform designed for the Automotive Industry which provides a modern and responsive search experience for your Vehicle Stock. Supports both Elastic and Azure Cognitive Services and can be integrated with an existing application.

---

S2 Search provides a Modern and Responsive UX that looks great on any device. It’s simple and easy to use UI is fully customisable to perfectly fit your branding and vehicle stock profile. S2 Search can be scaled to support any demand. The S2 Search platform is highly available, supporting both SaaS and OnPrem.

S2 Search supports both Elastic and Azure Cognitive Services. The codebase provides APIs and UIs designed to work specifically with each search technology.

S2 Search consists of multiple components such as APIs. UIs. SQL Databases, Azure Functions, Storage Integrations and FTP support. Each component builds as docker containers which are deployed and orchestrated to Kubernetes.

S2 Search provides rich search insights that are designed to provide business intelligence on your stock. What are you customers searching for? Which search attributes are higher and lower, what is the trend? These insights will provide a valuable tool to ensure you are purchasing stock which match your customer sentiment.

# This repository contains:

1. [The specification](spec.md) for how a standard README should look.
2. A link to [a linter](https://github.com/JGilmartin-S2/S2Search-preset) you can use to keep your README maintained ([work in progress](https://github.com/JGilmartin-S2/S2Search/issues/5)).
3. A link to [a generator](https://github.com/RichardLitt/generator-standard-readme) you can use to create standard READMEs.
4. [A badge](#badge) to point to this spec.
5. [Examples of standard READMEs](example-readmes/) - such as this file you are reading.

Standard Readme is designed for open source libraries. Although it’s [historically](#background) made for Node and npm projects, it also applies to libraries in other languages and package managers.

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

## Badge

If your README is compliant with Standard-Readme and you're on GitHub, it would be great if you could add the badge. This allows people to link back to this Spec, and helps adoption of the README. The badge is **not required**.

[![standard-readme compliant](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/JGilmartin-S2/S2Search)

To add in Markdown format, use this code:

```
[![standard-readme compliant](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/JGilmartin-S2/S2Search)
```

## Example Readmes

To see how the specification has been applied, see the [example-readmes](example-readmes/).

## Related Efforts

- [Art of Readme](https://github.com/noffle/art-of-readme) - 💌 Learn the art of writing quality READMEs.
- [open-source-template](https://github.com/davidbgk/open-source-template/) - A README template to encourage open-source contributions.

## Maintainers

[@RichardLitt](https://github.com/RichardLitt).

## Contributing

Feel free to dive in! [Open an issue](https://github.com/JGilmartin-S2/S2Search/issues/new) or submit PRs.

Standard Readme follows the [Contributor Covenant](http://contributor-covenant.org/version/1/3/0/) Code of Conduct.

### Contributors

This project exists thanks to all the people who contribute.
<a href="https://github.com/JGilmartin-S2/S2Search/graphs/contributors"><img src="https://opencollective.com/standard-readme/contributors.svg?width=890&button=false" /></a>

## License

[MIT](LICENSE) © Jonathan Gilmartin
