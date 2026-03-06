using CurrencyService.Shared.Domain.Entities;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace CurrencyService.BackgroundWorker.Services;

/// <summary>
/// Парсер курсов валют ЦБ
/// </summary>
/// <param name="httpClient"></param>
public class CbrXmlParser(HttpClient httpClient)
{
    /// <summary>
    /// Ссылка на ресурс курсов валют
    /// </summary>
    private const string CbrUrl = "http://www.cbr.ru/scripts/XML_daily.asp";

    /// <summary>
    /// Распарсить список курсов валют
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<List<Currency>> ParseCurrenciesAsync(CancellationToken ct = default)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var response = await httpClient.GetStringAsync(CbrUrl, ct);
        var encoding = Encoding.GetEncoding("windows-1251");
        var bytes = encoding.GetBytes(response);
        using var stream = new MemoryStream(bytes);

        var doc = XDocument.Load(stream);
        var root = doc.Root;
        var date = DateTime.TryParseExact(root?.Attribute("Date")?.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _date) ? _date : DateTime.Now;

        return root?.Elements("Valute").Select(x => new Currency
        {
            Id = Guid.NewGuid(),
            CharCode = x.Element("CharCode")?.Value ?? "XXX",
            Name = x.Element("Name")?.Value ?? "Unknown",
            Rate = ParseRate(x.Element("Value")?.Value, x.Element("Nominal")?.Value),
            UpdatedAt = DateTime.SpecifyKind(date, DateTimeKind.Utc)
        }).ToList() ?? [];
    }

    /// <summary>
    /// ВЫчислить курс валюты к рублю
    /// </summary>
    /// <param name="value">Курс валюты</param>
    /// <param name="nominal">Количество единиц иностранной валюты</param>
    private static decimal ParseRate(string? value, string? nominal)
    {
        if (string.IsNullOrWhiteSpace(value)) return 0;
        var rate = decimal.Parse(value.Replace(",", "."), CultureInfo.InvariantCulture);
        var nom = int.TryParse(nominal, out var n) ? n : 1;

        return rate / nom;
    }
}