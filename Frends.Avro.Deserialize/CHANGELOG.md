# Changelog

## [2.0.0] - 2025-07-25

## [Breaking] Renamed Input parameter and added Options tab with error handling

### Breaking Changes
- **Input parameter renamed**: `AvroFilePath` has been renamed to `FilePath` for consistency with Frends task naming conventions
- **New Options tab**: Added Options tab with error handling parameters

### Migration Instructions
To upgrade to the new version:
1. The `AvroFilePath` parameter has been moved and renamed to `FilePath` in the Input tab - the value will be automatically migrated
2. New Options tab will be available with default error handling settings that maintain backward compatibility

### New Features
- **Options tab**: Added new Options tab containing:
  - `ThrowErrorOnFailure` (boolean, default: true) - Controls whether the task throws exceptions on failure
  - `ErrorMessageOnFailure` (string, default: empty) - Custom error message to use when failures occur
- **Enhanced Result structure**: 
  - Added `Success` boolean property to indicate task execution status
  - Added `Error` object containing detailed error information when failures occur
- **Improved error handling**: Centralized error handling with configurable behavior

### Technical Improvements
- Added `Error` class in Definitions folder with `Message` and `AdditionalInfo` properties
- Added `Options` class in Definitions folder for task configuration
- Added `ErrorHandler` helper class for consistent error processing
- Updated task structure to follow Frends task development guidelines


## [1.0.0] - 2024-05-31

### Added

- Initial implementation