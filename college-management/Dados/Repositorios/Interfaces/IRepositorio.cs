using college_management.Dados.Modelos;


namespace college_management.Dados.Repositorios.Interfaces;


public interface IRepositorio<T> where T : Modelo
{
	public Task<bool> Adicionar(T modelo);

	public List<T> ObterTodos();

	public T ObterPorId(string? id);

	public T? ObterPorNome(string? nome);

	public Task<bool> Atualizar(T modelo);

	public Task<bool> Remover(string? id);

	public bool Existe(T modelo);
}
