using System.ComponentModel;
using System.Text;

namespace Nibbler.Core.Utilities;

public static class StringUtils
{
    private static string ApenasNumeros(this string input)
    {
        return !string.IsNullOrEmpty(input) ? new string(input.Where(char.IsDigit).ToArray()) : "";
    }

    public static bool EhNumero(this string input)
    {
        var numeros = input.ApenasNumeros();
        return numeros.All(char.IsDigit);
    }

    public static string RemoveAcentos(this string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < texto.Length; i++)
        {
            if (texto[i] > 255)
                sb.Append(texto[i]);
            else
                sb.Append(SDiacritics[texto[i]]);
        }

        return sb.ToString();
    }

    private static readonly char[] SDiacritics = GetDiacritics();

    private static char[] GetDiacritics()
    {
        char[] accents = new char[256];

        for (int i = 0; i < 256; i++)
            accents[i] = (char)i;

        accents[(byte)'á'] =
            accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
        accents[(byte)'Á'] =
            accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

        accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
        accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

        accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
        accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

        accents[(byte)'ó'] =
            accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
        accents[(byte)'Ó'] =
            accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

        accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
        accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

        accents[(byte)'ç'] = 'c';
        accents[(byte)'Ç'] = 'C';

        accents[(byte)'ñ'] = 'n';
        accents[(byte)'Ñ'] = 'N';

        accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
        accents[(byte)'Ý'] = 'Y';

        return accents;
    }

    public static string ToCamelCase(this string str) =>
        string.IsNullOrEmpty(str) || str.Length < 2
            ? str
            : char.ToLowerInvariant(str[0]) + str.Substring(1);


    public static string AlfanumericoAleatorio(int tamanho)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, tamanho)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        return result;
    }

    public static string CapitalizarNome(this string nome)
    {
        string[] excecoes = new string[] { "e", "de", "da", "das", "do", "dos" };
        var palavras = new Queue<string>();
        foreach (var palavra in nome.Split(' '))
        {
            if (!string.IsNullOrEmpty(palavra))
            {
                var emMinusculo = palavra.ToLower();
                var letras = emMinusculo.ToCharArray();
                if (!excecoes.Contains(emMinusculo)) letras[0] = char.ToUpper(letras[0]);
                palavras.Enqueue(new string(letras));
            }
        }

        return string.Join(" ", palavras);
    }

    public static string TrataEspacos(this string nome)
    {
        var palavras = new Queue<string>();

        foreach (var palavra in nome.Split(' '))
        {
            if (string.IsNullOrEmpty(palavra)) continue;

            var letras = palavra.Trim().ToCharArray();

            palavras.Enqueue(new string(letras));
        }

        return string.Join(" ", palavras);
    }

    public static string TrataCaracteresEspeciais(this string nome)
    {
        return nome.Replace("_", "")
            .Replace("#", "")
            .Replace("$", "")
            .Replace(".", "")
            .Replace("-", "")
            .Replace(",", "")
            .Replace("*", "")
            .Replace("%", "")
            .Replace("@", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("=", "")
            .Replace("|", "")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace("?", "")
            .Replace(";", "")
            .Replace(":", "")
            .Replace("&", "")
            .Replace("+", "");
    }

    public static DateTime? ConverterParaData(this string dataFormatada)
    {
        if (string.IsNullOrWhiteSpace(dataFormatada)) return null;

        if (!dataFormatada.Contains('-'))
        {
            var dataFormatoBrasil = ConverterParaDataBarras(dataFormatada);

            if (dataFormatoBrasil is null) return null;

            return dataFormatoBrasil;
        }

        var arrayDeData = dataFormatada.Split('-');

        try
        {
            return new DateTime(Convert.ToInt32(arrayDeData[0]),
                Convert.ToInt32(arrayDeData[1]), Convert.ToInt32(arrayDeData[2]));
        }
        catch
        {
            return null;
        }
    }

    private static DateTime? ConverterParaDataBarras(this string dataFormatoBrasil)
    {
        if (string.IsNullOrWhiteSpace(dataFormatoBrasil)) return null;

        if (!dataFormatoBrasil.Contains('/')) return null;

        var arrayDeData = dataFormatoBrasil.Split('/');

        try
        {
            return new DateTime(Convert.ToInt32(arrayDeData[2]),
                Convert.ToInt32(arrayDeData[1]), Convert.ToInt32(arrayDeData[0]));
        }
        catch
        {
            return null;
        }
    }


    public static string ObterDescricao(this Enum element)
    {
        var elementInfo = element.GetType().GetField(element.ToString());

        var attributes = (DescriptionAttribute[])elementInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes.Length == 0) return element.ToString();

        return attributes[0].Description;
    }
}
