using System.Globalization;
using System.Text;

public class NormalizationService
{
    // Hàm để chuyển đổi chuỗi từ có dấu sang không dấu
    public string RemoveDiacritics(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var normalizedText = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var ch in normalizedText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(ch);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
    }
}
