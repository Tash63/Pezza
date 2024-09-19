namespace Core.Side.Command
{
    public class UpdateSideCommandValidator:AbstractValidator<UpdateSideCommand>
    {
        public UpdateSideCommandValidator() {
            this.RuleFor(r => r.Data)
                .NotEmpty();

            this.RuleFor(r => r.Data.Name)
                .MaximumLength(100);

            this.RuleFor(r => r.Data.Description)
                .MaximumLength(500);

        }
    }
}
