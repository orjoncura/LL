Architecture

https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/

Clean architecture puts the business logic and application model at the center of the application. Instead of having business logic depend on data access or other infrastructure concerns, this dependency is inverted: infrastructure and implementation details depend on the Application Core. This functionality is achieved by defining abstractions, or interfaces, in the Application Core, which are then implemented by types defined in the Infrastructure layer. A common way of visualizing this architecture is to use a series of concentric circles, similar to an onion. Figure 5-7 shows an example of this style of architectural representation.

https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures
https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-9.png

User Secrets

https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows

Windows: %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
Linux: ~/.microsoft/usersecrets/<user_secrets_id>/secrets.json