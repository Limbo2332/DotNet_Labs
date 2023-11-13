using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Context.EntityConfigurations
{
    public class PieceOfNewsConfig : IEntityTypeConfiguration<PieceOfNews>
    {
        public void Configure(EntityTypeBuilder<PieceOfNews> builder)
        {
            builder
                .Property(pon => pon.Subject)
                .HasMaxLength(ConfigurationConstants.SUBJECT_MAXLENGTH);

            builder
                .Property(pon => pon.Content)
                .HasMaxLength(ConfigurationConstants.CONTENT_MAXLENGTH);

            builder
                .HasMany(pon => pon.NewsTags)
                .WithOne(nt => nt.PieceOfNews)
                .HasForeignKey(nt => nt.PieceOfNewsId);

            builder
                .HasMany(pon => pon.NewsRubrics)
                .WithOne(nt => nt.PieceOfNews)
                .HasForeignKey(nt => nt.PieceOfNewsId);
        }
    }
}
