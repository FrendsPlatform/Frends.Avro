# Changelog

## [2.0.0] - 2025-08-07

## [BREAKING] Renamed parameter and added standardized error handling

### Breaking Changes
- **[Breaking]** Renamed `OutputPath` parameter to `TargetFilePath` in Input tab
  - To upgrade to the new version, update your processes to use `TargetFilePath` instead of `OutputPath`
- **[Breaking]** Added new Options tab with error handling parameters
- **[Breaking]** Updated Result structure to include Success boolean and Error object

### Added
- New Options tab with standardized error handling:
  - `ThrowErrorOnFailure` (bool, default: true) - Controls whether the task throws exceptions on failure
  - `ErrorMessageOnFailure` (string, default: empty) - Custom error message to use when task fails
- Enhanced Result object:
  - `Success` (bool) - Indicates whether the task completed successfully
  - `Error` (object) - Contains error details when task fails, including Message and AdditionalInfo
- New ErrorHandler class for consistent error handling across the task

### Changed
- Improved error handling with standardized Frends error patterns
- Enhanced XML documentation for all public members
- Updated unit tests to cover new functionality and maintain 80%+ coverage

### Migration Notes
- Replace all instances of `OutputPath` with `TargetFilePath` in your processes
- The new Options tab parameters will use default values that maintain existing behavior
- Error handling is now standardized - set `ThrowErrorOnFailure` to false if you want to handle errors in your process flow instead of having the task throw exceptions


## [1.1.0] - 2024-12-XX
### Added
- Enhanced error handling and validation
- Improved documentation and examples
- Better compliance with Frends development guidelines

### Changed
- Updated task metadata for better discoverability
- Improved parameter validation

## [1.0.0] - 2024-05-23
### Added
- Initial implementation
