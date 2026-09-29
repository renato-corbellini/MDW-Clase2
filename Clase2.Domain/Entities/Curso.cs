namespace Clase2.Domain.Entities;

public class Curso
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Credits { get; private set; }
    
    private Curso() { }

    public Curso(string name, int credits)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentNullException(nameof(name), "Course name can not be null or empty.");
        
        Id = Guid.NewGuid();
        Name = name;
        Credits = credits;
    }

    public void UpdateDetails(string name, int credits)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Course name can not be null or empty.", nameof(name));
        if (credits is < 1 or > 12)
            throw new ArgumentOutOfRangeException(nameof(credits), "Course credits must be between 1 and 12.");

        Name = name;
        Credits = credits;
    }
}