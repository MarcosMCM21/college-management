using college_management.Dados.Modelos;
using college_management.Dados.Repositorios;

namespace college_management.Contextos.Interfaces;

public interface IContextoUsuarios : IContexto<Usuario>
{
    public Task EditarMatricula(Repositorio<Usuario> repositorio,
                                Usuario usuario);

    public void VerMatricula(Repositorio<Usuario> repositorio,
                             Usuario usuario);

    public void VerBoletim(Repositorio<Usuario> repositorio,
                           Usuario usuario);

    public void VerFinanceiro(Repositorio<Usuario> repositorio,
                              Usuario usuario);
}