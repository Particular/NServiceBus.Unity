NServiceBus.Unity
=================

Breaking changes in Unity will be handled as follows:

- Changes that require a change to the `NServiceBus.Unity` API (e.g. changes to `IUnityContainer`) will be released in a new major version of `NServiceBus.Unity`.
- Changes that do not require a change to the `NServiceBus.Unity` API will be released as a patch release to the latest minor version of `NServiceBus.Unity`.
- If Unity releases a new major version, we will take a dependency on that version in a new minor version of `NServiceBus.Unity`.