# Frends.Avro.Deserialize

Frends Task to deserialize Avro object container files and raw Avro binary files to JSON format. It converts all records to a JSON array, making it easy to work with Avro data in Frends workflows.

For raw payloads such as Salesforce Pub/Sub `PayloadBase64`, first Base64-decode the payload and write the resulting bytes to a file. Set `Input.FilePath` to that file and provide the matching `Input.SchemaJson`; raw Avro data cannot be read without a schema.

For Avro object container files, `SchemaJson` is optional. When supplied, it is used as the reader schema: compatible fields are resolved, missing reader fields receive their declared defaults, and incompatible schemas return an error. When omitted, the writer schema embedded in the file is used.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 
[![Build](https://github.com/FrendsPlatform/Frends.Avro/actions/workflows/Deserialize_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.Avro/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.Avro/Frends.Avro.Deserialize|main)

## Installing

You can install the Task via Frends UI Task View or by adding the NuGet package to your project.

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
