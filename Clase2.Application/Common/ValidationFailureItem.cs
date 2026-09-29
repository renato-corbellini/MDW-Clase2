namespace Clase2.Application.Common;

public sealed record ValidationFailureItem(string PropertyName, string ErrorMessage); // DTO, solo permite tener atributos, no metodos.