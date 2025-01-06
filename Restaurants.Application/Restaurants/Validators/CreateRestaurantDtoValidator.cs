using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validCategories = ["Japanese", "Italian", "Mexican", "American", "Indian"];
    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
         .NotEmpty().WithMessage("Description is required.");

        //RuleFor(dto => dto.Category)
        // .NotEmpty().WithMessage("Insert a valid category");

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories");
        //.Must(category => validCategories.Contains(category));
        //.Custom((value, context) =>
        //{
        //    var isValidCategory = validCategories.Contains(value);

        //    if (!isValidCategory)
        //    {
        //        context.AddFailure("Category", "Invalid category. Please choose from the valid catergories");
        //    }
        //});

        RuleFor(dto => dto.ContactEmail)
         .EmailAddress().WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostalCode)
         .Matches(@"^\d{2}-\d{3}$").WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}
