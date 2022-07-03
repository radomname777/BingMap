﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BingMap.Services.Models;

public class Attributes
{
    public string? BUS_ID { get; set; }
    public string? PLATE { get; set; }
    public string? DRIVER_NAME { get; set; }
    public string? PREV_STOP { get; set; }
    public string? CURRENT_STOP { get; set; }
    public string? SPEED { get; set; }
    public string? BUS_MODEL { get; set; }
    public string? LATITUDE { get; set; }
    public string? LONGITUDE { get; set; }
    public string? ROUTE_NAME { get; set; }
    public int LAST_UPDATE_TIME { get; set; }
    public string? DISPLAY_ROUTE_CODE { get; set; }
}

public class Bus
{
    [JsonPropertyName("@attributes")]
    public Attributes? Attributes { get; set; }
}
public class BakuBus
{
    public ObservableCollection<Bus>? BUS { get; set; }
}