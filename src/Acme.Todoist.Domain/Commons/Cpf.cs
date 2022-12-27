using System.Linq;

namespace Acme.Todoist.Domain.Commons;

public sealed record Cpf
{
    /// <summary>
    /// The email maximum length.
    /// </summary>
    public const int MaxLength = 11;

    /// <summary>
    /// Checks if the current string has a valid email address format.
    /// </summary>
    /// <param name="documentNumber">String to be checked.</param>
    /// <returns>Return true if the string has a valid email address format.</returns>
    public static bool IsValid(string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return false;

        // valid if the cpf length is differ 11 characters
        if (documentNumber.Length != 11)
            return false;

        // Returns false if all digits are equal.
        if (documentNumber.Distinct().Count() == 1) { return false; }

        // Stores the CPF numbers without the check digits
        string tempCpf = documentNumber.Substring(0, 9);

        // Calculates the first check digit
        string checkDigit1 = CalculateCpfDigit(tempCpf);

        // Calculates the second check digit
        string checkDigit2 = CalculateCpfDigit(tempCpf + checkDigit1);

        return documentNumber.EndsWith(checkDigit1 + checkDigit2);
    }

    private static string CalculateCpfDigit(string source)
    {
        var sum = 0;

        // Process 9 first digits from cpf.
        for (int i = 0; i < source.Length; i++) { sum += int.Parse(source[i].ToString()) * (source.Length + 1 - i); }

        // Armazena o resto da divisão por 11.
        int remainder = sum % 11;

        // Valores menores que 2 são convertidos para zero.
        remainder = (remainder < 2) ? 0 : 11 - remainder;

        // Retorna o dígito verificador.
        return remainder.ToString();
    }
}