namespace Core.Topping.Command
{
    class CreateToppingCommandValidator:AbstractValidator<CreateToppingCommand>
    {
        public CreateToppingCommandValidator() {
            this.RuleFor(x => x.data)
                    .NotEmpty()
                    .NotNull();
            this.RuleFor(x => x.data.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100);
            this.RuleFor(x => x.data.Price)
                .NotEmpty()
                .NotNull();
            this.RuleFor(x => x.data.PizzaID)
                .NotEmpty()
                .NotNull();
            this.RuleFor(x=> x.data.InStcok)
                .NotEmpty()
                .NotNull();
            this.RuleFor(x=> x.data.Additional)
                .NotEmpty()
                .NotNull();
        }
    }
}
