using college_management.Constantes;
using college_management.Contextos.Interfaces;
using college_management.Dados;
using college_management.Dados.Modelos;
using college_management.Views;


namespace college_management.Contextos;


public abstract class Contexto<T> : IContexto<T> where T : Modelo
{
	protected readonly BaseDeDados BaseDeDados;
	protected readonly Cargo       CargoContexto;
	protected readonly Usuario     UsuarioContexto;

	protected Contexto(BaseDeDados baseDeDados,
	                   Usuario usuarioContexto)
	{
		BaseDeDados     = baseDeDados;
		UsuarioContexto = usuarioContexto;
		CargoContexto
			= BaseDeDados
			  .Cargos
			  .ObterPorId(UsuarioContexto.CargoId);
	}

	protected bool TemAcessoRestrito =>
		CargoContexto.TemPermissao(PermissoesAcesso.AcessoEscrita) ||
		CargoContexto.TemPermissao(PermissoesAcesso.AcessoAdministradores);

	public bool ValidarPermissoes()
	{
		if (TemAcessoRestrito) return true;

		InputView inputPermissao = new("Acesso Restrito");
		inputPermissao.LerEntrada("Sem Permissão",
		                          "Você não tem permissão para acessar este recurso. Pressione [Enter] para voltar.");

		return false;
	}

	public void AcessarRecurso(string nomeRecurso)
	{
		var interfacesContexto = GetType().GetInterfaces();

		var recurso =
			interfacesContexto
				.Select(t => t.GetMethod(nomeRecurso))
				.FirstOrDefault(t => t is not null);

		if (recurso is null)
			throw new
				InvalidOperationException("Recurso inexistente");

		var task = (Task)recurso.Invoke(this, []);

		task?.Wait();
	}

	public abstract Task Cadastrar();

	public abstract Task Editar();

	public abstract Task Excluir();

	public abstract void Visualizar();

	public abstract void VerDetalhes();

	public void ListarOpcoes()
	{
		var opcoes = ObterOpcoes();

		MenuView menuRecursos = new("Menu Recursos",
		                            $"Bem vindo ao recuso de {typeof(T).Name}.",
		                            opcoes);

		menuRecursos.ConstruirLayout();
		menuRecursos.Exibir();
	}

	public string[] ObterOpcoes()
	{
		string[] recursosDisponiveis;

		if (TemAcessoRestrito)
		{
			recursosDisponiveis = typeof(T).Name switch
			{
				nameof(Usuario) => OperacoesRecursos.RecursosEscritaUsuarios,
				nameof(Curso)   => OperacoesRecursos.RecursosEscritaCursos,
				_ =>
				[
					..OperacoesRecursos.RecursosLeitura,
					..OperacoesRecursos.RecursosEscrita
				]
			};

			return recursosDisponiveis;
		}

		recursosDisponiveis = typeof(T).Name switch
		{
			nameof(Usuario) => OperacoesRecursos.RecursosLeituraUsuarios,
			nameof(Curso)   => OperacoesRecursos.RecursosLeituraCursos,
			_               => OperacoesRecursos.RecursosLeitura
		};

		return recursosDisponiveis;
	}
}
