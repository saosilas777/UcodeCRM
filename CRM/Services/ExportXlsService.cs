using CRM.Interfaces;
using CRM.Models.ExportXlsModel;
using ClosedXML;
using ClosedXML.Excel;
using CRM.Models;


namespace CRM.Services
{
	public static class ExportXlsService
	{
		public static void ExportXls(List<CustomerModel> customers)
		{
			List<ExportXlsModel> xlsModels = new List<ExportXlsModel>();
			string status = "";
			foreach (var item in customers)
			{
				if (item.Status)
				{
					status = "ATIVO";
				}
				else
				{
					status = "INATIVO";
				}
				string _mail = "";
				for (int i = 0; i <= 2; i++)
				{
					if(item.Emails[i].Email != null && item.Emails[i].Email != "")
					{
						_mail = item.Emails[i].Email;

					}
				}
				ExportXlsModel export = new()
				{
					Codigo = item.Codigo,
					Cnpj = item.Cnpj.ToString(),
					RazaoSocial = item.RazaoSocial,
					Status = status,
					Email = _mail,
					Telefone = item.Phones[0].Phone,
					Compradores = item.Contact,
					Cidade = item.Cidade,
					Uf = item.Uf,
					DataProximoContato = item.NextContactDate.ToString("dd/MM/yyyy")

				};
				if (item.ContactRecords.Count() > 0)
				{
					export.DataUltimoContato = item.ContactRecords[0].RegistrationDate.ToString("dd/MM/yyyy");
				}
				if (item.CustomerPurchases.Count() > 0)
				{
					export.DataUltimaCompra = item.CustomerPurchases.LastOrDefault().PurchaseDate.ToString();
					export.ValorUltimaCompra = item.CustomerPurchases.LastOrDefault().PurchaseValue.ToString();
				}
				

				xlsModels.Add(export);
			}
			IXLWorkbook workbook = new XLWorkbook();

			workbook.Worksheets.Add("Clientes").FirstCell().InsertTable<ExportXlsModel>(xlsModels, true);
			workbook.SaveAs("wwwroot/files/clientes.xlsx");
			

		}
	}
}
