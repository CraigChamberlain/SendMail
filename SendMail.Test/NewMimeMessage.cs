using SendMail;
using MimeKit;

namespace SendMail.Test
{
    [TestClass]
    public class NewMimeMessageTest
    {
        [TestMethod]
        public void TestBlank()
        {
            var message = NewMimeMessageCommand.MimeMessageFactory();
            Assert.IsNotNull(message);
            Assert.IsInstanceOfType(message, typeof(MimeMessage));

            Assert.AreEqual(message.From.Count(), 0);
            Assert.AreEqual(message.To.Count(), 0);
            Assert.AreEqual(message.Cc.Count(), 0);
            Assert.AreEqual(message.Bcc.Count(), 0);

            //TODO Body?

            Assert.IsTrue(string.IsNullOrEmpty(message.Subject));

            Assert.AreEqual(message.Attachments.Count(), 0);

            //autoset as current date as offset
            //Assert.Equals(message.Date, 0);

            Assert.AreEqual(message.Importance, MessageImportance.Normal);
            Assert.AreEqual(message.Priority, MessagePriority.Normal);

            Assert.AreEqual(message.ReplyTo.Count(), 0);
        }

        [TestMethod]
        public void TestCompleted()
        {
            var message = NewMimeMessageCommand.MimeMessageFactory(
                "Name <someone@fabrikam.com>",
                new string[] { "To1 <someoneelse@fabrikam.com>" },
                new string[] {
                    "Cc1 <someoneelse1@fabrikam.com>",
                    "Cc2 <someoneelse2@fabrikam.com>"
                },
                new string[] {
                    "someoneelse3@fabrikam.com",
                    "someoneelse4@fabrikam.com"
                },
                "Some Text",
                "<html>SomeHtml</html>",
                "Some Subject",
                null,
                DateTime.MinValue,
                MessageImportance.Low,
                MessagePriority.Urgent,
                new string[]{ "SomeReplyAddress@fabrikam.com" }
            );
            Assert.IsNotNull(message);
            Assert.IsInstanceOfType(message, typeof(MimeMessage));

            Assert.AreEqual(1, message.From.Count());
            var from = (MailboxAddress)message.From[0];
            Assert.AreEqual("Name", from.Name);
            Assert.AreEqual("someone@fabrikam.com", from.Address);

            Assert.AreEqual(1, message.To.Count());
            var to = (MailboxAddress)message.To[0];
            Assert.AreEqual("To1", to.Name);
            Assert.AreEqual("someoneelse@fabrikam.com", to.Address);

            
            Assert.AreEqual(2, message.Cc.Count());
            var cc = (MailboxAddress)message.Cc[0];
            Assert.AreEqual("Cc1", cc.Name);
            Assert.AreEqual("someoneelse1@fabrikam.com", cc.Address);
            cc = (MailboxAddress)message.Cc[1];
            Assert.AreEqual("Cc2", cc.Name);
            Assert.AreEqual("someoneelse2@fabrikam.com", cc.Address);


            Assert.AreEqual(2, message.Bcc.Count());
            var bcc = (MailboxAddress)message.Bcc[0];
            Assert.AreEqual("", bcc.Name);
            Assert.AreEqual("someoneelse3@fabrikam.com", bcc.Address);
            bcc = (MailboxAddress)message.Bcc[1];
            Assert.AreEqual("", bcc.Name);
            Assert.AreEqual("someoneelse4@fabrikam.com", bcc.Address);

            Assert.AreEqual("Some Text", message.TextBody);
            Assert.AreEqual("<html>SomeHtml</html>", message.HtmlBody);

            Assert.AreEqual("Some Subject", message.Subject);

            // TODO make a fixture for this.
            Assert.AreEqual(0, message.Attachments.Count());

            Assert.AreEqual((DateTimeOffset)DateTime.MinValue, message.Date);

            Assert.AreEqual(MessageImportance.Low, message.Importance);
            Assert.AreEqual(MessagePriority.Urgent, message.Priority);

            Assert.AreEqual(1, message.ReplyTo.Count());
            var reply = (MailboxAddress)message.ReplyTo[0];
            Assert.AreEqual("", bcc.Name);
            Assert.AreEqual("SomeReplyAddress@fabrikam.com", reply.Address);
        }
    }
}