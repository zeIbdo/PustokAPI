using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SettingDtos;

public class SettingCreateDto:IDto
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
