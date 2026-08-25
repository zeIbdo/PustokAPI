using Pustok.Application.Dtos.SubscriptionDtos;
using Pustok.Application.Services.Abstractions.Generic;

namespace Pustok.Application.Services.Abstractions;

public interface ISubscriptionService:IGetService<SubscriptionGetDto>,IModifyService<SubscriptionCreateDto,SubscriptionUpdateDto>
{
}
