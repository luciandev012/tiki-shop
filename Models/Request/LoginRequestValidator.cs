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

    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        UserRequestValidator()
        {
            RuleFor(u => u.PhoneNumber).NotNull().NotEmpty();
            RuleFor(u => u.Address).NotEmpty();
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(u => u.Password);
        }
    }
}
