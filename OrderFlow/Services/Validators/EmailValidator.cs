using System.Net.Mail;
using OrderFlow.Infrastructure;

namespace OrderFlow.Services.Validators;

public class EmailValidator : Validator
{
    public override bool Validate<T>(T obj)
    {
        if (obj is not string email) return false;
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}