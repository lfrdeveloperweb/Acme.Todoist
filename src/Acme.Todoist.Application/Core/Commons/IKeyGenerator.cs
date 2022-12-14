namespace Acme.Todoist.Application.Core.Commons;

/// <summary>
/// Generator of keys randomly generated
/// </summary>
public interface IKeyGenerator
{
    /// <summary>
    /// Generates random number.
    /// </summary>
    int GenerateNumber(int min = 1, int max = 999999);

    /// <summary>
    /// Generates a unique value to be used as identifier.
    /// </summary>
    string Generate();
}