using CRM.Models.ViewModels;
using CRM.Models;

namespace CRM.Interfaces
{
	public interface ICustomerRepository
	{
		bool Create(CustomerCreateViewModel customer);
		CustomerEditViewModel BuscarPorId(Guid id);
		List<CustomerModel> BuscarTodos(Guid id);
		bool Deletar(CustomerModel customer);
		List<CustomerModel> AdicionarTodos(List<CustomerModel> customers);
		CustomerModel Atualizar(CustomerEditViewModel customers);
		CustomerModel BuscarPorCodigo(int codigo);
		List<CustomerModel> AtualizarTodos(List<CustomerModel> customers);
		bool RegistrationContact(string anotation,string date, Guid id);
		



	}
}
