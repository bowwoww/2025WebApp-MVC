using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary; 

namespace DBContextClassLibrary
{

    public partial class HotelSysDBContext

    {
        public virtual DbSet<MemberWithTel> MemberWithTels { get; set; } = null!;
        public async Task<List<MemberWithTel>> GetMemberWithTelAsync(string memberId)
        {
            return await this.MemberWithTels
                .FromSqlRaw("EXEC getMemberWithTel @p0", memberId)
                .ToListAsync();
        }
    }
}