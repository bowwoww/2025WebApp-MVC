using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.Models
{

    public partial class HotelSysDBContext

    {
        public virtual DbSet<MemberWithTel> MemberWithTels { get; set; } = null!;
        public async Task<List<MemberWithTel>> GetMemberWithTelAsync(string memberId)
        {
            return await this.MemberWithTels
                .FromSqlInterpolated($"EXEC getMemberWithTel {memberId}")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}