using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CRM.Models;

namespace CRM.Data.Map
{
    public class _ContactRecordsMap : IEntityTypeConfiguration<ContactRecords>
    {
        public void Configure(EntityTypeBuilder<ContactRecords> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Customer);
        }
    }

}
