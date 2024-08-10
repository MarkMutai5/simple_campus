namespace FinalCampus.ModelDTOs;

public class ResponseObject
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public object? Error { get; set; }
    public object? Result { get; set; }
}