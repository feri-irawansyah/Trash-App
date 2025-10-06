namespace Services.Models;
public class ActionResult
{
    public bool Result { get; set; } = false;
    public string Message { get; set; } = "";
    public dynamic? Data { get; set; } = null;
}