# Changelog

## [2.0.0] - 2025-08-07

### Changed 
- [Breaking] Renamed `OutputPath` parameter to `TargetFilePath` in Input tab
  - To upgrade to the new version, update your processes to use `TargetFilePath` instead of `OutputPath`
- Updated Result structure to include Success boolean and Error object
- Improved error handling with standardized Frends error patterns

### Added 
- Added new Options tab with error handling parameters (ThrowErrorOnFailure, ErrorMessageOnFailure)

## [1.0.0] - 2024-05-23
### Added
- Initial implementation
