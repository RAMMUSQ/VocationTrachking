using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models;

public class StoredTokens {
    [Key]
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string UserName { get; set; } // Token
    
    public string Password { get; set; }
    
    public DateTime? CreatedDate { get; set; }

}//buras kullanılmıyor gibi duruyor düzenlemek lazım


