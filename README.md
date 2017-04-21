# Visual Studio extension for SharePoint Framework projects
The SharePoint Framework introduces a new set of tools for client-side web development that may be unfamiliar for many enterprise developers and which may not fit well with existing ALM processes, especially those tightly integrated with Team Foundation Server and IDE extensions. This repo contains the source code and releases of a Visual Studio extension for SharePoint Framework that integrates the traditional solution, build, debug and project management tools of Visual Studio with open-source tools like Node.js, Gulp and Yeoman generators. Creating new Framework projects is now as easy as selecting File > New > Project, just like any standard MVC, Web API or ASP.NET application.

The Visual Studio Extension for SharePoint Framework wraps the command-line UI of the Microsoft Yeoman Generator (yo @microsoft/sharepoint) with a more familiar Windows Forms experience, executes the generator project scaffolding behind the scenes and creates a Visual Studio project file that includes all the necessary files for a complete web part project. Also included is a web part item template for adding new web parts to an existing Framework project. Developers can launch the Workbench local sandbox environment to test their web parts by pressing F5 or by binding to the Gulp Serve event in the Task Runner Explorer. For developers who wish to dive deeper and learn more about the new web development toolchain used by the Framework, the Advanced mode provides full access to the Yeoman generator command parameters and an optional cmd.exe window to view the generator tasks in real time.

# Additional resources

* [Overview of the SharePoint Framework](http://dev.office.com/sharepoint/docs/spfx/sharepoint-framework-overview)
* [SharePoint Framework development tools and libraries](http://dev.office.com/sharepoint/docs/spfx/tools-and-libraries)
* [SharePoint Framework Reference](https://sharepoint.github.io/)

# Installation

## Pre-requisites

* [Set up your Office 365 tenant](https://dev.office.com/sharepoint/docs/spfx/set-up-your-developer-tenant)
* [Set up your development environment](https://dev.office.com/sharepoint/docs/spfx/set-up-your-development-environment)
* Install the extension in Visual Studio:
  * Download the .vsix file from the repo [Releases page](../../releases/latest)
  * Ensure Visual Studio is closed
  * Double-click on the downloaded .vsix file
  * Follow the installation prompts
  * Restart Visual Studio when the installer has completed

# Getting Started
A template named 'SPFx Web Part Project' is added to the template list in Visual Studio. Select this template to invoke a wizard that runs the Yeoman generator and creates the project. Additional web parts can be added to an existing solution using the "Add New Item" menu options.

Additional details are in the [Getting Started guide](../../wiki/Getting-Started).

## Contributions

We welcome your input on issues and suggestions for new features. We are also interested in community contributions to enhance and expand the capabilities of the extension. Let us know if you have any questions or comments.

Please have a look on our [Contribution Guidance](./.github/CONTRIBUTING.md) before submitting your pull requests, so that we can get your contribution processed as fast as possible. Thanks!

> Sharing is caring!
