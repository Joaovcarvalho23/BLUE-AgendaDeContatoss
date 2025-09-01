namespace Agenda.Application.Utils;

public static class CpfUtils
{
    public static bool ValidarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11) return false;
        if (cpf.Distinct().Count() == 1) return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string temp = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += (temp[i] - '0') * mult1[i];

        int resto = soma % 11;
        int dig1 = resto < 2 ? 0 : 11 - resto;

        temp += dig1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += (temp[i] - '0') * mult2[i];

        resto = soma % 11;
        int dig2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{dig1}{dig2}");
    }
}