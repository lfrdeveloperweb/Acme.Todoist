namespace Acme.Todoist.Application.Core.Security
{
    /// <summary>
    /// Represents the result of a sign-in operation.
    /// </summary>
    /// <param name="Succeeded">Flag indication whether the sign-in was successful.</param>
    /// <param name="IsLockedOut">Flag indication whether the user attempting to sign-in is locked out.></param>
    /// <param name="IsPhoneNumberConfirmed">Flag indication whether the user attempting to sign-in requires phone number confirmation.</param>
    public sealed record SignInResult(
        bool Succeeded = false , 
        bool IsLockedOut = false,
        bool IsEmailConfirmed = false, 
        bool IsPhoneNumberConfirmed = false)
    {
        /// <summary>
        /// Returns a <see cref="SignInResult" /> that represents a successful sign-in.
        /// </summary>
        public static SignInResult Success => new (Succeeded: true);

        /// <summary>
        /// Returns a <see cref="SignInResult" /> that represents a sign-in attempt that failed because the user was logged out.
        /// </summary>
        /// <returns>Represents sign-in attempt that failed due to the user being locked out.</returns>
        public static SignInResult LockedOut => new(IsLockedOut: true);

        /// <summary>
        /// Returns a <see cref="SignInResult" /> that represents a sign-in attempt that needs email confirmation.
        /// </summary>
        /// <returns>A <see cref="SignInResult" /> that represents sign-in attempt that needs phone number confirmation.</returns>
        public static SignInResult EmailNotConfirmed => new(IsEmailConfirmed: true);

        /// <summary>
        /// Returns a <see cref="SignInResult" /> that represents a sign-in attempt that needs phone number confirmation.
        /// </summary>
        /// <returns>A <see cref="SignInResult" /> that represents sign-in attempt that needs phone number confirmation.</returns>
        public static SignInResult PhoneNumberNotConfirmed => new(IsPhoneNumberConfirmed: true);

        public static readonly SignInResult LoginFailed = new();
    }
}
