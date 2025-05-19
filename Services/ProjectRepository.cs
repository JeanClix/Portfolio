using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IProjectRepository
    {
        List<ProyectoViewModel> getProyectos();
    }
    public class ProjectRepository : IProjectRepository
    {

        public List<ProyectoViewModel> getProyectos()
        {
            var proyectos = new List<ProyectoViewModel>
                {
                    new ProyectoViewModel
                    {
                        Nombre = "Ecommerce",
                        Descripcion = "Ecommerce interactivo desarrollado con Angular",
                        ImagenURL = "https://monkeyplusbc.com/assets/imags/blogs/conoce-eCommerce-conveniente-para-tu-negocio-pricipal.jpg",
                        Tecnologias = new List<string> { "Angular", "TypeScript", "Bootstrap" },
                        Links = new ProyectoLinks
                        {
                            Demo = "https://melodic-narwhal-09adba.netlify.app/",
                            Github = new List<string> { "https://github.com/JeanClix/IdeasDigitales" },
                            Youtube = null
                        },
                        Categoria = "Web",
                        Orden = 1
                    },
                    new ProyectoViewModel
                    {
                        Nombre = "Sistema de reservas de Hotel",
                        Descripcion = "Sistema de reservas con backend desarrollado en Spring y frontend con Angular. Base de datos utilizada: PostgreSQL",
                        ImagenURL = "https://www.hotelogix.com/images/get-more-done-for-less.png",
                        Tecnologias = new List<string> { "Angular", "Spring", "TypeScript", "Java", "Angular Material" },
                        Links = new ProyectoLinks
                        {
                            Demo = null,
                            Github = new List<string>
                            {
                                "https://github.com/JeanClix/Backend_JFS",
                                "https://github.com/JeanClix/Frontend_JFS"
                            },
                            Youtube = "https://youtu.be/VlAR6q_vVh8?si=AZWahNdT5Fob9zGp"
                        },
                        Categoria = "Web",
                        Orden = 2
                    }
                };


            return proyectos;
        }
    }
}
