using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyDiemSinhVien.Startup))]
namespace QuanLyDiemSinhVien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
