namespace Core.Customer.Commands
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            this.RuleFor(r => r.Data).NotEmpty();
            this.RuleFor(r => r.Data.Name).MaximumLength(100).NotEmpty();
            this.RuleFor(r=>r.Data.Address).MaximumLength(100).NotEmpty();
            this.RuleFor(r=>r.Data.Email).MaximumLength(500).NotEmpty().EmailAddress();
            this.RuleFor(r=>r.Data.Cellphone).MaximumLength(50).NotEmpty();
            this.RuleFor(r=>r.Data.DateCreated).NotEmpty();
        }
    }
}
