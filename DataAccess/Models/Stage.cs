﻿
namespace DataAccess.Models;

public class Stage
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }
    public string? PipelineId { get; set; }
}
