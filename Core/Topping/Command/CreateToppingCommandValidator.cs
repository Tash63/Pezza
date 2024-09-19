namespace Core.Topping.Command
{
    class CreateToppingCommandValidator:AbstractValidator<CreateToppingCommand>
    {
        public CreateToppingCommandValidator() {
            this.RuleFor(x => x.data)
                    .NotEmpty();
            this.RuleFor(x=>x.data.Name)
                .NotEmpty()
                .MaximumLength(100);
            this.RuleFor(x=>x.data.Price)
                .NotEmpty();
            this.RuleFor(x=>x.data.PizzaID)
                .NotEmpty();
            this.RuleFor(x=> x.data.InStcok)
                .NotEmpty();
            this.RuleFor(x=> x.data.Additional)
                .NotEmpty();
        }
    }
}
