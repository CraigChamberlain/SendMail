dotnet publish
Import-Module "./bin/Debug/net7.0/publish/MimeKit.PWSH.dll"

Invoke-Pester ./tests