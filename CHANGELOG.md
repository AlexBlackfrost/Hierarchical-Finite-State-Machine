# Hierarchical-Finite-State-Machine

## [1.1.1] - 04/08/2023
 
### Added
 Nothing was added.
### Changed
 Nothing was changed.
### Fixed
 * Fixed a bug in EventTransition.cs where multipled events with different args are processed at the same time but only the first one is evaluated. Now all of them are evaluated until one of them mets all the conditions.


## [1.1.0] - 07/12/2022
 
### Added
   Nothing was added.
### Changed
 * All the events listened by active state objects are consumed after Update ends.
### Fixed
  * Fixed some bugs in processInstantly flag for listening events.
  * LateUpdate is not called when state is changed in Update. Updated how events are consumed, now all the events listened by active state objects are consumed after Update ends. 