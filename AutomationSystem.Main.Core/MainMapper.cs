using AutoMapper;
using AutomationSystem.Main.Contract;

namespace AutomationSystem.Main.Core
{
    public class MainMapper : IMainMapper
    {
        private readonly IMapper _mapper;

        public MainMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TTarget Map<TTarget>(object source)
        {
            return _mapper.Map<TTarget>(source);
        }
    }
}
