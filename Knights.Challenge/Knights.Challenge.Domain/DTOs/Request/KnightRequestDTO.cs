using FluentValidation;
using Knights.Challenge.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Knights.Challenge.Domain.DTOs.Request
{
    public class KnightRequestDTO
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public DateTime Birthday { get; set; }
        public List<Weapon> Weapons { get; set; }
        public Attributes Attributes { get; set; }
        public string KeyAttribute { get; set; }
    }

    public class KnightRequestDTOValidator : AbstractValidator<KnightRequestDTO>
    {
        public KnightRequestDTOValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty().WithMessage("{PropertyName} não pode estar vazio.");

            RuleFor(c => c.Nickname)
               .NotEmpty().WithMessage("{PropertyName} não pode estar vazio.");

            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("{PropertyName} não pode estar vazio.")
                .NotNull().WithMessage("{PropertyName} não pode estar vazio.")
                .Must(BeAvalidAge).WithMessage("{PropertyName} deve maior que 18 anos");

            RuleFor(x => x.Weapons)
               .NotNull().WithMessage("A lista de armas do cavaleiro não pode ser nula.")
               .NotEmpty().WithMessage("A lista de armas do cavaleiro não pode estar vazia.");

            RuleFor(x => x.Attributes)
               .NotNull().WithMessage("Os atributos do cavaleiro não podem ser nulos.")
               .NotEmpty().WithMessage("Os atributos do cavaleiro não podem não pode estar vazia.");

            RuleFor(c => c.KeyAttribute)
               .NotEmpty().WithMessage("{PropertyName} não pode estar vazio.");
        }

        protected bool BeAvalidAge(DateTime date)
        {
            int curretDay = DateTime.Now.Day;
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            var age = currentYear - year;
            if (currentMonth >= month && curretDay > day)
                age--;

            return age >= 18;
        }
    }
}
