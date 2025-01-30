#Requires -Module Pester

Describe 'Disconnect-Smtp' {
    It 'Given no parameters, it should throw.' {
        Connect-Smtp -SmtpServer 127.0.0.1 -Port 25 -SecureSocket StartTlsWhenAvailable | Disconnect-Smtp
    }
}