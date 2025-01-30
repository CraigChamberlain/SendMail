using System.Management.Automation;
using MailKit.Net.Smtp;

namespace SendMail
{
    [Cmdlet("Disconnect", "Smtp")]
    public class DisconnectSmtpCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public SmtpClient Connection { get; set; }

        [Parameter(Position = 1)]
        public SwitchParameter Dispose { get; set; } = true;

        protected override void ProcessRecord()
        {
            Connection.Disconnect(true);
            if (Dispose)
            {
                Connection.Dispose();
            }

            base.ProcessRecord();
        }
    }
}

