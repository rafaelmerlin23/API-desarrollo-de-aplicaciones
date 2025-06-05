using FluentValidation;
using foroLIS_backend.DTOs.CommunitySurveyDtos;

namespace foroLIS_backend.Validators
{
    public class CreateSurveyValidator: AbstractValidator<CreateCommunitySurveyDto>
    {
        public CreateSurveyValidator() {
            RuleFor(ccs => ccs.EndDate).Must(date => date >= DateTime.UtcNow).WithMessage("la fecha de vencimiento de la encuesta tiene que ser despues de la fecha actual");
            RuleFor(css => css.AllowMoreOneAnswer).NotEmpty();
            RuleFor(css => css.Fields).Must(f => f.Count() >= 2).WithMessage("la encuesta debe de tener al menos 2 campos");
            RuleFor(css => css.Title).Length(1, 256);
        }
    }
}
