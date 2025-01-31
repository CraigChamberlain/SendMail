# SendMail
Module of helper tools for the MimeKit library replace and extend upon `Send-MailMessage`.

As you may know `Send-MailMessage` is depricated and there is no versitile replacement without having to import nuget packages or dlls and do C# coding in the powershell environment using a library such as [MimeKit](https://mimekit.net/).
https://adamtheautomator.com/powershell-email/

This tool will have support for creating connections to any supported SMTP server with any supported [Authentication prococol](https://mimekit.net/docs/html/N_MailKit_Security.htm).  Initially only the most common authentication will have cmdlet support but I will expose a way of constructing your own MailKit objects without having to add-types which can be frustrating when jumping between machines.

I have replicated the API of `Send-MailMessage` so that the new module can replace it without re-writing anything other than the command.

##Install##

````pwsh
Install-Module SendMail
````

##Examples##

````pwsh
Get-SomeData |
    For-EachObject {
        $name = $_.SomeProperty | Select-Object $SomeTransformation
        $OtherVarialbe = $_.SomeData | ConvertTo-Html
        [pscustomobject]@{
        To = "some.person@example.com"
        Body = Get-EmailBody -Arg1 $Name -Arg2 $OtherVariable
        Attachment = $Filename
        Subject = "Some $Variable1 Subject"
    } |
    Send-Mail -From "some.otherperson@example.com" -SmtpServer "smtp.example.com" -Port $config.Port 25 -Authentication $Cred -UseSsl
````
Or
````pwsh
$Connection = Connect-Smtp -SmtpServer 127.0.0.1 -Port 25 -SecureSocket StartTlsWhenAvailable

Get-SomeData |
    For-EachObject {
        ...
        @{
            To = $_.To
            Cc = @("recp1@example.com","RecipientName <recp2@example.com>")
            TextBody = $_.Body
            Attachments = $_.Attachments
        }
    } |
    New-MimeMessage -From "some.otherperson@example.com" |
    Send-Mail -Connection $Connection

$Connection | Disconnect-Smtp
````
      
