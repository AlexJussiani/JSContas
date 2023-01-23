using JS.Contas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JS.Contas.Infra.Data.Mappings
{
    public class ContaItemMapping : IEntityTypeConfiguration<ContaItem>
    {
        public void Configure(EntityTypeBuilder<ContaItem> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar(250)");
            
            builder.HasOne(c => c.Conta)
                .WithMany(c => c.ContaItems);

            builder.ToTable("ContaItems");
        }
    }
}
