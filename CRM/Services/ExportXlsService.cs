using CRM.Interfaces;
using CRM.Models.ExportXlsModel;
using ClosedXML;
using ClosedXML.Excel;
using CRM.Models;


namespace CRM.Services
{
	public static  class ExportXlsService
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
				ExportXlsModel export = new()
				{
					Codigo = item.Codigo,
					RazaoSocial = item.RazaoSocial,
					Cnpj = item.Cnpj,
					Status = status,
					Cidade = item.Cidade,
					Uf = item.Uf,
					Email = item.Emails[0].Email,
					Phone = item.Phones[0].Phone,
					Contact = item.Contact

				};
				xlsModels.Add(export);
			}
			IXLWorkbook workbook = new XLWorkbook();

			workbook.Worksheets.Add("Clientes").FirstCell().InsertTable<ExportXlsModel>(xlsModels, true);
			workbook.SaveAs("wwwroot/files/clientes.xlsx");
			

		}
	}
}
