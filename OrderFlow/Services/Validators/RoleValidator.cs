using OrderFlow.Infrastructure;

namespace OrderFlow.Services.Validators;

public class RoleValidator : Validator
{
    public override bool Validate<T>(T obj)
    {
        return base.Validate(obj);
    }
}