using System.Management.Automation;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SendMail;

namespace SendMail
{
    [Cmdlet("Send", "Mail", DefaultParameterSetName = "ClassicLegacy")]
    public class SendMailCommand : PSCmdlet
    {        
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0,  ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "ClassicLegacy")]
        public string[] To { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 1, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 1, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 1, ParameterSetName = "ClassicLegacy")]
        public string Subject { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 2, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 2, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, Position = 2, ParameterSetName = "ClassicLegacy")]
        public string Body { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "ClassicM")]
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public string SmtpServer { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true, ParameterSetName = "ClassicLegacy")]
        public string From { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public string[] Attachments { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public string[] Bcc { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public SwitchParameter BodyAsHtml { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public Encoding? Encoding { get; set; } 

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public string[] Cc { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public DeliveryNotificationOption? DeliveryNotificationOption { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public Priority Priority { get; set; } = Priority.Normal;

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        public MessagePriority MessagePriority { get; set; } = MessagePriority.Normal;

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Connection")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        public MessageImportance Importance { get; set; } = MessageImportance.Normal;

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicM")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public PSCredential Credential { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public SwitchParameter UseSsl  { get; set; } = true;

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicM")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ClassicLegacy")]
        public int Port { get; set; } = 25;

        [Parameter(Mandatory = true, ParameterSetName = "ConnectionM")]
        [Parameter(Mandatory = true, ParameterSetName = "Connection")]
        public SmtpClient Connection { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Classic")]
        public SecureSocketOptions SecureSocket { get; set; } = MailKit.Security.SecureSocketOptions.StartTls;

        [Parameter(Mandatory = true, ParameterSetName = "ConnectionM")]
        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0, Mandatory = true, ParameterSetName = "ClassicM")]
        public MimeMessage Message { get; set; }

        private string HtmlBody { get; set; }
        private string TextBody { get; set; }

        protected override void BeginProcessing()
        {
            if (Encoding != null) {
                // TODO how could this be used with MimeKit?
                WriteWarning("Encoding paramter is ignored.  It is only present to respect the API of the depricated Send-MailMessage command.");
            }
            if (DeliveryNotificationOption != null)
            {
                // TODO how could this be used with MimeKit?
                WriteWarning("Encoding paramter is ignored.  It is only present to respect the API of the depricated Send-MailMessage command.");
            }

            if (Connection == null) {
                if (UseSsl)
                {
                    SecureSocket = SecureSocketOptions.StartTls;
                }
                else if (ParameterSetName == "ClassicLegacy")
                {
                    WriteWarning("Please use Tls if your server supports this.  Consider using the SecureSocket parameter for the best control.");
                    SecureSocket = SecureSocketOptions.StartTlsWhenAvailable;
                }
                Connection = new SmtpClient();
                Connection.Connect(SmtpServer, Port, SecureSocket);
                if (Credential != null)
                    Connection.Authenticate((System.Net.NetworkCredential)Credential);
            }
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if(ParameterSetName != "ClassicLegacy")
            {
                MessagePriority = (MessagePriority)(int)Priority;
            }
            if (BodyAsHtml) {
                HtmlBody = Body;
            }
            else 
            {
                TextBody = Body;
            }
            if (Message is null) {
                Message = NewMimeMessageCommand.MimeMessageFactory(
                    From,
                    To,
                    Cc,
                    Bcc,
                    TextBody,
                    HtmlBody,
                    Subject,
                    Attachments,
                    null,
                    Importance,
                    MessagePriority
                    );
            }
            Connection.Send(Message);
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            if (ParameterSetName != "Connection")
            {
                Connection.Disconnect(true);
                Connection.Dispose();
            }

            base.EndProcessing();
        }
    }
}

