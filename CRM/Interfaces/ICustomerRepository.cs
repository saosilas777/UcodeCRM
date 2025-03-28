﻿using CRM.Models.ViewModels;
using CRM.Models;
using CRM.Enums;

namespace CRM.Interfaces
{
	public interface ICustomerRepository
	{
		public void AddNewContact(List<CustomerModel> customers);
		CustomerModel Create(CustomerCreateViewModel customer);
		CustomerEditViewModel BuscarPorId(Guid id);
		List<CustomerModel> BuscarTodos(Guid id);
		List<CustomerModel> BuscarClientesDaAgendaAtual(Guid id);
		bool Deletar(CustomerModel customer);
		void ContactDateEdit(DateTime date, Guid id);
		List<CustomerModel> AdicionarTodos(List<CustomerModel> customers);
		bool Atualizar(CustomerEditViewModel customers);
		CustomerModel BuscarPorCodigo(int codigo);
		List<CustomerModel> AtualizarTodos(List<CustomerModel> customers);
		bool RegistrationContact(string anotation,string date, Guid id);

		void ChangeAllNextContactsDates(string nextContactDate);
		void ChangePriority(string priority, string priorityId);

		void UpdateNextContactDates(List<UpdateNextContactDateViewModel> updates);





	}
}
