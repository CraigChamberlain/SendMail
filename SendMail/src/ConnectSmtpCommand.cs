using System;
using System.Management.Automation;
using MailKit.Net.Smtp;

namespace SendMail
{
    [Cmdlet("Connect", "Smtp")]
    public class ConnectSmtpCommand : PSCmdlet
    {
        // TODO can they be readonly fields?
        [Parameter(Mandatory = true, Position = 0)]
        public string SmtpServer { get; set; }

        [Parameter(Position = 1)]
        public int Port { get; set; } = 25;

        [Parameter(Position = 2)]
        public MailKit.Security.SecureSocketOptions SecureSocket { get; set; } = MailKit.Security.SecureSocketOptions.StartTls;


        protected override void BeginProcessing()
        {
            var smtp = new SmtpClient();
            smtp.Connect(SmtpServer, Port, SecureSocket);
            WriteObject(smtp);
            base.BeginProcessing();
        }


    }

    
}

