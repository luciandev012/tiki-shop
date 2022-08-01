using FluentValidation;

namespace tiki_shop.Models.Request
{

    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(u => u.PhoneNumber).NotNull().NotEmpty();
            RuleFor(u => u.Address).NotEmpty();
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(u => u.Password);
            RuleFor(u => u.FullName).NotNull().NotEmpty();
        }
    }

}
