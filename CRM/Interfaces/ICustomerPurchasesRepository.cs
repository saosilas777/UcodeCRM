﻿using CRM.Models;

namespace CRM.Interfaces
{
	public interface ICustomerPurchasesRepository
	{
		bool SavePurchases(List<PurchaseModel> purchases);
		void Add(PurchaseModel purchase);
		List<PurchaseModel> GetPurchases();
		PurchaseModel GetPurchaseById(Guid id);
		bool DeletePurchase(Guid id);
	}
}
