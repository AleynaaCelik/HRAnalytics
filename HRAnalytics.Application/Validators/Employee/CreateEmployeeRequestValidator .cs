using FluentValidation;
using HRAnalytics.Application.DTOs.Employee.Reguests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Validators.Employee
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir");

            RuleFor(x => x.HireDate)
                .NotEmpty().WithMessage("İşe giriş tarihi zorunludur")
                .Must(BeAValidDate).WithMessage("Geçersiz tarih");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Departman seçimi zorunludur");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default && date <= DateTime.UtcNow;
        }
    }

    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Departman seçimi zorunludur");
        }
    }
}
