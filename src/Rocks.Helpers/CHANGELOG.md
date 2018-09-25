## 4.2.1 - 2018-09-25
### Fixed
- Fixed issue with GlobalDbFactoriesProvider.Get throwing exception when factory is not defined in .NET Framework app.

## 4.2.0 - 2018-09-20
### Changed
- GlobalDbFactoriesProvider under .NET Framework will fallback to DbProviderFactories if no provider defined.
- Packages update 
### Fixed
- Fixed issue when using SqlExtensions.CreateDbConnection were not using GlobalDbFactoriesProvider in .NET Framework. 

## 4.1.0 - 2018-09-19
### Removed
- Removed unused dependency on Microsoft.AspNet.WebApi 

## 4.0.0 - 2018-06-01
### Added
- RetryOnExceptionAsync now support async logException
### Changed
- Packages update

## 3.0.0 - 2018-05-18
### Changed
- Renamed global provider classes

## 2.7.0 - 2018-05-18
### Added
- Added configuration provider and connection string provider that can be used in netfx and netcore

## 2.6.0 - 2018-04-28
### Removed
- Removed .NET 4.6.1, fixed dependencies

## 2.5.0 - 2018-04-27
### Modified
- Packages update
- Refactor tests to new major version fluent assertions and autofixture