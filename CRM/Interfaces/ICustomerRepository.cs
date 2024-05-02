using CRM.Models.ViewModels;
using CRM.Models;

namespace CRM.Interfaces
{
	public interface ICustomerRepository
	{
		string Create(CustomerCreateViewModel customer);
		CustomerEditViewModel BuscarPorId(Guid id);
		List<CustomerModel> BuscarTodos(Guid id);
		List<CustomerModel> BuscarClientesDaAgendaAtual(Guid id);

		List<CustomerModel> GetByStatus(string status);
		bool Deletar(CustomerModel customer);
		void ContactDateEdit(DateTime date, Guid id);
		List<CustomerModel> AdicionarTodos(List<CustomerModel> customers);
		bool Atualizar(CustomerEditViewModel customers);
		CustomerModel BuscarPorCodigo(int codigo);
		List<CustomerModel> AtualizarTodos(List<CustomerModel> customers);
		bool RegistrationContact(string anotation,string date, Guid id);
		



	}
}
