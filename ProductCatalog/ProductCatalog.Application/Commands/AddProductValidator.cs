using FluentValidation;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Commands;

public class AddProductValidator : AbstractValidator<AddProductCommand>
{
    public AddProductValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Please provide product name");

        RuleFor(p => p.CategoryName).NotEmpty().WithMessage("Please provide category for a product");

        RuleFor(p => p.CategoryName).MustAsync(async (category, _) =>
        {
            return string.IsNullOrEmpty(category) ? false :
             (await categoryRepository.GetByName(category) != null);
        }).WithMessage($"Invalid category name.");
    }
}
