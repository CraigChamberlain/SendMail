using System;
using System.Management.Automation;
using System.Net.Mail;
using MimeKit;

namespace SendMail
{
    [Cmdlet("New", "MimeMessage")]
    public class NewMimeMessageCommand : PSCmdlet
    {
        // "Name <someone@fabrikam.com>"
        public string From { get; set; }
        public string[] To { get; set; }
        public string[] Bcc { get; set; }
        public string[] Cc { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public string[] Attachments { get; set; }
        public DateTime? Date { get; set; }
        public MessageImportance? Importance { get; set; }
        public MessagePriority? Priority { get; set; }
        public string[] ReplyTo { get; set; }

        protected override void ProcessRecord()
        {
            var message = MimeMessageFactory(
                From,
                To,
                Cc,
                Bcc,
                TextBody,
                HtmlBody,
                Subject,
                Attachments,
                Date,
                Importance,
                Priority,
                ReplyTo              
                );
            WriteObject(message);
            base.ProcessRecord();
        }


        public static MimeMessage 
            MimeMessageFactory(
                string from = null,
                string[] to = null,
                string[] cc = null,
                string[] bcc = null,
                string textBody = null,
                string htmlBody = null,
                string subject = null,
                string[] attachments = null,
                DateTime? date = null,
                MessageImportance? importance = null,
                MessagePriority? priority = null,
                string[] replyTo = null
            )
        {
            var message = new MimeMessage();
            if (from is not null)
                message.From.Add((MailboxAddress)(new MailAddress(from)));

            if (to is not null)
                foreach (string address in to)
                    message.To.Add((MailboxAddress)(new MailAddress(address)));

            if (cc is not null)
                foreach (string address in cc)
                    message.Cc.Add((MailboxAddress)(new MailAddress(address)));

            if (bcc is not null)
                foreach (string address in bcc)
                    message.Bcc.Add((MailboxAddress)(new MailAddress(address)));

            if (htmlBody is not null || textBody is not null || attachments is not null)
            {
                var builder = new BodyBuilder();

                if (htmlBody is not null)
                {
                    builder.HtmlBody = htmlBody;
                }
                if (textBody is not null)
                {
                    builder.TextBody = textBody;
                }
                if (attachments is not null)
                {
                    foreach (var attachment in attachments)
                        builder.Attachments.Add(attachment);
                }
                message.Body = builder.ToMessageBody();

            }

            if (subject is not null)
                message.Subject = subject;
 
            if (date is not null)
                message.Date = (DateTimeOffset)date;
            if (importance is not null)
                message.Importance = (MessageImportance)importance;
            if (priority is not null)
                message.Priority = (MessagePriority)priority;

            if (replyTo is not null)
                foreach (string address in replyTo)
                    message.ReplyTo.Add((MailboxAddress)(new MailAddress(address)));

            return message;
        }

    }
}

