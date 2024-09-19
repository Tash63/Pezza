namespace Core.Side.Command
{

    public class CreateSideCommandValidator : AbstractValidator<CreateSideCommand>
    {
        public CreateSideCommandValidator() {
            this.RuleFor(r => r.Data)
                .NotEmpty();

            this.RuleFor(r=>r.Data.Name)
                .NotEmpty()
                .MaximumLength(100);

            this.RuleFor(r=>r.Data.Description)
                .NotEmpty()
                .MaximumLength(500);

            this.RuleFor(r => r.Data.Price)
                .NotEmpty();

            this.RuleFor(r => r.Data.InStock)
                .NotEmpty();
        }
    }
}