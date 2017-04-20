# Visual Studio extension for SharePoint Framework projects
This repo contains the source code and releases of a Visual Studio extension for SharePoint Framework web parts. The extension provides a form for entering the information required of the Yeoman generator for SharePoint Framework web parts, runs the generator and creates a Visual Studio project file that includes only the files necessary for the web part. The supporting files (node.js and gulp files) still exist on disk, but are not shown in the Solution explorer.

# Additional resources

* [Overview of the SharePoint Framework](http://dev.office.com/sharepoint/docs/spfx/sharepoint-framework-overview)
* [SharePoint Framework development tools and libraries](http://dev.office.com/sharepoint/docs/spfx/tools-and-libraries)
* [SharePoint Framework Reference](https://sharepoint.github.io/)

# Installation

## Pre-requisites

* [Set up your Office 365 tenant](https://dev.office.com/sharepoint/docs/spfx/set-up-your-developer-tenant)
* [Set up your development environment](https://dev.office.com/sharepoint/docs/spfx/set-up-your-development-environment)
* Install the extension in Visual Studio:
  * Download the .vsix file from the repo [Releases page](./releases/latest)
  * Ensure Visual Studio is closed
  * Double-click on the downloaded .vsix file

# Getting Started
A templated named 'SPFx Web Part Project' is added to the template list in Visual Studio. Select this template to invoke a wizard that runs the Yeoman generator and creates the project.

Additional details are in the [Getting Started guide](./wiki/Getting-Started).

## Contributions

We welcome your input on issues and suggestions for new features. We do also welcome community contributions around the extension. If there's any questions around that, just let us know.

Please have a look on our [Contribution Guidance](./.github/CONTRIBUTING.md) before submitting your pull requests, so that we can get your contribution processed as fast as possible. Thx.

> Sharing is caring!