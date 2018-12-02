using AutoMapper;

namespace Framework.IoC.AutoMapper
{
    public static class Configurator
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MasterProfile>();
                cfg.AllowNullCollections = true;
            });
            return config;
        }
    }
}
