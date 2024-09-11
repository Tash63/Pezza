namespace Core.Customer.Commands
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            this.RuleFor(r => r.Id).NotEmpty().NotNull();
            this.RuleFor(r => r.Data).NotNull();
            this.RuleFor(r => r.Data.Name).MaximumLength(100);
            this.RuleFor(r=>r.Data.Address).MaximumLength(500);
            this.RuleFor(r=>r.Data.Email).MaximumLength(500).EmailAddress();
            this.RuleFor(r => r.Data.Cellphone).MaximumLength(10);
        }
    }
}
    
   
