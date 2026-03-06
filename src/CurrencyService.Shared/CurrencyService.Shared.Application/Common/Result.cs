namespace CurrencyService.Shared.Application.Common;

/// <summary>
/// Результат операции, который может быть успешным или содержать ошибку
/// </summary>
public class Result<T>
{
    private readonly T? _value;
    private readonly string? _error;

    // Основной конструктор
    private Result(T? value, string? error)
    {
        _value = value;
        _error = error;
        IsSuccess = error == null;
    }

    /// <summary>
    /// Указывает, была ли операция успешной
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Получает значение результата, если операция успешна
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of failed result");

    /// <summary>
    /// Получает ошибку, если операция завершилась неудачей
    /// </summary>
    public string? Error => _error;

    /// <summary>
    /// Создает успешный результат с значением
    /// </summary>
    public static Result<T> Success(T value) => new(value, null);

    /// <summary>
    /// Создает неуспешный результат с ошибкой
    /// </summary>
    public static Result<T> Failure(string error) => new(default, error);

    /// <summary>
    /// Преобразует результат в другой тип
    /// </summary>
    public Result<TResult> Map<TResult>(Func<T, TResult> mapFunction)
    {
        if (IsSuccess)
            return Result<TResult>.Success(mapFunction(_value!));
        else
            return Result<TResult>.Failure(_error!);
    }
}

/// <summary>
/// Статический класс для создания успешных и неуспешных результатов
/// </summary>
public static class Result
{
    /// <summary>
    /// Создает успешный результат без значения
    /// </summary>
    public static Result<Unit> Success() => Result<Unit>.Success(Unit.Value);

    /// <summary>
    /// Создает неуспешный результат без значения
    /// </summary>
    public static Result<Unit> Failure(string error) => Result<Unit>.Failure(error);
}

/// <summary>
/// Пустое значение для результатов без значения
/// </summary>
public class Unit
{
    public static readonly Unit Value = new();
    private Unit() { }
}
