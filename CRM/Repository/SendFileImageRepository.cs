using CRM.Data;
using CRM.Models;

namespace CRM.Repository
{

	public class SendFileImageRepository
	{
		private readonly Context _context;

		public SendFileImageRepository(Context context)
		{
			_context = context;
		}

		public void SendFileImage(SendFileImageModel fileImage)
		{
			var urlDb = _context.ImageUrl.FirstOrDefault(x => x.UserId == fileImage.UserId);
			if (urlDb == null)
			{
				_context.Add(fileImage);
			}
			else
			{
				urlDb.Url = fileImage.Url;
				_context.Update(urlDb);
			}
			_context.SaveChanges();
		}
		public SendFileImageModel GetById(Guid id)
		{
			return _context.ImageUrl.FirstOrDefault(x => x.UserId == id);


		}
	}
}
