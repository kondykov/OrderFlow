namespace OrderFlow.Infrastructure;

public abstract class Validator
{
    /// <summary>
    /// Метод валидации объекта по определённой логике.
    /// </summary>
    /// <param name="obj">Объект для проверки</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <returns>Возвращает True или False в зависимости от вычислений. По умолчанию возвращает True.</returns>
    public virtual bool Validate<T>(T obj) => true;
}