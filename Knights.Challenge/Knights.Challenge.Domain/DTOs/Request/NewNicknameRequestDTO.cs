using FluentValidation;

namespace Knights.Challenge.Domain.DTOs.Request
{
    public class NewNicknameRequestDTO
    {
        public string NewNickname { get; set; }
    }

    public class NewNicknameRequestDTOValidator : AbstractValidator<NewNicknameRequestDTO>
    {
        public NewNicknameRequestDTOValidator()
        {
            RuleFor(c => c.NewNickname)
               .NotEmpty().WithMessage("{PropertyName} não pode estar vazio.")
               .NotNull().WithMessage("{PropertyName} não pode estar vazio.");
        }
    }
}
