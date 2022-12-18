using System;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Resources;
using FluentValidation;
using FluentValidation.Results;

namespace Acme.Todoist.Application.Extensions
{
    public static class RuleBuilderOptionsExtensions
    {
        /// <summary>
        /// Configure error code and message from <see cref="ReportCodeType"/>
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> WithMessageFromErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ReportCodeType reportCodeType)
        {
            return rule
                .WithErrorCode(reportCodeType.ToString())
                .WithMessage(m => ReportCodeMessage.GetMessage(reportCodeType));
        }

        /// <summary>
        /// Configure error code and message from <see cref="ReportCodeType"/>
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> WithMessageFromErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ReportCodeType reportCodeType, params object[] parameters)
        {
            return rule
                .WithErrorCode(reportCodeType.ToString())
                .WithMessage(c => ReportCodeMessage.GetMessage(reportCodeType, parameters));
        }

        /// <summary>
        /// Configure error code and message from <see cref="ReportCodeType"/>
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> WithMessageFromErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ReportCodeType reportCodeType, Func<T, TProperty, object[]> funcParameters)
        {
            return rule
                .WithErrorCode(reportCodeType.ToString())
                .WithMessage((c, property) => ReportCodeMessage.GetMessage(reportCodeType, funcParameters(c, property)));
        }

        /// <summary>
        /// Configure error code and message from <see cref="ReportCodeType"/>
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> WithMessageFromErrorCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, ReportCodeType reportCodeType, Func<T, TProperty, object> func)
        {
            return rule.WithMessageFromErrorCode(reportCodeType, (x, property) => new [] { func(x, property) });
        }

        /// <summary>
        ///  Validation will fail if the property is null or empty.
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> NotNullOrEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> rule)
        {
            return rule
                .NotNull().WithMessageFromErrorCode(ReportCodeType.MandatoryField)
                .NotEmpty().WithMessageFromErrorCode(ReportCodeType.MandatoryField);
        }

        /// <summary>
        /// Validation will fail if the length of the string is larger than the length specified.
        /// </summary>
        public static IRuleBuilderOptions<T, string> MaxLength<T>(this IRuleBuilder<T, string> rule, int maxLength)
        {
            return rule.Length(0, maxLength)
                .WithMessageFromErrorCode(ReportCodeType.InvalidSize, (request, str) => new object[] { maxLength, str.Length });
        }

        /// <summary>
        /// Validation will fail if the email address is invalid.
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNullOrEmpty()
                .MaxLength(Email.MaxLength)
                .Must(Email.IsValidEmailAddress)
                .WithMessageFromErrorCode(ReportCodeType.InvalidEmail);
        }

        /// <summary>
        /// Validation will fail if the brazilian phone number format is invalid.
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidPhoneNumber<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNullOrEmpty()
                .MaxLength(PhoneNumber.MaxLength)
                .IsValidPhoneNumber()
                .Must(PhoneNumber.IsValidPhoneNumber)
                .WithMessageFromErrorCode(ReportCodeType.InvalidPhoneNumber);
        }

        /// <summary>
        /// Validation will fail if the password contain minimum requirement of security.
        /// </summary>
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8)
        {
            return ruleBuilder
                .MinimumLength(minimumLength).WithMessageFromErrorCode(ReportCodeType.PasswordTooShort, (request, str) => new object[] { minimumLength })
                .Matches("[A-Z]").WithMessageFromErrorCode(ReportCodeType.PasswordRequiresUppercaseLetter)
                .Matches("[a-z]").WithMessageFromErrorCode(ReportCodeType.PasswordRequiresLowercaseLetter)
                .Matches("[0-9]").WithMessageFromErrorCode(ReportCodeType.PasswordRequiresDigit)
                .Matches("[^a-zA-Z0-9]").WithMessageFromErrorCode(ReportCodeType.PasswordRequiresNonAlphanumeric);
        }

        /// <summary>
        /// Add a new validation failure
        /// </summary>
        public static void AddFailure<T>(this ValidationContext<T> validationContext, string propertyName, ReportCodeType reportCodeType)
        {
            validationContext.AddFailure(new ValidationFailure(propertyName, ReportCodeMessage.GetMessage(reportCodeType))
            {
                ErrorCode = reportCodeType.ToString()
            });
        }
    }
}
