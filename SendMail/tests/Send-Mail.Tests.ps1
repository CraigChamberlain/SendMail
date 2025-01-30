#Requires -Module Pester

Describe 'Send-Mail' {
    It 'Given no parameters, it should throw.' {
        $Connection = Connect-Smtp -SmtpServer 127.0.0.1 -Port 25 -SecureSocket StartTlsWhenAvailable
        Send-Mail -Connection $Connection -To "Max@hello.com" -From "Ted@tidy.com"
    }
}