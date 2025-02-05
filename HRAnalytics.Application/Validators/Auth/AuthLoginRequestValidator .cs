using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Validators.Auth
{
    public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
        }
    }

    public class AuthRegisterRequestValidator : AbstractValidator<AuthRegisterRequest>
    {
        public AuthRegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir")
                .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir");
        }
    }

}
