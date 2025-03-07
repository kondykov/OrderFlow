namespace OrderFlow.Handlers;

public class Validator
{
    private readonly List<string> _errors = [];

    /// <summary>
    ///     Универсальный валидатор параметров.
    /// </summary>
    /// <param name="value">Параментр для проверки</param>
    /// <param name="condition">Метод проверки</param>
    /// <param name="errorMessage">Сообщение об ошибке</param>
    /// <typeparam name="T">Подстраивается под тип принимаемого параметра</typeparam>
    /// <returns></returns>
    public Validator Validate<T>(T value, Func<T, bool> condition, string errorMessage)
    {
        if (!condition(value)) _errors.Add(errorMessage);
        return this;
    }

    public List<string> GetErrors()
    {
        return _errors;
    }

    public bool IsValid()
    {
        return _errors.Count == 0;
    }
}