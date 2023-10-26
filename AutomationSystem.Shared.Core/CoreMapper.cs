using AutoMapper;
using AutomationSystem.Shared.Contract;

namespace AutomationSystem.Shared.Core
{
    public class CoreMapper : ICoreMapper
    {
        private readonly IMapper _mapper;

        public CoreMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TTarget Map<TTarget>(object source)
        {
            return _mapper.Map<TTarget>(source);
        }
    }
}
