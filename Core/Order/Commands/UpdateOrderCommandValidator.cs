namespace Core.Order.Commands
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            this.RuleFor(r => r.Data).NotEmpty();
            this.RuleFor(r => r.Data.status)
                .NotEmpty().
                IsInEnum();
            this.RuleFor(r=>r.Id).NotEmpty();
        }
    }
}
