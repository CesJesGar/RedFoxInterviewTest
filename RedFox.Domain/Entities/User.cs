 namespace RedFox.Domain.Entities;

 public class User
 {
     public int Id { get; set; }

     public string Email { get; set; } = string.Empty;
     public string Name { get; set; } = string.Empty;
     public string Username { get; set; } = string.Empty;
     public string Phone { get; set; } = string.Empty;
     public string Website { get; set; } = string.Empty;

     public int CompanyId { get; set; }
     public Company? Company { get; set; }
     public int AddressId { get; set; }
     public Address? Address { get; set; }
 }
