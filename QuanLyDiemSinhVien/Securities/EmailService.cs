using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace QuanLyDiemSinhVien.Securities
{

    internal class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}