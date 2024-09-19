using Common.Models.Topping;

namespace Core.Topping.Command
{
    public class UpdateToppingCommandValidator:AbstractValidator<UpdateToppingCommand>
    {
        public UpdateToppingCommandValidator() {

            this.RuleFor(x => x.data)
                .NotEmpty();

            this.RuleFor(x => x.data.Name)
                .MaximumLength(100);

            this.RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
