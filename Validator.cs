public class Validator
{
   
    public int disk { get; set; }
    public int os { get; set; }
    public int ram { get; set; }

    public Validator(int diskParam = null, int osParam = null, int ramParam = null)
    {
        this.disk = diskParam; 
        this.os = osParam;
        this.ram = ramParam;
    }
}