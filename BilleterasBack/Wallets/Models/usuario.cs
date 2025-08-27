using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("usuario")]
public class Usuario
{
    [Key]
    public int id_usuario { get; set; }

    
    public string? nombre { get; set; }

    
    public string? apellido { get; set; }

    public string? email { get; set; }
   
    public string? password_hash { get; set; }

    public int dni {  get; set; }

    public string? tipo_usuario {get; set; }

    public DateTime fecha_creacion { get; set; } = DateTime.Now;
    public bool activo { get; set; } = true;    
}