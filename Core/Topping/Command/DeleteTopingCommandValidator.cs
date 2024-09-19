namespace Core.Topping.Command
{
    public class DeleteTopingCommandValidator:AbstractValidator<DeleteToppingCommand>
    {

        public DeleteTopingCommandValidator() {

            this.RuleFor(x => x.Id)
                    .NotEmpty();
        }
    }
}
