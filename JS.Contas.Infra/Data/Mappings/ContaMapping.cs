using JS.Contas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JS.Contas.Infra.Data.Mappings
{
    public class ContaMapping : IEntityTypeConfiguration<ContaCliente>
    {
        public void Configure(EntityTypeBuilder<ContaCliente> builder)
        {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

            
            builder.HasMany(c => c.ContaItems)
                .WithOne(c => c.Conta)
                .HasForeignKey(c => c.ContaId);

            builder.ToTable("Contas");
        }
    }
}
