namespace Core.Email;

using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Models;
using FluentEmail.Core;
using FluentEmail.Smtp;
using HtmlAgilityPack;

public class EmailService
{
    public string HtmlContent { get; set; }

    public async Task<Result> SendEmail()
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(this.HtmlContent);
        var plainText = doc.DocumentNode.SelectSingleNode("//body").InnerText;
        plainText = Regex.Replace(plainText, @"\s+", " ").Trim();
        var sender = new SmtpSender(() => new System.Net.Mail.SmtpClient("localhost")
        {
            EnableSsl = false,
            DeliveryMethod=SmtpDeliveryMethod.SpecifiedPickupDirectory,
            PickupDirectoryLocation= "C:\\Users\\kiran.nariansamy\\Desktop\\repos\\Pezza\\Emails",
            Port=23
        });
        Email.DefaultSender = sender;
        var email = await Email
            .From("kirannariansamy85@gmail.com", "Pezza")
            .To("kirannariansamy1967@gmail.com", "Kiran Tash Nariansamy")
            .Subject("Collect your order it while it's hot")
            .Body(this.HtmlContent)
            .PlaintextAlternativeBody(plainText)
            .SendAsync();
         
        return email.Successful ? Result.Success() : Result.Failure("Email could not send");
    }
}