using CRM.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Map
{
    public class _EmailModelMap : IEntityTypeConfiguration<EmailModel>
    {
        public void Configure(EntityTypeBuilder<EmailModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Customer);
        }
    }
}
