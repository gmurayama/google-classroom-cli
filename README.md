# Google Classroom Cli

CLI tool to automate commands using Google Classroom API

## Getting Started

These instructions will guide you through project deployment and development process. For further questions, see the doc files (under development).

### Prerequisites

The project uses **[.NET Core]((https://www.microsoft.com/net/download))** framework to run. Visit Microsoft page to download.

### Building

Clone project using this command:

```
git clone https://github.com/gmurayama/google-classroom-cli.git
```

In the project root directory, restore the NuGet packages

```
dotnet restore
```

And build using

```
dotnet build
```

The compiled code will be found in the debug folder of **GoogleClassroom.Service**

### Deployment

After build the project, run the DLL using

```
dotnet GoogleClassroomCli.Service.dll <args>
```

## Commandline Options

```
  add-student      Add a student into a course
  exec-commands    Import a command list to be executed
  help             Display more information on a specific command.
  version          Display version information.
```

## References

* [**.NET Core**](https://docs.microsoft.com/pt-br/dotnet/core/get-started) - Getting started.
* [**Google Classroom API**](https://developers.google.com/classroom/) - API documentation.
* [**OAuth 2.0 protocol**](https://developers.google.com/identity/protocols/OAuth2) - Authentication method used in this project to connect to Google Classroom API. See [*Server to Server Applications*](https://developers.google.com/identity/protocols/OAuth2ServiceAccount) for futhers details.
* [**Clean Architecture**](https://en.wikipedia.org/wiki/Robert_C._Martin) - Architecture implemented
* [**Command Pattern**](https://en.wikipedia.org/wiki/Command_pattern) - Used pattern in this project


## Built With

* [**.NET Core 2.0**](https://www.microsoft.com/net/learn/get-started) - Framework
* [**Classroom API .NET**](https://developers.google.com/resources/api-libraries/documentation/classroom/v1/csharp/latest/) - Google Classroom API Client Library for .NET

## Contributors

* Gustavo Murayama - **[gmurayama](https://github.com/gmurayama)**
* Leonardo Nascimento - **[leonaascimento](https://github.com/leonaascimento)**
* Tiago Suzukayama - **[tzusukayama](https://github.com/tsuzukayama)**
* Renê Araujo - **[rene-araujo](https://github.com/rene-araujo)**

## License

This project is licensed under the Apache 2.0 License - see the [LICENSE.md](https://github.com/gmurayama/google-classroom-cli/blob/master/LICENSE) file for details
