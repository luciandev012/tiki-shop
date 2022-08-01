using FluentValidation;

namespace tiki_shop.Models.Request
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(l => l.PhoneNumber).NotNull().NotEmpty();
            RuleFor(l => l.Password).NotNull().NotEmpty();
        }
    }

}
