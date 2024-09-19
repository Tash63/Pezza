namespace Core.Side.Command
{
    public class DeleteSideCommandValidator:AbstractValidator<DeleteSideCommand>
    {
        public DeleteSideCommandValidator() {

            this.RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
