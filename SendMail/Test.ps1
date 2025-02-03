param(
	[switch] $JustTest
)

if  (!$JustTest){
dotnet publish "$PSScriptRoot" -c Release
}
Import-Module "$PSScriptRoot/bin/Release/net7.0/publish/SendMail.dll"

Invoke-Pester "$PSScriptRoot/tests"
