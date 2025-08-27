using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("usuario")]
public class Usuario
{
    [Key]
    public int id_usuario { get; set; }

    [Required]
    public string? nombre { get; set; }

    [Required]
    public string? apellido { get; set; }

    [Required]
    public int dni {  get; set; }

    public DateTime fecha_creacion { get; set; } = DateTime.Now;

    public bool activo { get; set; } = true;    
}