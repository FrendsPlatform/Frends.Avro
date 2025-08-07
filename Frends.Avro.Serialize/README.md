# Frends.Avro.Serialize

Frends Task to serialize JSON data to Avro file format using Apache Avro schemas.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 
[![Build](https://github.com/FrendsPlatform/Frends.Avro/actions/workflows/Serialize_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.Avro/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.Avro/Frends.Avro.Serialize|main)

## Installing

You can install the Task via Frends UI Task View or by using the NuGet package manager.

## Usage

### Input Parameters

- **JsonData**: JSON string or object to be serialized
- **AvroSchema**: Avro schema definition in JSON format
- **OutputPath**: Path where the Avro file will be created

### Options

- **ThrowErrorOnFailure**: Whether to throw exceptions on errors (default: true)
- **CustomErrorMessage**: Custom error message to use when errors occur
- **OverwriteFile**: Whether to overwrite existing files (default: false)

### Example

```json
{
  "JsonData": "{\"name\": \"John\", \"age\": 30}",
  "AvroSchema": "{\"type\": \"record\", \"name\": \"User\", \"fields\": [{\"name\": \"name\", \"type\": \"string\"}, {\"name\": \"age\", \"type\": \"int\"}]}",
  "OutputPath": "/path/to/output.avro"
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

## License

This project is licensed under the MIT License - see the LICENSE file for details.
