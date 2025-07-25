# Frends.Avro.Deserialize

Frends Task to deserialize Avro files to JSON format. This task reads Apache Avro files and converts all records to a JSON array, making it easy to work with Avro data in Frends workflows.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 
[![Build](https://github.com/FrendsPlatform/Frends.Avro/actions/workflows/Deserialize_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.Avro/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.Avro/Frends.Avro.Deserialize|main)

## Features

- Deserialize Avro files to JSON format
- Support for all Avro data types
- Configurable error handling (throw exceptions or return error information)
- Custom error messages
- Cancellation token support
- Comprehensive error reporting

## Installing

You can install the Task via Frends UI Task View or by adding the NuGet package to your project.

## Usage

### Input Parameters

- **FilePath**: Path to the Avro file you want to deserialize (e.g., `C:\data\myfile.avro`)

### Options

- **ThrowErrorOnFailure**: Determines whether to throw an exception on failure (default: `true`)
- **ErrorMessageOnFailure**: Custom error message to use when failure occurs and `ThrowErrorOnFailure` is `false`

### Return Value

The task returns a `Result` object with the following properties:

- **Json**: Dynamic object containing the deserialized JSON data (JArray of records)
- **Success**: Boolean indicating whether the operation was successful
- **Error**: Error information when the operation fails (null on success)

### Example

```csharp
var input = new Input 
{ 
    FilePath = @"C:\data\sample.avro" 
};

var options = new Options 
{ 
    ThrowErrorOnFailure = true,
    ErrorMessageOnFailure = ""
};

var result = Avro.Deserialize(input, options, cancellationToken);

if (result.Success)
{
    // Process the JSON data
    var jsonData = result.Json;
}
else
{
    // Handle the error
    var errorMessage = result.Error.Message;
}
```

## Building

Rebuild the project:

```bash
dotnet build
```

Run tests:
 
```bash
dotnet test
```

Create a NuGet package:

```bash
dotnet pack --configuration Release
```

## Documentation

For detailed documentation, visit: [Frends.Avro.Deserialize Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Avro.Deserialize)
