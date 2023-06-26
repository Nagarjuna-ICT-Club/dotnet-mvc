## Introduction to Dotnet

On Dotnet MVC, CRUD operations are carried out.One of the Entity Framework examples was created using [Microsoft's](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-7.0) official documentation.

## We'll go through

1. What is MVC architecture ?
2. How MVC Works ?
3. Folder Structure for MVC Pattern
4. Basic CRUD Example without Entity Framework
5. What is Entity Framework ?
6. Create Data Model
7. Create the database Context
8. Register the database Context
9. Create Controller and Views
10. Asynchronous Code

## Table of Contents

[Installation](#Installation) <br>
[MVC Architecture]("#MVC") <br>
[How MVC Works]("#howmvcworks")

<a name = "Installation">

## Installation

To create a dotnet MVC app at first you have to install dotnet cli and dotnet sdk or dotnet runtime in your system.We'll see the installation process on linux.You can see the installation process from [here](https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)

You can install dotnet in following ways:

- Manual Installation
- Scripted Installation

We will be using script installation for this process

You can download the script with wget:

```
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
```

Before running this script, you'll need to grant permission for this script to run as an executable:

```
sudo chmod +x ./dotnet-install.sh
```

The script defaults to installing the latest [long term support (LTS)](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) SDK version, which is .NET 6. To install the latest release, which may not be an (LTS) version, use the --version latest parameter.

```
./dotnet-install.sh --version latest
```

To install .NET Runtime instead of the SDK, use the --runtime parameter.

```
./dotnet-install.sh --version latest --runtime aspnetcore
```

You can install a specific major version with the --channel parameter to indicate the specific version. The following command installs .NET 7.0 SDK.

```
./dotnet-install.sh --channel 7.0
```
<a name ="MVC">

## MVC Architecture

<a name = "howmvcworks">

## How MVC Works
