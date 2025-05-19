namespace WebApplication1.Models
{
    public class ProyectoViewModel
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenURL { get; set; }
        public List<string> Tecnologias { get; set; } = new();
        public ProyectoLinks Links { get; set; } = new();
        public string Categoria { get; set; }
        public int Orden { get; set; }
    }

    public class ProyectoLinks
    {
        public string Demo { get; set; }
        public List<string> Github { get; set; } = new();
        public string Youtube { get; set; }
    }


}
