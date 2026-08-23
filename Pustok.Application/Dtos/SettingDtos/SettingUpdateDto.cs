using Pustok.Application.Dtos.Generic;

namespace Pustok.Application.Dtos.SettingDtos;

public class SettingUpdateDto:IDto
{
    public string? Key { get; set; } 
    public string? Value { get; set; } 
}
