using Application.DTOs.OfferDTOs;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.OfferValidator
{
    public class CreateOfferValidator: AbstractValidator<CreateOfferDto>
    {
        public CreateOfferValidator()
        {
            

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description Can't be Empty")
                .WithState(_ => ErrorCode.OfferDescriptionRequired);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date Can't be Empty")
                .WithState(_ => ErrorCode.OfferStartDateRequired);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date Can't be Empty")
                .WithState(_ => ErrorCode.OfferEndDateRequired)
                .GreaterThan(x => x.StartDate).WithMessage("End Date must be greater than Start Date")
                .WithState(_ => ErrorCode.OfferInvalidEndDate);

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Discount Value must be greater than 0")
                .WithState(_ => ErrorCode.OfferInvalidDiscountValue);
        }

    }
}
