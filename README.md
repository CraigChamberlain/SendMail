# SendMail
Module of helper tools for the MimeKit library to replace `Send-MailMessage`.

As you may know `Send-MailMessage` is depricated and there is no versitile replacement without having to import nuget packages or dlls and do C# coding in the powershell environment using a library such as [MimeKit](https://mimekit.net/).
https://adamtheautomator.com/powershell-email/

This tool will have support for creating connections to any supported SMTP server with any supported [Authentication prococol](https://mimekit.net/docs/html/N_MailKit_Security.htm).  Initially only the most common authentication will have cmdlet support but I will expose a way of constructing your own MailKit objects without having to add-types which can be frustrating when jumping between machines.

I propose a workflow such as this:

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
Or

    $Connection = Connect-Smtp -SmtpServer "smtp.example.com" -Port $config.Port 25 -Authentication $Cred -UseSsl

    Get-SomeData |
      For-EachObject {
          ...
      } |
      Send-Mail -From "some.otherperson@example.com" -Connection $Connection

    $Connection | Disconnect-Smtp
      
