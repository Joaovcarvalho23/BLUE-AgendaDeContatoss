namespace Agenda.Application.Utils;

public static class SenhaUtils
{
    public static bool SenhaValida(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha)) return false;
        if (senha.Length < 6) return false;
        if (!senha.Any(char.IsUpper)) return false;
        if (!senha.Any(char.IsDigit)) return false;
        if (!senha.Any(ch => !char.IsLetterOrDigit(ch))) return false;

        return true;
    }
}