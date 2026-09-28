# Changelog

## [2.2.0] - 2026-09-28

### Added

- Add support for JsonSchema and both Avro Object Container Files and raw Avro files. The task will now automatically detect the file type and deserialize accordingly.

## [2.1.0] - 2026-07-17

### Changed

- Task class is now static, aligning with Frends task conventions.

## [2.0.0] - 2025-07-25

### Added

- New Options tab with error handling parameters (ThrowErrorOnFailure, ErrorMessageOnFailure)
- Success property in Result to indicate task status
- Error object in Result with detailed failure info
- Error class with Message and AdditionalInfo properties
- Options class for task configuration
- ErrorHandler helper class for standardized error handling
- Improved error handling with centralized logic

### Changed
- [Breaking] Renamed AvroFilePath to FilePath for consistency with Frends naming conventions
- [Breaking] Moved FilePath from old parameter location to Input tab
- Updated task structure to align with Frends development guidelines

## [1.0.0] - 2024-05-31

### Added

- Initial implementation