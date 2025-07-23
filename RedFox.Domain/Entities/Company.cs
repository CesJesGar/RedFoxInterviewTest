namespace RedFox.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string CatchPhrase { get; set; } = string.Empty;

    public string Bs { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
}