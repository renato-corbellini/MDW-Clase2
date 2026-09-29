namespace Clase2.Domain.Entities;

public sealed class Profesor
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Legajo { get; private set; } = string.Empty;

    private Profesor() { }

    public Profesor(string nombre, string apellido, string legajo)
    {
        Id = Guid.NewGuid();
        UpdateDetails(nombre, apellido, legajo);
    }

    public void UpdateDetails(string nombre, string apellido, string legajo)
    {
        Nombre = ValidateAndNormalize(nombre, nameof(nombre), 100);
        Apellido = ValidateAndNormalize(apellido, nameof(apellido), 100);
        Legajo = ValidateAndNormalize(legajo, nameof(legajo), 20);
    }

    private static string ValidateAndNormalize(string value, string parameterName, int maxLength)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrEmpty(normalized))
            throw new ArgumentException("El campo es obligatorio.", parameterName);
        if (normalized.Length > maxLength)
            throw new ArgumentException($"El campo no puede superar {maxLength} caracteres.", parameterName);

        return normalized;
    }
}
