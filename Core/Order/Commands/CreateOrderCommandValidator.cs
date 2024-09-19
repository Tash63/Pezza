namespace Core.Order.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<OrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            this.RuleFor(r => r.Data).NotEmpty();
            this.RuleFor(r => r.Data.CustomerId).NotEmpty();
            this.RuleFor(r => r.Data.Status).IsInEnum();
        }
    }
}